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
    public enum BuildingState { Inactive, Active}
    public BuildingState buildingState = BuildingState.Inactive;

    [Header("Building Activate Components")]

    Collector _collector;
    [SerializeField] GameObject buildingFrontTarget;
    [SerializeField] GameObject dummyBuilding;
    [SerializeField] GameObject buildTeam;
    readonly float _buildTime=.75f;

    [Header("UI & VFX")]

    [SerializeField] ParticleSystem imageUpgradeParticle;
    [SerializeField] ParticleSystem buildingUpgradeParticle;
    public ParticleSystem pieceSmashParticle;
    [SerializeField] GameObject circleBackgroundImage;
    [SerializeField] Image progressCircleImage;
    [SerializeField] GameObject hammerObj;
    [SerializeField] GameObject houseObj;
    [SerializeField] TextMeshProUGUI houseText;

    float _progressIncrement;
    [SerializeField] TextMeshProUGUI incomeText;
    public int incomeAmount;
    public bool playerBuilding;
    [SerializeField] GameObject playerIcon;
    
    CGameManager _cGameManager;
    
    private void Awake()
    {
        _progressIncrement = 1f / brickAmountToActivate;
        
        // Assign the building text properties
        incomeText.text= "$" + incomeAmount.ToString("F0") + "/s";
        houseText.text = "$" + incomeAmount.ToString("F0")+"/s";
    }

    private void Start()
    {
        _cGameManager = CGameManager.Instance;
        _cGameManager.inactiveBuildingList.Add(buildingFrontTarget); // Adding all of the buildings to inactive list
        _cGameManager.totalBuildingList.Add(buildingFrontTarget); // Adding all of the buildings to total building list
    }

    public void Build()
    {
        if (brickAmountToActivate >= 0)
        {
            brickAmountToActivate--;
            progressCircleImage.fillAmount += _progressIncrement;
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
        circleBackgroundImage.SetActive(false);

        hammerObj.SetActive(false);
        houseText.gameObject.SetActive(true);
        houseObj.SetActive(true);
        progressCircleImage.gameObject.SetActive(false);

        if (playerBuilding)
        {
            playerIcon.SetActive(true);
        }
    }

 
}
