# Snow-Simulation
Snow - Custom Tessallation- Unity 2019.3.12f1


Custom Dynamic snow

***Every object that gets rendered by the ortho camera that's beneath the ground will deform the snow.

***The "Height" of the snow has to match the near-far clip plane distances of the camera in order to work properly.

***By using an ortho camera the performance stays the same no matter how many objects are inside the snow

***Character has full IK support and interacts with the enviroment if the layers have been set correctly.
The IK parameters haven't been fine tweaked .(Fine tweaking is recommended)

***Some optimizations should be done since the entire project is in a beta state.

***To test the project just import everything into your "Assets" folder and run the "TestScene"
WASD-Movement
RMOUSE-RUN


***Edit:Disable Snapping for "Horizontal" and "Vertical" axes in input manager.


***There are no lights inside the scene since points lights can't cast realtime shadows.
The lighting system is completely custom and supports a single point light . Shadows are done via geometry shaders by extruding meshes allong the light direction.





