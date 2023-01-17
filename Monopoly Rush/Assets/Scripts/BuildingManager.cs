using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BuildingManager : MonoBehaviour
{

    [Header("Building Properties")]

    public float brickAmountToActivate=15;
    public int buildingCapacity;
    private int _startingCapacity;
    public enum BuildingState { Inactive, Active, Occupied }
    public BuildingState buildingState = BuildingState.Inactive;

    [Header("Building Activate Components")]

    Collector _collector;
    [SerializeField] GameObject buildingFrontTarget;
    [SerializeField] GameObject dummyBuilding;
    [SerializeField] List<GameObject> windowList;
    [SerializeField] GameObject buildTeam;
    readonly float _buildTime=.75f;

    [Header("UI & VFX")]

    [SerializeField] ParticleSystem imageUpgradeParticle;
    [SerializeField] ParticleSystem buildingUpgradeParticle;
    public ParticleSystem pieceSmashParticle;
    [SerializeField] Image borderImage;
    [SerializeField] GameObject circleImage;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject hammerObj;
    [SerializeField] GameObject houseObj;
    [SerializeField] TextMeshProUGUI houseText;

    float _imageIncrement;
    [SerializeField] TextMeshProUGUI incomeText;
    public int incomeAmount;
    public bool playerBuilding;
    [SerializeField] GameObject playerIcon;


    CGameManager _cGameManager;
    


    private void Awake()
    {
        _imageIncrement = 1f / brickAmountToActivate;

        _startingCapacity = buildingCapacity;

        incomeText.text= "$" + incomeAmount.ToString("F0") + "/s";
        houseText.text = "$" + incomeAmount.ToString("F0")+"/s";
    }

    private void Start()
    {
        _cGameManager = CGameManager.Instance;
        _cGameManager.inactive_Building_List.Add(buildingFrontTarget); // Adding all of the buildings to inactive list
        _cGameManager.total_Building_List.Add(buildingFrontTarget); // Adding all of the buildings to total building list
    }

    public void Build()
    {
        if (brickAmountToActivate >= 0)
        {
            brickAmountToActivate--;
            fillImage.fillAmount += _imageIncrement;
        }
    }

    public void CheckBuildingStatus(GameObject gameobj)
    {
        if (brickAmountToActivate <= 0)
        {
            _cGameManager.UpdateBuildingList(buildingFrontTarget);
            _cGameManager.CheckIfGameFinished();
            buildingState = BuildingState.Active;
            this.gameObject.tag = "ActiveBuilding";
            transform.name = gameobj.transform.name + "_Building";
            buildingState = BuildingState.Active;
            StartCoroutine(SpawnBuildTeam());

        }

    }
    IEnumerator SpawnBuildTeam()
    {
        buildTeam.SetActive(true);
        buildTeam.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
        yield return new WaitForSeconds(_buildTime);
        buildTeam.SetActive(false);     
        ActivateDummyBuilding();
    }


    private void ActivateDummyBuilding()
    {
        dummyBuilding.SetActive(true);

        incomeText.gameObject.SetActive(false);

        imageUpgradeParticle.Play();
        buildingUpgradeParticle.Play();
        circleImage.SetActive(false);

        hammerObj.SetActive(false);
        houseText.gameObject.SetActive(true);
        houseObj.SetActive(true);
        fillImage.gameObject.SetActive(false);

        if (playerBuilding)
        {
            playerIcon.SetActive(true);
        }
    }

 
}
