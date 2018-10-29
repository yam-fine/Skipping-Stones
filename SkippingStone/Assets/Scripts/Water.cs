using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Water : MonoBehaviour {

    [SerializeField]
    List<GameObject> obstacles;
    [SerializeField]
    List<GameObject> skyObstacles;

    Transform[] spawns;
    Transform[] skySpawns;
    int chanceOfObjectSpawn = 3;    

    private void Start()
    {
        spawns = gameObject.FindChildGameObjectsWithTag<Transform>("SpawnPoint");
        skySpawns = gameObject.FindChildGameObjectsWithTag<Transform>("SkySpawnPoint");
    }

    public void ChangeSpawnPoints()
    {
        foreach (Transform spawn in spawns)
        {
            if (Random.Range(1, chanceOfObjectSpawn + 1) == 1)
            {
                spawn.gameObject.SetActive(true);
                SpawnObstacle(spawn, obstacles);
            }
            else
                spawn.gameObject.SetActive(false);
        }

        foreach (Transform spawn in skySpawns)
        {
            if (Random.Range(1, chanceOfObjectSpawn + 1) == 1)
            {
                spawn.gameObject.SetActive(true);
                SpawnObstacle(spawn, skyObstacles);
            }
            else
                spawn.gameObject.SetActive(false);
        }
    }

    void SpawnObstacle(Transform pos, List<GameObject> list)
    {
        GameObject obj = list[Random.Range(0, list.Count)];

        LeanPool.Spawn(obj, pos.position, Quaternion.Euler(Vector3.forward));
    }
}