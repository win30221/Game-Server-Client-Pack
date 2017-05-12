let log = logger.getLogger(__filename);
let gameService = require("../service/gameService");
let gameConfig = require("../config/gameConfig");

let roomNum = 0;
let roomList = [];

RoomStatus = {
	WAITING: 0,
	PLAYING: 1,
	CANCELED: 2
}

class Room {

	constructor(roomNum, player) {
		this.players = [player];
		this.roomNum = roomNum;
		this.roomLevel = player.level;
		this.roomStatus = RoomStatus.WAITING;

		this.winUID = 2;
		this.startTime = gameConfig.BATTLE_START_TIME;
		this.time = gameConfig.BATTLE_TIME;
		this.spaceShipPosition = 0;
		this.dataChanged = false;
	}

	join(player) {
		this.players.push(player);
	}

	//Game start
	start() {
        for (let i = 0; i < this.players.length; i++) {
            let data = {"UID" : i};
            this.players[i].emit("StartBattle", data);
			this.players[i].UID = i;
        }
	}

	//Game logic
	update() {

		if (this.startTime > 0) {

			this.startTime -= gameConfig.BATTLE_INTERVAL;
			if (this.startTime <= 0) {
				for (let i = 0; i < this.players.length; i++) {
					this.players[i].emit("OnBattleStart", {});
				}
			}
			
		} else if (this.time > 0) {
			
			this.time -= gameConfig.BATTLE_INTERVAL;
			if (this.time <= 0) {
				this.battleEnd();
			}
			
		}
		
	}

	//Game over
	battleEnd() {
		let data = {"winUID" : this.winUID};
		for (let i = 0; i < this.players.length; i++) {
			this.players[i].emit("OnBattleEnd", data);
			if (this.winUID != 2) gameService.processGameResult(this.players[i].playerID, this.winUID == this.players[i].UID);
			this.players[i].room = null;
		}
		RemoveRoom(this);
	}

	//Player input
	inRoomMessage(handle, data) {
		switch (handle) {
			case 0:
				if (data.UID == 0) {
					this.spaceShipPosition += gameConfig.BATTLE_MOVE;
					if (this.spaceShipPosition > gameConfig.BATTLE_BOUNDARY) {
						this.winUID = 0;
						this.battleEnd();
					}
				} else {
					this.spaceShipPosition -= gameConfig.BATTLE_MOVE;
					if (this.spaceShipPosition < -gameConfig.BATTLE_BOUNDARY) {
						this.winUID = 1;
						this.battleEnd();
					}
				}
				break;
		}
	}

	//Player gets game status
	getGameStatus(player) {
		if (this.startTime > 0) {
			let data = {"time" : this.startTime};
			player.emit("OnBattleStartCountDown", data);
		} else if (this.time > 0) {
			let data = {"time" : this.time, "spaceShipPosition" : this.spaceShipPosition};
			player.emit("OnUpdateSpaceShipPosition", data);
		}
	}

}



function MatchMaking(player) {
	if (!SearchSuitableRoom(player)) {
		//Create a room for the player
		let room = new Room(roomNum, player);
		roomNum++;
		player.room = room;
		roomList.push(room);
		ListAllRoom();
		log.info(`${player.playerName} is in room.`);
	}
}

function SearchSuitableRoom(player) {
	let isSuitable = false;
	roomList.some(function (room) {
		if (room.roomStatus == RoomStatus.WAITING && room.roomLevel == player.level) {
			log.info("room start");
			//Asign the player to this room
			room.join(player);
			player.room = room;
			room.roomStatus = RoomStatus.PLAYING;
			room.start();
			isSuitable = true;
		}
	});
	return isSuitable;
}

function RemoveRoom(room) {
	log.info(`Remove room`);
	let removeIndex = _.indexOf(roomList, room);
	roomList.splice(removeIndex, 1);
}

function ListAllRoom() {
	log.info(`========================`);
	for (let i in roomList) {
		let r = roomList[i];
		log.info(`Room num: ${r.roomNum} Room status: ${r.roomStatus}`);
	}
}


exports.Update = () => {
    roomList.some(function(room) {
        if (room.roomStatus == RoomStatus.PLAYING) {
			room.update();
        }
    });
}


exports.socketListener = (io) => {
    io.on('connection', (socket) => {

		socket.on('disconnect', () => {
			if (socket.room != null && socket.room.roomStatus == RoomStatus.WAITING) {
				RemoveRoom(socket.room);
			}
		});

        socket.on('Login', (msg) => {
			if (!_.contains(players, socket) || socket.process < processCount) {
				socket.process++;
			}
        });

        //When player request for battle.
        socket.on('RequestBattle', (msg) => {
            if (socket.room == null) {
                socket.level = msg.level;
                MatchMaking(socket);
            } else {
                log.info("Player is in queue.");
            }
        });

		//When player cancel battle.
        socket.on('CancelBattle', (msg) => {
            let room = socket.room;
            if (room != null) {
                if (room.roomStatus == RoomStatus.WAITING) {
                    log.info("Leave success");

                    socket.room.roomStatus = RoomStatus.CANCELED;
					RemoveRoom(socket.room);

                    ListAllRoom();
                    socket.room = null;
                } else {
                    log.info("Game is starting");
                    ListAllRoom();
                }
            } else {
                log.info("Player isn't in room.");
                ListAllRoom();
            }
        });


        //When player input battle command.
        socket.on('InRoomMessage', (msg) => {
            let handle = msg.handle,
                data = msg.data;
			console.log(`Player input battle command:`);
			console.log(msg);
            if (socket.room != null) socket.room.inRoomMessage(handle, data);
        });

		//When player request for game status
		socket.on('GetGameStatus', (msg) => {
            if (socket.room != null) socket.room.getGameStatus(socket);
        });

    });
}