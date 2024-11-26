using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle{
    public class Spring
    {
        public Particle startParticle { get; private set; }
        public Particle endParticle { get; private set; }
        
        private readonly float restDist;
        private readonly float springStiffness;
        private readonly float springDamping;
        
        public Spring(Particle p1, Particle p2, float startLength,float springConstant, float dampValue)
        {
            startParticle = p1;
            endParticle = p2;

            restDist = startLength;
            springStiffness = springConstant;
            springDamping = dampValue;

        }
        public void ApplyForce(float dt)
        {
            Vector3 direction = endParticle.pos - startParticle.pos;
            float distance = direction.magnitude;

            if (distance == 0) return;
            
            direction.Normalize();

            float springForce = springStiffness * (distance - restDist);

            Vector3 relativeVelocity = endParticle.GetRelativeVelocity(dt) - startParticle.GetRelativeVelocity(dt);

            float dampingForce = springDamping * Vector3.Dot(relativeVelocity, direction);

            Vector3 force = (springForce + dampingForce) * direction;

            startParticle.AddForce(force);
            endParticle.AddForce(-force);
        }



    }

}

