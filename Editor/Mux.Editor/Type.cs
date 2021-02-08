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

namespace Mux.Editor
{
    internal struct Slot
    {
        public string muxNamespace;
        public string muxName;

        private Slot(string full)
        {
            var dot = full.LastIndexOf('.');

            if (dot < 0)
            {
                muxNamespace = "";
                muxName = full;
            }
            else
            {
                muxNamespace = full.Substring(0, dot);
                muxName = full.Substring(dot + 1);
            }
        }

        public static IEnumerable<Slot> Parse(string name)
        {
            return name.Split('/').Select(xamlTypeName => new Slot(xamlTypeName));
        }
    }

    internal struct Type
    {
        public Dictionary<Slot, Type> nested;
        public string asset;
        public List<string> names;
    }
}
