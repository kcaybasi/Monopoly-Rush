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
            if (_brickManager.BricksList.Count == 0) return;
            StartCoroutine(SpendObject(other));
        }
    }
    
    private IEnumerator SpendObject(Collider collider)
    {
        // Get last object on the list
        int lastObjOnTheList = _brickManager.BricksList.Count - 1; 
        GameObject spendObj = _brickManager.BricksList[lastObjOnTheList];
        _brickManager.BricksList.Remove(spendObj); // Remove from list
        spendObj.transform.parent = null; // Remove parent
        _building.Build(); // Build building
        Tween spendTween = spendObj.transform.DOJump(collider.transform.position, 2f, 1, 0.5f, false); // Jump to building
        yield return spendTween.WaitForCompletion(); // Wait for jump to finish
        ObjectPooler.Instance.BrickPool.Release(spendObj); // Release object to pool
        _brickManager.CollectedBricks--; // Decrease collected bricks
        _brickManager.UpdateBrickStackPosition(false);
    }
    
    private void SetPlayerBuildingStatus()
    {
        if (gameObject.CompareTag("Player"))
        {
            _building.IsPlayerBuilding = true;
        }
        else
        {
            _building.IsPlayerBuilding = false;
        }
    }
}
