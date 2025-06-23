using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    public class StructuralSpring : Spring
    {
        private readonly float _springConstant;
        private readonly float _springDamping;
        public StructuralSpring(Particle particleOne, Particle particleTwo, float restLength, float springStiffness, float dampValue)
        {
            startParticle = particleOne;
            endParticle = particleTwo;

            _restDist=restLength;
            _springConstant = springStiffness;
            _springDamping = dampValue;

        }

        public override void ApplyForce(float dt)
        {
            Vector3 direction = endParticle.pos - startParticle.pos;
            float distance = direction.magnitude;

            if (distance == 0) return;

            direction.Normalize();

            float springForce = _springConstant * (distance - _restDist);

            Vector3 relativeVelocity = endParticle.GetRelativeVelocity(dt) - startParticle.GetRelativeVelocity(dt);

            float dampingForce = _springDamping * Vector3.Dot(relativeVelocity, direction);

            Vector3 force = (springForce + dampingForce) * direction;

            startParticle.AddForce(force);
            endParticle.AddForce(-force);
        }
        public override void Draw()
        {
            Debug.DrawLine(startParticle.pos, endParticle.pos, Color.green);
        }
    }

}
