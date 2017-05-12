global.path = require('path');
global.GAME_SERVER_ROOT_PATH = path.resolve();
global._ = require('underscore');
global.fs = require('fs');
global.logger = require('./logger');
global.util = require('./util');
global.config = require('./config/config.json');
global.status = require('./config/configStatus').config;
global.mongo = require('./config/configDB');

let express = require('express'),
    app = express(),
	bodyParser = require('body-parser'),
	routes = require('./routes'),
	socket = require('./socketio'),
	log = logger.getLogger(__filename);

app.use(bodyParser.urlencoded({limit: '50mb', extended: true }));
app.use(logger.getExpressLogger());
app.use(config.DOMAIN_NAME, routes);

app.listen(config.HTTP_PORT, function () {
	log.info(`Server is running on ${config.HTTP_PORT} port...`);
    socket.start();
});