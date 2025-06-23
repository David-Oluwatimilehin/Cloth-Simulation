
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
        public void CreateBendingSpring(Particle a, Particle b, float restAngle)
        {
            BendingSpring bendSpring = new BendingSpring(a, b, simValues.bendSpringConstant, restAngle);
            springList.Add(bendSpring);
        }
        
        public void UpdateSprings(float dt)
        {
            foreach (var spring in springList)
            {
                spring.ApplyForce(dt);
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
                        CreateSpring(current, particleArray[j, i + 1]); // [row, column + 1]
                    
                    if (j < simValues.rows - 1) 
                        CreateSpring(current, particleArray[j + 1, i]); // [row + 1, column]

                    // For shearing springs (connecting diagonally)
                    // Connect to the bottom-right particle (next row, next column)
                    if (i < simValues.columns - 1 && j < simValues.rows - 1)
                        CreateDiagonalSpring(current, particleArray[j + 1, i + 1], diagLength); // Correct: [row + 1, column + 1]

                    
                    if (i > 0 && j < simValues.rows - 1) // Corrected: ensure 'i-1' and 'j+1' are valid
                        CreateDiagonalSpring(current, particleArray[j + 1, i - 1], diagLength); // Corrected: [row + 1, column - 1]

                    // For Bending Springs (connecting to particles two steps away)
                    // Uncomment and correct these if you intend to use them.
                    /*
                    // Connect to the particle two columns to the right (same row, column + 2)
                    if (i + 2 < simValues.columns)
                        CreateBendingSpring(current, particleArray[j, i + 2], simValues.spacing * 2); // Corrected: [row, column + 2]

                    // Connect to the particle two rows down (row + 2, same column)
                    if (j + 2 < simValues.rows)
                        CreateBendingSpring(current, particleArray[j + 2, i], simValues.spacing * 2); // Corrected: [row + 2, column]
                    */
                }
            }

        }
        public void Draw()
        {
            // Draws Gizmos.
            foreach (var spring in springList)
            {
                spring.Draw();
            }

        }
    }
}
