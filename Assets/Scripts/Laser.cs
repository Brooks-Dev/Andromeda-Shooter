using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //laser velocity variable in meters per second
    private float _speed = 8.0f;

    // Update is called once per frame
    void Update()
    {
        //move laser upward
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        //if laser is outside field of play, i.e. y > 6.3 then destroy laser
        if(transform.position.y > 6.3f)
        {
            Destroy(gameObject);
        }
    }
}
