using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    public class Spring
    {
        public Particle startParticle { get; set; }

        /// <summary>
        /// /public Particle middleParticle { get; set; }
        /// </summary>
        public Particle endParticle { get; set; }
        
        protected float _restDist { get; set; }
        public Spring()
        {

        }
        public virtual void Draw() { }
        public virtual void ApplyForce(float dt) { }

    }

}

