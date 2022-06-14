using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slightshot : MonoBehaviour
{
    [Header("Set in Inspectore")]
    public GameObject prefabProjictle;
    public float velocityMult = 8f;

    [Header("Set Dinamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectle;
    public bool aminigMode;
    private Rigidbody projectileRigidbody;


    private void Awake()
    {
        Transform launchPointTrans = transform.Find("launchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseEnter()
    {
        //print("Sightshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Sightshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //Player click mouseButton when cursor pos over slightshot
        aminigMode = true;
        //Made projectle
        projectle = Instantiate(prefabProjictle) as GameObject;
        //Placed projectle in launchPoint
        projectle.transform.position = launchPos;
        //Make it kinematically
        projectileRigidbody = projectle.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    void Update()
    {
        //If slihgtshot dont in aminig Mode disregared the code
        if (!aminigMode) return;
        //Get mouse's pos in realtime
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //Find deferent between launchPos and mousepos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //restrict mouseDelta to calaider's radius of Slightshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        //move Projectle in new pos
        Vector3 projPos = launchPos + mouseDelta;
        projectle.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            aminigMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectle;
            projectle = null;
        }
    }

}
