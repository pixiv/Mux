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
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mux.Tests.Editor.Build
{
    internal sealed class CustomBuildEventArgs : Microsoft.Build.Framework.CustomBuildEventArgs
    {
    }

    [TestFixture]
    public class Logger
    {
        private Mux.Editor.Build.BuildEngine _engine;
        private Mux.Editor.Build.Logger _logger;

        [SetUp]
        public void Setup()
        {
            _engine = new Mux.Editor.Build.BuildEngine();
            _logger = new Mux.Editor.Build.Logger();
            _logger.Initialize(_engine);
        }

        [TearDown]
        public void TearDown()
        {
            _logger.Shutdown();
        }

        [TestCase(MessageImportance.High, LoggerVerbosity.Minimal)]
        [TestCase(MessageImportance.Normal, LoggerVerbosity.Normal)]
        [TestCase(MessageImportance.Low, LoggerVerbosity.Detailed)]
        public void ImportantMessage(MessageImportance importance, LoggerVerbosity verbosity)
        {
            _logger.Verbosity = verbosity;

            _engine.LogMessageEvent(new BuildMessageEventArgs(
                "message",
                "helpKeyword",
                "senderName",
                importance));

            LogAssert.Expect(LogType.Log, "message");
        }

        [TestCase(MessageImportance.High, LoggerVerbosity.Quiet)]
        [TestCase(MessageImportance.Normal, LoggerVerbosity.Minimal)]
        public void TrivialMessage(MessageImportance importance, LoggerVerbosity verbosity)
        {
            _logger.Verbosity = verbosity;

            _engine.LogMessageEvent(new BuildMessageEventArgs(
                "message",
                "helpKeyword",
                "senderName",
                importance));

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void Error()
        {
            _engine.LogErrorEvent(new BuildErrorEventArgs(
                "subcategory",
                "code",
                "file",
                0,
                1,
                2,
                3,
                "message",
                "helpKeyword",
                "senderName"));

            LogAssert.Expect(LogType.Error, "file : subcategory error code: message");
        }

        [Test]
        public void Warning()
        {
            _engine.LogWarningEvent(new BuildWarningEventArgs(
                "subcategory",
                "code",
                "file",
                0,
                1,
                2,
                3,
                "message",
                "helpKeyword",
                "senderName"));

            LogAssert.Expect(LogType.Warning, "file : subcategory warning code: message");
        }

        [Test]
        public void CustomEvent()
        {
            _engine.LogCustomEvent(new CustomBuildEventArgs());
            LogAssert.Expect(LogType.Log, "Null");
        }
    }

    [TestFixture]
    public class BuildEngine
    {
        private Mux.Editor.Build.BuildEngine _engine;

        [SetUp]
        public void Setup()
        {
            _engine = new Mux.Editor.Build.BuildEngine();
        }

        [Test]
        public void LogErrorEvent()
        {
            var expected = new BuildErrorEventArgs(
                "subcategory",
                "code",
                "file",
                0,
                1,
                2,
                3,
                "message",
                "helpKeyword",
                "senderName");

            BuildEventArgs anyEvent = null;
            BuildErrorEventArgs errorEvent = null;

            _engine.AnyEventRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                anyEvent = args;
            };

            _engine.ErrorRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                errorEvent = args;
            };

            _engine.LogErrorEvent(expected);

            Assert.AreEqual(expected, anyEvent);
            Assert.AreEqual(expected, errorEvent);
        }

        [Test]
        public void LogWarningEvent()
        {
            var expected = new BuildWarningEventArgs(
                "subcategory",
                "code",
                "file",
                0,
                1,
                2,
                3,
                "message",
                "helpKeyword",
                "senderName");

            BuildEventArgs anyEvent = null;
            BuildWarningEventArgs warningEvent = null;

            _engine.AnyEventRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                anyEvent = args;
            };

            _engine.WarningRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                warningEvent = args;
            };

            _engine.LogWarningEvent(expected);

            Assert.AreEqual(expected, anyEvent);
            Assert.AreEqual(expected, warningEvent);
        }

        [Test]
        public void LogMessageEvent()
        {
            var expected = new BuildMessageEventArgs(
                "message",
                "helpKeyword",
                "senderName",
                MessageImportance.High);

            BuildEventArgs anyEvent = null;
            BuildMessageEventArgs messageEvent = null;

            _engine.AnyEventRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                anyEvent = args;
            };

            _engine.MessageRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                messageEvent = args;
            };

            _engine.LogMessageEvent(expected);

            Assert.AreEqual(expected, anyEvent);
            Assert.AreEqual(expected, messageEvent);
        }

        [Test]
        public void LogCustomEvent()
        {
            var expected = new CustomBuildEventArgs();
            BuildEventArgs anyEvent = null;
            Microsoft.Build.Framework.CustomBuildEventArgs customEvent = null;

            _engine.AnyEventRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                anyEvent = args;
            };

            _engine.CustomEventRaised += (sender, args) =>
            {
                Assert.AreEqual(_engine, sender);
                customEvent = args;
            };

            _engine.LogCustomEvent(expected);

            Assert.AreEqual(expected, anyEvent);
            Assert.AreEqual(expected, customEvent);
        }

        [Test]
        public void ContinueOnError()
        {
            Assert.IsFalse(_engine.ContinueOnError);
        }

        [Test]
        public void LineNumberOfTaskNode()
        {
            Assert.AreEqual(0, _engine.LineNumberOfTaskNode);
        }

        [Test]
        public void ColumnNumberOfTaskNode()
        {
            Assert.AreEqual(0, _engine.ColumnNumberOfTaskNode);
        }

        [Test]
        public void ProjectFileOfTaskNode()
        {
            Assert.Null(_engine.ProjectFileOfTaskNode);
            _engine.ProjectFileOfTaskNode = "";
            Assert.AreEqual("", _engine.ProjectFileOfTaskNode);
        }

        [Test]
        public void BuildProjectFile()
        {
            Assert.Throws<NotImplementedException>(
                () => _engine.BuildProjectFile(null, null, null, null));
        }
    }
}
