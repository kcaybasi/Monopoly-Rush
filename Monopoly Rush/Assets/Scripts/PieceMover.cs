using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PieceMover : MonoBehaviour
{
    C_GameManager _cGameManager;
    readonly float _waitTime=1.5f;
    public List<GameObject> targetList=new List<GameObject>();
    int _targetIndex=0;
    bool _isMovementAllowed=true;
    
    
    void Start()
    {
        _cGameManager = C_GameManager.instance;
        targetList = _cGameManager.total_Building_List;

        C_GameManager.OnGameFinish += C_GameManager_OnGameFinish;
    
    }

    private void C_GameManager_OnGameFinish()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator MoveOnBoard()
    {
      
        _targetIndex = Random.Range(0, 19);
        Vector3 targetPos = targetList[_targetIndex].transform.position;
        transform.DOJump(targetPos, 20, 1, 1f, false);
        _isMovementAllowed = false;
        yield return new WaitForSeconds(_waitTime);
        _isMovementAllowed = true;
    }


    private void Update()
    {
        if (_isMovementAllowed)
        {
            StartCoroutine(MoveOnBoard());
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        BuildingManager buildingManager = other.GetComponent<BuildingManager>();

        if (buildingManager == null) { return; }
        int incomeAmount = buildingManager.incomeAmount;
        if (!buildingManager.pieceSmashParticle.isPlaying)
        {
            buildingManager.pieceSmashParticle.Play();
        }

        switch (other.transform.name)
        {
            case "Player_Building":
                
                _cGameManager.UpdatePlayerScores(4, incomeAmount);
                _cGameManager.player_cash += incomeAmount;
                _cGameManager.player_cash_text.text = _cGameManager.player_cash.ToString("F0");
   
                break;
            case "Jonathan_Building":
                _cGameManager.UpdatePlayerScores(3, incomeAmount);
               
                break;
            case "Martha_Building":
                _cGameManager.UpdatePlayerScores(2, incomeAmount);
                
                break;
            case "Josh_Building":
                _cGameManager.UpdatePlayerScores(1, incomeAmount);
                
                break;
            case "Marry_Building":
                _cGameManager.UpdatePlayerScores(0, incomeAmount);
                
                break;
            
        }
    }



    IEnumerator PlayerCashTween()
    {
        Tween tween= _cGameManager.player_cash_text.transform.DOPunchScale(Vector3.one, .25f, 5, 0.2f);

        yield return tween.WaitForCompletion();

        _cGameManager.player_cash_text.transform.DORewind();
    }
}
