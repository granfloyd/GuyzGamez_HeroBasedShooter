using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawning : NetworkBehaviour
{
    public GameObject enemyPrefab;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            StartCoroutine(SpawnAndDestroyEnemies());
        }
    }

    IEnumerator SpawnAndDestroyEnemies()
    {
        while (true)
        {
            for (int i = 0; i < 1; i++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(i * 1.5f, 0, 0);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemy.GetComponent<NetworkObject>().Spawn();
                spawnedEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(10);

            foreach (GameObject enemy in spawnedEnemies)
            {
                Destroy(enemy);
            }

            spawnedEnemies.Clear();
        }
    }
}
