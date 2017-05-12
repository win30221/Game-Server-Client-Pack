let playerService = require('../service/playerService'),
    log = logger.getLogger(__filename);

exports.dropDB = (req, res) => {
	playerService.dropDB((err, response) => {
		if (err) {
			log.error(err);
			res.status(500).send({
				message: err
			});
			return;
		}
		res.json(response);
	});
}


/*
 * Create an player account
 * @param {string} playerName - Name input by user
 * return
 * 	{
 *		"status": 1,
 *		"id": "5889a07a1304665b1ceae9bb"
 *		"playerPWD" : "gfne2"
 *	};
 */
exports.createPlayer = (req, res) => {

	let playerName = req.body.playerName;

	if (!playerName) {
		res.status(400).send({
			message: 'Lack required parameter'
		});
		return;
	}
	playerService.createPlayer(playerName).then(
		response => {
			log.info('\x1b[33;1m%s\x1b[0m', `Create Player`);
			log.info(response);
			res.json(response);
		}
	).catch(
		err => {
			log.error(err);
			res.status(500).send(err);
		}
	);

}





/*
 * Player login
 * @param {string} id - Player id
 * @param {string} pwd - Player password
 * return
 * 	{
  	"status": 1,
	"account": {
		"_id": "59151de9dc585b0528697fc7",
		"playerName": "Hugo",
		"playerPWD": "gfne2",
		"playerMeteorites": 0
	}
}
 */
exports.login = function(req, res) {

    let id = req.body.id,
        pwd = req.body.pwd;

    if (!(id && pwd)) {
        res.status(400).send({
            message: 'Lack required parameter'
        });
        return;
    }

    playerService.login(id, pwd).then(
        response => {
            log.info('\x1b[33;1m%s\x1b[0m', `Login`);
            log.info(response);
            res.json(response);
        }
    ).catch(
        err => {
            log.error(err);
            res.status(500).send(err);
        }
    );

}