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

    void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player in enely is null");
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
            Destroy(gameObject);
        }
        //if collision with laser destroy laser and enemy
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            //update player score
            _player.PlayerScore(10);
            Destroy(gameObject);
        }
    }

}
