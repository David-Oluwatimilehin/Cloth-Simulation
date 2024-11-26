using DavidOluwatimilehin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    public class Particle
    {
        public Vector3 pos { get; private set; }
        public bool IsFixed { get => _isFixed; set => _isFixed = value; }

        private Vector3 prevPos;
        private Vector3 acc;

        private float _gravity;
        private float _friction;
        private float _mass;
        private bool _isFixed;

        private float windIntensity;

        public Particle(Vector3 initPos, SimulationValues constants)
        {
            pos = initPos;
            prevPos = initPos;

            _mass = constants.mass;
            _gravity = constants.gravity;
            _friction = constants.friction;
            
            acc = Vector3.zero;
            _isFixed = false;

        }

        public void AddForce(Vector3 force)
        {
            acc += force/_mass;
        }
        public void SetMass(float mass)
        {
            _mass = mass;
        }
                     
        public void SumForces(Vector3 windForce)
        {
            //if (_mass < 0) return;

            Vector3 gravityForce = new Vector3(0, -_gravity, 0);
            AddForce(gravityForce);

            //AddForce(windForce);
            
            //Debug.Log(acc);
        }
        public void Update(float time)
        {
            if (_isFixed) return;

            Vector3 tempPos = pos;
            
            pos += (pos - prevPos) * (1-_friction) + acc * time * time;

            prevPos = tempPos;

            acc = Vector3.zero;
        }
        public Vector3 GetRelativeVelocity(float deltaTime)
        {
            return acc * deltaTime;
        }
        

    }
}
