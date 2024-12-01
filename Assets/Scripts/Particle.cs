using DavidOluwatimilehin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    // TODO: Change the SimVal to accept fixed mass 
    public class Particle
    {
        public Vector3 pos { get; private set; }
        public bool IsFixed { get => _isFixed; set => _isFixed = value; }

        private Vector3 prevPos;
        private Vector3 acc;

        private float _gravity;
        private float _friction;
        private float _dragCoefficient;
        private float _mass;
        private bool _isFixed;

        private float windIntensity;

        public Particle(Vector3 initPos, SimulationValues constants)
        {
            pos = initPos;
            prevPos = initPos;

            _mass = constants.mass;
            _friction = constants.friction;
            _dragCoefficient = constants.dragCoefficient;
            
            acc = Vector3.zero;
            _isFixed = false;

        }

        public void AddForce(Vector3 force)
        {
            //if(force == Vector3.zero) return;

            acc += force / _mass;
        }
        public void SetMass(float mass)
        {
            _mass = mass;
        }
        public void SumInternalForces(float time)
        {
            Vector3 dragForce = -GetRelativeVelocity(time).normalized * _dragCoefficient * (GetRelativeVelocity(time).magnitude * GetRelativeVelocity(time).magnitude);
            AddForce(dragForce);

            Vector3 frictionForce= -GetRelativeVelocity(-time).normalized * _friction;
            AddForce(frictionForce);
        }
        
                     
        
        public void Update(float time)
        {
            if (_isFixed) return;

            Vector3 tempPos = pos;

            pos += (pos - prevPos) * (1 - _friction) + acc * time * time;

            prevPos = tempPos;

            acc = Vector3.zero;
        }
        public Vector3 GetRelativeVelocity(float deltaTime)
        {
            return acc * deltaTime;
        }
        

    }
}
