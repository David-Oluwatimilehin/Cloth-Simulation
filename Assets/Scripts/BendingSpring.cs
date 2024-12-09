using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace RevisedParticle
{
    public class BendingSpring : Spring
    {
        private float _restLength;
        private float _bendSpringConstant;
        public BendingSpring(Particle particleOne, Particle particleThree, float bendConstant, float restAngle)
        {
            startParticle = particleOne;
    
            endParticle = particleThree;

            _bendSpringConstant = bendConstant;
            _restLength = restAngle;
        }
        public override void ApplyForce(float dt)
        {
            Vector3 direction = endParticle.pos - startParticle.pos;
            float currentLength = direction.magnitude;

            if (currentLength == 0) return;

            float lengthDifference = currentLength - _restLength;


            Vector3 force = direction.normalized * lengthDifference * _bendSpringConstant;

            startParticle.AddForce(force);
            endParticle.AddForce(-force);
        }
        public override void Draw()
        {
            base.Draw();
                     
        }
    }
}
