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

using Mono.Cecil;
using System.IO;
using System.Reflection;

public sealed class InternalizeXamarin : Microsoft.Build.Utilities.Task
{
    public override bool Execute()
    {
        var readerParameters = new ReaderParameters { ReadWrite = true };
        var assembly = AssemblyDefinition.ReadAssembly("../Runtime/Plugins/Xamarin.Forms.Core.dll", readerParameters);

        foreach (var readAttribute in assembly.CustomAttributes)
        {
            var readAttributeType = readAttribute.AttributeType;
            if (readAttributeType.Namespace != "System.Runtime.CompilerServices" ||
                readAttributeType.Name != "InternalsVisibleToAttribute")
            {
                continue;
            }

            var readParameters = readAttribute.Constructor.Parameters;
            if (readParameters.Count != 1)
            {
                continue;
            }

            var readParameterType = readParameters[0].ParameterType;
            if (readParameterType.Namespace != "System" || readParameterType.Name != "String")
            {
                continue;
            }

            foreach (var argumentValue in new[] { "Mux.Editor", "Mux.Markup" })
            {
                var argument = new CustomAttributeArgument(readParameterType, argumentValue);
                var newAttribute = new CustomAttribute(readAttribute.Constructor);

                newAttribute.ConstructorArguments.Add(argument);
                assembly.CustomAttributes.Add(newAttribute);
            }

            var writerParameters = new WriterParameters();

            using (var stream = new FileStream("Xamarin.Forms/xamarin.forms.snk", FileMode.Open))
            {
                writerParameters.StrongNameKeyPair = new StrongNameKeyPair(stream);
            }

            assembly.Write(writerParameters);

            return true;
        }

        return false;
    }
}
