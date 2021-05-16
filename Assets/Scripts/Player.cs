using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player velocity in meters per second
    private float _velocity = 3.5f;
    //laser prefab object
    [SerializeField]
    private GameObject _laserPrefab;
    //cool down for laser fire in seconds
    private float _fireRate = 0.25f;
    //time index for laser cooldown in seconds
    private float _canFire;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        //fire laser on pressing space key
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireWeapons();
        }
    }

    void CalculateMovement()
    {
        //grap vertical and horizontal inputs
        float verticalInput = Input.GetAxis("Vertical"), horizontalInput = Input.GetAxis("Horizontal");

        //move player in the horizontal (x) & vertical (y) axis with set speed in real time
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _velocity * Time.deltaTime);

        //check vertical (y) boundary violation
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-5.0f, 0) , 0);

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

    void FireWeapons()
    {
        //set laser cool down time
        _canFire = Time.time + _fireRate;
        //spawn a laser at player position with an offset, 0.75,  from player
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
    }
}
