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
    private float _powerupSpawnRate = 12f;
    [SerializeField]
    private float _enemySpawnRate = 3f;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    //spawn game objects every five seconds at top of play area and random X position
    IEnumerator SpawnPowerups()
    {
        while (_stopSpawning == false)
        {
            int powerupID = Random.Range(0, 3);
            float delay = Random.Range(_powerupSpawnRate*2f/3f, _powerupSpawnRate*4f/3f);
            float randomX = Random.Range(-9.5f, 9.5f);
            if (_powerupsPrefab[powerupID] != null)
            {
                Instantiate(_powerupsPrefab[powerupID], new Vector3(randomX, 6.4f, 0), Quaternion.identity);
            }
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator SpawnEnemies()
    {
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
