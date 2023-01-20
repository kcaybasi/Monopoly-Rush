using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using DG.Tweening;
using MoreMountains.NiceVibrations;



public class Collector : MonoBehaviour
{
    // Components 
    private BrickManager _brickManager;

    private void Awake()
    {
        // Get Components
        _brickManager = GetComponent<BrickManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
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
}

  
