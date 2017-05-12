let log = logger.getLogger(__filename);

let logService = require("./logService");

exports.processGameResult = (playerID, isWin) => {
	let res = {},
		db;
	return new Promise(function (resolve, reject) {
		mongo.connectAsync(DB_URL).then(
			tmpDB => {
				db = tmpDB;
				return logService.log("ProcessGameResult", "Start", {
					"playerID": playerID,
                    "isWin" : isWin
				});
			}
		).then(
			logResult => {
				return db.collection(ACCOUNT_TABLE).findOne( ObjectId(playerID) );
			}
        ).then(
			searchResult => {
                let playerMeteorites = (isWin) ? searchResult["playerMeteorites"] + 1 : searchResult["playerMeteorites"] - 1;
                if (playerMeteorites < 0) playerMeteorites = 0;
				return db.collection(ACCOUNT_TABLE).update( {"_id" : ObjectId(playerID)}, { $set: { "playerMeteorites" : playerMeteorites } } );
			}
        ).then(
			updateResult => {
				res.status = status.STATUS_OK;
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