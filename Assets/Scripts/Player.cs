using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player velocity in meters per second
    private float _velocity = 3.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //grap vertical and horizontal inputs
        float verticalInput = Input.GetAxis("Vertical"), horizontalInput = Input.GetAxis("Horizontal");

        //move player in the horizontal (x) & vertical (y) axis with set speed in real time
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _velocity * Time.deltaTime);

        //check vertical (y) boundary violation
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(transform.position.x, -5.0f, 0);
        }
        //wrap horizontal(x) on boundary violation
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
}
