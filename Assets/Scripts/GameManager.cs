using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameOver = false;
    
     // Update is called once per frame
    void Update()
    {
        //restart game on pressing R if game is over
        if (Input.GetKeyDown(KeyCode.R) && _gameOver == true)
        {
            // loads the Game scene
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // loads the Game scene
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void GameOver()
    {
        _gameOver = true;
    }
}
