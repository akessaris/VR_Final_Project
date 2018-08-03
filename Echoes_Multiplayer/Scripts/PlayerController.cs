using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

namespace Echoes_Multiplayer
{
    public class PlayerController : NetworkBehaviour
    {
        private GameObject cam_Holder; //holds camera since GVR overrides camera's position
        private Camera[] cams;
        private Camera cam;
        private int cam_counter = 0;
        private GameObject NPC;

        private NetworkStartPosition[] spawnPoints;
        public static Echoes_Multiplayer.PlayerController Instance;

        private Rigidbody rb;
        public float speed = 5.0f;

        private void Awake()
        {
            //Singleton
            if (isLocalPlayer)
                Destroy(this);
            else
                Instance = this;
            cams = Camera.allCameras;
            cam_Holder = GameObject.Find("Cam_Holder"); //get parent object of camera
        }

        void Start()
        {
            if (isLocalPlayer) {
                spawnPoints = FindObjectsOfType<NetworkStartPosition>();
                rb = GetComponent<Rigidbody>();
            }
        }

        void Update()
        {
            if (!isLocalPlayer) return;

            //Switch cameras
            foreach (Camera i in cams) {
                i.enabled = false;
            }
            cam.enabled = true;

            //Track rotation
            transform.rotation = cam.transform.rotation;

            // Reset momentum of player
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //If trigger, move
            if (Input.GetMouseButton(0))
            {
                //Calculate where to move
                Vector3 forward = transform.forward;
                forward.y = 0;
                Vector3 newPosition = forward * Time.deltaTime * 5.0f + transform.position;

                //Constrain movement
                newPosition.x = Mathf.Clamp(newPosition.x, -50, 50);
                newPosition.z = Mathf.Clamp(newPosition.z, -50, 50);

                //Update position
                transform.position = newPosition; //move player
            }
            //Update camera (parent) position
            cam_Holder.transform.position = transform.position;

            //=========== CASEY CHANGES HERE
            //// Reset momentum of player
            //transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //rb.velocity = transform.forward * 0;
            //transform.rotation = cam.transform.rotation;
            //cam_Holder.transform.position = transform.position;
            ////If trigger, fire and move
            //if (Input.GetMouseButton(0))
            //{
            //    //Calculate where to move
            //    Vector3 forward = transform.forward;
            //    forward.y = 0;
            //    //Vector3 newPosition = forward * Time.deltaTime * speed + transform.position;
            //    rb.velocity = forward * speed;
            //    //Constrain movement
            //    // newPosition.x = Mathf.Clamp(newPosition.x, -15, 15);
            //    //newPosition.z = Mathf.Clamp(newPosition.z, -15, 15);

            //    //Update position
            //    // transform.position = newPosition; //move player

            //    //Update camera (parent) position

            //}
            //=========== END CASEY CHANGES HERE
        }

        public override void OnStartLocalPlayer()
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.blue); //set color of local player to blue
            cam = cams[cam_counter++]; //set new camera
            cam_Holder.transform.position = transform.position; //set position of camera's parent object to player
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isLocalPlayer) return;

            //If localPlayer collides with the enemy, respawn the player
            if (collision.gameObject.name == "Enemy(Clone)") {
                RpcRespawn();
            }
        }

        void RpcRespawn()
        {
            if (isLocalPlayer)
            {
                // Set the spawn point to origin as a default value
                Vector3 spawnPoint = Vector3.zero;

                // If there is a spawn point array and the array is not empty, pick one at random
                if (spawnPoints != null && spawnPoints.Length > 0)
                {
                    spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                }
                // Set the player’s position to the chosen spawn point
                transform.position = spawnPoint;

                // Reset momentum of player
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                // Reset NPC orientation
                GameObject.Find("Enemy(Clone)").transform.LookAt(transform);

                //Reset momentum of NPC
                //GameObject.Find("Enemy(Clone)").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                //GameObject.Find("Enemy(Clone)").GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
