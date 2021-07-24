using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy speed variable
    [SerializeField]
    private float _speed = 4.0f;
    private float _spawnTime;
    [SerializeField]
    private float _frequency;
    private float _phase;

    private float _distanceY;


    private Animator _animator;
    private AudioSource _enemyAudio;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private float _fireRate;
    private float _canFire;

    private Transform _enemyShield;
    [SerializeField]
    private bool _enemyShieldOn = false;

    private int _enemyID = 0;
    //access player component
    private Player _player;
    //access enemy rigidbody
    private Rigidbody2D _rigidBody;
    private float _playerDetectRange = 4f;
    private bool _attackPlayer = false;
    private float _angleChangingSpeed = 180f;

    void Start()
    {
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player in enemy is null");
        }
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody2D is null in enemy");
        }
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator in enemy is null.");
        }

        _enemyAudio = GetComponent<AudioSource>();
        if(_enemyAudio == null)
        {
            Debug.LogError("Audio source in enemy is null");
        }

        _enemyShield = gameObject.transform.Find("Shield");
        if (_enemyShield == null)
        {
            Debug.LogError("Enemy shield is null in Enemy");
        }

        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;

        _spawnTime = Time.time;
        _speed *= Random.Range(0.75f, 1.25f);
        _enemyID = Random.Range(0, 5);
        //20% chance to generate an enemy (ID = 1) that moves horizonatallyif (random == 3)
        if (_enemyID == 1)
        {
            _frequency = Mathf.PI * Random.Range(0.16f, 0.64f);
            _phase = Random.Range(0f, 2f);
        }
        //20% chance to generate an enemy (ID = 2) that has a shield
        else if (_enemyID == 2)
        {
            _enemyShieldOn = true;
            _enemyShield.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check player and rigidbody exist, noting that rigidbody is destroyed for explosion animation
        if (_player != null && _rigidBody != null)
        {
            //if enemyID is 3 and player in range ram player
            if (_enemyID == 3 && Vector3.Distance(_player.transform.position, transform.position) <= _playerDetectRange)
            {
                _attackPlayer = true;
            }
        }
        else
        {
            _attackPlayer = false;
        }
        if (_attackPlayer == true)
        {
            //ram player
            Vector2 direction = _rigidBody.position - (Vector2)_player.transform.position;

            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, -transform.up).z;
            _rigidBody.angularVelocity = _angleChangingSpeed * rotateAmount;
            _rigidBody.velocity = transform.up * -_speed;
        }
        else
        {
            //if enmemyID is 1 then move horizontally
            if (_enemyID == 1)
            {
                _distanceY = _speed * Mathf.Sin(_frequency * Time.time - _spawnTime + _phase) * Time.deltaTime;
            }
            else
            {
                _distanceY = 0f;
            }
            transform.Translate(Vector3.right * _distanceY);
            //move enyme down
            transform.Translate(_speed * Time.deltaTime * Vector3.down);
        }

        //if enemy off bottom of the screen then respawn at top with new random x position
        if (transform.position.y < -6.4f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 6.4f, 0);
        }
        //wrap horizontal(x) on boundary violation
        if (transform.position.x > 11.0f)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.0f)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
        // enemy fire
        if (Time.time >= _canFire && _enemyID < 3)
        {
            //fire laser downward
            GameObject laser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
        if (_player != null)
        {
            //fires enemy laser at player if ID 4 and enemy is behind player on y axis.
            if (Time.time >= _canFire && _enemyID == 4 && _player.transform.position.y > transform.position.y)
            {

                GameObject laser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
                //find vector to player
                Vector3 diff = transform.position - _player.transform.position;
                diff.Normalize();
                //point at player
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                laser.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                //pass player position to enemy laser
                laser.GetComponent<Laser>().AimedLaser(_player.transform.position);
                _canFire = Time.time + _fireRate;
            }
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        //check to see if collision is with player
        if (other.CompareTag("Player"))
        {
            //damage player
            if (_player != null)
            {
                _player.DamagePlayer();
            }
            if (_enemyShieldOn == true)
            {
                _enemyShield.gameObject.SetActive(false);
                _enemyShieldOn = false;
                return;
            }
            //destroy enemy
            EnemyDestroyed();
        }
        //if collision with laser destroy laser and enemy
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            //if shielded, enemy is not destroyed
            if (_enemyShieldOn == true)
            {
                _enemyShield.gameObject.SetActive(false);
                _enemyShieldOn = false;
                return;
            }
            //update player score
            _player.PlayerScore(10);
            //destroy enemy
            EnemyDestroyed();
        }
        //if collision with missle destroy missile and enemy
        else if (other.CompareTag("Missile"))
        {
            Destroy(other.gameObject);
            //if shielded, enemy is not destroyed
            if (_enemyShieldOn == true)
            {
                _enemyShield.gameObject.SetActive(false);
                _enemyShieldOn = false;
                return;
            }
            //update player score
            _player.PlayerScore(10);
            //destroy enemy
            EnemyDestroyed();
        }
    }

    private void EnemyDestroyed()
    {
        //turn on explosion animation for enemy
        _animator.SetTrigger("OnEnemyDeath");
        //plays explosion audio clip
        _enemyAudio.Play();
        //stops the enmy from moving after death
        _speed = 0;
        _attackPlayer = false;
        //turn off collisions so dead enemy does not destroy player
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        //prevent enemy laser from firing after destroyed
        _canFire = Time.time + 3f;
        //wait for animation to finish then destroy the enemy game object
        Destroy(gameObject, 2.7f);
    }
}
