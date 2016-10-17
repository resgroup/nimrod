var path = require('path');
var webpack = require("webpack");


var config = {

    entry: ['./src/nimrod.demo.module'],
    output: {
        path: path.resolve(__dirname, '../WebServer/'),
        filename: 'nimrod.demo.bundle.js'
    },
    resolve: {
        extensions: ['.ts', '.js'],
    },
    module: {
        loaders: [{ test: /\.ts$/, loader: 'awesome-typescript-loader' }]
    }
};

module.exports = config;
