using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using MoreMountains.NiceVibrations;
using UnityEngine.Serialization;


public class Collector : MonoBehaviour
{
    // Components 
    private BuildingManager _buildingManager;

    [Header("Brick Collection")]
    Vector3 _brickStackPosition; // Last path point for collect, local position on player 
    [SerializeField] float brickStackingSpace; // Space between bricks
    private readonly List<GameObject> _collectedBrickList = new List<GameObject>(); // List of collected bricks
    public int CollectedBricks { get; set; }
    
    [Header("Cash Collection")]
    [SerializeField] private float _cashCollectionTime; // Time to collect cash
    public int TotalCashAmount { get; }
    
    private void Awake()
    {
        // Set DoTween capacity
        DOTween.Init();
        DOTween.SetTweensCapacity(1000, 250);

        // Starting collect positions
        if (transform.CompareTag("Player"))
            _brickStackPosition = new Vector3(0f, 1f, -0.5f);
        else
            _brickStackPosition = new Vector3(0f, 1f, -0.25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get building manager
        if (other.CompareTag("InactiveBuilding"))
        {
            _buildingManager = other.GetComponent<BuildingManager>();
        }
        // Collect bricks
        if (other.CompareTag("Brick"))
        {
            CollectObject(other, _brickStackPosition);
        }
    }
    
    private void CollectObject(Collider collider,Vector3 stackPosition)
    {
        collider.transform.parent = transform; // Set parent to player
        collider.enabled = false; // Disable collider to prevent double collect
        collider.transform.DOLocalJump(stackPosition, 2f, 1, 0.3f, false); // Jump to stack position
        Vector3 rotationVec = new Vector3(0, 90f, 0); // Set rotation vector
        collider.transform.DOLocalRotate(rotationVec, 0.5f, RotateMode.Fast); // Rotate to stack position
        _collectedBrickList.Add(collider.gameObject); // Add to collected list
        CollectedBricks++; // Increase collected bricks
        _brickStackPosition.y += brickStackingSpace; // Increase stack position y
        CollectFeedback(); // Play collect feedback
    }

    private void CollectFeedback()
    {
        if (transform.CompareTag("Player"))
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            AudioManager.Instance.Play("Collect");
        }
    }

    private IEnumerator SpendObject(Collider collider)
    {
        // Get last object on the list
        int lastObjOnTheList = _collectedBrickList.Count - 1; 
        GameObject spendObj = _collectedBrickList[lastObjOnTheList];
        _collectedBrickList.Remove(spendObj); // Remove from list
        spendObj.transform.parent = null; // Remove parent
        _buildingManager.Build(); // Build building
        Tween spendTween = spendObj.transform.DOJump(collider.transform.position, 2f, 1, 0.5f, false); // Jump to building
        yield return spendTween.WaitForCompletion(); // Wait for jump to finish
        ObjectPooler.Instance.BrickPool.Release(spendObj); // Release object to pool
        CollectedBricks--; // Decrease collected bricks
        _brickStackPosition.y -= brickStackingSpace; // Decrease stack position y
    }
    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("InactiveBuilding"))
        {
            if (CollectedBricks > 0)
            {
                _buildingManager.CheckBuildingStatus(gameObject); // Check building status constantly
                SetPlayerBuildingStatus();
                if (_collectedBrickList.Count == 0) { return; } // If list is empty, return because building is complete
                StartCoroutine(SpendObject(other)); // Spend object from list
            }
        }
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
