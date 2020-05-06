using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class TapToPlace : MonoBehaviour
{
    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycast;
    private Pose placementPose;
    private bool poseValid = false;
    AudioSource audio;

    public GameObject placementIndicator;
    public GameObject playerArena;
    public GameObject player;
    public GameObject playerSpawn;
    public GameObject gameManager;
    public GameObject gameUI;
    public AudioClip placeClip;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        audio = GetComponent<AudioSource>();
        arRaycast = arOrigin.GetComponent<ARRaycastManager>();
        playerArena.SetActive(false);
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        
        if (poseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }


    }

    private void PlaceObject()
    {
        playerArena.transform.position = placementPose.position;
        playerArena.transform.rotation = placementPose.rotation;
        player.transform.position = playerSpawn.transform.position;

        playerArena.SetActive(true);
        player.SetActive(true);

        //Instantiate(playerArena, placementPose.position, placementPose.rotation);
        placementIndicator.SetActive(false);

        gameManager.SetActive(true);
        gameUI.SetActive(true);

        audio.clip = placeClip;
        audio.Play();

        //disable interaction
        this.enabled = false;
    }

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

        Debug.Log(hits.Count);

        if (poseValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
