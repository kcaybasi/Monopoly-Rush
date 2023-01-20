using System.Collections.Generic;
using UnityEngine;

namespace AIStateMachine
{
    public class AIMovementState : AIBaseState
    {
        GameObject _destination;
        List<GameObject> _decidedList = new List<GameObject>();

        public AIMovementState(AIStateManager context, AIStateFactory aiStateFactory):base(context, aiStateFactory)
        {
            
        }
        
        public override void EnterState()
        {
            _decidedList = StateManager.GetDecidedTargetList();
            _destination = StateManager.GetClosestTarget(_decidedList);
            StateManager.MoveTo(_destination.transform.position);
        }

        public override void UpdateState()
        { 
            if (StateManager.TargetReached(_destination.transform.position))
            {
                if (StateManager.selectedListNo == 0) // Checks which list we selected 
                    SwitchState(StateFactory.CollectingState());
                else
                    SwitchState(StateFactory.BuildingState());
            }
        }
        public override void ExitState()
        {
            
        }
        public override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InactiveBuilding"))
            {
                StateManager.reachedBuilding = other.gameObject;
            }
            if (other.CompareTag("ActiveBuilding")) // To avoid stuck at finished building 
            {
                SwitchState(StateFactory.MovementState());
            }
        }
    }
}
