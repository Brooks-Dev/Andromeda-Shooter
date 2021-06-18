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
    
    // Update is called once per frame
    void Update()
    {
        //move down at speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
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
}
