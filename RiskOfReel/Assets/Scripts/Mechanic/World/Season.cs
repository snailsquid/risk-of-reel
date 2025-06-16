using System.Collections.Generic;
using UnityEngine;

namespace Mechanic.World
{
    [CreateAssetMenu(fileName = "Season", menuName = "RiskOfReel/World/Season")]
    public class Season : ScriptableObject
    {
        public string seasonName;

        [Tooltip("List of possible weather types for this season")]
        public List<Weather> weathers;
        public Material skyboxMaterial;
    }
}