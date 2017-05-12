let CONTROLLER_HOME = '../controller/',
    config = require('../config/configController').config;
let express = require('express'),
    router = express.Router();

router.all('/*', (req, res) => {
    let url = req.path, controller;
    if ( !config[url] || !config[url]['controller'] || !config[url]['method'] ) {
        res.status(404).send({ message:'Page Not Exist' });
        return;
    }
    controller = require(CONTROLLER_HOME + config[url]['controller']);
    controller[config[url]['method']](req, res);
});

module.exports = router;