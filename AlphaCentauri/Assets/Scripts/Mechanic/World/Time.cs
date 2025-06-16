using System;
using UnityEngine;

namespace Mechanic.World
{
    [Serializable]
    public class Time
    {
        [SerializeField] public int currentTick = 0;
        [SerializeField] public int ticksPerSecond ;
        [Tooltip("How many seconds for one full in game day+night")]
        [SerializeField] public int secondsPerDayCycle;

        public static Action OnDayCycleFinished;

        public void Tick(int seconds)
        {
            if (IsDayCycleFinished())
            {
                OnDayCycleFinished?.Invoke();
            }
            else
            {
                currentTick += seconds * ticksPerSecond;
            }
        }

        public void ResetTick()
        {
            currentTick = 0;
        }

        public bool IsDayCycleFinished()
        {
            return currentTick >= ticksPerSecond*secondsPerDayCycle;
        }
    }
}