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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Mux.Tests.Markup
{
    [TestFixture]
    public static class TemplatableCollection
    {
        private sealed class Collection : TemplatableCollection<Content>
        {
            public readonly List<TemplatedItem<Content>> list = new List<TemplatedItem<Content>>();

            public Collection(BindableObject container) : base(container)
            {
            }

            protected override IList<TemplatedItem<Content>> GetList()
            {
                return list;
            }

            public override void InsertListRange(int index, IEnumerable<TemplatedItem<Content>> enumerable)
            {
                list.InsertRange(index, enumerable);
            }

            public override void RemoveListRange(int index, int count)
            {
                list.RemoveRange(index, count);
            }
        }

        private class Content : BindableObject
        {
        }

        /// <remarks>It is a common scenario when binding source.</remarks>
        [Test]
        public static void ChangeSourceFromNullToNull()
        {
            var collection = new Collection(null);
            Assert.DoesNotThrow(() => collection.ChangeSource(null));
        }

        [Test]
        public static void ChangeSourceAndChangeTemplate()
        {
            var content = new Content();
            var collection = new Collection(null);
            var template = new DataTemplate(() => content);

            collection.ChangeSource(new[] { 0 });
            collection.ChangeTemplate(template);

            CollectionAssert.AreEqual(collection, new[]
            {
                new TemplatedItem<Content>
                {
                    Content = content,
                    Template = template
                }
            });
        }

        /// <remarks>It is a common scenario when binding template.</remarks>
        [Test]
        public static void ChangeTemplateFromNullToNull()
        {
            var collection = new Collection(null);
            Assert.DoesNotThrow(() => collection.ChangeTemplate(null));
        }

        [Test]
        public static void ChangeTemplateAndChangeSource()
        {
            var content = new Content();
            var collection = new Collection(null);
            var template = new DataTemplate(() => content);

            collection.ChangeTemplate(template);
            collection.ChangeSource(new[] { 0 });

            CollectionAssert.AreEqual(collection, new[]
            {
                new TemplatedItem<Content>
                {
                    Content = content,
                    Template = template
                }
            });
        }

        [Test]
        public static void Count()
        {
            var collection = new Collection(null);
            Assert.AreEqual(collection.Count, 0);
        }

        [Test]
        public static void IsReadOnlyWithoutSourceAndTemplate()
        {
            var collection = new Collection(null);
            Assert.IsFalse(collection.IsReadOnly);
        }

        [Test]
        public static void IsReadOnlyWithSource()
        {
            var collection = new Collection(null);
            collection.ChangeSource(new int[0]);
            Assert.IsTrue(collection.IsReadOnly);
        }

        [Test]
        public static void IsReadOnlyWithTemplate()
        {
            var collection = new Collection(null);
            collection.ChangeTemplate(new DataTemplate());
            Assert.IsTrue(collection.IsReadOnly);
        }

        [Test]
        public static void SourceChangeWithoutTemplate()
        {
            var collection = new Collection(null);
            var source = new ObservableCollection<int>();
            collection.ChangeSource(source);
            Assert.DoesNotThrow(() => source.Add(0));
        }

        [Test]
        public static void SourceAddition()
        {
            var collection = new Collection(null);
            var content = new Content();
            var source = new ObservableCollection<int>();

            collection.ChangeSource(source);
            collection.ChangeTemplate(new DataTemplate(() => content));
            source.Add(0);

            Assert.AreEqual(content, collection.First());
            Assert.AreEqual(0, content.BindingContext);
        }

        [Test]
        public static void SourceMove()
        {
            var collection = new Collection(null);
            var source = new ObservableCollection<int> { 2, 3 };

            collection.ChangeSource(source);
            collection.ChangeTemplate(new DataTemplate(() => new Content()));
            source.Move(0, 1);

            CollectionAssert.AreEqual(new[] { 3, 2 }, collection.Select(content => content.BindingContext));
        }

        [Test]
        public static void SourceRemove()
        {
            var collection = new Collection(null);
            var source = new ObservableCollection<int> { 1 };

            collection.ChangeSource(source);
            collection.ChangeTemplate(new DataTemplate(() => new Content()));
            source.RemoveAt(0);

            CollectionAssert.IsEmpty(collection);
        }

        [Test]
        public static void SourceReplace()
        {
            var collection = new Collection(null);
            var content = new Content();
            var source = new ObservableCollection<int> { 1 };
            var template = new DataTemplate(() => content);

            collection.ChangeSource(source);
            collection.ChangeTemplate(template);
            source[0] = 2;

            CollectionAssert.AreEqual(new[]
            {
                new TemplatedItem<Content>
                {
                    Content = content,
                    Template = template
                }
            }, collection);
            Assert.AreEqual(2, content.BindingContext);
        }

        [Test]
        public static void SourceReset()
        {
            var collection = new Collection(null);
            var source = new ObservableCollection<int> { 0 };

            collection.ChangeSource(source);
            collection.ChangeTemplate(new DataTemplate(() => new Content()));
            source.Clear();

            CollectionAssert.IsEmpty(collection);
        }

        [Test]
        public static void Add()
        {
            var collection = new Collection(null);
            var content = new Content();

            collection.Add(content);

            CollectionAssert.AreEqual(
                new[] { new TemplatedItem<Content> { Content = content } },
                collection);
        }

        [Test]
        public static void AddToReadOnly()
        {
            var collection = new Collection(null);
            collection.ChangeSource(new int[0]);

            Assert.Throws(
                Is.TypeOf<Exception>().And.Message.EqualTo("this collection is read-only"),
                () => collection.Add(new Content()));
        }

        [Test]
        public static void Clear()
        {
            var collection = new Collection(null);

            collection.Add(new Content());
            collection.Clear();

            CollectionAssert.IsEmpty(collection);
        }

        [Test]
        public static void ClearReadOnly()
        {
            var collection = new Collection(null);
            collection.ChangeSource(new int[0]);

            Assert.Throws(
                Is.TypeOf<Exception>().And.Message.EqualTo("this collection is read-only"),
                () => collection.Clear());
        }

        [Test]
        public static void Contains()
        {
            var collection = new Collection(null);
            var content = new Content();

            collection.Add(content);
            Assert.IsTrue(collection.Contains(content));
        }

        [Test]
        public static void CopyTo()
        {
            var collection = new Collection(null);
            var content = new Content();
            var list = new Content[1];

            collection.Add(content);
            collection.CopyTo(list, 0);

            CollectionAssert.AreEqual(new[] { content }, list);
        }

        [Test]
        public static void Remove()
        {
            var collection = new Collection(null);
            var content = new Content();

            collection.Add(content);
            Assert.IsTrue(collection.Remove(content));

            CollectionAssert.IsEmpty(collection);
        }

        [Test]
        public static void RemoveAlreadyRemoved()
        {
            var collection = new Collection(null);
            Assert.IsFalse(collection.Remove(new Content()));
        }

        [Test]
        public static void RemoveFromReadOnly()
        {
            var collection = new Collection(null);
            collection.ChangeSource(new int[0]);

            Assert.Throws(
                Is.TypeOf<Exception>().And.Message.EqualTo("this collection is read-only"),
                () => collection.Remove(new Content()));
        }
    }
}
