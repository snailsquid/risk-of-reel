using UnityEngine;

namespace Mechanic.World
{
    [CreateAssetMenu(fileName = "Weather", menuName = "RiskOfReel/World/Weather")]
    public class Weather : ScriptableObject
    {
        public string weatherName;
        public GameObject weatherEffectPrefab;
        public AudioClip weatherSound;
    }
}