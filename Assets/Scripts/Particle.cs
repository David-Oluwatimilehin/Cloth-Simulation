
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    public class Particle
    {
        public Vector3 pos { get; private set; }
        private Vector3 _prevPos;
        private Vector3 _acc;
        private float _friction;
        public bool IsFixed { get => _isFixed; set => _isFixed = value; }
        
        private float _dragCoefficient;
        private float _mass;
        private bool _isFixed;

        

        public Particle(Vector3 initPos, float mass, float friction, float dragValue)
        {
            pos = initPos;
            _prevPos = initPos;

            _mass = mass;
            _friction = friction;
            _dragCoefficient = dragValue;
            
            _acc = Vector3.zero;
            _isFixed = false;

        }

        public void AddForce(Vector3 force)
        {
            if(force == Vector3.zero) return;

            _acc += force / _mass;
        }
        public void SetMass(float mass)
        {
            _mass = mass;
        }
        public void SumInternalForces(float time)
        {
            // Calcualates the 
            Vector3 dragForce = -GetRelativeVelocity(time).normalized * (_dragCoefficient * (GetRelativeVelocity(time).magnitude * GetRelativeVelocity(time).magnitude));
            AddForce(dragForce);

            Vector3 frictionForce= -GetRelativeVelocity(time).normalized * _friction;
            AddForce(frictionForce);
        }
        
        public void Update(float time)
        {
            if (_isFixed) return;
            
            // Uses the Stormer-Verlet integration to move the position 
            Vector3 tempPos = pos; 

            pos = (tempPos * 2) - _prevPos + _acc * ((1 - _friction) * time * time);

            _prevPos = tempPos;

            _acc = Vector3.zero;
        }
        public Vector3 GetRelativeVelocity(float deltaTime)
        {
            return _acc * deltaTime;
        }
        

    }
}
