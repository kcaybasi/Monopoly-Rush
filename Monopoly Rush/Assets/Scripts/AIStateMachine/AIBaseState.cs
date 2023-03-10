using UnityEngine;

namespace AIStateMachine
{
     public abstract class AIBaseState
     {
          protected AIStateManager StateManager;
          protected AIStateFactory StateFactory;

          protected AIBaseState(AIStateManager stateManager, AIStateFactory stateFactory)
          {
               StateManager = stateManager;
               StateFactory = stateFactory;
          }

          public abstract void EnterState();
          public   abstract void UpdateState();
          public  abstract void ExitState();
          public abstract void OnTriggerEnter(Collider other);

          protected void SwitchState(AIBaseState newState)
          {
               ExitState();
               newState.EnterState();
               StateManager.CurrentState = newState;
          }
     }
}
