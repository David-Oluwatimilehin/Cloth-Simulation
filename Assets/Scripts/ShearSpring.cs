using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    public class ShearSpring : Spring
    {
        private float _shearSpringConstant;
        private float _shearSpringDamping;
        public ShearSpring(Particle particleOne, Particle particleTwo, float restLength, float springConstant, float dampValue)
        {
            startParticle = particleOne;
            endParticle = particleTwo;

            _restDist = restLength;
            _shearSpringConstant = springConstant;
            _shearSpringDamping = dampValue;

        }

        public override void ApplyForce(float dt)
        {
            Vector3 direction = endParticle.pos - startParticle.pos;
            float distance = direction.magnitude;

            if (distance == 0) return;

            direction.Normalize();

            float springForce = _shearSpringConstant * (distance - _restDist);

            Vector3 relativeVelocity = endParticle.GetRelativeVelocity(dt) - startParticle.GetRelativeVelocity(dt);

            float dampingForce = _shearSpringDamping * Vector3.Dot(relativeVelocity, direction);

            Vector3 force = (springForce + dampingForce) * direction;

            startParticle.AddForce(force);
            endParticle.AddForce(-force);
        }

        public override void Draw()
        {
            Vector3 posA = startParticle.pos;
            Vector3 posB = endParticle.pos;

            Debug.DrawLine(posA, posB, Color.yellow);
        }
    }
}
