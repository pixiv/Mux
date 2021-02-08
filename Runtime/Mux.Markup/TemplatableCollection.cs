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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace Mux.Markup
{
    /// <summary>A class that abstracts a list of templated objects.</summary>
    /// <typeparam name="T">The type of templated objects.</typeparam>
    public abstract class TemplatableCollectionList<T>
    {
        /// <summary>Gets backing list of templated objects.</summary>
        /// <remarks>The returned list must not directly be modified.</remarks>
        protected abstract IList<T> GetList();

        /// <summary>Removes all elements.</summary>
        public virtual void ClearList()
        {
            GetList().Clear();
        }

        /// <summary>Inserts the elements of a collection at the specified index.</summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="enumerable">The collection whose elements should be inserted.</param>
        public abstract void InsertListRange(int index, IEnumerable<T> enumerable);

        /// <summary>Moves the elements of a collection.</summary>
        /// <param name="from">The zero-based index at which the elements currently are.</param>
        /// <param name="to">The zero-based index at which the elements will be.</param>
        /// <param name="count">The number of elements to move.</param>
        public virtual void MoveListRange(int from, int to, int count)
        {
            var list = GetList();

            if (from < to)
            {
                while (count > 0)
                {
                    count--;
                    var temporary = list[from + count];
                    list[from + count] = list[to + count];
                    list[to + count] = temporary;
                }
            }
            else
            {
                for (var index = 0; index < count; index++)
                {
                    var temporary = list[from + index];
                    list[from + index] = list[to + index];
                    list[to + index] = temporary;
                }
            }
        }

        /// <summary>Removes a range of elements.</summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public abstract void RemoveListRange(int index, int count);

        /// <summary>Replaces the elements of a collection.</summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to replace.</param>
        /// <param name="enumerable">The collection whose elements should be replaced with.</param>
        public virtual void ReplaceListRange(int index, int count, IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            var list = GetList();

            while (count > 0 && enumerator.MoveNext())
            {
                list[index] = enumerator.Current;
                count--;
                index++;
            }
        }
    }

    /// <summary>A class to template <see cref="BindableObject" />.</summary>
    /// <remarks>
    /// Interfaces <see cref="TemplatableCollection{T}" /> provides are available only when templating is not
    /// performed.
    /// </remarks>
    /// <typeparam name="T">The type of templated objects.</typeparam>
    public abstract class TemplatableCollection<T> : TemplatableCollectionList<TemplatedItem<T>>, ICollection<T> where T : BindableObject
    {
        private IReadOnlyCollection<object> _source = null;
        private DataTemplate _template = null;

        /// <summary>A <see cref="BindableObject" /> that owns this instance.</summary>
        protected readonly BindableObject container;

        public int Count => GetList().Count;
        public bool IsReadOnly => _source != null || _template != null;

        /// <param name="container">A <see cref="BindableObject" /> that owns this instance.</param>
        public TemplatableCollection(BindableObject container)
        {
            this.container = container;
        }

        /// <summary>Changes the source list of items to template.</summary>
        /// <param name="source">The new source list of items to template.</param>
        public void ChangeSource(IEnumerable source)
        {
            if (_source == source)
            {
                return;
            }

            if (_source != null)
            {
                ((INotifyCollectionChanged)_source).CollectionChanged -= OnSourceCollectionChanged;
            }

            _source = new ListProxy(source, int.MaxValue);
            ((INotifyCollectionChanged)_source).CollectionChanged += OnSourceCollectionChanged;

            if (_template != null)
            {
                Reset();
            }
        }

        /// <summary>Changes the template.</summary>
        /// <param name="template">The new template.</param>
        public void ChangeTemplate(DataTemplate template)
        {
            if (_template == template)
            {
                return;
            }

            _template = template;

            if (template == null)
            {
                ClearList();
            }
            else if (_source != null)
            {
                Reset();
            }
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_template == null)
            {
                return;
            }

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    InsertListRange(args.NewStartingIndex, Template(args.NewItems));
                    break;

                case NotifyCollectionChangedAction.Move:
                    MoveListRange(args.OldStartingIndex, args.NewStartingIndex, args.OldItems.Count);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveListRange(args.OldStartingIndex, args.OldItems.Count);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    ReplaceListRangeBySource(args.OldStartingIndex, args.NewItems.Count, args.NewItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
            }
        }

        private void Reset()
        {
            var count = ReplaceListRangeBySource(0, int.MaxValue, _source);
            RemoveListRange(count, GetList().Count - count);
        }

        private int ReplaceListRangeBySource(int index, int count, IEnumerable source)
        {
            var array = new TemplatedItem<T>[1];
            var enumerator = source.GetEnumerator();
            var list = GetList();
            var replacedCount = 0;

            while (replacedCount < count && enumerator.MoveNext())
            {
                DataTemplate selected;
                var item = enumerator.Current;

                if (_template is DataTemplateSelector selector)
                {
                    selected = selector.SelectTemplate(item, container);
                }
                else
                {
                    selected = _template;
                }

                if (index < list.Count)
                {
                    var templated = list[index];

                    if (templated.Template == selected)
                    {
                        templated.Content.BindingContext = item;
                    }
                    else
                    {
                        templated.Template = selected;
                        templated.Content = (T)selected.CreateContent();
                        templated.Content.BindingContext = item;
                        array[0] = templated;
                        ReplaceListRange(index, 1, array);
                    }
                }
                else
                {
                    var templated = new TemplatedItem<T>();
                    templated.Template = selected;
                    templated.Content = (T)selected.CreateContent();
                    templated.Content.BindingContext = item;
                    array[0] = templated;
                    InsertListRange(index, array);
                }

                replacedCount++;
                index++;
            }

            return replacedCount;
        }

        private TemplatedItem<T> Template(object source)
        {
            var item = new TemplatedItem<T>();

            if (_template is DataTemplateSelector selector)
            {
                item.Template = selector.SelectTemplate(source, container);
            }
            else
            {
                item.Template = _template;
            }

            item.Content = (T)item.Template.CreateContent();
            item.Content.BindingContext = source;
            return item;
        }

        private IEnumerable<TemplatedItem<T>> Template(IEnumerable sources)
        {
            foreach (var source in sources)
            {
                yield return Template(source);
            }
        }

        public void Add(T content)
        {
            if (IsReadOnly)
            {
                throw new Exception("this collection is read-only");
            }

            var item = new TemplatedItem<T>();
            item.Content = content;
            InsertListRange(GetList().Count, new[] { item });
        }

        public void Clear()
        {
            if (IsReadOnly)
            {
                throw new Exception("this collection is read-only");
            }

            ClearList();
        }

        public bool Contains(T content)
        {
            foreach (var item in GetList())
            {
                if (item.Content == content)
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] destination, int index)
        {
            foreach (var item in GetList())
            {
                destination[index] = item.Content;
                index++;
            }
        }

        public bool Remove(T content)
        {
            if (IsReadOnly)
            {
                throw new Exception("this collection is read-only");
            }

            var list = GetList();
            for (var index = 0; index < list.Count; index++)
            {
                if (list[index].Content == content)
                {
                    RemoveListRange(index, 1);
                    return true;
                }
            }

            return false;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var item in GetList())
            {
                yield return item.Content;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetList().GetEnumerator();
        }
    }

    public struct TemplatedItem<T>
    {
        public DataTemplate Template;
        public T Content;
    }
}
