using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace RevisedParticle
{
    public class BendingSpring : Spring
    {
        private readonly float _restAngle;
        private readonly float _bendSpringConstant;
        private readonly float _dampFactor;
        private readonly Particle particleOne;
        private readonly Particle particleTwo;
        private readonly Particle particleThree;
        private readonly Particle particleFour;
        
        public BendingSpring(Particle p0, Particle p1, Particle p2, Particle p3, float restAngle, float bendingStiffness, float dampingFactor)
        {
            _restAngle = restAngle;
            _bendSpringConstant = bendingStiffness;
            _dampFactor = dampingFactor;
            
            particleOne = p0;
            particleTwo = p1;
            particleThree = p2;
            particleFour = p3;

        }

        public override void ApplyForce(float dt)
        {
            Vector3 positionZero= particleOne.pos;
            Vector3 positionOne = particleTwo.pos;
            Vector3 positionTwo = particleThree.pos;
            Vector3 positionThree = particleFour.pos;

            Vector3 normalOne = Vector3.Normalize(Vector3.Cross(positionTwo-positionZero,positionOne-positionZero));
            Vector3 normalTwo = Vector3.Normalize(Vector3.Cross(positionOne-positionThree,positionTwo-positionThree));

            float dotProduct = Mathf.Clamp(Vector3.Dot(normalOne, normalTwo), -1.0f, 1.0f);

            float theta = Mathf.Acos(dotProduct);
            float angleDiff = theta - _restAngle;

            float forceMag = -_bendSpringConstant * angleDiff;

            Vector3 forceDir = Vector3.Normalize(Vector3.Cross(normalOne, normalTwo));
            if (Vector3.Normalize(forceDir) == Vector3.zero)
                return;

            Vector3 combinedForce = forceDir * forceMag;

            particleOne.AddForce(_dampFactor * combinedForce);
            particleTwo.AddForce(-_dampFactor * combinedForce);
            particleThree.AddForce(-_dampFactor * combinedForce);
            particleFour.AddForce(_dampFactor * combinedForce);
        }
    }
}