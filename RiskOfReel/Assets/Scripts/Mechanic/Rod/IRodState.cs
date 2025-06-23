namespace Mechanic.Rod
{
    public interface IRodState
    {
        public void Enter();
        public void UpdateState();
        public void Exit();
    }
}