let Promise = require('bluebird');
let mongoDB = require('mongodb');
let MongoClient = mongoDB.MongoClient;
let Collection = mongoDB.Collection;

Promise.promisifyAll(Collection.prototype);
Promise.promisifyAll(MongoClient);

global.DB_URL = config.DB_URL;
global.ObjectId = mongoDB.ObjectId;

global.LOG_TABLE = "Log";
global.ACCOUNT_TABLE = "Account";

module.exports = MongoClient;