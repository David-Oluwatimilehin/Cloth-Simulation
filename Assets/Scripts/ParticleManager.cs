using Unity.VisualScripting;
using UnityEngine;

namespace RevisedParticle
{
    public class ParticleManager
    {
        // Forces
        private WindForce _windForce;
        public Particle[,] ParticleArr { get; }
        
        private SimulationValues _sv;
        
        private readonly float _radius;
        private readonly float _spacing;
        private readonly float _gravity;
        
        public ParticleManager(SimulationValues simValues) 
        {
            _spacing = simValues.spacing;
            _radius = simValues.particleRadius;
            _gravity = simValues.gravity;
            _sv = simValues;
            
            ParticleArr = new Particle[simValues.rows, simValues.columns];
            
            _windForce = new WindForce(new Vector3(0, -1, 0), simValues.windStrength, simValues.windChangability, simValues.changeSpeed);
        }
        
        public void SetupParticles(Transform parentTransform)
        {
            Vector3 origin = parentTransform.position;
            
            // The functions populate the 2d grid by row x column count
            for (int y = 0; y < _sv.columns; y++)
            {
                for (int x = 0; x < _sv.rows; x++)
                {
                    Vector3 spawnPos = origin - new Vector3(x * _spacing, y * _spacing, 0);
                    Particle tempParticle = new Particle(spawnPos, _sv.mass, _sv.friction, _sv.dragCoefficient);

                    if (y == 0) // The first row is fixed
                    {
                        tempParticle.SetMass(500);
                        tempParticle.IsFixed = true;
                    }

                    ParticleArr[x, y] = tempParticle;
                }
            }
            Debug.Log(ParticleArr.Length);

        }

        public void CalculateForces(float fdt)
        {
            float currentTime = Time.time;
            Vector3 windForce = _windForce.GetWindForce(currentTime);

            foreach (var particle in ParticleArr)
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
            foreach (var particle in ParticleArr)
            {
                particle.Update(deltaTime);
            }

        }
        
        public void Draw()
        {
            if (ParticleArr.IsUnityNull()) return;
                   
            // Draws the particles using Gizmos
            foreach (var particle in ParticleArr)
            {
                Gizmos.color = particle.IsFixed ? Color.white : Color.red;
                Gizmos.DrawSphere(particle.pos, _radius);
            }
        }

    }


} 