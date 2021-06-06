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

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
    }

    public void UpdateScoreUI (int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int CurrentLives)
    {
        _livesImage.sprite = _livesSprites[CurrentLives];
    }
}
