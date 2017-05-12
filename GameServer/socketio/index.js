let io = require('socket.io').listen(config.SOCKET_PORT),
    gameRoom = require('./gameRoom'),
    log = logger.getLogger(__filename);
global.players = [];
global.processCount = 1;

io.on('connection', (socket) => {

    //When player connected, add them to playerlist and report them connected.
    log.info(`Player connected.`);
    socket.emit("OnSocketIOConnected");

    //When player disconnected, remove them from the players.
    socket.on('disconnect', () => {
        log.info(`Player disconnected`);
        let removeIndex = _.indexOf(players, socket);
        players.splice(removeIndex, 1);
        log.info(`Players count : ${players.length}`);
    });

    //When player received the connected command, they will request login.
    socket.on('Login', (msg) => {
        if (!_.contains(players, socket)) {
            socket.playerID = msg.playerID;
            socket.playerName = msg.playerName;
            socket.process = 0;
            players.push(socket);
            log.info(`Player ID: ${msg.playerID} Player name: ${msg.playerName} logged in.`);
            log.info(`Players count : ${players.length}`);
        }
    });
});


function Update() {
    gameRoom.Update();
}


exports.start = () => {
	log.info(`Socket is running on ${config.SOCKET_PORT} port...`);

    gameRoom.socketListener(io);

    setInterval(function() {
        Update();
    }, 50);
};