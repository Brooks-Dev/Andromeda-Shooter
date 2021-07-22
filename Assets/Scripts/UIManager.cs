using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartGameText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _missileText;
    [SerializeField]
    private Image _thrusterEnergyImage;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Laser Power: " + 15 + "/" + 15;
        _missileText.text = "Missiles: " + 0 + "/" + 2;
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game manager is null in UI manager.");
        }
    }

    public void UpdateScoreUI (int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateAmmo(int ammo)
    {
        if (ammo == 500)
        {
            _ammoText.text = "Laser Power: " + "\u221E";
            _ammoText.fontSize = 20;
        }
        else
        {
            _ammoText.text = "Laser Power: " + ammo + "/" + 15;
        }
    }

    public void UpdateMissiles(int _missiles)
    {
        _missileText.text = "Missiles: " + _missiles +  "/" + 2;
    }

    public void UpdateThrusterEnergy(float energy)
    {
        _thrusterEnergyImage.fillAmount -= energy;
    }

    public void UpdateLives(int CurrentLives)
    {
        _livesImage.sprite = _livesSprites[CurrentLives];
        if (CurrentLives <= 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        _restartGameText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    private IEnumerator GameOverFlicker()
    {
        //flickers game over text
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
