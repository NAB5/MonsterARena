using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARtest : MonoBehaviour
{
    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycast;
    private Pose placementPose;
    private bool poseValid = false;

    public GameObject placementIndicator;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycast = arOrigin.GetComponent<ARRaycastManager>();
        //playerArena.SetActive(false);
        //player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        //if (poseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    PlaceObject();
        //}


    }

    //private void PlaceObject()
    //{
    //    playerArena.transform.position = placementPose.position;
    //    playerArena.transform.rotation = placementPose.rotation;
    //    player.transform.position = playerSpawn.transform.position;

    //    playerArena.SetActive(true);
    //    player.SetActive(true);

    //    //Instantiate(playerArena, placementPose.position, placementPose.rotation);
    //    placementIndicator.SetActive(false);

    //    gameManager.SetActive(true);
    //    gameUI.SetActive(true);

    //    //disable interaction
    //    this.enabled = false;
    //}

    private void UpdatePlacementIndicator()
    {
        if (poseValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

        }
        else placementIndicator.SetActive(false);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycast.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        poseValid = hits.Count > 0;

        if (poseValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
