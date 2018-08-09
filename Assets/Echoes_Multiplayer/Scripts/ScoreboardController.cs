using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace Echoes_Multiplayer {

    public class ScoreboardController : NetworkBehaviour
    {
        //Number of remainingPlayers
        [SyncVar(hook = "OnChangePlayers")]
        public int remaingingPlayersVal;
        public Text remainingPlayersText;

        //Check for last person standing (only applies if more than 1 person has joined the game)
        public bool multi = false;

        public override void OnStartClient()
        {
            remainingPlayersText.text = remaingingPlayersVal.ToString() + " Remaining";
        }

        void Update()
        {
            //If more than one person has joined the game, allow winner system to be activated
            if (remaingingPlayersVal >= 2 && multi == false) {
                Debug.Log("Winning enabled");
                multi = true;
            }

            //Set the remaining player value equal to the number of NPC's targets
            if (remaingingPlayersVal != GameObject.Find("true_scarecrow(Clone)").GetComponent<NPCController>().targets.Count) {
                remaingingPlayersVal = GameObject.Find("true_scarecrow(Clone)").GetComponent<NPCController>().targets.Count;
            }

            //If there is only one player remaining during multiplayer...
            if (multi == true && remaingingPlayersVal == 1) {
                //Declare winner
                Debug.Log("Winner!");

                //Get player object's transform
                List<Transform> players = GameObject.Find("true_scarecrow(Clone)").GetComponent<NPCController>().targets;
                GameObject survivor;

                foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                    if (player.name == "Player(Clone)" && player.GetComponent<PlayerController>().dead != true) {
                        survivor = player;
                        //Declare player winner and dead (so NPC stops chasing them)
                        survivor.transform.GetComponent<PlayerController>().winner = true;
                        survivor.transform.GetComponent<PlayerController>().dead = true;

                        //Teleport them to winner's room
                        survivor.transform.position = new Vector3(0, -97.5f, -3);
                    }
            }
        }

        public void OnChangePlayers(int remaingingPlayersVal) {
            remainingPlayersText.text = remaingingPlayersVal.ToString() + " Remaining";
        }

        //void RpcRespawnWinner() {
        //    if (isLocalPlayer)
        //        transform
        //}
    }
}
