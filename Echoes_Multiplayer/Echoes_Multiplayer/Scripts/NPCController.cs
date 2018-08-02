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
        public float speed = 2;

        void Awake()
        {
            //Initialize list of targets
            targets = new List<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            float step = speed * Time.deltaTime;

            //Check for new targets
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
                if (player.name == "Player(Clone)" && !targets.Contains(player.transform)) {
                    targets.Add(player.transform);
                }
            }

            //Move NPC closer to nearest target (if there is one)
            if (targets.Count != 0 && target != null)
                transform.position = Vector3.MoveTowards(transform.position, findTarget(target).position, step);
            else if (targets.Count != 0 && target == null) {
                target = targets[0];
            }
            transform.LookAt(target);
        }

        public Transform findTarget(Transform target)
        {
            //need to find way to access all players
            foreach (Transform player in targets)
            {
                if (Vector3.Distance(player.position, transform.position) < Vector3.Distance(target.position, transform.position))
                {
                    target = player;
                    transform.LookAt(target);
                }
            }
            return target;
        }
    }
}
