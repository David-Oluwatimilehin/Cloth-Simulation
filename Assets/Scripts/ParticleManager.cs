using DavidOluwatimilehin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace RevisedParticle
{
    public class ParticleManager
    {
        //public List<Particle> particleList { get; private set; }
        public Particle[,] particleArr { get; private set; }
        private SpringManager springManager;
        private SimulationValues sv;

        int rows, columns;

        private Vector3 windForce;
        private float windTime = 0f;

        readonly float radius;
        readonly float spacing;
        public ParticleManager(SimulationValues simValues) {
                 
            //
                              
            rows = simValues.rows;
            columns = simValues.columns;
            spacing = simValues.spacing;
            radius = simValues.particleRadius;
            
            sv = simValues;
            
            particleArr = new Particle[rows, columns];
            
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

        public Vector3 GenerateWindForce(float time, float scale, float intensity)
        {
            float noiseX = Mathf.PerlinNoise(time * scale, 0) * 2 - 1; // Range [-1, 1]
            float noiseY = Mathf.PerlinNoise(0, time * scale) * 2 - 1;

            // Generate wind force vector
            return new Vector3(noiseX, noiseY,0) * intensity;
        }

        public void UpdateParticles(float deltaTime)
        {
            windTime += deltaTime;
            windForce = GenerateWindForce(windTime, 0.1f, 10f);

            foreach (var particle in particleArr)
            {
                particle.SumForces(windForce);
            }

            springManager.UpdateSprings(deltaTime);

            foreach (var particle in particleArr)
            {
                particle.Update(deltaTime); 
                //particle.ClearForce();
            }                 
            
        }


        public void DrawParticles()
        {
            
            if (particleArr.IsUnityNull()) return;

            //Debug.Log("Got To Here");
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Gizmos.DrawSphere(particleArr[i, j].pos, radius);
                }
            }
            springManager.DrawSprings();

        }

    }


} 