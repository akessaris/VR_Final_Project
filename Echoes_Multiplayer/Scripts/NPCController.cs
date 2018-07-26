using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Echoes_Multiplayer
{
    public class NPCController : NetworkBehaviour
    {
        //Targetted player
        public Transform target;

        //Players
        public List<Transform> players;
        public Transform player1;

        //Speed of NPC
        public float speed = 1;

        private NetworkStartPosition[] spawnPoints;


        // Use this for initialization
        void Start()
        {
            players = new List<Transform>();
            players.Add(player1);
            target = players[0];
        }

        // Update is called once per frame
        void Update()
        {
            float step = speed * Time.deltaTime;

            //Move NPC closer to nearest target
            transform.position = Vector3.MoveTowards(transform.position, findTarget(target).position, step);
            Debug.Log("Target = " + target);
        }

        private Transform findTarget(Transform target)
        {
            //need to find way to access all players
            foreach (Transform player in players)
            {
                if (Vector3.Distance(player.position, transform.position) < Vector3.Distance(target.position, transform.position))
                {
                    target = player;
                }
            }
            return target;
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
                GameObject.Find("Player").transform.position = spawnPoint;
                GameObject.Find("Camera_Holder").transform.position = spawnPoint;
            }
        }
    }
}
