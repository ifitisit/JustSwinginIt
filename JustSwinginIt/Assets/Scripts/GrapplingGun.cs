using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope grapplingRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;
    [SerializeField] private int zippableLayerNumber = 8;
    [SerializeField] private int groundLayerNumber = 10;

    [Header("Main Camera")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;
    public Transform groundCheck;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;
    [SerializeField] private float minDistance = 3;

    [Header("Speed:")]
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float minSpeed = 5f;

    private enum LaunchType
    {
        Transform_Launch,
        Zip_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float physicsSpeed = .5f;
    [SerializeField] private float transformSpeed = 10;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequency = 1;

    [SerializeField] private float clickRadius = 10;

    [SerializeField] private float startingSpeed = 10;
    
    [Header("Power Up Factor")]
    [SerializeField] private float boostFactor = 1.5f;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    private float launchSpeed = .5f;
    private bool isGrounded = false;
    private bool wasGrounded = false;
    private bool passedZip = false;
    private float landingSpeed;
    private bool boosting;
    private float boostTimer;
    private float boostTimeLimits = 2;
    private int directionX = 0;
    private int directionY = 0;

    public int coinValue = 1;
    public AudioClip coin;
    public AudioClip boost;


    [Header("Analytics")]
    [SerializeField]
    private LevelCompleteActions manager;

    private void Start()
    {
        grapplingRope.enabled = false;
        m_springJoint2D.enabled = false;
        m_rigidbody.velocity = new Vector2(startingSpeed, 0f);
        boosting = false;
        boostTimer = 0;
    }

    private void FixedUpdate()
    {
        ContactFilter2D filter2D = new ContactFilter2D();

        int ground = 1 << groundLayerNumber;
        int zip = 1 << zippableLayerNumber;
        int grapple = 1 << grappableLayerNumber;
        filter2D.layerMask = grapple | zip | ground;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .15f, filter2D.layerMask);
        if (isGrounded)
        {
            if (!wasGrounded || boosting)
            {
                landingSpeed = m_rigidbody.velocity.x;
            }
            if (landingSpeed < minSpeed)
            {
                landingSpeed += 0.1f;
            }
            m_rigidbody.velocity = new Vector2(landingSpeed, 0f);
            wasGrounded = true;
        }
        else
            wasGrounded = false;

        if (m_rigidbody.velocity.magnitude > maxSpeed && !boosting)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * maxSpeed;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
            passedZip = false;
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grapplingRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grapplingRope.isGrappling)
            {
                if (launchType == LaunchType.Zip_Launch)
                {
                    if (!passedZip)
                    {
                        Vector2 currPos = transform.position;
                        float currDistance = (grapplePoint - currPos).magnitude;
                        float currXDistance = grapplePoint.x - currPos.x;
                        float currYDistance = grapplePoint.y - currPos.y;


                        float currSpeed = m_rigidbody.velocity.magnitude;

                        if (currSpeed >= maxSpeed - (maxSpeed / 4))
                        {
                            float forceMultiplier = maxSpeed - (currSpeed / maxSpeed);
                            m_rigidbody.AddForce(grappleDistanceVector * forceMultiplier);
                        }

                        else
                        {
                            m_rigidbody.AddForce(grappleDistanceVector * launchSpeed);
                        }

                        //Debug.Log(currDistance);

                        if (currDistance <= 0.5f || ((currXDistance * directionX <= 0) && Math.Abs(m_rigidbody.velocity.x) > launchSpeed)
                            || ((currYDistance * directionY <= 0) && Math.Abs(m_rigidbody.velocity.y) > launchSpeed))

                        {
                            Debug.Log("X: " + currXDistance * directionX);
                            Debug.Log("Y: " + currYDistance * directionY);
                            grapplingRope.enabled = false;
                            grapplingRope.isGrappling = false;
                            m_rigidbody.gravityScale = .5f;
                            passedZip = true;
                        }
                    }

                }

                else if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistance = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistance;
                    //Do this a different way to burst through power ups
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grapplingRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }

        if (m_rigidbody.gravityScale < 1 && m_rigidbody.gravityScale != 0)
        {
            m_rigidbody.gravityScale += Time.deltaTime / 2;
            if (m_rigidbody.gravityScale > 1)
                m_rigidbody.gravityScale = 1;
        }

        if (boosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= boostTimeLimits)
            {
                boostTimer = 0;
                boosting = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Powerup"))
        {
            if (!boosting)
            {
                boosting = true;
                m_rigidbody.velocity = m_rigidbody.velocity.normalized * maxSpeed * boostFactor;
            }
            else
            {
                boostTimer = 0;
            }
            Destroy(collision.gameObject);
            GetComponent<AudioSource>().PlayOneShot(boost);
        }

        if (collision.CompareTag("Coin"))
        {
            ScoreManager.instance.ChangeScore(coinValue);

            Destroy(collision.gameObject);
            GetComponent<AudioSource>().PlayOneShot(coin);
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        Vector2 click = m_camera.ScreenToWorldPoint(Input.mousePosition);
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.useLayerMask = true;
        int grapple = 1 << grappableLayerNumber;
        int zip = 1 << zippableLayerNumber;
        filter2D.layerMask = grapple | zip;
        Physics2D.OverlapCircle(click, clickRadius, filter2D, results);

        Collider2D bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Collider2D result in results)
        {
            Vector2 directionToTarget = (Vector2)result.gameObject.transform.position - click;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = result;
            }
        }

        if (bestTarget != null)
        {
            Vector2 closestPoint;
            Vector2 pointerPoint = bestTarget.ClosestPoint(click);
            RaycastHit2D ray = Physics2D.Raycast(firePoint.position, pointerPoint - (Vector2) firePoint.position, Mathf.Infinity, filter2D.layerMask);
            closestPoint = ray.point;
            
            Vector2 distanceVector = closestPoint - (Vector2)firePoint.position;
            if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
            {
                int layerMask = (1 << grappableLayerNumber) | (1 << zippableLayerNumber);
                RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, Mathf.Infinity, layerMask);

                if (_hit || grappleToAll)
                {
                    if (_hit.transform.gameObject.tag == ("Powerup"))
                    {
                        launchSpeed = transformSpeed;
                        launchType = LaunchType.Transform_Launch;
                        launchToPoint = true;
                    }
                    else
                    {
                        launchSpeed = physicsSpeed;
                        launchType = LaunchType.Physics_Launch;
                    }
                    if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                    {
                        if (_hit.transform.gameObject.layer == zippableLayerNumber)
                        {
                            launchSpeed = 10;
                            launchType = LaunchType.Zip_Launch;
                            launchToPoint = true;
                        }
                        else
                        {
                            launchSpeed = 1;
                            launchType = LaunchType.Physics_Launch;
                        }

                        //Case 2: Call HasPivoted.PivotRange() if general area has been grappled.
                        GameObject go = _hit.transform.gameObject;
                        var objScript = (HasPivoted)go.GetComponent(typeof(HasPivoted));
                        objScript.PivotRange();

                        grapplePoint = _hit.point;
                        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                        grapplingRope.enabled = true;
                    }
                }
            }

            else
            {
                manager.missedGrapples += 1;
            }
        }
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequency;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = Math.Max(distanceVector.magnitude, minDistance);
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Zip_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    if (transform.position.x < grapplePoint.x) directionX = 1;
                    else directionX = -1;
                    if (transform.position.y < grapplePoint.y) directionY = 1;
                    else directionY = -1;
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

    public bool onGround()
    {
        return isGrounded;
    }
}
