using UnityEngine;

namespace Mechanic.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "RiskOfReel/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public int Strength { get; private set; }
        public int Endurance { get; private set; }
        public int Speed { get; private set; }
    }
}