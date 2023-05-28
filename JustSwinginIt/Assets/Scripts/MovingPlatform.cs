using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pos1,pos2;
    public float speed;
    public Transform startPos;
    Vector3 nextPos;
    void Start()
    {
        nextPos=startPos.position;
    }

    // Update is called once per frame 
    void Update()
    {
        if(transform.position==pos1.position){
            nextPos = pos2.position;
            Debug.Log("Here1");
        }
        if(transform.position==pos2.position){
            nextPos = pos1.position;
            Debug.Log("Here2");
        }
        Debug.Log(nextPos);
        transform.position = Vector3.MoveTowards(transform.position,nextPos,speed*Time.deltaTime);
    }

    private void OnDrawGizoms(){
        Gizmos.DrawLine(pos1.position,pos2.position); 
    }
}
