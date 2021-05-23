using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawnDelay = 5.0f;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies(_spawnDelay));
    }

    //spawn game objects every five seconds at top of play area and random X position
    IEnumerator SpawnEnemies(float wait)
    {
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
