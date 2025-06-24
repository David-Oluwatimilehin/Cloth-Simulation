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

            if (distance == 0) return; // If the particles dont move exit

            direction.Normalize(); // Grabs the unit vector
            
            float springForce = _springConstant * (distance - _restDist); // Grabs force from the springs extension 
            Vector3 relativeVelocity = endParticle.GetRelativeVelocity(dt) - startParticle.GetRelativeVelocity(dt); // Get relative velocity between the two particles

            float dampingForce = _springDamping * Vector3.Dot(relativeVelocity, direction); // Calculates the dampening force which opposes relative motion of the spring
            Vector3 combinedForce = (springForce + dampingForce) * direction; 

            // Applies an equal and opposite force to the particles.
            startParticle.AddForce(combinedForce); 
            endParticle.AddForce(-combinedForce);
        }
        public override void Draw()
        {
            Debug.DrawLine(startParticle.pos, endParticle.pos, Color.green);
        }
    }

}
