var webpack = require('webpack');
var path = require('path');
var autoprefixer = require('autoprefixer');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');

module.exports = {
  context: __dirname,
  entry: [
    './src/index.js'
  ],
  output: {
    path: path.join(__dirname, 'www'),
    filename: 'bundle.js'
  },

  module: {
    loaders: [
      {
        test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
        loader: 'url-loader?limit=10000&minetype=application/font-woff'
      },
      {
        test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
        loader: 'file-loader'
      },
      {
        test: /\.json$/,
        loader: 'json'
      },
      {
        test: /\.css$/,
        loader: 'style!css!postcss'
      },
      {
        test: /\.styl$/,
        loader: 'style!css!postcss!stylus?paths=node_modules'
      },
      {
        test: /\.js$|\.jsx$/,
        exclude: [/node_modules/],
        loaders: [
          'babel?' + JSON.stringify({ presets: ["es2015", "react", "stage-2"] })
        ]
      }
    ]
  },

  postcss: function () {
    return [autoprefixer];
  },

  plugins: [
    new webpack.DefinePlugin({
      'process.env': {
        'NODE_ENV': JSON.stringify('production')
      }
    }),
    new UglifyJSPlugin({
      sourceMap: true
    })
  ]
};

