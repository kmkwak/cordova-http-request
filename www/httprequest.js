var exec = require('cordova/exec');

function HttpRequest() {

}

HttpRequest.prototype.echo = function (params, callback) {
    var options = [];
    options.push(params.url);
    options.push(params.cookie);
    var temp = [];
    var headers = params.headers;
    for (var prop in headers) {
        temp.push({Key:prop, Value: headers[prop]});
    }
    options.push(JSON.stringify(temp));
    
    exec(callback, function (err) {
        callback('Nothing to echo.');
    }, "HttpRequest", "echo", [JSON.stringify(options)]);
};

HttpRequest.prototype.request = function (params, callback) {
    var options = [];
    options.push(params.url);
    options.push(params.cookie);
    var temp = [];
    var headers = params.headers;
    for (var prop in headers) {
        temp.push({ Key: prop, Value: headers[prop] });
    }
    options.push(JSON.stringify(temp));

    exec(callback, function (err) {
        callback();
    }, "HttpRequest", "request", [JSON.stringify(options)]);
};

module.exports.httpRequest = new HttpRequest();
