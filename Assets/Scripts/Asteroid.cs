using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //rotation of asteroid in degrees per second
    [SerializeField]
    private float _rotation = -15f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager in asteroid is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward * _rotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _spawnManager.StartSpawning();
            GameObject _asteroidExlosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(_asteroidExlosion, 2.5f);
            gameObject.SetActive(false);
            Destroy(gameObject, 2.5f);
        }
    }
}
