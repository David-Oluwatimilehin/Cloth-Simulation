using Unity.VisualScripting;
using UnityEngine;

namespace RevisedParticle
{
    public class ParticleManager
    {
        // Forces
        private WindForce _windForce;
        
        public Particle[,] particleArr { get; }
        private SpringManager _springManager;
        private SimulationValues _sv;
        
        
        private readonly float _radius;
        private readonly float _spacing;
        private readonly float _gravity;
        public int rows {get; }
        public int columns { get; }
        
        
        public ParticleManager(SimulationValues simValues) {
            
            rows = simValues.rows;
            columns = simValues.columns;
            _spacing = simValues.spacing;
            _radius = simValues.particleRadius;
            _gravity = simValues.gravity;
            _sv = simValues;
            
            particleArr = new Particle[rows, columns];
            
            _windForce = new WindForce(new Vector3(0, -1, 0), simValues.windStrength, 0.5f, 0.1f);
        }
        
        public void SetupParticles(Transform parentTransform)
        {
            Vector3 origin = parentTransform.position;
            
            // The functions populate the 2d grid by row x column count
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Vector3 spawnPos = origin - new Vector3(x * _spacing, y * _spacing, 0);
                    Particle tempParticle = new Particle(spawnPos, _sv.mass, _sv.friction, _sv.dragCoefficient);

                    if (y == 0) // The first row is fixed
                    {
                        tempParticle.SetMass(500);
                        tempParticle.IsFixed = true;
                    }

                    particleArr[x, y] = tempParticle;
                }
            }
            Debug.Log(particleArr.Length);

        }

        public void CalculateForces(float fdt)
        {
            float currentTime = Time.time;
            Vector3 windForce = _windForce.GetWindForce(currentTime);

            foreach (var particle in particleArr)
            {
                // If the particle isn't fixed add the forces 
                if (!particle.IsFixed)
                {
                    particle.AddForce(new Vector3(0,-_gravity));
                    particle.AddForce(windForce);
                    particle.SumInternalForces(fdt);
                }                

            }
        }

        public void UpdateParticles(float deltaTime)
        {
            foreach (var particle in particleArr)
            {
                particle.Update(deltaTime);
            }

        }
        
        public void Draw()
        {
            if (particleArr.IsUnityNull()) return;
                   
            // Draws the particles using Gizmos
            foreach (var particle in particleArr)
            {
                Gizmos.color = particle.IsFixed ? Color.white : Color.red;
                Gizmos.DrawSphere(particle.pos, _radius);
            }
        }

    }


} 