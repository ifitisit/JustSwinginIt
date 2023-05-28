using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFollow : MonoBehaviour
{
    Vector2 screenBounds;
    float maxHeight;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.transform.position.z));
        transform.position = new Vector2(transform.position.x, screenBounds.y+1);
        maxHeight = transform.position.y + 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.transform.position.z));
        transform.position = new Vector2(transform.position.x, Mathf.Min(maxHeight, screenBounds.y+1));
        // Debug.Log(transform.position.y);
    }
}
