
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

namespace RevisedParticle
{
    public class ParticleManager
    {
        // Forces
        Force force;

        
        public Particle[,] particleArr { get; private set; }
        private SpringManager springManager;
        
        private SimulationValues sv;
        
        private int rows, columns;
        bool windEffect;
        private float windTime = 0f;

        readonly float radius;
        readonly float spacing;
        public ParticleManager(SimulationValues simValues) {

            //
            windEffect = false;
            
            rows = simValues.rows;
            columns = simValues.columns;
            spacing = simValues.spacing;
            radius = simValues.particleRadius;
            sv = simValues;
            
            particleArr = new Particle[rows, columns];

            force = new Force(simValues.windStrength,simValues.gravity);
            springManager = new SpringManager();
            
                        
        }
        
        public void SetupParticles(Transform parentTransform)
        {

            Vector3 origin = parentTransform.position;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {

                    Vector3 spawnPos = origin - new Vector3(x * spacing, y * spacing, 0);
                    Particle tempParticle = new Particle(spawnPos, sv);

                    if (y == 0)
                    {
                        tempParticle.SetMass(500);
                        tempParticle.IsFixed = true;
                    }

                    particleArr[x, y] = tempParticle;
                }
            }
            
            Debug.Log(particleArr.Length);

            springManager.SpawnSprings(particleArr, sv);


        }
        public void SetWindActing()
        {
            windEffect = !windEffect;
        }

        public void SpawnParticles(Transform parentTransform)
        {
            float posX = parentTransform.position.x;
            float posY = parentTransform.position.y;
            float posZ = parentTransform.position.z;

            Vector3 spawnPos = Vector3.zero;
                        
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    posX = -x + spacing;
                    posY = -y + spacing;

                    spawnPos = new Vector3(posX, posY, posZ);
                    
                    Particle particle = new Particle(spawnPos, sv);
                    

                    if (y == 0)
                    {
                        particle.IsFixed = true;              
                    }
                    Debug.Log(particle.IsFixed);

                                        
                    //particleList.Add(particle);
                }
            }

        }

        public void UpdateParticles(float deltaTime)
        {
            windTime += deltaTime;
            
            Vector3 windForce = Vector3.zero;
            Vector3 gravityForce = force.GenerateGravityForce();  

            if (windEffect)
                windForce += force.GenerateWindForce(windTime, 0.25f);           
            
            foreach (var particle in particleArr)
            {
                              
                particle.AddForce(gravityForce);
                particle.AddForce(windForce);
                particle.SumInternalForces(deltaTime);
                
                
            }

            springManager.UpdateSprings(deltaTime);

            foreach (var particle in particleArr)
            {
                particle.Update(deltaTime);
            }

        }
        

        public void DrawParticlesAndSprings()
        {
            if (particleArr.IsUnityNull()) return;

                     
            foreach (var particle in particleArr)
            {
                Gizmos.color = particle.IsFixed ? Color.white : Color.red;
                Gizmos.DrawSphere(particle.pos, radius);
            }

            springManager.DrawSprings();

        }

    }


} 