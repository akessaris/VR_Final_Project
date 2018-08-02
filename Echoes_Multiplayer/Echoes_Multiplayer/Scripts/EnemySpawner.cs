using UnityEngine;
using UnityEngine.Networking;

namespace Echoes_Multiplayer
{
    public class EnemySpawner : NetworkBehaviour
    {

        public GameObject enemyPrefab;

        public override void OnStartServer()
        {
            if (!isServer) return;
            var spawnPosition = new Vector3(0, 0, 0);
            var spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }
}