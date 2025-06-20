
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
            // Resting Distance for Shear Springs
            float diagLength = Mathf.Sqrt(2) * simValues.spacing;
            int bendSpringCount = 0;
            for (int i = 0; i < simValues.rows; i++)
            {
                for (int j = 0; j < simValues.columns; j++)
                {
                    Particle current = particleArray[i, j];

                    // For Structural Springs
                    if (i < simValues.rows - 1)
                        CreateSpring(current, particleArray[i + 1, j]);

                    if (j < simValues.columns - 1)
                        CreateSpring(current, particleArray[i, j + 1]);

                    // For Shearing Springs
                    if (i < simValues.rows - 1 && j < simValues.columns - 1)
                        CreateDiagonalSpring(current, particleArray[i + 1, j + 1], diagLength);
                        

                    if (i < simValues.rows - 1 && j > 0)
                        CreateDiagonalSpring(current, particleArray[i + 1, j - 1], diagLength);

                    // For Bending Springs
                    if (i + 2 < simValues.rows)
                        CreateBendingSpring(current, particleArray[i + 2, j], simValues.spacing * 2);
                        //bendSpringCount++;

                    if (j +2 < simValues.columns)
                        CreateBendingSpring(current, particleArray[i, j + 2], simValues.spacing * 2);
                        //bendSpringCount++;

                }
            }

        }
        public void Draw()
        {
            //if (springList.IsUnityNull()) return;

            //Gizmos.color = Color.green;

            foreach (var spring in springList)
            {
                spring.Draw();
            }

        }
    }
}
