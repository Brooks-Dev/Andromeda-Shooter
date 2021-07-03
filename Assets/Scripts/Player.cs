using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player velocity in meters per second
    [SerializeField]
    private float _velocity = 3.5f;
    //speed boost multiplier
    private float _velocityMultiplier = 1.0f;
    //laser prefab object
    [SerializeField]
    private GameObject _laserPrefab;
    //cool down for laser fire in seconds
    [SerializeField]
    private float _fireRate = 0.25f;
    //time index for laser cooldown in seconds
    private float _canFire;
    //player lives variable
    [SerializeField]
    private int _lives = 3;
    //access spawn manager script
    private SpawnManager _spawnManager;
    //triple shot active
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _tripleShotDuration = 4.0f;
    [SerializeField]
    private float _speedDuration = 4.0f;
    //shield active
    private bool _isShieldActive = false;
    //shield visualizer
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private int _score = 0;
    //access spawn manager
    private UIManager _uiManager;
    [SerializeField]
    private GameObject[] _engines;
    [SerializeField]
    private AudioClip _laserShotClip;
    private AudioSource _playerAudio;
    [SerializeField]
    private GameObject _explosionPrefab;

    private GameObject _thruster;

    private GameObject _afterburner;

    private bool _afterburnersOn = false;

    private MainCamera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)  
        {
            Debug.LogError("Spawn manager in player is null.");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null in player.");
        }
        transform.position = new Vector3(0, 0, 0);
        _playerAudio = GetComponent<AudioSource>();
        if (_playerAudio == null)
        {
            Debug.LogError("Laser shot audio source is null in player.");
        }
        else
        {
            _playerAudio.clip = _laserShotClip;
        }
        _thruster = gameObject.transform.Find("Thruster").gameObject;
        if (_thruster == null)
        {
            Debug.LogError("Thruster is null in player.");
        }
        _afterburner = gameObject.transform.Find("Afterburner").gameObject;
        if (_afterburner == null)
        {
            Debug.LogError("Afterburner is null in player.");
        }
        _mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        if (_mainCamera == null)
        {
            Debug.LogError("Main camera is null in player.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_afterburnersOn == false)
            {
                _velocityMultiplier *= 2f;
                _afterburner.SetActive(true);
                _thruster.SetActive(false);
                _afterburnersOn = true;
            }
        }
        else
        {
            if (_afterburnersOn == true)
            {
                _velocityMultiplier /= 2f;
                _afterburnersOn = false;
                _afterburner.SetActive(false);
                _thruster.SetActive(true);
            }
        }

        CalculateMovement();

        //fire laser on pressing space key
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireWeapons();
        }
    }
    void FireWeapons()
    {
        //set laser cool down time
        _canFire = Time.time + _fireRate;
        //check if triple shot is active
        if (_isTripleShotActive == true)
        {
            //spawn three lasers at player position with no offset  from player
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            //spawn a laser at player position with an offset, 0.75,  from player
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity);
        }
        //plays laser shot audio clip
        _playerAudio.Play();
    }
    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(InactivateTripleShot());
    }

    IEnumerator InactivateTripleShot()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        Debug.Log("Triple shot powerup is inactive");
        _isTripleShotActive = false;
    }

    void CalculateMovement()
    {
        //grap vertical and horizontal inputs
        float verticalInput = Input.GetAxis("Vertical"), horizontalInput = Input.GetAxis("Horizontal");

        //move player in the horizontal (x) & vertical (y) axis with set speed in real time
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _velocity * _velocityMultiplier * Time.deltaTime);

        //check vertical (y) boundary violation
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-5.0f, 0) , 0);

        //wrap horizontal(x) on boundary violation
        if (transform.position.x > 11.0f)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.0f)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
    }

    public void ActivateSpeed()
    {
        _velocityMultiplier *= 2f;
        StartCoroutine(InactivateSpeed());
    }

    IEnumerator InactivateSpeed()
    {
        yield return new WaitForSeconds(_speedDuration);
        Debug.Log("Speed boost is inactive");
        _velocityMultiplier /= 2f;
    }

    public void ActivateShield()
    {
        //turn player shield on
        _isShieldActive = true;
        _playerShield.SetActive(true);
    }

    public void PlayerScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreUI(_score);
    }
    
    public void DamagePlayer()
    {
        //is player shield on?
        if (_isShieldActive == true)
        {
            // turn player shield off
            _isShieldActive = false;
            _playerShield.SetActive(false);
            //avoid damage
            return;
        }
        else
        {
            //remove a life
            _lives--;
            if (_lives == 2)
            {
                _engines[Random.Range(0, 2)].SetActive(true);
            }
            else
            {
                _engines[0].SetActive(true);
                _engines[1].SetActive(true);
            }
            _uiManager.UpdateLives(_lives);
        }

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        _mainCamera.CameraShake();
    }

    public void HealthBoost()
    {
        //add one life to player, not to exceed 3 lives
        _lives ++;
        if(_lives >= 3)
        {
            _lives = 3;
            _engines[0].SetActive(false);
            _engines[1].SetActive(false);
        }
        else
        {
            _engines[Random.Range(0, 2)].SetActive(false);
        }
        _uiManager.UpdateLives(_lives);
    }
}
