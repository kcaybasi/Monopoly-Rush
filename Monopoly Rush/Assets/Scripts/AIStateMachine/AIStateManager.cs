using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AIStateMachine
{
    public class AIStateManager : MonoBehaviour
    {
        public AIBaseState CurrentState { get; set; }
        private AIStateFactory _stateFactory;
        NavMeshAgent _navMeshAgent;
        public BrickManager brickManager;
        public int selectedListNo;
        private readonly float _reachDistance =3f;
        int _brickCapacity;
        public string State;
        public GameObject reachedBuilding;
       
        private void Start()
        {
            //Randomize brick capacity for every AI 

            _brickCapacity = Random.Range(10, 25);

            // Get components

            _navMeshAgent = GetComponent<NavMeshAgent>();
            brickManager = GetComponent<BrickManager>();

            //Set initial state
            _stateFactory = new AIStateFactory(this);
            CurrentState = _stateFactory.MovementState();
            CurrentState.EnterState();

            CGameManager.OnGameFinish += C_GameManager_OnGameFinish; // Subscribe to game finish event
        }

        private void C_GameManager_OnGameFinish()
        {
            gameObject.SetActive(false);
            GetComponent<Collector>().enabled = false;
        }

        private void Update()
        {
            State=CurrentState.ToString();
            CurrentState.UpdateState();
        }
        
        public void MoveTo(Vector3 targetPos)
        {
            _navMeshAgent.SetDestination(targetPos);
        }
        
        public GameObject GetClosestTarget(List<GameObject> targetList)
        {
            List<float> distanceList = new List<float>(); // List of distances between AI and targets
            for (int i = 0; i < targetList.Count; i++)
            {
                float distance = Vector3.Magnitude(transform.position - targetList[i].transform.position); // Calculate distance between AI and target
                distanceList.Add(distance); // Add distance to list
            }
            var minValue = Mathf.Min(distanceList.ToArray()); // Get minimum value from list
            int index = distanceList.IndexOf(minValue);
            return targetList[index].transform.gameObject;
        }

        public bool TargetReached(Vector3 targetPos)
        {
            return Vector3.Magnitude(transform.position - targetPos) < _reachDistance; 
        }

        public bool BrickCapacityFull()
        {
            return brickManager.CollectedBricks >= _brickCapacity;
        }

        public List<GameObject> GetDecidedTargetList()
        {
            if (!BrickCapacityFull())
            {
                selectedListNo = 0;
                return CGameManager.Instance.supplyLineList;
            }
            selectedListNo = 1;
            return CGameManager.Instance.inactiveBuildingList;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            CurrentState.OnTriggerEnter(other);
        }

        private void OnDestroy()
        {
            CGameManager.OnGameFinish -= C_GameManager_OnGameFinish;
        }
    }
}
