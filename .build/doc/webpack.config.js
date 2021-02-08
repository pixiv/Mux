// Copyright 2019 pixiv Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

const { readFileSync } = require("fs");
const { LicenseWebpackPlugin } = require("license-webpack-plugin");
const MonacoWebpackPlugin = require("monaco-editor-webpack-plugin");
const { resolve } = require("path");

module.exports =
{
    entry: ".",
    module:
    {
        rules:
        [
            { test: /\.css$/, use: ["style-loader", "css-loader"] },
            { test: /\.ttf$/, use: ["file-loader"] }
        ]
    },
    output: { path: resolve(__dirname, "bin/playground/playground") },
    plugins:
    [
        new LicenseWebpackPlugin(
        {
            outputFilename: "../../../obj/licenses.md",
            perChunkOutput: false,
            renderLicenses(modules)
            {
                const lines =
                [
                    "# Licenses\n\n## Mux\n\n",
                    readFileSync("../../LICENSE.md"),
                    "## Plugins\n",
                    readFileSync("../../Third Party Notices.md"),
                    "## Playground\n"
                ];

                for (const { packageJson, licenseId, licenseText } of modules)
                {
                    let rendered = "Component Name: " + packageJson.name;

                    rendered += "\n\nLicense Type: ";
                    rendered += licenseId;
                    rendered += "\n\n```\n";
                    rendered += licenseText;
                    rendered += "```\n";

                    lines.push(rendered);
                }

                return lines.join("\n");
            }
        }),
        new MonacoWebpackPlugin({ languages: [ "xml" ] })
    ]
};
