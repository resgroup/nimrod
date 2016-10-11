var path = require('path');
var webpack = require("webpack");


var config = {

    entry: ['./src/nimrod.demo.module'],
    output: {
        path: path.resolve(__dirname, '../WebServer/'),
        filename: 'nimrod.demo.bundle.js'
    },
    resolve: {
        extensions: ['', '.ts', '.tsx', '.js', '.jsx'],
        alias: {
        }
    },
    module: {
        loaders: [{ test: /\.tsx?$/, loader: 'awesome-typescript-loader' }]
    }
};

module.exports = config;
