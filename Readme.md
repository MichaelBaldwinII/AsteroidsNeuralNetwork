[LATEST RELEASE](https://github.com/MikeBaldwin21/Asteroids-Neural-Network/releases/latest)


--Asteroids Neural Network--
-By Mike Baldwin

This project is my attempt at using a Neural Network refined by a Genetic Algorithm.

The game the computer will train on will be the classic game "Asteroids".

My hope is to not only have the algorithm perform mutations on the next generation but also to have the options to perform "breeding" using the two strongest parents and possibly have the ability to mutate the number of hidden layers in the neural network (I'm not sure if that will negatively affect the genetic algorithm).

The whole thing is done in the Unity game engine.

--NEW RUN SETUP--  
-Num of Inputs: The number of raycasts to perform/2. Please keep above 1. Game slows down above 720 due to vector maths.  
-Num Of Hidden Layers: How many distinct layers there are for processing. More equals slower calculations but possibly better behaviour.  
-Num Of Hidden Nodes (Per Layer): How many nodes are in each hidden layer. More is slower, but "possibly" better behaviour later on.  
-Initial Roid Spawn: How many large asteroids are spawned at the start of a new run.  
-Min Large Roid Spawn: Minimum number of large asteroids that must be on screen at any one time.  
-Move Speed: Speed of the ship, in "units" per second.  
-Rotation Speed: Degrees per second the ship can rotate at.  
-Reload Speed: How many seconds must elapse between shots.  
-Bullet Lifetime: How many seconds the bullets are allowed to exist.  
-Number Per Generation: How many Neural Networks are simulated each generation.  
-Mutation Percentage: Percentage chance of a single value in a single hidden layer has to change between generations. Smaller chance increases "refined" behaviours but takes a LOT longer to reach. Larger chance can reach maximum potential at a quicker rate but will most likely overshoot the optimal network solution.  
