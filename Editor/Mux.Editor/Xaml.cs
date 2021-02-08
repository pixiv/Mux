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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Mux.Editor
{
    internal readonly struct Xaml
    {
        public readonly string classFullName;
        public readonly List<string> names;

        private Xaml(string classFullName, List<string> names)
        {
            this.classFullName = classFullName;
            this.names = names;
        }

        public static Xaml Parse(string xaml)
        {
            using (var xml = XmlReader.Create(xaml, new XmlReaderSettings { Async = true }))
            {
                while (!xml.IsStartElement())
                {
                    if (!xml.Read())
                    {
                        return new Xaml(null, null);
                    }
                }

                var classFullName = xml["Class", "http://schemas.microsoft.com/winfx/2009/xaml"];
                var names = new List<string>();

                do
                {
                    if (!xml.IsStartElement())
                    {
                        continue;
                    }

                    var name = xml["Name", "http://schemas.microsoft.com/winfx/2009/xaml"];
                    if (name != null)
                    {
                        names.Add(name);
                    }

                    if ((xml.NamespaceURI == "http://schemas.microsoft.com/winfx/2009/xaml" && xml.Name == "DataTemplate") ||
                        (xml.NamespaceURI == "http://xamarin.com/schemas/2014/forms" && (xml.Name == "ControlTemplate" || xml.Name == "Style" || xml.Name == "VisualStateManager.VisualStateGroups")))
                    {
                        xml.Skip();
                    }
                } while (xml.Read());

                return new Xaml(classFullName, names);
            }
        }
    }
}
