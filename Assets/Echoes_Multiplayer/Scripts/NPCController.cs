using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Echoes_Multiplayer
{
    public class NPCController : NetworkBehaviour
    {
        //List of possible targets
        public List<Transform> targets;

        //Currently targetted player
        public Transform target;

        //Speed of NPC
        public float speed = 10.0f;

        void Awake()
        {
            if (!isServer) return;

            //Initialize list of targets
            targets = new List<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isServer) return;


            float step = speed * Time.deltaTime;

            //Check for new targets that aren't already dead
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                if (player.name == "Player(Clone)" && !targets.Contains(player.transform) && player.GetComponent<PlayerController>().dead != true)
                    targets.Add(player.transform);
                //If one of the targets has been eliminated...
                else if (player.name == "Player(Clone)" && targets.Contains(player.transform) && player.GetComponent<PlayerController>().dead == true) {
                    //If player was the current target, reset current target
                    if (target == player.transform)
                    {
                        target = null;
                        Debug.Log("reset removed target");
                    }

                    //Remove that player from the list of targets
                    targets.Remove(player.transform);
                    Debug.Log("removed player " + player.GetComponent<PlayerController>().playerId);
                }

            //Debug.Log("Targets = " + targets.Count);


            //Move NPC closer to nearest target (if there is one)
            if (targets.Count != 0 && target != null) {
                transform.position = Vector3.MoveTowards(transform.position, findTarget(target).position, step);
                transform.LookAt(findTarget(target));
            }
            //If target isn't already assigned, pick one from the list of targets
            else if (targets.Count != 0 && target == null) {
                target = targets[0];
                transform.LookAt(target);
            }
        }

        //Dynamically makes whichever player is closer the new target of the NPC
        public Transform findTarget(Transform target)
        {
            foreach (Transform player in targets)
            {
                //If any of the players are closer than the target, switch targets
                if (Vector3.Distance(player.position, transform.position) < Vector3.Distance(target.position, transform.position))
                {
                    target = player;
                    transform.LookAt(target);
                }
            }
            return target;
        }

        public void AddTarget(Transform target) {
            targets.Add(target);
        }

        public void RemoveTarget(Transform target) {
            targets.Remove(target);
        }
    }
}
