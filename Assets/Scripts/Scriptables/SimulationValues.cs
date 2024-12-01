using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavidOluwatimilehin
{
    [CreateAssetMenu(fileName = "SimValue", menuName = "ScriptableObjects/Simulation Values")]
    
    public class SimulationValues : ScriptableObject
    {
        [Header("Constants")]
        public float mass = 1;
        public float gravity = 9.81f;
        public float windStrength = 10f;
        public float dragCoefficient = 1f;

        public float springConstant=10f;
        public float dampValue = 0.9f;
        public float friction=0.95f;

        [Header("Shear Springs")]
        public float shearSpringConstant=7f;
        public float shearDampValue = 0.7f;
        
        

        [Header("Setup")]
        public int rows=10;
        public int columns=10;
        public float spacing=1;
        public float particleRadius=0.1f;
    }
}
