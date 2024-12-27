# Cloth-Simulation
![Screenshot 2024-12-02 152005](https://github.com/user-attachments/assets/6edb1ca5-20fa-476b-b7a7-15211e6e64c0)

The approach here uses the mass-spring model, which uses a network of particles given mass connected through springs.
I used a force-based dynamics approach for the simulation. This involved calculating the forces and adding them to the acceleration.
I chose to move the particles using the verlet integration for stability. It required some dampening to prevent turbulent oscillation. 

![image](https://github.com/user-attachments/assets/5c66c0b3-29be-48d6-992a-68ab3e15ea03)

## Areas for Improvement
### Rendering
I would change the use of Debug Drawline and Gizmos to render the springs and particles using compute shaders.
### Approach
I would rewrite this simulation for a real-time game by using position-based dynamics. That would involve predicting the 
particle positions based on their velocity and enforcing constraints. These would keep the particles within an acceptable range.
For the integration, I would use a modified velocity verlet.

## References
Helpful resources that made everything easier 

https://www.youtube.com/watch?v=-GWTDhOQU6M

https://en.wikipedia.org/wiki/Verlet_integration

https://graphics.stanford.edu/~mdfisher/cloth.html

https://www.scss.tcd.ie/michael.manzke/CS7057/cs7057-1516-14-MassSpringSystems-mm.pdf
