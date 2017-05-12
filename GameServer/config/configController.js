exports.config = {
	//Asset bundle
	'/AssetBundleData' : { controller:'assetBundle', method:'assetBundleData' },
	'/AssetBundleUI'   : { controller:'assetBundle', method:'assetBundleUI' },
	'/AssetBundleGameobject'   : { controller:'assetBundle', method:'assetBundleGameobject' },
	
	'/DropDB'   : { controller:'player', method:'dropDB' },
	'/CreatePlayer'    : { controller:'player', method:'createPlayer' },
	'/Login'    : { controller:'player', method:'login' }
}