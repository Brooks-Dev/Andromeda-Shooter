using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //ID for powerups
    [SerializeField] //0 = triple shot, 1 = speed & 2 = shields
    private int _powerupID;
    [SerializeField]
    private AudioClip _powerupAudio;

    private bool _moveToPlayer = false;
    private GameObject _player;
    
    // Update is called once per frame
    void Update()
    {
        if (_moveToPlayer == true  && _player != null)
        {
            //get vector to player
            Vector2 direction = (Vector2)_player.transform.position - (Vector2)transform.position;
            direction.Normalize();
            //move toward player
            transform.Translate(_speed * Time.deltaTime * new Vector3(direction.x, direction.y, 0f));
        }
        else
        {
            //move down at speed of 3
            transform.Translate(_speed * Time.deltaTime * Vector3.down);
        }
        //destroy when leave the view
        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    //ontriggerCollision
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Only be collected by player using tag
        if (other.CompareTag("Player"))
        {
            //player collects powerup 
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        Debug.Log("Triple shot activated");
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        Debug.Log("Speed boost activated");
                        player.ActivateSpeed();
                        break;
                    case 2:
                        Debug.Log("Shields activated");
                        player.ActivateShield();
                        break;
                    case 3:
                        Debug.Log("Health boost");
                        player.HealthBoost();
                        break;
                    case 4:
                        Debug.Log("Laser power added");
                        player.LaserPower();
                        break;
                    case 5:
                        Debug.Log("Missiles added");
                        player.AddMissiles();
                        break;
                    default:
                        Debug.Log("PowerupID default value");
                        break;
                }
            }
            //play powerup audio clip
            AudioSource.PlayClipAtPoint(_powerupAudio, Camera.main.transform.position);
            //destroy power up
            Destroy(gameObject);
        }
    }

    public void MoveTowardPlayer(GameObject player)
    {
        _moveToPlayer = true;
        _player = player;
    }
}
