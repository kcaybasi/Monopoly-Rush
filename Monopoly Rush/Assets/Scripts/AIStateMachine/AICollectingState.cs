using UnityEngine;

namespace AIStateMachine
{
    public class AICollectingState : AIBaseState
    {
        GameObject _selectedSupplyLine;
        readonly float _wayPointTolerance = 8f;
        BrickPath _selectedBrickPath;
        private int _currentWaypointIndex;
        private float _timeSinceLastWaypoint;
        Vector3 _currentLocation;
        readonly float _dwellTime=0.2f;

        public AICollectingState(AIStateManager context, AIStateFactory aiStateFactory):base(context,aiStateFactory)
        {
            
        }

        public override void EnterState()
        {
            if (StateManager.selectedListNo == 0)
            {
                _selectedSupplyLine = StateManager.GetClosestTarget(CGameManager.Instance.supply_Line_List);
            }
            _selectedBrickPath = _selectedSupplyLine.transform.GetChild(10).GetComponent<BrickPath>();
            _currentLocation = _selectedBrickPath.transform.position;
        }

        public override void UpdateState()
        {
            if (!StateManager.BrickCapacityFull())
            {
                FollowBrickPath(StateManager);
            }      
            else
            {
                SwitchState(StateFactory.MovementState());
            }

        }

        public override void ExitState()
        {
            
        }

        public override void CheckSwitchStates()
        {
            
        }

        private void FollowBrickPath(AIStateManager aiPlayer)
        {
            Vector3 nextPosition = _currentLocation;

            if (_selectedBrickPath != null)
            {
                if (AtWayPoint(aiPlayer.transform.position))
                {
                    _timeSinceLastWaypoint = 0f;
                    CycleWayPoint();

                }
                nextPosition = GetCurrentWayPoint();

            }
            if (_timeSinceLastWaypoint > _dwellTime)
            {
                aiPlayer.MoveTo(nextPosition);

            }

            _timeSinceLastWaypoint += Time.deltaTime;
        }

        private bool AtWayPoint(Vector3 playerPos)
        {
            float distanceToWayPoint = Vector3.Distance(playerPos, GetCurrentWayPoint());
            return distanceToWayPoint < _wayPointTolerance;
        }

        private Vector3 GetCurrentWayPoint()
        {
            return _selectedBrickPath.GetWaypoint(_currentWaypointIndex);
        }

        private void CycleWayPoint()
        {
            _currentWaypointIndex = _selectedBrickPath.GetNextIndex(_currentWaypointIndex);
        }
    }
}
