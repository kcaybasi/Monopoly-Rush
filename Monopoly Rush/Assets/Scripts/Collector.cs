using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;
using ES3Types;
using TMPro;
using MoreMountains.NiceVibrations;
using UnityEngine.Serialization;


public class Collector : MonoBehaviour
{
    // Components 
    private BuildingManager _buildingManager;
    private BrickManager _brickManager;
    

  
    private void Awake()
    {
        // Get Components
        _brickManager = GetComponent<BrickManager>();
        
        // Set DoTween capacity
        DOTween.Init();
        DOTween.SetTweensCapacity(1000, 250);
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
            CollectObject(other);
        }
    }
    private void CollectObject(Collider collider)
    {
        collider.transform.parent = transform; // Set parent to player
        collider.enabled = false; // Disable collider to prevent double collect
        collider.transform.DOLocalJump(_brickManager.brickStackPosition, 2f, 1, 0.3f, false); // Jump to stack position
        Vector3 rotationVec = new Vector3(0, 90f, 0); // Set rotation vector
        collider.transform.DOLocalRotate(rotationVec, 0.5f, RotateMode.Fast); // Rotate to stack position
        _brickManager.BricksList.Add(collider.gameObject); // Add to collected list
        _brickManager.CollectedBricks++; // Increase collected bricks
        _brickManager.UpdateBrickStackPosition(true);
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
    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("InactiveBuilding"))
        {
           // _buildingManager.CheckBuildingStatus(gameObject); // Check building status constantly
           // BuildingProcess(other);
        }
    }

    /*public void BuildingProcess(Collider other)
    {
        if (_builder.CollectedBricks > 0)
        {
            SetPlayerBuildingStatus(); // To check if player has build it or not
            if (_builder.ColllectedBricksList.Count == 0) return; // If list is empty, return because building is complete
            StartCoroutine(SpendObject(other)); // Spend object from list
        }
    }
    private IEnumerator SpendObject(Collider collider)
    {
        // Get last object on the list
        int lastObjOnTheList = _builder.ColllectedBricksList.Count - 1; 
        GameObject spendObj = _builder.ColllectedBricksList[lastObjOnTheList];
        
        _builder.ColllectedBricksList.Remove(spendObj); // Remove from list
        spendObj.transform.parent = null; // Remove parent
        _buildingManager.Build(); // Build building
        Tween spendTween = spendObj.transform.DOJump(collider.transform.position, 2f, 1, 0.5f, false); // Jump to building
        yield return spendTween.WaitForCompletion(); // Wait for jump to finish
        ObjectPooler.Instance.BrickPool.Release(spendObj); // Release object to pool
        _builder.CollectedBricks--; // Decrease collected bricks
        //_brickStackPosition.y -= brickStackingSpace; // Decrease stack position y
    }*/

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
