# UnityNetworkExample
A stripped down example of working multiplayer with Unity.Network

As simple as it gets with a very commented code. It should prove useful for most that want to get started with Unity.Network or just check the basics.

It focuses on server/client connectability mostly but it does have spawnfunctions and a playeravatar you can run around with.

There is no interpolation though so running it on default 15 network updates will look awful. But it works.

It isn't perfect but it is a good start for those that preffer learning by reading and messing with code.

Getting Started: Setup a new project and add all the files to the Asset folder. After that you should be able to load the default scene and build the game and start testing.
You might want to up the network updaterate to 30 or more. Otherwise it will look very choppy due to lack of interpolation. You find it under Edit>Project Settings>Network>Sendrate.