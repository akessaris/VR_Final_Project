using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

namespace Echoes
{
    public class Generate_Forest: NetworkBehaviour
    {
        public GameObject treePrefab;
        public int tree_count;

        private void Start()
        {
            for (var i = 0; i < tree_count; i++)
            {
                
                //create tree prefab at random location and scale
                var x = Random.Range(-50, 50);
                var z = Random.Range(-50, 50);
                var scale = Random.Range(1,3);


                var spawnPosition = new Vector3(x, 0.0f, z);

                var tree = (GameObject)Instantiate(treePrefab, spawnPosition, Quaternion.Euler(0f,0f,0f));
                NetworkServer.Spawn(tree);
            }
        }

    }
}
