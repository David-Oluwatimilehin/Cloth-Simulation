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

            if (distance == 0) return; // If the particles dont move exit

            direction.Normalize(); // Grabs the unit vector

            float springForce = _shearSpringConstant * (distance - _restDist); // Grabs force from the springs extension 
            Vector3 relativeVelocity = endParticle.GetRelativeVelocity(dt) - startParticle.GetRelativeVelocity(dt); // Get relative velocity between the two particles.

            float dampingForce = _shearSpringDamping * Vector3.Dot(relativeVelocity, direction); // Calculates the dampening force which opposes relative motion of the spring
            Vector3 combinedForce = (springForce + dampingForce) * direction;

            // Applies an equal and opposite force to the particles.
            startParticle.AddForce(combinedForce);
            endParticle.AddForce(-combinedForce);
        }

        public override void Draw()
        {
            Debug.DrawLine(startParticle.pos,  endParticle.pos, Color.yellow);
        }
    }
}
