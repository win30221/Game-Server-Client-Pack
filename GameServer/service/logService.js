exports.log = (logType, logSubType, log) => {
	let db;
	return new Promise(function (resolve, reject) {
		mongo.connectAsync(DB_URL).then(
			tmpDB => {
				db = tmpDB;
				let time = new Date().getTime();
				return db.collection(LOG_TABLE).insert({
					"logType": logType,
					"logSubType": logSubType,
					"log": log,
					"logTime": time
				});
			}
		).then(
			logResult => {
				resolve(logResult);
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