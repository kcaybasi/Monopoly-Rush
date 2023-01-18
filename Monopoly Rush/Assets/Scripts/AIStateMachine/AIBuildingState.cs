using UnityEngine;

namespace AIStateMachine
{
    public class AIBuildingState : AIBaseState
    {
        public AIBuildingState(AIStateManager context, AIStateFactory aiStateFactory):base(context,aiStateFactory)
        {
            
        }

        public override void EnterState()
        {
        
        }

        public override void UpdateState()
        {
            if (StateManager.reachedBuilding != null)
            {
                if (StateManager.reachedBuilding.CompareTag("ActiveBuilding") || StateManager.collector.CollectedBricks <= 0)
                {
                    SwitchState(StateFactory.MovementState());
                }
            }

        }

        public override void ExitState()
        {
            
        }

        public override void CheckSwitchStates()
        {
            
        }

        public override void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
