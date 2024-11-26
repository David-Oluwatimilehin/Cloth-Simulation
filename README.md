# Cloth-Simulation

The approach here uses the mass-spring model. The model uses a network of particles given mass connected through springs.
I used force based dynamics approach for the simulation. This involved calculating the forces and adding them to the acceleration.
To move the particles, I chose to use the verlet integration for stability. It required some dampening to prevent turbulent oscillation. 


## Areas for Improvement
### Rendering
I would change from using Debug Drawline and Gizmos to rendering the particles and springs with compute shaders.
### Approach
I would rewrite this simulation to be more condusive for a real time game by using a position based dynamics. That would involve predicting the 
particle positions based on their velocity and enforcing constraints. These would keep the particles within an acceptable range.

## References
Helpful sources that made everything easier 

https://www.youtube.com/watch?v=-GWTDhOQU6M

https://graphics.stanford.edu/~mdfisher/cloth.html

https://www.scss.tcd.ie/michael.manzke/CS7057/cs7057-1516-14-MassSpringSystems-mm.pdf
