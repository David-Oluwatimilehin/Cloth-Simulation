
using UnityEngine;

namespace RevisedParticle
{
    public class WindForce
    {
        // 
        private Vector3 _windDirection;
        private float _strength;
        private readonly float _variability;
        private readonly float _windSpeed;
        public WindForce(Vector3 initialDirection, float initialStrength, float changeability, float changeSpeed)
        {
            // Initialises the values within constructor
            _windDirection = initialDirection.normalized;
            _strength = initialStrength;
            _variability = changeability;
            _windSpeed = changeSpeed;
        }

        public Vector3 GetWindForce(float time)
        {
            // Uses Perlin Noise to return random values between 0.0 and the time multiplied by windspeed
            float offSetX = Mathf.PerlinNoise(time * _windSpeed, 0.0f);
            float offSetY = Mathf.PerlinNoise(0.0f, time * _windSpeed);
            float offSetZ = Mathf.PerlinNoise(time * _windSpeed, 0.0f);

            Vector3 dynamicDirection = _windDirection + new Vector3(offSetX, offSetY, offSetZ) * _variability;

            return dynamicDirection.normalized * _strength;
        }
        public void UpdateWindValue(Vector3 newDirection, float newStrength)
        {
            _windDirection = newDirection.normalized;
            _strength = newStrength;
        }
    }
}
