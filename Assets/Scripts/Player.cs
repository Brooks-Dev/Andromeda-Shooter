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


    //shield level, max 3
    private int _shields = 0;
    //shield visualizer
    [SerializeField]
    private GameObject[] _playerShield;
    
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

    private int _ammo = 15;
    [SerializeField]
    private AudioClip _powerLowClip;
    [SerializeField]
    private AudioClip _powerDepletedClip;
    private bool _audioWarningOn = false;

    private GameObject _thruster;

    private GameObject _afterburner;

    private bool _afterburnersOn = false;

    private MainCamera _mainCamera;

    //thruster duration and overheats
    [SerializeField]
    private float _afterburnDuration = 0.5f;
    //time when engine coolsdown after excessive afterburn
    private float _canAfterburn = 0;
    //float for afterburner energy
    private float _afterburnEnergy = 1.0f;

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
        if (Input.GetKey(KeyCode.LeftShift)  && Time.time > _canAfterburn)
        {
            //if afterburners turned on give speed boost and change VFX
            if (_afterburnersOn == false)
            {
                _velocityMultiplier *= 2f;
                _afterburner.SetActive(true);
                _thruster.SetActive(false);
                _afterburnersOn = true;
            }
            //decrease energy on afterburner use
            _afterburnEnergy -= Time.deltaTime/_afterburnDuration;
            _afterburnEnergy = Mathf.Clamp(_afterburnEnergy, 0f, 1f);
            //check for overheating afterburner
            if (_afterburnEnergy <= 0)
            {
                _canAfterburn = Time.time + _afterburnDuration;
            }
            //update UI afterburner energy
            _uiManager.UpdateThrusterEnergy(Time.deltaTime / _afterburnDuration);
        }
        else
        {
            //if afterburners turned off stop speed boost and change VFX
            if (_afterburnersOn == true)
            {
                _velocityMultiplier /= 2f;
                _afterburnersOn = false;
                _afterburner.SetActive(false);
                _thruster.SetActive(true);
            }
            //only recharge afterburner if not overheated
            if (_canAfterburn < Time.time)
            {
                _afterburnEnergy += Time.deltaTime / _afterburnDuration;
                _afterburnEnergy = Mathf.Clamp(_afterburnEnergy, 0f, 1f);
                _uiManager.UpdateThrusterEnergy(-Time.deltaTime / _afterburnDuration);
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
        //plays laser shot audio clip based upon ammo levels
        if (_ammo <= 0)
        {
            if (_audioWarningOn == false)
            {
                _playerAudio.clip = _powerDepletedClip;
                StartCoroutine(AudioWarning());
            }
            return;
        }
        else if (_ammo == 1)
        {
            _playerAudio.Stop();
            _playerAudio.clip = _powerDepletedClip;
            StartCoroutine(AudioWarning());
        }
        else if (_ammo <= 5)
        {
            if (_audioWarningOn == false)
            {
                _playerAudio.clip = _powerLowClip;
                StartCoroutine(AudioWarning());
            }
        }
        //set laser cool down time
        _canFire = Time.time + _fireRate;
        //check if triple shot is active
        if (_isTripleShotActive == true)
        {
            //spawn three lasers at player position with no offset  from player
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            //triple shot does not cost ammo
        }
        else
        {
            //spawn a laser at player position with an offset, 0.75,  from player
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity);
            // deplete ammo when laser fired
            _ammo--;
        }
        _uiManager.UpdateAmmo(_ammo);
        _playerAudio.PlayOneShot(_laserShotClip);
    }

    public void LaserPower()
    {
        _ammo = 15;
        _uiManager.UpdateAmmo(_ammo);
    }
    
    public void ActivateTripleShot()
    {
        _ammo = 15;
        _uiManager.UpdateAmmo(_ammo); 
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
        if (_shields < 3)
        {
            _shields++;
        }
        //turn player shield on
        for (int i = 0; i < _shields; i++)
        {
            _playerShield[i].SetActive(true);
        }
    }

    public void PlayerScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreUI(_score);
    }
    
    public void DamagePlayer()
    {
        //is player shield on?
        if (_shields > 0)
        {
            // turn player shield off
            _playerShield[_shields-1].SetActive(false);
            _shields--;
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

    private IEnumerator AudioWarning()
    {
        Debug.Log("Audio warning.");
        _playerAudio.Play();
        _audioWarningOn = true;
        yield return new WaitForSeconds(2.5f);
        _audioWarningOn = false;
    }
}
