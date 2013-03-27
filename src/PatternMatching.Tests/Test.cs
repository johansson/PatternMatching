/*
 * Copyright (c) 2013, Will Johansson
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met: 
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer. 
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PatternMatching.Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void WithEqualPredicate()
        {
            int a = 5;
            FutureWithPredicate<int> actual = null;
            Func<int,bool> predicate = x => x == 5;

            actual = a.With(predicate);

            Assert.IsNotNull(actual);
            Assert.AreSame(predicate, actual.Predicate);
            Assert.AreEqual(a, actual.Value);
            Assert.IsTrue(actual.Matched);
        }

        [TestMethod]
        public void WithUnequalPredicate()
        {
            int a = 5;
            FutureWithPredicate<int> actual = null;
            Func<int, bool> predicate = x => x != 5;

            actual = a.With(predicate);

            Assert.IsNotNull(actual);
            Assert.AreSame(predicate, actual.Predicate);
            Assert.AreEqual<int>(a, actual.Value);
            Assert.IsFalse(actual.Matched);
        }

        [TestMethod]
        public void WithDoEqualPredicate()
        {
            int a = 5;
            int actual = -1;
            string output = null;
            Func<int, bool> predicate = x => x == 5;
            Action<int> lambda = x => output = x.ToString();

            actual = a.With(predicate).Do(lambda);

            Assert.AreEqual<int>(5, actual);
            Assert.AreEqual<string>("5", output);
        }

        [TestMethod]
        public void WithDoUnequalPredicate()
        {
            int a = 5;
            int actual = -1;
            string output = null;
            Func<int, bool> predicate = x => x != 5;
            Action<int> lambda = x => output = x.ToString();

            actual = a.With(predicate).Do(lambda);

            Assert.AreEqual<int>(5, actual);
            Assert.IsNull(output);
        }

        [TestMethod]
        public void Fluent()
        {
            int a = 5;
            int actual = -1;
            string output1 = null, output2 = null;
            Func<int, bool> predicate1 = x => x == 5;
            Func<int, bool> predicate2 = x => x != 5;
            Action<int> lambda1 = x => output1 = "It is 5!";
            Action<int> lambda2 = x => output2 = "It is not 5!";

            actual = a.With(predicate1).Do(lambda1)
                        .With(predicate2).Do(lambda2);

            Assert.AreEqual<int>(5, actual);
            Assert.AreEqual<string>("It is 5!", output1);
            Assert.IsNull(output2);
        }
    }
}
