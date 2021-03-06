﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Echoes
{
    public class PlayerController : MonoBehaviour
    {
        private GameObject cam_Holder; //holds camera since GVR overrides camera's position
        private Camera[] cams;
        public Camera cam;
        private int cam_counter = 0;

        public float speed = 2.0f;

        public static Echoes.PlayerController Instance;

        private void Awake()
        {
            cams = Camera.allCameras;
            cam_Holder = GameObject.Find("Camera_Holder"); //get parent object of camera
            cam = cams[cam_counter++]; //set new camera
            cam_Holder.transform.position = transform.position; //set position of camera's parent object to player
        }

        void Update()
        {
            //Switch cameras
            foreach (Camera i in cams)
            {
                i.enabled = false;
            }
            cam.enabled = true;

            //Track rotation
            transform.rotation = cam.transform.rotation;

            //If trigger, fire and move
            if (Input.GetMouseButton(0))
            {
                //Calculate where to move
                Vector3 forward = transform.forward;
                forward.y = 0;
                Vector3 newPosition = forward * Time.deltaTime * speed + transform.position;

                //Constrain movement
                newPosition.x = Mathf.Clamp(newPosition.x, -10, 10);
                newPosition.z = Mathf.Clamp(newPosition.z, -10, 10);

                //Update position
                transform.position = newPosition; //move player

                //Update camera (parent) position
                cam_Holder.transform.position = transform.position;
            }
         }
    }
}