using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]

    public float movementSpeed;
    [SerializeField] float rotationSpeed;


    CharacterController _characterController;
    Animator _pAnimator;
    CGameManager _cGameManager;

    // Variables to store input values

    Vector3 _currentMovement;
    [SerializeField] DynamicJoystick dynamicJoystick;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");


    private void Awake()
    {  
        _characterController = GetComponent<CharacterController>();
        _pAnimator = GetComponent<Animator>();

    }

    private void Start()
    {
        _cGameManager = CGameManager.Instance;
        CGameManager.OnGameFinish += C_GameManager_OnGameFinish;
    }

    private void C_GameManager_OnGameFinish()
    {
        GetComponent<PlayerController>().enabled = false;
    }

    private void HandleMovement()
    {
        _currentMovement.x = dynamicJoystick.Horizontal;
        _currentMovement.z = dynamicJoystick.Vertical;
        if (_currentMovement.x != 0 || _currentMovement.z != 0)
        {
            _pAnimator.SetBool(IsWalking, true);
           
        }
        else
        {
            _pAnimator.SetBool(IsWalking, false);
         
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = dynamicJoystick.Horizontal;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = dynamicJoystick.Vertical;
  

        Quaternion currentRotation = transform.rotation;
        if (positionToLookAt != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


    }

    void HandleGravity()
    {
        if (_characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            _currentMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.8f;
            _currentMovement.y += gravity;
        }
    }


    private void Update()
    {
        HandleMovement();
        HandleGravity();
        HandleRotation();
        _characterController.Move(_currentMovement * Time.deltaTime * movementSpeed);
    }


}
