using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerupsPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _powerupSpawnRate = 15.6f;
    [SerializeField]
    private float _enemySpawnRate = 3f;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnAmmoPowerups());
    }

    //spawn game objects every five seconds at top of play area and random X position
    IEnumerator SpawnPowerups()
    {
        while (_stopSpawning == false)
        {
            float delay = Random.Range(_powerupSpawnRate * 2f / 3f, _powerupSpawnRate * 4f / 3f);
            yield return new WaitForSeconds(delay);
            int powerupID;
            int random = Random.Range(0, 5);
            if (random <= 1)
            {
                //speed boost power up
                powerupID = 1;
            }
            else if (random > 1 && random <= 3)
            {
                //shield powerup
                powerupID = 2;
            }
            else
            {
                //health powerup
                powerupID = 3;
            }
            float randomX = Random.Range(-9.5f, 9.5f);
            if (_powerupsPrefab[powerupID] != null)
            {
                Instantiate(_powerupsPrefab[powerupID], new Vector3(randomX, 6.4f, 0), Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnAmmoPowerups()
    {
        while (_stopSpawning == false)
        {
            float delay = Random.Range(_powerupSpawnRate * 2f / 3f, _powerupSpawnRate * 4f / 3f); 
            yield return new WaitForSeconds(delay); int powerupID;
            int random = Random.Range(0, 4);
            if (random == 0)
            {
                //triple shot power up
                powerupID = 0;
            }
            else if (random > 0 && random <= 2)
            {
                //laser power powerup
                powerupID = 4;
            }
            else
            {
                //missile powerup
                powerupID = 5;
            }

            float randomX = Random.Range(-9.5f, 9.5f);
            if (_powerupsPrefab[powerupID] != null)
            {
                Instantiate(_powerupsPrefab[powerupID], new Vector3(randomX, 6.4f, 0), Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3.0f);
        float wait = _enemySpawnRate;
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 6.4f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(wait);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
