let log = logger.getLogger(__filename);

exports.assetBundleData = (req, res) => {
	log.info(`Download the asset bundle json data.`);
	let file = `${GAME_SERVER_ROOT_PATH}/assetBundles/assetBundleVersion.json`;
	res.download(file);
}

exports.assetBundleUI = (req, res) => {
	log.info(`Download the assetBundleUI.`);
	let file = `${GAME_SERVER_ROOT_PATH}/assetBundles/ui`;
	res.download(file);
}

exports.assetBundleGameobject = (req, res) => {
	log.info(`Download the assetBundleGameobject.`);
	let file = `${GAME_SERVER_ROOT_PATH}/assetBundles/gameobject`;
	res.download(file);
}