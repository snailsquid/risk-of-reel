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
        
        /**
         * 
         */
        public IEnumerator ThrowCoroutine(Vector2 normalizedDirection, float normalizedPower)
        {
            var lineLength = _rod.attachedFishingLine.length;
            var power = normalizedPower * lineLength;
            var rotation = _rod.GetPlayer().GetPlayerCameraDirection().y;
            var direction = normalizedDirection * power;
            Debug.Log("power : " + lineLength + " * " + normalizedPower);
            Debug.Log("direction : " + normalizedDirection + " * " + power);
            Debug.Log("rotate : " + rotation);
            transform.position = _rod.transform.position + new Vector3(direction.x, 0, direction.y);
            // transform.Rotate(_rod.GetPlayer().transform.position, rotation);
            yield return new WaitForSeconds(bobberData.bobberLandDuration);
        }
    }
}