using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("Set in Inspector")]
    static public GameObject POI; //(point of interest) url on interested obj
    public Vector2 minXY = Vector2.zero;
    public float easing = 0.05f;

    [Header("Set Dynamically")]
    public float camZ; //Wishing z pos

    private void Awake()
    {
        camZ = this.transform.position.z;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 destination;
        //If havent poi set P:[0.0.0]
        if (POI == null)
        {
            destination = Vector3.zero;
        } else
        {
            //Get pos of poi
            destination = POI.transform.position;
            //If POi is projectile - make sure that it stop
            if (POI.tag == "Projectile")
            {
                //if it dont move
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //back the source value of camera
                    POI = null;
                    //in next frame
                    return;
                }
            }
        }
        // 1 variant for stabilazation camrea in shot moment
        //find point between cam pos and destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //2 variant for stabilazation camrea in shot moment
        //Restritc X and Y with minimal value
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y); // im use both of them for better result
        //set destenation.z = camZ for push back camera
        destination.z = camZ;
        //put camera in destination's pos
        transform.position = destination;
    }
}
