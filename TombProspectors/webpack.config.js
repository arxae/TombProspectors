var path = require("path");
var UglifyPlugin = require("uglifyjs-webpack-plugin");
var webpack = require("webpack");

module.exports = {
	entry: {
		site: [
			"./Scripts/TombProspectorsApp.ts"
		]
	},
	output: {
		filename: "app.js",
		path: path.resolve(__dirname, "wwwroot/dist/")
	},
	module: {
		rules: [
			{
				test: /\.vue$/,
				loader: "vue-loader",
				options: {
					loaders: {
						"scss": "vue-style-loader!css-loader!sass-loader",
						"sass": "vue-style-loader!css-loader!sass-loader?indentedSyntax"
					}
				}
			},
			{
				test: /\.tsx?$/,
				loader: "ts-loader",
				exclude: /node_modules/
			}
		]
	},
	//plugins: [new UglifyPlugin()],
	resolve: {
		extensions: [".tsx", ".ts", ".js", ".vue"],
		alias: {
			"vue$": "vue/dist/vue.esm.js"
		}
	}
};