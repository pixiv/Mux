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

using Mux.Markup;
using NUnit.Framework;
using System.Collections.Generic;

namespace Mux.Tests.Markup
{
    [TestFixture]
    public static class TemplatableCollectionList
    {
        private sealed class Implementation : TemplatableCollectionList<int>
        {
            public readonly List<int> list = new List<int>();

            protected override IList<int> GetList()
            {
                return list;
            }

            public override void InsertListRange(int index, IEnumerable<int> enumerable)
            {
                list.InsertRange(index, enumerable);
            }

            public override void RemoveListRange(int index, int count)
            {
                list.RemoveRange(index, count);
            }
        }

        [Test]
        public static void ClearList()
        {
            var implementation = new Implementation();
            implementation.list.Add(0);

            implementation.ClearList();

            Assert.IsEmpty(implementation.list);
        }

        [Test]
        public static void InsertRange()
        {
            var implementation = new Implementation();
            implementation.InsertListRange(0, new[] { 1 });

            CollectionAssert.AreEqual(new[] { 1 }, implementation.list);
        }

        [Test]
        public static void MoveListRangeBackward()
        {
            var implementation = new Implementation();
            implementation.list.AddRange(new[] { 2, 3 });

            implementation.MoveListRange(1, 0, 1);

            CollectionAssert.AreEqual(new[] { 3, 2 }, implementation.list);
        }

        [Test]
        public static void MoveListRangeForward()
        {
            var implementation = new Implementation();
            implementation.list.AddRange(new[] { 2, 3 });

            implementation.MoveListRange(0, 1, 1);

            CollectionAssert.AreEqual(new[] { 3, 2 }, implementation.list);
        }

        [Test]
        public static void RemoveListRange()
        {
            var implementation = new Implementation();
            implementation.list.AddRange(new[] { 2 });

            implementation.RemoveListRange(0, 1);

            Assert.IsEmpty(implementation.list);
        }

        [Test]
        public static void ReplaceListRange()
        {
            var implementation = new Implementation();
            implementation.list.AddRange(new[] { 2 });

            implementation.ReplaceListRange(0, 1, new[] { 3, 4 });

            CollectionAssert.AreEqual(new[] { 3 }, implementation.list);
        }
    }
}
