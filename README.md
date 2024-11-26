# Cloth-Simulation

The approach here uses the mass-spring model. The model uses a network of particles given mass connected through springs.
I used force based dynamics approach for the simulation. This involved calculating the forces and adding them to the acceleration.
To move the particles, I chose to use the verlet integration for stability. It required some dampening to prevent turbulent oscillation. 


## Areas for Improvement
### Rendering
I would change from using Debug Drawline and Gizmos to rendering the particles and springs with compute shaders.
