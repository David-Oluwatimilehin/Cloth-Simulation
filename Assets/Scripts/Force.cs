using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    public class Force {
        
        private float intensity;
        private float gravity;
        public Force(float strength, float gravityValue)
        {
            intensity = strength;
            gravity = gravityValue;
        }
        public Vector3 GenerateGravityForce()
        {
            return new Vector3(0, -gravity, 0);
        }

        
        public Vector3 GenerateWindForce(float time, float scale)
        {
            float noiseX = Mathf.PerlinNoise(time * scale, 0) * 2 - 1;
            float noiseY = Mathf.PerlinNoise(0, time * scale) * 2 - 1;

            return new Vector3(noiseX, noiseY, 0) * intensity;
        }
    }
}
