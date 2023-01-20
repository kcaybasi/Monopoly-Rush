using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Builder : MonoBehaviour
{
    // Components 
    private Building _building;
    private BrickManager _brickManager;
    private void Start()
    {
        _brickManager = GetComponent<BrickManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InactiveBuilding"))
        {
            _building=other.GetComponent<Building>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("InactiveBuilding"))
        {
            _building.CheckBuildingStatus(gameObject); // Check building status constantly
            SetPlayerBuildingStatus();
            if (_brickManager.BricksList.Count == 0) return;
            SpendObject(other);
        }
    }

    async void SpendObject(Collider collider)
    {
        // Get last object on the list
        int lastObjOnTheList = _brickManager.BricksList.Count - 1; 
        GameObject spendObj = _brickManager.BricksList[lastObjOnTheList];
        _brickManager.BricksList.Remove(spendObj); 
        spendObj.transform.parent = null;
        _building.Build();
        Tween spendTween = spendObj.transform.DOJump(collider.transform.position, 2f, 1, 0.5f, false);
        await spendTween.AsyncWaitForCompletion(); // Wait for jump to finish
        ObjectPooler.Instance.BrickPool.Release(spendObj); 
        _brickManager.CollectedBricks--; 
        _brickManager.UpdateBrickStackPosition(false);
    }
    
    private void SetPlayerBuildingStatus()
    {
        if (gameObject.CompareTag("Player"))
            _building.IsPlayerBuilding = true;
        else
            _building.IsPlayerBuilding = false;
    }
}
