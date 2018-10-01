[LATEST RELEASE](https://github.com/MikeBaldwin21/Asteroids-Neural-Network/releases/latest)


--Asteroids Neural Network--
-By Mike Baldwin

This project is my attempt at using a Neural Network refined by a Genetic Algorithm.

The game the computer will train on will be the classic game "Asteroids".

My hope is to not only have the algorithm perform mutations on the next generation but also to have the options to perform "breeding" using the two strongest parents and possibly have the ability to mutate the number of hidden layers in the neural network (I'm not sure if that will negatively affect the genetic algorithm).

The whole thing is done in the Unity game engine.

--NEW RUN SETUP--
-Num of Input Raycasts = 720 (or greater, I need to fix a bug, just set it to 720 and be happy)
-Num Of Hidden Nodes = 1-99 (I personally set to 99, but it really doesn't change much)
-Initial Roid Spawn = 10 (How many asteroids to initially spawn at start)
-Min Large Roid Spawn = 3 (Minimum number of large asteroids that must be on screen)
-Move Speed = 3.0 (speed of the ship, in "units" per second)
-Rotation Speed = 100 (degrees per second the ship can rotate)
-Reload Speed = 1.0 (how many seconds must elapse between shots)
-Bullet Lifetime = 1.5 (How many seconds the bullets stay "alive")
-Number Per Generation = 25 (How many Neural Networks are simulated each generation)
-Mutation Percentage = 5 (Percentage chance of a single value in a single hidden node (matrix) has to change between generations. Smaller chance increases "refined" behaviours but takes a LOT longer to reach. Larger chance can reach maximum potential at a quicker rate but will most likely overshoot the optimal network solution)
