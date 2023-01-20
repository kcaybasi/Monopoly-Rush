using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Building : MonoBehaviour
{
    // Components
    CGameManager _cGameManager;
    Collector _collector;

    [Header("Building Properties")]
    public float brickAmountToActivate=15;
    public enum BuildingState { Inactive, Active}
    public BuildingState buildingState = BuildingState.Inactive;
    [SerializeField] GameObject buildingFrontTarget;
    public int IncomeAmount { get; set; }
    
    [Header("Building Progress")]
    [SerializeField] GameObject circleBackgroundImage;
    [SerializeField] Image progressCircleImage;
    float _progressIncrement;
    
    [Header("Building Activation")]
    [SerializeField] TextMeshProUGUI activeIncomeText;
    [SerializeField] TextMeshProUGUI inactiveIncomeText;
    [SerializeField] GameObject hammerImageObj;
    [SerializeField] GameObject activeBuilding;
    [SerializeField] GameObject buildTeam;
    readonly float _buildTime=.75f;
    [SerializeField] private ParticleSystem buildParticle;
    public bool IsPlayerBuilding { get; set; }
    [SerializeField] GameObject playerIcon;
    
    [Header("Piece VFX")]
    public ParticleSystem pieceSmashParticle;
    
    private void Awake()
    {
        // Calculate the progress increment value
        _progressIncrement = 1f / brickAmountToActivate;
        
        // Assign the building text properties
        inactiveIncomeText.text= "$" + IncomeAmount.ToString("F0") +"/s";
        activeIncomeText.text = "$" + IncomeAmount.ToString("F0")+"/s";
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
            buildingState = BuildingState.Active; // Changing the building state to active
            gameObject.tag = "ActiveBuilding"; // Changing the tag of the building
            transform.name = gameobj.transform.name + "_Building"; // Changing the name of the building
            StartCoroutine(SpawnBuildTeam()); // Spawning the build team
        }
    }
    IEnumerator SpawnBuildTeam()
    {
        buildTeam.SetActive(true);
        buildTeam.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
        yield return new WaitForSeconds(_buildTime);
        buildTeam.SetActive(false);     
        ActivateBuilding();
    }
    private void ActivateBuilding()
    {
        // Deactivate the inactive building
        inactiveIncomeText.gameObject.SetActive(false);
        circleBackgroundImage.SetActive(false);
        progressCircleImage.gameObject.SetActive(false);
        hammerImageObj.SetActive(false);
        
        // Play the build particle
        buildParticle.Play();
        
        // Activate building
        activeBuilding.SetActive(true);
        activeIncomeText.gameObject.SetActive(true);
        if (IsPlayerBuilding)
        {
            playerIcon.SetActive(true); // Activate the player icon if the building is player's building
        }
    }
    
}
