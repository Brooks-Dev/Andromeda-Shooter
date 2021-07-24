using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //laser velocity variable in meters per second
    [SerializeField]
    private float _speed = 8.0f;
    private Vector3 _laserTargetVector = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        //move laser upward
        transform.position += _speed * Time.deltaTime * (new Vector3(_laserTargetVector.x, _laserTargetVector.y, 0f));

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
        Debug.Log("laser collision detected");
        Debug.Log("Other object: " + other.tag);
        Debug.Log("This object: " + this.tag);
        if (this.CompareTag("EnemyLaser") && other.CompareTag("Player"))
        {
            Debug.Log("player collision detected with enemy laser");
            Player player = other.GetComponent<Player>();
            if (player != null) 
            {
                Debug.Log("call damage player from enemy laser");
                player.DamagePlayer(); 
            }
            else
            {
                Debug.Log("player is null in laser");
            }
            Destroy(gameObject);
        }
    }

    public void AimedLaser(Vector3 target)
    {
        _laserTargetVector = (transform.position-target).normalized;
    }
}
