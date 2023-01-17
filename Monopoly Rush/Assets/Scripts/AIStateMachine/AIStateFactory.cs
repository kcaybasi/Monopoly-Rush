namespace AIStateMachine
{
    public class AIStateFactory
    {
        AIStateManager _context;
        
        public AIStateFactory(AIStateManager context)
        {
            _context = context;
        }

        public AIBaseState BuildingState()
        {
            return new AIBuildingState(_context,this);
        }

        public AIBaseState CollectingState()
        {
            return new AICollectingState(_context,this);
        }

        public AIBaseState MovementState()
        {
            return new AIMovementState(_context, this);
        }
    }
}
