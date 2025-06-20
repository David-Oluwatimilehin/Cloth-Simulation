
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RevisedParticle
{
    // Implementation of the Mass spring model for cloth 
    public class ClothBehaviour : MonoBehaviour
    {
        public SimulationValues simStats;

        //[SerializeField] bool setWind;
        public ParticleManager ParticleManager { get; private set; }
        public SpringManager SpringManager { get; private set; }
        
        private Transform cachedTransform;

        private int frameCounter = 0;
        private const int gizmoUpdateFrequency = 1;

        
        void Start()
        {
            cachedTransform = transform;
            // Creates the Particle and Spring Manager classes
            ParticleManager = new ParticleManager(simStats);
            SpringManager = new SpringManager(simStats);

            ParticleManager.SetupParticles(cachedTransform);
            SpringManager.SpawnSprings(ParticleManager.particleArr);

        }
        
        void FixedUpdate()
        {
            ParticleManager.CalculateForces(Time.fixedDeltaTime);
            SpringManager.UpdateSprings(Time.fixedDeltaTime);
            ParticleManager.UpdateParticles(Time.fixedDeltaTime);
        }
        
        private void OnDrawGizmos()
        {
            if (ParticleManager.IsUnityNull()) return;
            
            //
            frameCounter++;
            if (frameCounter % gizmoUpdateFrequency != 0) return; // Makes sure the draw call isn't higher than framerate

            SpringManager.Draw();
            ParticleManager.Draw();
            
        } 
    }
}
