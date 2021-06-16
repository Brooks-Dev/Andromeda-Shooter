using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy speed variable
    [SerializeField]
    private float _speed = 4.0f;
    //access player component
    private Player _player;
    private Animator _animator;

    void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player in enemy is null");
        }
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator in enemy is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move enyme down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //if enemy off bottom of the screen then respawn at top with new random x position
        if (transform.position.y < -6.4f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 6.4f, 0);
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        //check to see if collision is with player
        if (other.CompareTag("Player"))
        {
            //damage player
            if (_player != null)
            {
                _player.DamagePlayer();
            }
            //destroy enemy
            EnemyDestroyed();
        }
        //if collision with laser destroy laser and enemy
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            //update player score
            _player.PlayerScore(10);
            //destroy enemy
            EnemyDestroyed();
        }
    }

    private void EnemyDestroyed()
    {
        //turn on explosion animation for enemy
        _animator.SetTrigger("OnEnemyDeath");
        //stops the enmy from moving after death
        _speed = 0;
        //turn off collisions so dead enemy does not destroy player
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        //wait for animation to finish then destroy the enemy game object
        Destroy(gameObject, 2.7f);
    }
}
