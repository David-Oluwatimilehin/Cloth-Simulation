using DavidOluwatimilehin;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace RevisedParticle
{
    public class SpringManager
    {
        private List<Spring> springList;
        
        public SpringManager() 
        {
            springList = new List<Spring>();
            
        }

        public void CreateSpring(Particle a, Particle b, SimulationValues sv, bool isDiag)
        {
            // TODO: Refactor this to accept a single constructor 
            Spring spring;
            
            if (isDiag)
            {
                float diagRestLength = sv.spacing * Mathf.Sqrt(2);
                spring = new Spring(a, b, diagRestLength, sv.shearSpringConstant, sv.shearDampValue);
            }
            else
            {
                spring = new Spring(a, b, sv.spacing, sv.springConstant, sv.dampValue);
            }
            
            springList.Add(spring);
        }
        public void UpdateSprings(float dt)
        {
            foreach (var spring in springList)
            {
                spring.ApplyForce(dt);
            }
        }
        public void SpawnSprings(Particle[,] particleArray, SimulationValues simValues)
        {
            for (int i = 0; i < simValues.rows; i++)
            {
                for (int j = 0; j < simValues.columns; j++)
                {
                    Particle current = particleArray[i, j];

                    if (i < simValues.rows - 1)
                        CreateSpring(current, particleArray[i + 1, j], simValues,false);

                    if (j < simValues.columns - 1)
                        CreateSpring(current, particleArray[i, j + 1], simValues,false);

                    if (i < simValues.rows - 1 && j < simValues.columns - 1)
                        CreateSpring(current, particleArray[i + 1, j + 1], simValues, true);

                    if (i < simValues.rows - 1 && j > 0)
                        CreateSpring(current, particleArray[i + 1, j - 1], simValues, true);
                }
            }

            Debug.Log("Spring Count: " + springList.Count);

        }
        public void DrawSprings()
        {
            if (springList.IsUnityNull()) return;
            
            Gizmos.color = Color.green;
            
            foreach (Spring s in springList)
            {
                Vector3 posA = s.startParticle.pos;
                Vector3 posB = s.endParticle.pos;

                Debug.DrawLine(posA, posB, Color.green);
                
            }
        }
    }
}
