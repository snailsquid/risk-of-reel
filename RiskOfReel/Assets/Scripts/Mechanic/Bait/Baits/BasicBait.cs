namespace Mechanic.Bait.Baits
{
    public class BasicBait : Bait
    {
        public override bool Condition()
        {
            return true;
        }

        public override bool Activate()
        {
            return true;
        }
    }
}