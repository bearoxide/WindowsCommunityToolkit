﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Enumerables;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.Toolkit.HighPerformance.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Memory
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_ReadOnlySpan2DT
    {
        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Empty()
        {
            ReadOnlySpan2D<int> empty1 = default;

            Assert.IsTrue(empty1.IsEmpty);
            Assert.AreEqual(empty1.Size, 0);
            Assert.AreEqual(empty1.Width, 0);
            Assert.AreEqual(empty1.Height, 0);

            ReadOnlySpan2D<string> empty2 = ReadOnlySpan2D<string>.Empty;

            Assert.IsTrue(empty2.IsEmpty);
            Assert.AreEqual(empty2.Size, 0);
            Assert.AreEqual(empty2.Width, 0);
            Assert.AreEqual(empty2.Height, 0);
        }

#if !WINDOWS_UWP
        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public unsafe void Test_ReadOnlySpan2DT_RefConstructor()
        {
            ReadOnlySpan<int> span = stackalloc[]
            {
                1, 2, 3, 4, 5, 6
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(span[0], 2, 3, 0);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);
            Assert.AreEqual(span2d[0, 0], 1);
            Assert.AreEqual(span2d[1, 2], 6);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(Unsafe.AsRef<int>(null), -1, 0, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(Unsafe.AsRef<int>(null), 1, -2, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(Unsafe.AsRef<int>(null), 1, 0, -5));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public unsafe void Test_ReadOnlySpan2DT_PtrConstructor()
        {
            int* ptr = stackalloc[]
            {
                1, 2, 3, 4, 5, 6
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(ptr, 2, 3, 0);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);
            Assert.AreEqual(span2d[0, 0], 1);
            Assert.AreEqual(span2d[1, 2], 6);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>((void*)0, -1, 0, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>((void*)0, 1, -2, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>((void*)0, 1, 0, -5));
        }
#endif

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Array1DConstructor()
        {
            int[] array =
            {
                1, 2, 3, 4, 5, 6
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 1, 2, 2, 1);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 4);
            Assert.AreEqual(span2d.Width, 2);
            Assert.AreEqual(span2d.Height, 2);
            Assert.AreEqual(span2d[0, 0], 2);
            Assert.AreEqual(span2d[1, 1], 6);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new ReadOnlySpan2D<object>(new string[1], 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, -99, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 0, -10, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 0, 1, 1, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 0, 1, -100, 1));
            Assert.ThrowsException<ArgumentException>(() => new ReadOnlySpan2D<int>(array, 0, 10, 1, 120));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Array2DConstructor_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);
            Assert.AreEqual(span2d[0, 1], 2);
            Assert.AreEqual(span2d[1, 2], 6);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new ReadOnlySpan2D<object>(new string[1, 2]));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Array2DConstructor_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 0, 1, 2, 2);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 4);
            Assert.AreEqual(span2d.Width, 2);
            Assert.AreEqual(span2d.Height, 2);
            Assert.AreEqual(span2d[0, 0], 2);
            Assert.AreEqual(span2d[1, 1], 6);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new ReadOnlySpan2D<object>(new string[1, 2], 0, 0, 2, 2));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Array3DConstructor_1()
        {
            int[,,] array =
            {
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 }
                },
                {
                    { 10, 20, 30 },
                    { 40, 50, 60 }
                }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 1);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);
            Assert.AreEqual(span2d[0, 0], 10);
            Assert.AreEqual(span2d[0, 1], 20);
            Assert.AreEqual(span2d[1, 2], 60);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 20));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Array3DConstructor_2()
        {
            int[,,] array =
            {
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 }
                },
                {
                    { 10, 20, 30 },
                    { 40, 50, 60 }
                }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 1, 0, 1, 2, 2);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 4);
            Assert.AreEqual(span2d.Width, 2);
            Assert.AreEqual(span2d.Height, 2);
            Assert.AreEqual(span2d[0, 0], 20);
            Assert.AreEqual(span2d[0, 1], 30);
            Assert.AreEqual(span2d[1, 1], 60);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, -1, 1, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 1, -1, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 1, 1, -1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 1, 1, 1, -1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array, 1, 1, 1, 1, -1));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_CopyTo_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            int[] target = new int[array.Length];

            span2d.CopyTo(target);

            CollectionAssert.AreEqual(array, target);

            Assert.ThrowsException<ArgumentException>(() => new ReadOnlySpan2D<int>(array).CopyTo(Span<int>.Empty));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_CopyTo_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 0, 1, 2, 2);

            int[] target = new int[4];

            span2d.CopyTo(target);

            int[] expected = { 2, 3, 5, 6 };

            CollectionAssert.AreEqual(target, expected);

            Assert.ThrowsException<ArgumentException>(() => new ReadOnlySpan2D<int>(array).CopyTo(Span<int>.Empty));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_CopyTo2D_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            int[,] target = new int[2, 3];

            span2d.CopyTo(target);

            CollectionAssert.AreEqual(array, target);

            Assert.ThrowsException<ArgumentException>(() => new ReadOnlySpan2D<int>(array).CopyTo(Span2D<int>.Empty));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_CopyTo2D_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 0, 1, 2, 2);

            int[,] target = new int[2, 2];

            span2d.CopyTo(target);

            int[,] expected =
            {
                { 2, 3 },
                { 5, 6 }
            };

            CollectionAssert.AreEqual(target, expected);

            Assert.ThrowsException<ArgumentException>(() => new ReadOnlySpan2D<int>(array).CopyTo(new Span2D<int>(target)));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_TryCopyTo()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            int[] target = new int[array.Length];

            Assert.IsTrue(span2d.TryCopyTo(target));
            Assert.IsFalse(span2d.TryCopyTo(Span<int>.Empty));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_TryCopyTo2D()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            int[,] target = new int[2, 3];

            Assert.IsTrue(span2d.TryCopyTo(target));
            Assert.IsFalse(span2d.TryCopyTo(Span2D<int>.Empty));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public unsafe void Test_ReadOnlySpan2DT_GetPinnableReference()
        {
            Assert.IsTrue(Unsafe.AreSame(
                ref Unsafe.AsRef<int>(null),
                ref ReadOnlySpan2D<int>.Empty.GetPinnableReference()));

            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            ref int r0 = ref span2d.GetPinnableReference();

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref array[0, 0]));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public unsafe void Test_ReadOnlySpan2DT_DangerousGetReference()
        {
            Assert.IsTrue(Unsafe.AreSame(
                ref Unsafe.AsRef<int>(null),
                ref ReadOnlySpan2D<int>.Empty.DangerousGetReference()));

            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            ref int r0 = ref span2d.DangerousGetReference();

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref array[0, 0]));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Slice_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            ReadOnlySpan2D<int> slice1 = span2d.Slice(1, 1, 2, 1);

            Assert.AreEqual(slice1.Size, 2);
            Assert.AreEqual(slice1.Height, 1);
            Assert.AreEqual(slice1.Width, 2);
            Assert.AreEqual(slice1[0, 0], 5);
            Assert.AreEqual(slice1[0, 1], 6);

            ReadOnlySpan2D<int> slice2 = span2d.Slice(0, 1, 2, 2);

            Assert.AreEqual(slice2.Size, 4);
            Assert.AreEqual(slice2.Height, 2);
            Assert.AreEqual(slice2.Width, 2);
            Assert.AreEqual(slice2[0, 0], 2);
            Assert.AreEqual(slice2[1, 0], 5);
            Assert.AreEqual(slice2[1, 1], 6);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).Slice(-1, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).Slice(1, -1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).Slice(1, 1, -1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).Slice(1, 1, 1, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).Slice(10, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).Slice(1, 12, 12, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).Slice(1, 1, 1, 55));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_Slice_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            ReadOnlySpan2D<int> slice1 = span2d.Slice(0, 0, 2, 2);

            Assert.AreEqual(slice1.Size, 4);
            Assert.AreEqual(slice1.Height, 2);
            Assert.AreEqual(slice1.Width, 2);
            Assert.AreEqual(slice1[0, 0], 1);
            Assert.AreEqual(slice1[1, 1], 5);

            ReadOnlySpan2D<int> slice2 = slice1.Slice(1, 0, 2, 1);

            Assert.AreEqual(slice2.Size, 2);
            Assert.AreEqual(slice2.Height, 1);
            Assert.AreEqual(slice2.Width, 2);
            Assert.AreEqual(slice2[0, 0], 4);
            Assert.AreEqual(slice2[0, 1], 5);

            ReadOnlySpan2D<int> slice3 = slice2.Slice(0, 1, 1, 1);

            Assert.AreEqual(slice3.Size, 1);
            Assert.AreEqual(slice3.Height, 1);
            Assert.AreEqual(slice3.Width, 1);
            Assert.AreEqual(slice3[0, 0], 5);
        }

#if !WINDOWS_UWP
        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_GetRowReadOnlySpan()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            ReadOnlySpan<int> span = span2d.GetRowSpan(1);

            Assert.IsTrue(Unsafe.AreSame(
                ref Unsafe.AsRef(span[0]),
                ref array[1, 0]));
            Assert.IsTrue(Unsafe.AreSame(
                ref Unsafe.AsRef(span[2]),
                ref array[1, 2]));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetRowSpan(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetRowSpan(5));
        }
#endif

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_TryGetReadOnlySpan_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            bool success = span2d.TryGetSpan(out ReadOnlySpan<int> span);

#if WINDOWS_UWP
            // Can't get a ReadOnlySpan<T> over a T[,] array on UWP
            Assert.IsFalse(success);
            Assert.AreEqual(span.Length, 0);
#else
            Assert.IsTrue(success);
            Assert.AreEqual(span.Length, span2d.Size);
#endif
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_TryGetReadOnlySpan_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 0, 0, 2, 2);

            bool success = span2d.TryGetSpan(out ReadOnlySpan<int> span);

            Assert.IsFalse(success);
            Assert.IsTrue(span.IsEmpty);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_ToArray_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            int[,] copy = span2d.ToArray();

            Assert.AreEqual(copy.GetLength(0), array.GetLength(0));
            Assert.AreEqual(copy.GetLength(1), array.GetLength(1));

            CollectionAssert.AreEqual(array, copy);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_ToArray_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array, 0, 0, 2, 2);

            int[,] copy = span2d.ToArray();

            Assert.AreEqual(copy.GetLength(0), 2);
            Assert.AreEqual(copy.GetLength(1), 2);

            int[,] expected =
            {
                { 1, 2 },
                { 4, 5 }
            };

            CollectionAssert.AreEqual(expected, copy);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_ReadOnlySpan2DT_Equals()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            _ = span2d.Equals(null);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_ReadOnlySpan2DT_GetHashCode()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            _ = span2d.GetHashCode();
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_ToString()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d = new ReadOnlySpan2D<int>(array);

            string text = span2d.ToString();

            const string expected = "Microsoft.Toolkit.HighPerformance.Memory.ReadOnlySpan2D<System.Int32>[2, 3]";

            Assert.AreEqual(text, expected);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_opEquals()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d_1 = new ReadOnlySpan2D<int>(array);
            ReadOnlySpan2D<int> span2d_2 = new ReadOnlySpan2D<int>(array);

            Assert.IsTrue(span2d_1 == span2d_2);
            Assert.IsFalse(span2d_1 == ReadOnlySpan2D<int>.Empty);
            Assert.IsTrue(ReadOnlySpan2D<int>.Empty == ReadOnlySpan2D<int>.Empty);

            ReadOnlySpan2D<int> span2d_3 = new ReadOnlySpan2D<int>(array, 0, 0, 2, 2);

            Assert.IsFalse(span2d_1 == span2d_3);
            Assert.IsFalse(span2d_3 == ReadOnlySpan2D<int>.Empty);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_ImplicitCast()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            ReadOnlySpan2D<int> span2d_1 = array;
            ReadOnlySpan2D<int> span2d_2 = new ReadOnlySpan2D<int>(array);

            Assert.IsTrue(span2d_1 == span2d_2);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_GetRow()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            int i = 0;
            foreach (ref readonly int value in new ReadOnlySpan2D<int>(array).GetRow(1))
            {
                Assert.IsTrue(Unsafe.AreSame(ref Unsafe.AsRef(value), ref array[1, i++]));
            }

            ReadOnlyRefEnumerable<int> enumerable = new ReadOnlySpan2D<int>(array).GetRow(1);

            int[] expected = { 4, 5, 6 };

            CollectionAssert.AreEqual(enumerable.ToArray(), expected);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetRow(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetRow(2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetRow(1000));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_GetColumn()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            int i = 0;
            foreach (ref readonly int value in new ReadOnlySpan2D<int>(array).GetColumn(1))
            {
                Assert.IsTrue(Unsafe.AreSame(ref Unsafe.AsRef(value), ref array[i++, 1]));
            }

            ReadOnlyRefEnumerable<int> enumerable = new ReadOnlySpan2D<int>(array).GetColumn(2);

            int[] expected = { 3, 6 };

            CollectionAssert.AreEqual(enumerable.ToArray(), expected);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetColumn(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetColumn(3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ReadOnlySpan2D<int>(array).GetColumn(1000));
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_GetEnumerator()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            int[] result = new int[4];
            int i = 0;

            foreach (var item in new ReadOnlySpan2D<int>(array, 0, 1, 2, 2))
            {
                result[i++] = item;
            }

            int[] expected = { 2, 3, 5, 6 };

            CollectionAssert.AreEqual(result, expected);
        }

        [TestCategory("ReadOnlySpan2DT")]
        [TestMethod]
        public void Test_ReadOnlySpan2DT_GetEnumerator_Empty()
        {
            var enumerator = ReadOnlySpan2D<int>.Empty.GetEnumerator();

            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}