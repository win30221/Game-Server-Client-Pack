exports.error = (err) => {
	return new Promise(function(resolve, reject) {
		reject(err);
	});
}