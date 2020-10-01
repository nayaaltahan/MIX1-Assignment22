﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

#if UNITY_EDITOR
using input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviour
{
    private List<DetectedPlane> m_NewTrackedPlanes = new List<DetectedPlane>();

    public GameObject GridPrefab;

    public GameObject Portal;

    public GameObject ARCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if ARcore session is tracking
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        
        //We get the the newly detected planes in this frame
        Session.GetTrackables<DetectedPlane>(m_NewTrackedPlanes, TrackableQueryFilter.New);

        //for each detected plane we create a grid
        foreach (var detectedPlane in m_NewTrackedPlanes)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);
            
            grid.GetComponent<GridVisualizer>().Initialize(detectedPlane);
        }

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        
        // 
        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            //Placing the Portal on the tracked plane.
            Portal.SetActive(true);
            
            //creating the anchor
            var anchor = Session.CreateAnchor(hit.Pose, hit.Trackable);

            Instantiate(Portal, anchor.transform.position, anchor.transform.rotation, anchor.transform);

            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;


            //portal should face the camera
            Vector3 cameraPosition = ARCamera.transform.position;

            //set the Y as a fixed coordinate 
            cameraPosition.y = hit.Pose.position.y;
            
            //Rotate the portal to face the camera
            Portal.transform.LookAt(cameraPosition, Portal.transform.up);

            //Update coordinates when the anchor is updated.
            Portal.transform.parent = anchor.transform;
        }
    }
}
