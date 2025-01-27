
using DavidOluwatimilehin;
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
        private WindManager windManager;

        private SimulationValues sv;

        public int rows {get; private set; }
        public int columns { get; private set; }
       
        //private float windTime = 0f;

        readonly float radius;
        readonly float spacing;
        public ParticleManager(SimulationValues simValues) {

            
            //windEffect = false;
            
            rows = simValues.rows;
            columns = simValues.columns;
            spacing = simValues.spacing;
            radius = simValues.particleRadius;
            sv = simValues;
            
            particleArr = new Particle[rows, columns];

            force = new Force(simValues.windStrength,simValues.gravity);
            windManager = new WindManager(new Vector3(0, -1, 0), simValues.windStrength, 0.5f, 0.1f);
            
                        
        }
        
        public void SetupParticles(Transform parentTransform)
        {

            Vector3 origin = parentTransform.position;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {

                    Vector3 spawnPos = origin - new Vector3(x * spacing, y * spacing, 0);
                    Particle tempParticle = new Particle(spawnPos, sv.mass, sv.friction, sv.dragCoefficient);

                    if (y == 0)
                    {
                        tempParticle.SetMass(500);
                        tempParticle.IsFixed = true;
                    }

                    particleArr[x, y] = tempParticle;
                }
            }
            //particleArr[0, 0].SetMass(500);
            //particleArr[0, 0].IsFixed = true; // Top-left corner

            //particleArr[rows/2-1, 0].SetMass(500);
            //particleArr[rows/2-1,0].IsFixed = true;

            //particleArr[rows - 1, 0].SetMass(500);
            //particleArr[rows - 1, 0].IsFixed = true; // Bottom-right corner
            Debug.Log(particleArr.Length);

            


        }
        

        public void CalculateForces(float fdt)
        {
            float currentTime = Time.time;
            Vector3 windForce = windManager.GetWindForce(currentTime);
            Vector3 gravityForce = force.GenerateGravityForce();

            foreach (var particle in particleArr)
            {
                if (!particle.IsFixed)
                {
                    particle.AddForce(gravityForce);
                    particle.AddForce(windForce);
                    particle.SumInternalForces(fdt);
                }                

            }
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
                    
                    //Particle particle = new Particle(spawnPos, sv);
                    

                    if (y == 0)
                    {
                        //particle.IsFixed = true;              
                    }
                    //Debug.Log(particle.IsFixed);

                                        
                    //particleList.Add(particle);
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
                     
            foreach (var particle in particleArr)
            {
                Gizmos.color = particle.IsFixed ? Color.white : Color.red;
                Gizmos.DrawSphere(particle.pos, radius);
            }


        }

    }


} 