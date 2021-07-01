using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //laser velocity variable in meters per second
    [SerializeField]
    private float _speed = 8.0f;

    // Update is called once per frame
    void Update()
    {
        //move laser upward
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        //if laser is outside field of play, i.e. y > 6.3 then destroy laser
        if(transform.position.y > 7.6f | transform.position.y < -6.2f )
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CompareTag("EnemyLaser") && other.CompareTag("Player"))
        {
            other.GetComponent<Player>().DamagePlayer();
        }
    }
}
