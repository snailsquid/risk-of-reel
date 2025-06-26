using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "RiskOfReel/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public int strength; // Max Power
        public int endurance;
        public int speed;
    }
}