var rootPath = path.dirname(require.resolve('../index.js'));

exports.config = {
	appenders: [
		{type:'console'},
		{
			type: "logLevelFilter",
			level: "ALL",
			appender: {
			   type: "file",
			   maxLogSize: 10*1024*1024,
			   backups: 10,
			   pattern: ".yyyy-MM-dd",
			   filename: path.join(rootPath,'logs','GameServer.log')
	        }
		},
		{
			type: "logLevelFilter",
			level: "ERROR",
			appender: {
			   type: "file",
			   maxLogSize: 20480,
			   backups: 10,
			   pattern: ".yyyy-MM-dd",
			   filename: path.join(rootPath,'logs','Error.log')
	        }
		}
    ],
    replaceConsole: true
};