using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Player
{
    public class Player : MonoBehaviour
    {
        public Rod.Rod Rod { get; private set; }
        public Inventory.Inventory Inventory { get; private set; }
        public Bucket.Bucket Bucket { get; private set; }
        public PlayerData playerData;

        void OnEnable()
        {
            Mechanic.Rod.Rod.OnFishCaught += OnFishCaught;
        }
        
        public void EquipRod(Rod.Rod rod)
        {
            Rod = rod;
        }

        public void OnFishCaught(Fish.Fish fish)
        {
            Bucket.AddToBucket(fish);
        }

        public Vector2 GetPlayerCameraDirection()
        {
            return transform.GetComponentInChildren<Camera>().transform.right;
        }
    }
}