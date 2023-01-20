using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Builder : MonoBehaviour
{
    // Components 
    private BuildingManager _buildingManager;
    private BrickManager _brickManager;
    private List<GameObject> _colllectedBricksList = new List<GameObject>();
    public int CollectedBricks { get; set; }
    Vector3 _brickStackPosition; // Last path point for collect, local position on player 
    public float brickStackingSpace; // Space between bricks
    public List<GameObject> ColllectedBricksList
    {
        get => _colllectedBricksList;
        set => _colllectedBricksList = value;
    }

    private void Start()
    {
        _brickManager = GetComponent<BrickManager>();
        
        // Starting collect positions
        if (transform.CompareTag("Player"))
            _brickStackPosition = new Vector3(0f, 1f, -0.5f);
        else
            _brickStackPosition = new Vector3(0f, 1f, -0.25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InactiveBuilding"))
        {
            _buildingManager=other.GetComponent<BuildingManager>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("InactiveBuilding"))
        {
            _buildingManager.CheckBuildingStatus(gameObject); // Check building status constantly
            if (_colllectedBricksList.Count == 0) return;
            StartCoroutine(SpendObject(other));
        }
    }
    
    private IEnumerator SpendObject(Collider collider)
    {
        // Get last object on the list
        int lastObjOnTheList = ColllectedBricksList.Count - 1; 
        GameObject spendObj = ColllectedBricksList[lastObjOnTheList];
        
        ColllectedBricksList.Remove(spendObj); // Remove from list
        spendObj.transform.parent = null; // Remove parent
        _buildingManager.Build(); // Build building
        Tween spendTween = spendObj.transform.DOJump(collider.transform.position, 2f, 1, 0.5f, false); // Jump to building
        yield return spendTween.WaitForCompletion(); // Wait for jump to finish
        ObjectPooler.Instance.BrickPool.Release(spendObj); // Release object to pool
        CollectedBricks--; // Decrease collected bricks
        _brickManager.UpdateBrickStackPosition(false);
    }
    
    private void SetPlayerBuildingStatus()
    {
        if (gameObject.CompareTag("Player"))
        {
            _buildingManager.IsPlayerBuilding = true;
        }
        else
        {
            _buildingManager.IsPlayerBuilding = false;
        }
    }
}
