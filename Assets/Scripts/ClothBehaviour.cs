using DavidOluwatimilehin;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RevisedParticle
{
    public class ClothBehaviour : MonoBehaviour
    {
        public SimulationValues simStats;

        public ParticleManager manager { get; private set; }
        private Transform cachedTransform;

        private int frameCounter = 0;
        private const int gizmoUpdateFrequency = 1;

        [SerializeField]
        private Logger _logger;

        
        void Start()
        {
            cachedTransform = transform;
            
            manager = new ParticleManager(simStats);

            manager.SetupParticles(cachedTransform);

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            manager.UpdateParticles(Time.fixedDeltaTime);
        }
        
        private void OnDrawGizmos()
        {
                
            if (manager.IsUnityNull()) return;
            
            frameCounter++;
            if (frameCounter % gizmoUpdateFrequency != 0) return;

            //Gizmos.color = Color.red;

            manager.DrawParticlesAndSprings();
        } 

        void Log(object message)
        {
            if (_logger)
                _logger.Log(message, this);
        }
    }
}
