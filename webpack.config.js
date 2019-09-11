
const path = require('path');
const WebpackShellPlugin = require('webpack-shell-plugin');

module.exports = {


    entry:'./client/index.js',
    output: {
        path: path.resolve(__dirname, "client"),
        filename: 'app.js'
    },
    module: {
        rules: [
          {
            test: /\.m?js$/,
            exclude: /node_modules/,
            use: {
              loader: 'babel-loader',
            }
          }
        ]
      },
      watch:true,
      watchOptions: {
        aggregateTimeout: 200
      },
      plugins: [
        new WebpackShellPlugin({onBuildStart:['echo "Webpack Start"'], onBuildEnd:['nodemon resource/resource.js --watch resource --watch client']})
      ]

    }