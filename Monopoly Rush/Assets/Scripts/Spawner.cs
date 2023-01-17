using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour
{

    ObjectPooler objectPooler;

    [Header("Cash Spawn")]

    [SerializeField] Transform cashSpawnTransform;
    float cashSpawnTime;
    private bool isCashSpawningAllowed;
    BuildingManager building_Manager;

   

    [Header("Brick Spawn")]

    [SerializeField] Transform brickSpawnTransform;
    [SerializeField] float brickSpawnTime;
    private bool isBrickSpawningAllowed;
    int brick_Spawn_Limit;
    public int activeBrick;
    




    private void Start()
    {
        isCashSpawningAllowed = true;
        isBrickSpawningAllowed = true;

        if (cashSpawnTransform != null)
        {
            building_Manager = GetComponent<BuildingManager>();
        }
        objectPooler = ObjectPooler.instance;
        cashSpawnTime = 2;
        brick_Spawn_Limit = 500;

    }

    private void Update()
    {
        if (cashSpawnTransform != null)
        {
            if (building_Manager.buildingState == BuildingManager.BuildingState.Active )
            {
                if (isCashSpawningAllowed)
                {
                    StartCoroutine(SpawnCash());
                    
                }
                else
                {
                    return;
                }
            }

        }

        if (brickSpawnTransform != null)
        {
            if (isBrickSpawningAllowed)
            {
                if (activeBrick < brick_Spawn_Limit)
                {
                    StartCoroutine(SpawnBrick());
                }
                else
                {
                    return;
                }
                

            }
            else
            {
                return;
            }
        }

        
    }


    IEnumerator SpawnCash()
    {
        isCashSpawningAllowed = false;

        GameObject obj= objectPooler.cashPool.Get();
        float randomX = Random.Range(-4.0f, 4.0f);
        float randomZ = Random.Range(-4.0f, 4.0f);
        
     
        Vector3 offsetVector = new Vector3(randomX, 0.5f, randomZ);
        obj.transform.position = cashSpawnTransform.position + offsetVector;
        obj.transform.DOPunchScale(Vector3.one * 0.5f, 0.75f, 5, 0f);

        yield return new WaitForSeconds(cashSpawnTime);
        isCashSpawningAllowed = true;
    }

    
   

    IEnumerator SpawnBrick()
    {

        isBrickSpawningAllowed = false;
        GameObject obj = objectPooler.brickPool.Get();
        activeBrick++;

        float randomX = Random.Range(-5.0f, 5.0f);
        float randomZ = Random.Range(-5.0f, 5.0f);

        Vector3 offsetVector = new Vector3(randomX, 0.25f, randomZ);
        Vector3 spawnPos = brickSpawnTransform.position + offsetVector;

        
        obj.transform.position = spawnPos+offsetVector;
 
        StartCoroutine(WaitForPunchTween(obj));

        yield return new WaitForSeconds(brickSpawnTime);

        isBrickSpawningAllowed = true;

    }


    IEnumerator WaitForPunchTween(GameObject obj)
    {

        Tween tween= obj.transform.DOPunchScale(Vector3.one * 0.5f, 0.75f, 5, 0f);

        yield return tween.WaitForCompletion();

        obj.GetComponent<BoxCollider>().enabled = true; //To get bricks in right scale
    }


  

}
