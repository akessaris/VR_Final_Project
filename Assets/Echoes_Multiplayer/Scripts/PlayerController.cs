using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Echoes_Multiplayer
{
    public class PlayerController : NetworkBehaviour
    {
        private GameObject cam_Holder; //holds camera since GVR overrides camera's position
        private Camera[] cams;
        private Camera cam;
        private int cam_counter = 0;
        private GameObject NPC;

        public static Echoes_Multiplayer.PlayerController Instance;

        private Rigidbody rb;
        public float speed = 5.0f;
        public bool dead = false;
        public bool winner = false;

        //Networking
        private NetworkStartPosition[] spawnPoints;

        public GameManager gameManager;

        public Text idText;

        [SyncVar(hook = "OnPlayerIdChange")]
        public int playerId;

        // if you have a SyncVar hook, the variable itself is not changed directly;
        // instead you get the new value in the hook function and need to set the var yourself
        void OnPlayerIdChange(int id)
        {
            playerId = id;
            //Debug.Log("Player id set on player " + playerId);

            //and now set my text label so I can show my id
            //idText.text = playerId.ToString();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            //Debug.Log("Get GameManager on Server");

            // Wait until the Game Manager object spawns and then grab a handle to it
            while (gameManager == null)
            {
                GameObject temp = GameObject.Find("Game Manager");
                if (temp != null)
                    gameManager = temp.GetComponent<GameManager>();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            // this is a non-local player object.  it has received an id from the server;
            // need to set my text to show it.
            //idText.text = playerId.ToString();

            //Debug.Log("Client non-local player started with id: " + playerId);
        }

        public override void OnStartLocalPlayer()
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.blue); //set color of local player to blue
            cam = cams[cam_counter++]; //set new camera
            cam_Holder.transform.position = transform.position; //set position of camera's parent object to player
        
            //Debug.Log("Get GameManager on Player");
            // Wait until the Game Manager object spawns and then grab a handle to it
            while (gameManager == null)
            {
                GameObject temp = GameObject.Find("Game Manager");
                if (temp != null)
                    gameManager = temp.GetComponent<GameManager>();
            }

            // Tell server a new player has started
            CmdIncrPlayerId();
        }

        // This Increments the player count on the server
        [Command]
        void CmdIncrPlayerId()
        {
            //Debug.Log("Incrementing lastPlayerId");
            gameManager.AddNewPlayer();   // this increments lastPlayerId and adds an entry in the scoreboard array

            // Here I am letting the server send the new value via the syncvar
            playerId = gameManager.lastPlayerId;

            // Alternately I can call an Rpc method on the client and send it the player id
            //            RpcSetId(gameManager.lastPlayerId);
        }

        // this is alternate to using syncvar: can use RPC to set value on each client
        [ClientRpc]  // called on the Server, but invoked on the Clients
        void RpcSetId(int id)
        {
            //Debug.Log("Player got rpcsetid = " + id);
            playerId = id;

            // then I need to set my local text label to show the id
            idText.text = playerId.ToString();
        }

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
            // Reset momentum of all players
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            if (!isLocalPlayer) return;

            //Switch cameras
            foreach (Camera i in cams) {
                i.enabled = false;
            }
            cam.enabled = true;

            //Track rotation
            transform.rotation = cam.transform.rotation;

            if (winner)
            {
                transform.rotation = cam.transform.rotation;
                cam_Holder.transform.position = transform.position;
                return;
            }

            //If trigger, move
            if (Input.GetMouseButton(0))
            {
                //Calculate where to move
                Vector3 forward = transform.forward;
                forward.y = 0;
                Vector3 newPosition = forward * Time.deltaTime * 5.0f + transform.position;

                //Constrain movement
                newPosition.x = Mathf.Clamp(newPosition.x, -75, 75);
                newPosition.z = Mathf.Clamp(newPosition.z, -75, 75);

                //If the player has won, keep them in the winner's box
                if (winner) {
                    newPosition.x = Mathf.Clamp(newPosition.x, -75, 75);
                    newPosition.z = Mathf.Clamp(newPosition.z, -75, 75);
                    newPosition.y = -100;
                }
                //If the player is dead, keep them at "spectator mode" height
                else if (dead)
                    newPosition.y = 100;
                //Otherwise, keep them on ground level
                else
                    newPosition.y = 0;

                //Update position
                transform.position = newPosition; //move player
            }
            //Prevent height changes even when not moving
            else {
                float height;

                //If the player has won, keep them in the winner's box
                if (winner)
                    height = -100;
                //If the player is dead, keep them at "spectator mode" height
                else if (dead)
                    height = 100;
                //Otherwise, keep them on ground level
                else
                    height = 0;

                //Make sure player is kept on ground level
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
            }
            //Update camera (parent) position
            cam_Holder.transform.position = transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            //If localPlayer collides with the enemy, respawn the player
            if (collision.gameObject.name == "true_scarecrow(Clone)") {
                var lives = GetComponent<Lives>();
                if (GetComponent<Lives>() != null) {
                    RpcRespawn();
                    lives.LoseALife(1);

                    if (lives.currentLives <= 0) {
                        RpcEliminate();
                    }
                    //CmdLoseLife();
                }
            }
        }

        [Command]
        void CmdLoseLife() {
            gameManager.LoseLife(playerId);
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
            }
        }

        void RpcEliminate()
        {
            if (isLocalPlayer)
            {
                //Declare the player dead
                GetComponent<PlayerController>().dead = true;

                //Put them in "spectator mode"
                transform.position = new Vector3(0, 100, 0);

                //Remove them from list of targets
                GameObject.Find("true_scarecrow(Clone)").GetComponent<NPCController>().targets.Remove(gameObject.transform);
            }
        }
    }
}
