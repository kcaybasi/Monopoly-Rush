using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveLoadManager : MonoBehaviour
{
    private int _currentSceneIndex;
    private int _sceneToContinue;
 


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    
    }
    void Start()
    {
       
        ContinueGame();

    }


    private void ContinueGame()
    {
        _sceneToContinue = PlayerPrefs.GetInt("SavedScene");

        if (_sceneToContinue != 0)
        {
            SceneManager.LoadScene(_sceneToContinue);
            _sceneToContinue = 0;

        }
 
    }


    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("SavedScene", _currentSceneIndex);
        }
    }
}
