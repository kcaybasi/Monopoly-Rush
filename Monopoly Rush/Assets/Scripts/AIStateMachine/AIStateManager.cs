using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIStateMachine
{
    public class AIStateManager : MonoBehaviour
    {
        public AIBaseState CurrentState { get; set; }
        private AIStateFactory _stateFactory;

        NavMeshAgent _navMeshAgent;
        public Collector collector;
        public int selectedListNo;
        private readonly float _reachDistance =3f;
        int _brickCapacity;
        public GameObject reachedBuilding;


        private void Start()
        {
            //Randomize brick capacity for every AI 

            _brickCapacity = Random.Range(10, 25);

            // Get components

            _navMeshAgent = GetComponent<NavMeshAgent>();
            collector = GetComponent<Collector>();

            //Set initial state
            _stateFactory = new AIStateFactory(this);
            CurrentState = _stateFactory.MovementState();
            CurrentState.EnterState();
            // Subscribe to game finish event

            C_GameManager.OnGameFinish += C_GameManager_OnGameFinish;
        }

        private void C_GameManager_OnGameFinish()
        {
            this.gameObject.SetActive(false);
            GetComponent<Collector>().enabled = false;
        }

        private void Update()
        {
            CurrentState.UpdateState();
        }

        public void SwitchState(AIBaseState state)
        {
            CurrentState = state;
            state.EnterState();
        }

        public void MoveTo(Vector3 targetPos)
        {
            _navMeshAgent.SetDestination(targetPos);
        }


        public GameObject GetClosestTarget(List<GameObject> targetList)
        {

            List<float> distanceList = new List<float>();

            for (int i = 0; i < targetList.Count; i++)
            {
                float distance = Vector3.Magnitude(transform.position - targetList[i].transform.position);
                distanceList.Add(distance);
            }

            var minValue = Mathf.Min(distanceList.ToArray());
            int index = distanceList.IndexOf(minValue);
            
            return targetList[index].transform.gameObject;
        }

        public bool TargetReached(Vector3 targetPos)
        {
            return Vector3.Magnitude(transform.position - targetPos) < _reachDistance;
        }

        public bool BrickCapacityFull()
        {
            return collector.collectedBricks >= _brickCapacity;
        }

        public List<GameObject> GetDecidedTargetList()
        {
            if (collector.collectedBricks < _brickCapacity)
            {
                selectedListNo = 0;
                return C_GameManager.instance.supply_Line_List;
            }
            else
            {
                selectedListNo = 1;
                return C_GameManager.instance.inactive_Building_List;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InactiveBuilding"))
            {
                reachedBuilding = other.gameObject;
            }

            if (other.CompareTag("ActiveBuilding")) // To avoid stucking at finished building. Later on stop timing will be add. 
            {
                SwitchState(_stateFactory.MovementState());
            }
        }
    }
}
