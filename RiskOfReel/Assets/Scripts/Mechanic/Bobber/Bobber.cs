using System.Collections;
using UnityEngine;

namespace Mechanic.Bobber
{
    public class Bobber : MonoBehaviour
    {
        [SerializeField] private BobberData bobberData;
        private Rod.Rod _rod;

        public void Initialize(Rod.Rod rod)
        {
            _rod = rod;
        }
        
        public IEnumerator ThrowCoroutine()
        {
            yield return new WaitForSeconds(bobberData.bobberLandDuration);
        }
    }
}