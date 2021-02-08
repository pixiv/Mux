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

using System.ComponentModel;
using UnityEngine;

namespace Mux.Playground
{
    /// <summary>
    /// A class that stores data referenced by Mux views.
    /// </summary>
    [CreateAssetMenu]
    internal sealed class Resources : ScriptableObject, INotifyPropertyChanged
    {
#pragma warning disable 0649
        [SerializeField] private Font _font;
        [SerializeField] private Sprite _background;
        [SerializeField] private Sprite _checkmark;
        [SerializeField] private Sprite _dropdownArrow;
        [SerializeField] private Sprite _inputFieldBackground;
        [SerializeField] private Sprite _knob;
        [SerializeField] private Sprite _uiMask;
        [SerializeField] private Sprite _uiSprite;
#pragma warning restore 0649

        private void OnValidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public Font Font => _font;
        public Sprite Background => _background;
        public Sprite Checkmark => _checkmark;
        public Sprite DropdownArrow => _dropdownArrow;
        public Sprite InputFieldBackground => _inputFieldBackground;
        public Sprite Knob => _knob;
        public Sprite UIMask => _uiMask;
        public Sprite UISprite => _uiSprite;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
