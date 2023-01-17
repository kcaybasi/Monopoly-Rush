using UnityEngine;
using UnityEngine.Pool;


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    public ObjectPool<GameObject> brickPool;
    public ObjectPool<GameObject> cashPool;

    public GameObject cashPrefab,brickPrefab;
  
    

    private void Awake()
    {
        instance = this;

        brickPool = new ObjectPool<GameObject>(CreateBrick, OnTakeBrickFromPool, OnReturnBrickToPool);
        cashPool = new ObjectPool<GameObject>(CreateCash, OnTakeCashFromPool, OnReturnCashToPool);

    }

    #region Brick Pooling

    GameObject CreateBrick()
    {
        var brick = Instantiate(brickPrefab);
        
        return brick;
    }

    void OnTakeBrickFromPool(GameObject gameObject)
    {
        gameObject.SetActive(true);
        gameObject.transform.eulerAngles = new Vector3(0, 90, 0);


    }

    void OnReturnBrickToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        
    }


    #endregion

    #region Cash Pooling

    GameObject CreateCash()
    {
        var cash = Instantiate(cashPrefab);
        return cash;
    }

    void OnTakeCashFromPool(GameObject gameObject)
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.GetComponent<Collider>().enabled = true;

    }

    void OnReturnCashToPool(GameObject gameObject)
    {
        gameObject.transform.parent = null;
        gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        gameObject.SetActive(false);


    }

    #endregion



}
