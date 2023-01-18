using UnityEngine;
using UnityEngine.Pool;


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public ObjectPool<GameObject> BrickPool;
    public ObjectPool<GameObject> CashPool;

    [SerializeField] GameObject cashPrefab,brickPrefab;
    [SerializeField] GameObject cashParent,brickParent;
  
    

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        BrickPool = new ObjectPool<GameObject>(CreateBrick, OnTakeBrickFromPool, OnReturnBrickToPool);
        CashPool = new ObjectPool<GameObject>(CreateCash, OnTakeCashFromPool, OnReturnCashToPool);

    }

    #region Brick Pooling

    GameObject CreateBrick()
    {
        var brick = Instantiate(brickPrefab,brickParent.transform);
        return brick;
    }

    void OnTakeBrickFromPool(GameObject gameObject)
    {
        gameObject.SetActive(true);
        gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
    }

    void OnReturnBrickToPool(GameObject gameObject)
    {
        gameObject.transform.parent = brickParent.transform;
        gameObject.SetActive(false);
    }


    #endregion

    #region Cash Pooling

    GameObject CreateCash()
    {
        var cash = Instantiate(cashPrefab,cashParent.transform);
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
        gameObject.transform.parent = cashParent.transform;
        gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        gameObject.SetActive(false);


    }

    #endregion
    
}
