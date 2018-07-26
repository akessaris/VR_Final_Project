using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

namespace Echoes_Multiplayer
{
    public class PlayerController : NetworkBehaviour, MonoBehaviour
    {
        private GameObject cam_Holder; //holds camera since GVR overrides camera's position
        private Camera[] cams;
        private Camera cam;
        private int cam_counter = 0;
        private GameObject NPC;

        public static Echoes_Multiplayer.PlayerController Instance;

        private void Awake()
        {
            //Singleton
            if (isLocalPlayer)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            cams = Camera.allCameras;
            cam_Holder = GameObject.Find("Cam_Holder"); //get parent object of camera
            GameObject.Find("Enemy").GetComponent<PlayerScript>().players.Add();
        }

        void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            //Switch cameras
            foreach (Camera i in cams) {
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
                Vector3 newPosition = forward * Time.deltaTime * 5.0f + transform.position;

                //Constrain movement
                newPosition.x = Mathf.Clamp(newPosition.x, -10, 10);
                newPosition.z = Mathf.Clamp(newPosition.z, -10, 10);

                //Update position
                transform.position = newPosition; //move player
            }
            //Update camera (parent) position
            cam_Holder.transform.position = transform.position;
        }

        public override void OnStartLocalPlayer()
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.blue); //set color of local player to blue
            cam = cams[cam_counter++]; //set new camera
            cam_Holder.transform.position = transform.position; //set position of camera's parent object to player
        }
    }
}
