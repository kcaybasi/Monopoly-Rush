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


public class C_GameManager : MonoBehaviour
{
    public static C_GameManager instance;


    public List<Collector> collector_List;

    public TextMeshProUGUI player_cash_text;
    public int player_cash;

    public List<Score> score_list = new List<Score>();
    public List<TextMeshProUGUI> username_List=new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> score_Text_List=new List<TextMeshProUGUI>();
    public GameObject leaderboard;


    public List<GameObject> inactive_Building_List;
    [HideInInspector]
    public List<GameObject> active_Building_List;
    public List<GameObject> total_Building_List;
    public TextMeshProUGUI active_building_count_text;

    public List<GameObject> supply_Line_List;

    public List<GameObject> player_list;
    [SerializeField] ParticleSystem player_confetti;

    int scene_index;
  

    public delegate void GameAction();
    public static event GameAction OnGameFinish;

    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;


        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(this.gameObject, pointer, ExecuteEvents.beginDragHandler);

        for (int i = 0; i < collector_List.Count; i++)
        {
            score_list.Add(new Score(collector_List[i].transform.name, collector_List[i].total_cash_amount));
           
        }

        scene_index = SceneManager.GetActiveScene().buildIndex;

        FB.Init();
        GameAnalytics.Initialize();
    }
    
 
    public void UpdateBuildingList(GameObject building)
    {
        inactive_Building_List.Remove(building); 
        active_Building_List.Add(building);
        active_building_count_text.text = active_Building_List.Count.ToString("F0")+" / "+(inactive_Building_List.Count+active_Building_List.Count).ToString("F0");  //Update building text

        if (!DOTween.IsTweening(active_building_count_text.transform))
        {
            active_building_count_text.transform.DOPunchScale(Vector3.one, .3f, 5, 0.2f);
        }
        
        
    }

    public void CheckIfGameFinished()
    {
      
        if (inactive_Building_List.Count <= 0)
        {
           
            if (OnGameFinish != null)
            {
                OnGameFinish();
            }
            SortScore();
            ShowLeaderboard();
            if (username_List.Count > 0)
            {
                if (scene_index != 0)
                {
                    if (username_List[4].text == "Player")
                    {
                        if (!player_confetti.isPlaying)
                        {
                            player_confetti.Play();
                        }
                    }
                }
 
            }

            
        }
    }


    public void ShowLeaderboard()
    {
        leaderboard.SetActive(true);
        leaderboard.transform.DOScale(0.75f, 1f);

    }



    public void SortScore()
    {
        if (username_List.Count>0)
        {
            score_list.Sort();


            for (int i = 0; i < collector_List.Count; i++)
            {
                username_List[i].text = score_list[i].name;
                score_Text_List[i].text = score_list[i].score.ToString("F0");
            }
        }

    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdatePlayerScores(int player_no,int increment_value)
    {
        switch (player_no)
        {
            case 0: 
                score_list[4].score += increment_value;
                break;
            case 1:
                score_list[3].score += increment_value;
                break;
            case 2:
                score_list[2].score += increment_value;
                break;
            case 3:
                score_list[1].score += increment_value;
                break;
            case 4:
                score_list[0].score += increment_value;
                break;

        }
    }


}
