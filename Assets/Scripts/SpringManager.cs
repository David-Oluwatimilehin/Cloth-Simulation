
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RevisedParticle
{
    public class SpringManager
    {
        
        private readonly List<Spring> _springList;
        
        private readonly SimulationValues _simValues;
        
        public SpringManager(SimulationValues sv)
        {
            _springList = new List<Spring>();
            _simValues = sv;

        }
        
        private void CreateSpring(Particle p1, Particle p2)
        {
            StructuralSpring spring = new StructuralSpring(p1, p2, _simValues.spacing, _simValues.springConstant, _simValues.dampValue);      
            _springList.Add(spring);
        }

        private void CreateDiagonalSpring(Particle p1, Particle p2, float diagRestLength)
        {
            ShearSpring shearSpring = new ShearSpring(p1, p2, diagRestLength, _simValues.shearSpringConstant,
                _simValues.shearDampValue);
            _springList.Add(shearSpring);
        }

        private void CreateBendingSpring(Particle p0, Particle p1, Particle p2, Particle p3, float restAngle)
        {
            BendingSpring bendingSpring = new BendingSpring(p0,p1, p2,p3,restAngle,_simValues.bendSpringConstant,_simValues.bendDamping);
            _springList.Add(bendingSpring);
        }

        public void UpdateSprings(float dt)
        {
            foreach (var spring in _springList)
            {
                spring.ApplyForce(dt); // Calculate the spring forces
            }
        }
        
        public void SpawnSprings(Particle[,] particleArray)
        {
            // Gets the rest distance for shear Springs
            float diagLength = Mathf.Sqrt(2) * _simValues.spacing;
            //float restAngle = Mathf.PI; 
            for (int i = 0; i < _simValues.columns; i++) 
            {
                for (int j = 0; j < _simValues.rows; j++) 
                {
                    Particle current = particleArray[j, i]; // current particle at [row, column]
                    
                    // For Structural springs
                    if (i < _simValues.columns - 1) 
                        CreateSpring(current, particleArray[j, i + 1]); 
                    
                    if (j < _simValues.rows - 1) 
                        CreateSpring(current, particleArray[j + 1, i]); 

                    // For shearing springs (connecting diagonally)
                    if (i < _simValues.columns - 1 && j < _simValues.rows - 1)
                        CreateDiagonalSpring(current, particleArray[j + 1, i + 1], diagLength); 
                    
                    if (i > 0 && j < _simValues.rows - 1) 
                        CreateDiagonalSpring(current, particleArray[j + 1, i - 1], diagLength);

                    if (i < _simValues.columns - 1 && j < _simValues.rows - 1)
                    {
                        Particle p1 = particleArray[j, i];
                        Particle p2 = particleArray[j, i + 1];
                        Particle p3 = particleArray[j + 1, i];     
                        Particle p4 = particleArray[j + 1, i + 1]; 

                        // triangle normals in the rest (flat) configuration
                        Vector3 n1 = Vector3.Cross(p3.pos - p1.pos, p2.pos - p1.pos).normalized; 
                        Vector3 n2 = Vector3.Cross(p2.pos - p4.pos, p1.pos - p4.pos).normalized;

                        float restAngle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(n1, n2), -1f, 1f));
                        
                        CreateBendingSpring(p1, p2, p3, p4, restAngle);
                        
                        p1 = particleArray[j, i];
                        p2 = particleArray[j + 1, i];
                        p3 = particleArray[j, i + 1];     
                        p4 = particleArray[j + 1, i + 1]; 

                        CreateBendingSpring(p1, p2, p3, p4,restAngle);
                    }
                }
            }
            
            Debug.Log("Total Spring Count: " + _springList.Count);
            
        }
        
        public void Draw()
        {
            // Gets Spring to draw Gizmos.
            foreach (var spring in _springList)
            {
                spring.Draw();
            }

        }
    }
}
