#  Echoes 
For our project, we will be developing a horror game called Echoes. In this experience, players will be spawned into a procedurally generated forest at night, and, since it’s a horror game, they won’t be alone. An NPC (non-playable-character) will spawn into the map, and it will always move towards the player that is closest to it at any given time. If the NPC “catches” (or collides) with a player, the player will lose a life. Once the player loses all of it's life, the player is elminated.The last person standing wins. Although visibility will be limited, players will be able to view the space in their immediate proximity. 

## MVP
* 4 players + 1 NPC
* Players spawn in predetermined spawn points
* NPC spawns in the middle of the map
* The forest is procedurally generated
* NPC always moves toward closest player at any given point
* If NPC and a player collide, the player loses a life. 
* If a player looses all of their lifes they become a spectator
* Last person standing wins
* Winner goes to a winner box


## Added Features
* A life count view for each player
* Sounds that change dynamically based on how far the players are from the NPC - footsteps, voices, ...
* A flashlight for each player
* Lighting adjustments - changed global skybox, reduced skybox lighting
