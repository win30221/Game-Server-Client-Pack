let config = require('../config/configLogger').config,
	log4js = require('log4js');

createDir(checkFolderExist(parsePath(config,[])));

log4js.configure(config);

exports.getExpressLogger = () => {
	let logger = log4js.getLogger('GameServer');
	return log4js.connectLogger(logger, {level:'auto', format:':method :url'});
};

exports.getLogger = (name) => {
	let logger = log4js.getLogger(name);
	//logger.setLevel('DEBUG');
	return logger;
};

function parsePath(config, ary) {
	if ("appenders" in config) {
		if (_.isArray(config['appenders'])) {
			_.each(config['appenders'], (v) => {
				parsePath(v, ary);
			});
		}
	} else if ("appender" in config) {
		if (_.isObject(config['appender'])) {
			parsePath(config['appender'], ary);
		}
	} else if ("filename" in config) {
		ary.push(config['filename']);
	}
	return ary;
}

function checkFolderExist(paths) {
	let ary = [];
	_.reduce(paths, (set, p) => {
		let end = p.lastIndexOf(path.sep),
			dirPath = p.substring(0, end),
			exist = fs.existsSync(dirPath);
		set.push({path:dirPath, exist:exist});
		return set;
	}, ary);
	return ary;
}

function createDir(paths) {
	_.each(paths, (v) => {
		if (!fs.existsSync(v.path)) {
			fs.mkdirSync(v.path);
		}
	});
}