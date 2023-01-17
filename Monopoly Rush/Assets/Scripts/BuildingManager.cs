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

    [Header("Visuals")]
    [SerializeField] GameObject playerIcon;
    [SerializeField] Image progressCircleImage;
    [SerializeField] GameObject circleBackgroundImage;
    [SerializeField] TextMeshProUGUI activeIncomeText;
    [SerializeField] TextMeshProUGUI inactiveIncomeText;
    [SerializeField] GameObject hammerImageObj;
    [SerializeField] GameObject activeBuilding;
    [SerializeField] GameObject buildTeam;
    [SerializeField] private ParticleSystem buildParticle;
    [SerializeField] GameObject buildingFrontTarget;
    public ParticleSystem pieceSmashParticle;
    
    readonly float _buildTime=.75f;
    float _progressIncrement;
    public int incomeAmount;
    public bool playerBuilding;
    
    // Components
    
    CGameManager _cGameManager;
    Collector _collector;
    
    private void Awake()
    {
        // Calculate the progress increment value
        _progressIncrement = 1f / brickAmountToActivate;
        
        // Assign the building text properties
        inactiveIncomeText.text= "$" + incomeAmount.ToString("F0") +"/s";
        activeIncomeText.text = "$" + incomeAmount.ToString("F0")+"/s";
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
        activeBuilding.SetActive(true);

        inactiveIncomeText.gameObject.SetActive(false);

        buildParticle.Play();
        circleBackgroundImage.SetActive(false);

        hammerImageObj.SetActive(false);
        activeIncomeText.gameObject.SetActive(true);
   
        progressCircleImage.gameObject.SetActive(false);

        if (playerBuilding)
        {
            playerIcon.SetActive(true);
        }
    }

 
}
