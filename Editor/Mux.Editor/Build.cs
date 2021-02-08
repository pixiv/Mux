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
using System;
using System.Collections;

namespace Mux.Editor.Build
{
    internal sealed class Logger : Microsoft.Build.Utilities.Logger
    {
        public override void Initialize(IEventSource source)
        {
            source.MessageRaised += (sender, args) =>
            {
                if ((args.Importance == MessageImportance.High && IsVerbosityAtLeast(LoggerVerbosity.Minimal)) ||
                    (args.Importance == MessageImportance.Normal && IsVerbosityAtLeast(LoggerVerbosity.Normal)) ||
                    IsVerbosityAtLeast(LoggerVerbosity.Detailed))
                {
                    UnityEngine.Debug.Log(args.Message);
                }
            };

            source.ErrorRaised += (sender, args) =>
                UnityEngine.Debug.LogError(FormatErrorEvent(args));

            source.WarningRaised += (sender, args) =>
                UnityEngine.Debug.LogWarning(FormatWarningEvent(args));

            source.CustomEventRaised += (sender, args) =>
                UnityEngine.Debug.Log(args.Message);
        }
    }

    internal sealed class BuildEngine : IBuildEngine, IEventSource
    {
        public event AnyEventHandler AnyEventRaised;
        public event BuildMessageEventHandler MessageRaised;
        public event BuildErrorEventHandler ErrorRaised;
        public event BuildWarningEventHandler WarningRaised;
        public event CustomBuildEventHandler CustomEventRaised;

#pragma warning disable 0067
        public event BuildStartedEventHandler BuildStarted;
        public event BuildFinishedEventHandler BuildFinished;
        public event ProjectStartedEventHandler ProjectStarted;
        public event ProjectFinishedEventHandler ProjectFinished;
        public event TargetStartedEventHandler TargetStarted;
        public event TargetFinishedEventHandler TargetFinished;
        public event TaskStartedEventHandler TaskStarted;
        public event TaskFinishedEventHandler TaskFinished;
        public event BuildStatusEventHandler StatusEventRaised;
#pragma warning restore 0067

        public void LogErrorEvent(BuildErrorEventArgs args)
        {
            ErrorRaised?.Invoke(this, args);
            AnyEventRaised?.Invoke(this, args);
        }

        public void LogWarningEvent(BuildWarningEventArgs args)
        {
            WarningRaised?.Invoke(this, args);
            AnyEventRaised?.Invoke(this, args);
        }

        public void LogMessageEvent(BuildMessageEventArgs args)
        {
            MessageRaised?.Invoke(this, args);
            AnyEventRaised?.Invoke(this, args);
        }

        public void LogCustomEvent(CustomBuildEventArgs args)
        {
            CustomEventRaised?.Invoke(this, args);
            AnyEventRaised?.Invoke(this, args);
        }

        public bool ContinueOnError => false;
        public int LineNumberOfTaskNode => 0;
        public int ColumnNumberOfTaskNode => 0;
        public string ProjectFileOfTaskNode { get; set; }

        public bool BuildProjectFile(string project, string[] targets, IDictionary properties, IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }
    }
}
