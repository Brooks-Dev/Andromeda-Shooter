using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    
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
                player.ActivateTripleShot();
            }
            //destroy power up
            Destroy(gameObject);
        }
    }
}
