using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Echoes_Multiplayer
{
    public class PlayerController1 : MonoBehaviour
    {
        private GameObject cam_Holder; //holds camera since GVR overrides camera's position
        private Camera[] cams;
        public Camera cam;
        private int cam_counter = 0;

        public float speed = 2.0f;

        public static Echoes.PlayerController Instance;

        private Rigidbody rb;

        private void Awake()
        {
            cams = Camera.allCameras;
            cam_Holder = GameObject.Find("Camera_Holder"); //get parent object of camera
            cam = cams[cam_counter++]; //set new camera
            cam_Holder.transform.position = transform.position; //set position of camera's parent object to player
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
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

            //=========== CASEY CHANGES HERE
            rb.velocity = transform.forward * 0;
            transform.rotation = cam.transform.rotation;
            cam_Holder.transform.position = transform.position;
            //If trigger, fire and move
            if (Input.GetMouseButton(0))
            {
                //Calculate where to move
                Vector3 forward = transform.forward;
                forward.y = 0;
                //Vector3 newPosition = forward * Time.deltaTime * speed + transform.position;
                rb.velocity = forward * speed;
                //Constrain movement
                // newPosition.x = Mathf.Clamp(newPosition.x, -15, 15);
                //newPosition.z = Mathf.Clamp(newPosition.z, -15, 15);

                //Update position
                // transform.position = newPosition; //move player

                //Update camera (parent) position

            }
            //=========== END CASEY CHANGES HERE
        }
    }
}