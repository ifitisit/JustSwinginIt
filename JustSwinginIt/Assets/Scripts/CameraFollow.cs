using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeedX = .125f;
    public float smoothSpeedY = .005f;

    [SerializeField]
    public Vector3 offset;

    private Vector3 def = new Vector3(-1, -1, -1);

    private void Start()
    {
        if(offset == def)
            offset = new Vector3(0, 3, -20);
    }

    private void FixedUpdate()
    {
        Vector3 destinPosition = target.position + offset;
        float smoothX = transform.position.x + (destinPosition.x - transform.position.x) * smoothSpeedX;
        float smoothY = transform.position.y + (destinPosition.y - transform.position.y) * smoothSpeedY;
        Vector3 smoothedPosition = new Vector3(smoothX, smoothY, transform.position.z);
        transform.position = smoothedPosition;
    }
}