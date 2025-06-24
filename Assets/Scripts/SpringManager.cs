
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RevisedParticle
{
    public class SpringManager
    {
        
        private List<Spring> springList;
        private readonly SimulationValues simValues;
        
        public SpringManager(SimulationValues sv) 
        {
            springList = new List<Spring>();
            simValues = sv;

        }
        public void CreateSpring(Particle a, Particle b)
        {
            StructuralSpring spring = new StructuralSpring(a, b, simValues.spacing, simValues.springConstant, simValues.dampValue);      
            springList.Add(spring);
        }
        public void CreateDiagonalSpring(Particle a, Particle b,float diagRestLength)
        {
            ShearSpring shearSpring = new ShearSpring(a, b, diagRestLength, simValues.shearSpringConstant, simValues.shearDampValue);
            springList.Add(shearSpring);
        }
        
        
        public void UpdateSprings(float dt)
        {
            foreach (var spring in springList)
            {
                spring.ApplyForce(dt); // Calculate the spring forces
            }
        }
        public void SpawnSprings(Particle[,] particleArray)
        {
            // Gets the rest distance for shear Springs
            float diagLength = Mathf.Sqrt(2) * simValues.spacing;
            
            for (int i = 0; i < simValues.columns; i++) 
            {
                for (int j = 0; j < simValues.rows; j++) 
                {
                    Particle current = particleArray[j, i]; // current particle at [row, column]
                    
                    // For Structural springs
                    if (i < simValues.columns - 1) 
                        CreateSpring(current, particleArray[j, i + 1]); 
                    
                    if (j < simValues.rows - 1) 
                        CreateSpring(current, particleArray[j + 1, i]); 

                    // For shearing springs (connecting diagonally)
                    if (i < simValues.columns - 1 && j < simValues.rows - 1)
                        CreateDiagonalSpring(current, particleArray[j + 1, i + 1], diagLength); 
                    
                    if (i > 0 && j < simValues.rows - 1) 
                        CreateDiagonalSpring(current, particleArray[j + 1, i - 1], diagLength); 
                    
                }
            }

        }
        public void Draw()
        {
            // Gets Spring to draw Gizmos.
            foreach (var spring in springList)
            {
                spring.Draw();
            }

        }
    }
}
