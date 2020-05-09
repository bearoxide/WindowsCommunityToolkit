﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCOREAPP3_1

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_NullableRefOfT
    {
        [TestCategory("NullableRefOfT")]
        [TestMethod]
        public void Test_NullableRefOfT_CreateNullableRefOfT_Ok()
        {
            int value = 1;
            var reference = new NullableRef<int>(ref value);

            Assert.IsTrue(reference.HasValue);
            Assert.IsTrue(Unsafe.AreSame(ref value, ref reference.Value));

            reference.Value++;

            Assert.AreEqual(value, 2);
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        public void Test_NullableRefOfT_CreateNullableRefOfT_Null()
        {
            NullableRef<int> reference = default;

            Assert.IsFalse(reference.HasValue);
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_NullableRefOfT_CreateNullableRefOfT_Null_Exception()
        {
            NullableRef<int> reference = default;

            _ = reference.Value;
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        public void Test_NullableRefOfT_CreateNullableRefOfT_ExplicitCastOfT()
        {
            int value = 42;
            var reference = new NullableRef<int>(ref value);

            Assert.AreEqual(value, (int)reference);
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_NullableRefOfT_CreateNullableRefOfT_ExplicitCastOfT_Exception()
        {
            NullableRef<int> invalid = default;

            _ = (int)invalid;
        }
    }
}

#endif