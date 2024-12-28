using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavidOluwatimilehin
{
    public class WindManager
    {

        private Vector3 windDirection;
        private float strength;
        private float variability;
        private float windSpeed;
        public WindManager(Vector3 initialDirection, float initialStrength, float changability, float changeSpeed)
        {
            windDirection = initialDirection.normalized;
            strength = initialStrength;
            variability = changability;
            windSpeed = changeSpeed;
        }

        public Vector3 GetWindForce(float time)
        {
            float OffSetX = Mathf.PerlinNoise(time * windSpeed, 0.0f);
            float OffSetY = Mathf.PerlinNoise(0.0f, time * windSpeed);
            //float OffSetZ = Mathf.PerlinNoise(time * windSpeed, 0.0f);

            Vector3 dynamicDirection = windDirection + new Vector3(OffSetX, OffSetY, 0) * variability;

            return dynamicDirection.normalized * strength;
        }
        public void UpdateWind(Vector3 newDirection, float newStrength)
        {
            windDirection = newDirection.normalized;
            strength = newStrength;
        }
    }
}
