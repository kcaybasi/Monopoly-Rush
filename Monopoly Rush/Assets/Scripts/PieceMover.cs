using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PieceMover : MonoBehaviour
{
    CGameManager _cGameManager;
    readonly float _waitTime=1.5f;
    public List<GameObject> targetList=new List<GameObject>();
    int _targetIndex=0;
    bool _isMovementAllowed=true;
    
    
    void Start()
    {
        _cGameManager = CGameManager.Instance;
        targetList = _cGameManager.totalBuildingList;

        CGameManager.OnGameFinish += C_GameManager_OnGameFinish;
    
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
                _cGameManager.playerCash += incomeAmount;
                _cGameManager.playerCashText.text = _cGameManager.playerCash.ToString("F0");
   
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
        Tween tween= _cGameManager.playerCashText.transform.DOPunchScale(Vector3.one, .25f, 5, 0.2f);

        yield return tween.WaitForCompletion();

        _cGameManager.playerCashText.transform.DORewind();
    }
}
