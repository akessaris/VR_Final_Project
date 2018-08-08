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

        //NOTE: this is the radius of the fence so trees don't spawn on the fence
        public int fence_radius;

        //NOTE: this is the total radius trees will be spawned
        public int total_radius;

        //NOTE: These are the x and z values of the spawn location
        public int[] x_locs;
        public int[] z_locs;

        private void Start()
        {
            
            if (!isLocalPlayer)
            {
                for (var i = 0; i < tree_count; i++)
                {
                    //create tree prefab at random location and scale
                    //NOTE: change this dependednt on map size
                    var x = Random.Range(-total_radius, total_radius);
                    var z = Random.Range(-total_radius, total_radius);

                    bool valid_coordinates = true;
                    foreach (int x_in in x_locs)
                    {
                        if (Mathf.Abs(x_in - x) < 1)
                        {
                            valid_coordinates = false;
                        }
                    }
                    foreach (int z_in in z_locs)
                    {
                        if (Mathf.Abs(z_in - z) < 1)
                        {
                            valid_coordinates = false;
                        }
                    }


                    // Note: this is where you edit the distance of the fences from the center.
                    if (Mathf.Abs(z - (0 - fence_radius)) < 1)
                    {
                        valid_coordinates = false;
                    }
                    if (Mathf.Abs(z - fence_radius) < 1)
                    {
                        valid_coordinates = false;
                    }
                    if (Mathf.Abs(x - (0 - fence_radius)) < 1)
                    {
                        valid_coordinates = false;
                    }
                    if (Mathf.Abs(x - fence_radius) < 1)
                    {
                        valid_coordinates = false;
                    }

                    if (valid_coordinates)
                    {
                        var scale = Random.Range(1, 3);

                        var spawnPosition = new Vector3(x, 0.0f, z);

                        var tree = (GameObject)Instantiate(treePrefab, spawnPosition, Quaternion.Euler(0f, 0f, 0f));
                        tree.transform.localScale *= scale;
                        NetworkServer.Spawn(tree);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

    }
}
