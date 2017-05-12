let log = logger.getLogger(__filename);

let logService = require("./logService");

exports.dropDB = (callback) => {
	mongo.connect(DB_URL, (err, db) => {
		db.dropDatabase();
		db.close();
		callback(null, { status: "ok" });
	});
}

exports.createPlayer = (playerName) => {
	let res = {},
		db;
	return new Promise(function (resolve, reject) {
		mongo.connectAsync(DB_URL).then(
			tmpDB => {
				db = tmpDB;
				return logService.log("CreatePlayer", "Start", {
					"playerName": playerName
				});
			}
		).then(
			logResult => {
				let playerPWD = Math.random().toString(36).substring(2, 7);
				return db.collection(ACCOUNT_TABLE).insert({
                    "playerName"     : playerName,
					"playerPWD"      : playerPWD,
					"playerMeteorites" : 0
				});
			}
        ).then(
			createResult => {
				log.info(createResult);
				res.status = status.STATUS_OK;
				res.playerID = createResult["ops"][0]["_id"];
				res.playerPWD = createResult["ops"][0]["playerPWD"];
				db.close();
				resolve(res);
			}
		).catch(
			err => {
				db.close();
				err.status = status.STATUS_FAIL;
				reject(err);
			}
		);
	});
}

let authentication = (id, pwd) => {
	let res = {},
		db;
	return new Promise(function (resolve, reject) {
		mongo.connectAsync(DB_URL).then(
			tmpDB => {
				db = tmpDB;
				return db.collection(ACCOUNT_TABLE).findOne({
					"_id": ObjectId(id),
					"playerPWD": pwd
				});
			}
		).then(
			searchResult => {
				db.close();
				if (searchResult) {
					res = searchResult;
					resolve(res);
				} else {
					res.status = status.STATUS_OK;
					return util.error(res);
				}
			}
		).catch(
			err => {
				db.close();
				err.status = status.STATUS_USER_NOT_EXISTED;
				reject(err);
			}
		);
	});
}

exports.login = function(id, pwd) {
    let res = {},
        db;
    return new Promise(function(resolve, reject) {
        mongo.connectAsync(DB_URL).then(
            tmpDB => {
                db = tmpDB;
                let time = (new Date()).getTime();
				return logService.log("Login", "Start", {
					"id": id,
					"pwd": pwd
				});
            }
        ).then(
            logResult => {
                return authentication(id, pwd);
            }
        ).then(
            accountResult => {
                res.account = accountResult;
                res.status = status.STATUS_OK;
				resolve(res);
            }
        ).catch(
            err => {
                db.close();
                reject(err);
            }
        );
    });
}