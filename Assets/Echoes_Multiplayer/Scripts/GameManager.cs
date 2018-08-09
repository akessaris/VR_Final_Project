using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Echoes_Multiplayer
{
    public class GameManager : NetworkBehaviour
    {

        [SyncVar]
        public int lastPlayerId = 0;

        [SyncVar]
        public int captures = 0;

        // SyncLists are SyncVars for lists.  There are several different types.
        // They do not need a SyncVar attribute tag.
        public SyncListInt players = new SyncListInt();
        //public List<Transform> playerTransforms = new List<Transform>;

        // local UI text fields to display data
        public Text LastIdText;
        public Text ScorebdText;


        public override void OnStartServer()
        {
            base.OnStartServer();
        }


        // Update is called once per frame
        // This runs on all clients
        void Update()
        {
            // Update the information display

            LastIdText.text = "Remaining players: " + lastPlayerId.ToString();

            // Display scores for each player
            string board = "Scoreboard\n";
            for (int i = 0; i < players.Count; i++)
                board += "P" + i.ToString() + " Lives: " + players[i].ToString() + "\n";
            
            ScorebdText.text = board;
        }

        // Increment the global count of button clicks, and also the score for the given player
        // this is called from a Command by the player, so it runs as server
        public void LoseLife(int playerId)
        {
            captures++;

            // decrement count for the given player
            players[playerId]--;

            Debug.Log("Player " + playerId + " out of " + lastPlayerId + " players lost a life. Remaining lives = " + players[playerId]);

            //If player is dead, decrease player count
            if (players[playerId] <= 0) {
                lastPlayerId--;
            }

            //if (players[playerId] <= 0) {
            //    RpcEliminate();
            //}
        }

        [ClientRpc]
        void RpcEliminate()
        {
            //if (isLocalPlayer)
            //{
            //    //Declare the player dead
            //    GetComponent<PlayerController>().dead = true;

            //    //Put them in "spectator mode"
            //    transform.position = new Vector3(0, 100, 0);

            //    //Remove them from list of targets
            //    GameObject.Find("true_scarecrow(Clone)").GetComponent<NPCController>().targets.Remove(gameObject.transform);
            //}
        }

        // this increments lastPlayerId and adds an entry in the scoreboard array
        // this is called from a Command by the player, so it runs as server
        public void AddNewPlayer()
        {
            lastPlayerId++; //increment player count
            players.Add(3); //add player's lives to list

            //playerTransforms.Add(player); //add player transform to list of player transforms
            //Debug.Log("Added new player: " + lastPlayerId + " now players len = " + players.Count);
        }

        public SyncListInt getPlayerLives()
        {
            return players;
        }
    }
}
