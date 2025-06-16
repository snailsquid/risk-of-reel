using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanic.World
{
    public class World : MonoBehaviour
    {
        [Header("Season")]
        public Season currentSeason;
        public List<Season> seasons;

        [Header("Weather")]
        public Weather currentWeather;
        public List<Weather> weathers;
        
        [Header("Time")]
        public Time time;
        
        #region Singleton
        public static World Instance { get; set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
        }
        #endregion
    }
}