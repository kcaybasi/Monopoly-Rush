using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using GameAnalyticsSDK;


public class CGameManager : MonoBehaviour
{
    public static CGameManager Instance;
    
    public List<Collector> collectorList;

    public TextMeshProUGUI playerCashText;
    public int playerCash;

    private readonly List<Score> _scoreList = new List<Score>();
    public List<TextMeshProUGUI> usernameList=new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> scoreTextList=new List<TextMeshProUGUI>();
    public GameObject leaderboard;


    public List<GameObject> inactiveBuildingList;
    [HideInInspector]
    public List<GameObject> activeBuildingList;
    public List<GameObject> totalBuildingList;
    public TextMeshProUGUI activeBuildingCountText;

    public List<GameObject> supplyLineList;
    
    [SerializeField] ParticleSystem playerConfetti;

    int _sceneIndex;
    public static Action OnGameFinish;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Quality Settings
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        
        CreateScoreList(); 
        
        _sceneIndex = SceneManager.GetActiveScene().buildIndex; // Get the current scene index

        // Initialize Facebook and GameAnalytics SDK
        FB.Init();
        GameAnalytics.Initialize();
    }

    private void CreateScoreList()
    {
        for (int i = 0; i < collectorList.Count; i++)
        {
            _scoreList.Add(new Score(collectorList[i].transform.name, 0));
        }
    }
    
    public void UpdateBuildingList(GameObject building)
    {
        inactiveBuildingList.Remove(building); // Remove the building from inactive list
        activeBuildingList.Add(building); // Add the building to active list
        activeBuildingCountText.text = activeBuildingList.Count.ToString("F0")+" / "+(inactiveBuildingList.Count+activeBuildingList.Count).ToString("F0");  //Update building text

        if (!DOTween.IsTweening(activeBuildingCountText.transform))
        {
            activeBuildingCountText.transform.DOPunchScale(Vector3.one, .3f, 5, 0.2f); 
        }
    }

    public void CheckIfGameFinished()
    {
        if (inactiveBuildingList.Count <= 0)
        {
            OnGameFinish?.Invoke();
            SortScore();
            ShowLeaderboard();
            if (usernameList.Count > 0)
            {
                if (_sceneIndex != 0)
                {
                    if (usernameList[4].text == "Player")
                    {
                        if (!playerConfetti.isPlaying)
                        {
                            playerConfetti.Play();
                        }
                    }
                }
            }
        }
    }

    private void ShowLeaderboard()
    {
        leaderboard.SetActive(true);
        leaderboard.transform.DOScale(0.75f, 1f);
    }

    private void SortScore()
    {
        if (usernameList.Count>0)
        {
            _scoreList.Sort();
            for (int i = 0; i < collectorList.Count; i++)
            {
                usernameList[i].text = _scoreList[i].name;
                scoreTextList[i].text = _scoreList[i].score.ToString("F0");
            }
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdatePlayerScores(int playerNo,int incrementValue)
    {
        switch (playerNo)
        {
            case 0: 
                _scoreList[4].score += incrementValue;
                break;
            case 1:
                _scoreList[3].score += incrementValue;
                break;
            case 2:
                _scoreList[2].score += incrementValue;
                break;
            case 3:
                _scoreList[1].score += incrementValue;
                break;
            case 4:
                _scoreList[0].score += incrementValue;
                break;
        }
    }
}
