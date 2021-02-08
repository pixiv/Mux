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

#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mux.Markup
{
    /// <summary>A class that holds references to all <see cref="Transform"/>s.</summary>
    /// <remarks>This class is only available on Unity Editor.</remarks>
    public static class Transforms
    {
        private static readonly Dictionary<string, HashSet<WeakReference<IInternalTransform>>> s_dictionary =
            new Dictionary<string, HashSet<WeakReference<IInternalTransform>>>();

        internal static void Add(string path, WeakReference<IInternalTransform> reference)
        {
            lock (((ICollection)s_dictionary).SyncRoot)
            {
                if (!s_dictionary.TryGetValue(path, out var xamlInstances))
                {
                    xamlInstances = new HashSet<WeakReference<IInternalTransform>>();
                    s_dictionary[path] = xamlInstances;
                }

                xamlInstances.Add(reference);
            }
        }

        internal static void Remove(string path, WeakReference<IInternalTransform> reference)
        {
            lock (((ICollection)s_dictionary).SyncRoot)
            {
                if (s_dictionary.TryGetValue(path, out var xamlInstances))
                {
                    xamlInstances.Remove(reference);

                    if (xamlInstances.Count <= 0)
                    {
                        s_dictionary.Remove(path);
                    }
                }
            }
        }

        /// <summary>A method to reload XAML files specified by given paths.</summary>
        public static void Reload(IEnumerable<string> paths)
        {
            var parents = new HashSet<IInternalTransform>();

            lock (((ICollection)s_dictionary).SyncRoot)
            {
                foreach (var path in paths)
                {
                    if (!s_dictionary.TryGetValue(path, out var value))
                    {
                        continue;
                    }

                    foreach (var reference in value)
                    {
                        if (!reference.TryGetTarget(out var target))
                        {
                            continue;
                        }

                        var parent = target;

                        while (true)
                        {
                            if (parent is IReloadable)
                            {
                                parents.Add(parent);
                                break;
                            }

                            if (parent.Parent == null)
                            {
                                Debug.Log($"Reloadable Mux ancestor not found for {target}.");
                                break;
                            }

                            parent = parent.Parent;
                        }
                    }
                }
            }

            foreach (var parent in parents)
            {
                var active = parent.ActiveSelf;

                parent.BindTransforms();

                try
                {
                    parent.ActiveSelf = false;
                    parent.Clear();
                    ((IReloadable)parent).Reload();
                }
                finally
                {
                    parent.ActiveSelf = active;
                }

                parent.ReleaseTransforms();
            }

            if (parents.Count > 0)
            {
                Debug.Log("Reloaded Mux");
            }
        }
    }
}

#endif
