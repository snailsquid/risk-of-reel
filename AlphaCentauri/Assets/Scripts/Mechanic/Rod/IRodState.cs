namespace Mechanic.Rod
{
    public interface IRodState
    {
        public void Enter();
        public void Execute();
        public void Exit();
    }
}