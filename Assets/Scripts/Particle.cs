
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    // TODO: Change the SimVal to accept fixed mass 
    public class Particle
    {
        public Vector3 pos { get; private set; }
        private Vector3 prevPos;
        private Vector3 acc;
        private float _friction;
        public bool IsFixed { get => _isFixed; set => _isFixed = value; }
        
        private float _dragCoefficient;
        private float _mass;
        private bool _isFixed;

        

        public Particle(Vector3 initPos, float mass, float friction, float dragValue)
        {
            pos = initPos;
            prevPos = initPos;

            _mass = mass;
            _friction = friction;
            _dragCoefficient = dragValue;
            
            acc = Vector3.zero;
            _isFixed = false;

        }

        public void AddForce(Vector3 force)
        {
            if(force == Vector3.zero) return;

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

            Vector3 frictionForce= -GetRelativeVelocity(time).normalized * _friction;
            AddForce(frictionForce);
        }
        
                     
        
        public void Update(float time)
        {
            if (_isFixed) return;

            Vector3 tempPos = pos; //

            pos = (tempPos * 2) - prevPos + acc * (1 - _friction) * time * time;

            prevPos = tempPos;

            acc = Vector3.zero;
        }
        public Vector3 GetRelativeVelocity(float deltaTime)
        {
            return acc * deltaTime;
        }
        

    }
}
