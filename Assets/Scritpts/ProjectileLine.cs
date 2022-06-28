using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //single

    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        S = this; // set url to single
        // get url to alineRenderer
        line = GetComponent<LineRenderer>();
        // off LineRenderer, until we need it
        line.enabled = false;
        // init list of points
        points = new List<Vector3>();
    }

    //this method camouflage as stroke
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null ) {
                // If -poi have true url, break all parametrs to saurce value
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }


    //this method need to clear line
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }


    public void AddPoint()
    {
        // Call it for add point to line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            // If point dont far enough away from last, just exit
            return;
        }
        if (points.Count == 0)
        { // If point of launch...
            Vector3 launchPosDiff = pt - Slightshot.LAUNCH_POS; // to define
            // ...add optional peace of line for take aim better
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Add first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // on LineRenderer
            line.enabled = true;
        }
        else
        {
            // Simple sequence of points adding
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }


    //Set pos of last point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                // If havent points retutn Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
    void FixedUpdate()
    {
        if (poi == null)
        {
            // If poi have null, find POI
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; // Exit if POI not found
                }
            }
            else
            {
                return; // Exit if POI not found
            }
        }
        // if POI found, try to add point with POI's pos in every FixedUp
        AddPoint();
        if (FollowCam.POI == null)
        {
            // If FollowCam.POI have null, get nulll in poi
            poi = null;
        }
    }
}
