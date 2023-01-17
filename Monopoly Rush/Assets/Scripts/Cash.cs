using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cash : MonoBehaviour
{
    private void Awake()
    {
        transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 0.5f);
    }
}
