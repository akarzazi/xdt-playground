const path = require('path');

module.exports = {
    entry: {
        "app": './src/MonacoInterop.ts',
        //"editor.worker": 'monaco-editor/esm/vs/editor/editor.worker.js',
    },
    output: {
        globalObject: "self",
        filename: "[name].bundle.js",
        path: path.resolve(__dirname, '../wwwroot/dist/monaco-editor'),
        publicPath: "/xdt-playground/dist/monaco-editor/",
    },
    module: {
        rules: [
            {
                test: /\.ts?$/,
                use: "ts-loader",
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader']
            },
            {
                test: /\.ttf$/,
                loader: 'file-loader'
            }]
    }
};