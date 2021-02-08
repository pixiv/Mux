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

__webpack_public_path__ = window.__webpack_public_path__;

import(/* webpackMode: "eager" */ "monaco-editor").then(({ editor }) =>
{
    for (const { viewNode, editorNode, value } of mux)
    {
        const xamlEditor = editor.create(editorNode, { value, language: "xml" });

        createUnityInstance(viewNode, {
            dataUrl: `${__webpack_public_path__}playground.data`,
            frameworkUrl: `${__webpack_public_path__}playground.framework.js`,
            codeUrl: `${__webpack_public_path__}playground.wasm`,
            symbolsUrl: `${__webpack_public_path__}playground.symbols.json`,
            streamingAssetsUrl: "StreamingAssets",
            companyName: "pixiv",
            productName: "Mux Documentation",
            productVersion: "1.0",
        }).then((instance) =>
        {
            function update()
            {
                instance.SendMessage("Main Camera", "SetXaml", xamlEditor.getValue())
            }

            instance.Module.xamlEditor = xamlEditor;
            xamlEditor.onDidChangeModelContent(update);
            update();
        });
    }
});
