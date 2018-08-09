using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace Echoes_Multiplayer
{
    public class Lives : NetworkBehaviour
    {

        public const int maxLives = 3;
        public bool destroyOnDeath;

        //[SyncVar(hook = "OnChangeLives")]
		public int currentLives = maxLives;

        public Text livesText;

        public override void OnStartClient()
        {
            livesText.text = currentLives.ToString();
        }

        private void Update()
        {
            if (livesText.text != currentLives.ToString())
                livesText.text = currentLives.ToString();
        }

       
        public void LoseALife(int amount)
        {
            currentLives -= amount;

			if (currentLives <= 0)
            {
                if (destroyOnDeath)
                {
                    //RpcEliminate();
                }
                else
                {
					//Reset the player's lives
                    currentLives = maxLives;
                }
            }
        }
    }
}
