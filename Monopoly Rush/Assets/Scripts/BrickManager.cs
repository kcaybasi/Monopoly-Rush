using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    private List<GameObject> _bricksList = new List<GameObject>();
    public int CollectedBricks { get; set; }
    public Vector3 brickStackPosition; // Last path point for collect, local position on player 
    public float brickStackingSpace; // Space between bricks
    public List<GameObject> BricksList
    {
        get => _bricksList;
        set => _bricksList = value;
    }
    
    public void UpdateBrickStackPosition(bool collect)
    {
        if (collect)
        {
            brickStackPosition = new Vector3(brickStackPosition.x, brickStackPosition.y + brickStackingSpace, brickStackPosition.z);
        }
        else
        {
            brickStackPosition = new Vector3(brickStackPosition.x, brickStackPosition.y - brickStackingSpace, brickStackPosition.z);
        }
    }
}
