using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using MoreMountains.NiceVibrations;


public class Collector : MonoBehaviour
{

    [Header("Brick Collection")]

    Vector3 brickStackPosition; // Last path point for collect, local position on player 
    [SerializeField] float brickStackingSpace; // Space between bricks
    List<GameObject> collectedBrickList = new List<GameObject>(); // List of collected bricks
    
    public int collectedBricks; // Number of collected brick 

    public  BuildingManager building_Manager;

    public Tween putback_tween;

    [Header("Cash Collection")]

    C_GameManager c_GameManager;
    [SerializeField] string cash_tag;
    [SerializeField] TextMeshProUGUI cash_text;
    int collected_cash;
    int cash_value=50; // Will depend on building in future 
    public int total_cash_amount;

    private void Awake()
    {
        DOTween.Init();
        DOTween.SetTweensCapacity(1000, 250);

        // Starting collect positions
        if (transform.CompareTag("Player"))
        {
            brickStackPosition = new Vector3(0f, 1f, -0.5f);
        }
        else
        {
            brickStackPosition = new Vector3(0f, 1f, -0.25f);
        }
        
        
    }

    private void Start()
    {
        c_GameManager = C_GameManager.instance;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            CollectObject(other, brickStackPosition);


        }
        if (other.CompareTag(cash_tag))
        {
            StartCoroutine(CollectCash(other));
           
        }

        if (other.CompareTag("InactiveBuilding"))
        {
            building_Manager = other.GetComponent<BuildingManager>();
        }

    }


    IEnumerator CollectCash(Collider collider)
    {
        collider.transform.parent = transform;
        collider.enabled = false;

        Tween scaleTween = collider.transform.DOScale(0.2f, 0.25f);
        Tween moveTween = collider.transform.DOLocalMove(Vector3.up*2, 0.35f, false);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(moveTween);
        mySequence.Insert(0,scaleTween);


        yield return mySequence.WaitForCompletion();
        collected_cash++;
        total_cash_amount = collected_cash * cash_value;
        UpdateCashText();
        ObjectPooler.instance.cashPool.Release(collider.gameObject);
       
       

    }

    private void UpdateCashText()
    {
        if (cash_text != null)
        {
            cash_text.text = total_cash_amount.ToString("F0");
        }

   
    }


    private void CollectObject(Collider collider,Vector3 stackPosition)
    {
        collider.transform.parent = transform;
        collider.enabled = false;

        collider.transform.DOLocalJump(stackPosition, 2f, 1, 0.3f, false);

        Vector3 rotationVec = new Vector3(0, 90f, 0);
        collider.transform.DOLocalRotate(rotationVec, 0.5f, RotateMode.Fast);


        collectedBrickList.Add(collider.gameObject);

        collectedBricks++;
        brickStackPosition.y += brickStackingSpace;

        if (transform.CompareTag("Player"))
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            AudioManager.Instance.Play("Collect");
        }
       

        
    }

    public IEnumerator SpendObject(Collider collider)
    {
        
        int lastObjOnTheList = collectedBrickList.Count - 1;
        GameObject spendObj = collectedBrickList[lastObjOnTheList];
        collectedBrickList.Remove(spendObj);

        spendObj.transform.parent = null;

        building_Manager.Build();
        Tween putbacktween = spendObj.transform.DOJump(collider.transform.position, 2f, 1, 0.5f, false);

        yield return putbacktween.WaitForCompletion();
       

        ObjectPooler.instance.brickPool.Release(spendObj);

        collectedBricks--;
        brickStackPosition.y -= brickStackingSpace;
    }

    
    private void OnTriggerStay(Collider other)
    {

        if ( other.CompareTag("InactiveBuilding"))
        {
            if (collectedBricks > 0)
            {
                
                building_Manager.CheckBuildingStatus(this.gameObject);
                if (this.gameObject.CompareTag("Player"))
                {
                    building_Manager.playerBuilding = true;
                }
                else
                {
                    building_Manager.playerBuilding = false;
                }
                if (collectedBrickList.Count == 0) { return; }
                StartCoroutine(SpendObject(other));
            }

        }

    }


}
