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

using Microsoft.Build.Framework;
using System.IO;

public sealed class CreateThirdPartyNotices : Microsoft.Build.Utilities.Task
{
    [Required]
    public string[] ComponentNames { get; set; }

    [Required]
    public string[] Notices { get; set; }

    public override bool Execute()
    {
        using (var writer = new StreamWriter("../Third Party Notices.md"))
        {
            writer.Write("This package contains third-party software components governed by the license(s) indicated below:\r\n");

            for (var index = 0; index < ComponentNames.Length; index++)
            {
                if (ComponentNames[index] != "")
                {
                    writer.Write("\r\nComponent Name: ");
                    writer.Write(ComponentNames[index]);
                    writer.Write("\r\n\r\nLicense Type: MIT\r\n");
                }

                writer.Write("\r\n```\r\n");
                writer.Write(File.ReadAllText(Notices[index]));
                writer.Write("```\r\n");
            }
        }

        return true;
    }
}
