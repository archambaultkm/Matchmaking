In Progress!

This contains a server program and a client program. The server handles the player queue management, matchmaking, and relaying completed party information back to the player, while the client communicates player data to the server.

For now, parties can be 2 players, and users are considered a 'match' if they're in the same region and within 10 levels of each other. Players will continue to wait in the queue until they find a match, and their connections are severed once they're paired up to make room in the queue.

Next steps:
  - more detailed matching criteria. (if players are waiting forever relax region/level requirement, etc).
  - bigger/varied party size (players select if they want to play 3v3 or 5v5, for example).
