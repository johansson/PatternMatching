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
using System.Linq;
using System.Text;

namespace PatternMatching.Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void WithEqualPredicate()
        {
            int a = 5;
            Tuple<FutureWithPredicate<int>, bool> actual = null;
            Func<int,bool> predicate = x => x == 5;

            actual = a.When(predicate);

            Assert.IsNotNull(actual);
            Assert.AreSame(predicate, actual.Item1.Predicate);
            Assert.AreEqual<int>(a, actual.Item1.Value);
            Assert.IsTrue(actual.Item1.Matched);
        }

        [TestMethod]
        public void WithUnequalPredicate()
        {
            int a = 5;
            Tuple<FutureWithPredicate<int>, bool> actual = null;
            Func<int, bool> predicate = x => x != 5;

            actual = a.When(predicate);

            Assert.IsNotNull(actual);
            Assert.AreSame(predicate, actual.Item1.Predicate);
            Assert.AreEqual<int>(a, actual.Item1.Value);
            Assert.IsFalse(actual.Item1.Matched);
        }

        [TestMethod]
        public void WithDoEqualPredicate()
        {
            int a = 5;
            int actual = -1;
            string output = null;
            Func<int, bool> predicate = x => x == 5;
            Action<int> lambda = x => output = x.ToString();

            actual = a.When(predicate).Do(lambda).Item1;

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

            actual = a.When(predicate).Do(lambda).Item1;

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

            actual = a.When(predicate1).Do(lambda1)
                        .When(predicate2).Do(lambda2).Item1;

            Assert.AreEqual<int>(5, actual);
            Assert.AreEqual<string>("It is 5!", output1);
            Assert.IsNull(output2);
        }

        /*
         * See note in PatternMatching.cs
         *
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithAfterOtherwise()
        {
            int a = 5;
            a.With(x => x > 0).Do(_ => { })
             .Otherwise(_ => { })
             .With(x => x.Item1 == 0).Do(_ => { });
        }
         */

        [TestMethod]
        public void FizzBuzz()
        {
            var sb = new StringBuilder(640);

            foreach (var i in Enumerable.Range(1, 100))
                i.When(x => x % 3 == 0).Do(_ => sb.Append("Fizz"))
                 .When(x => x % 5 == 0).Do(_ => sb.Append("Buzz"))
                 .Then(() => sb.AppendLine())
                 .Otherwise(x => sb.AppendLine(x.ToString()));

            Assert.AreEqual<string>(@"1
2
Fizz
4
Buzz
Fizz
7
8
Fizz
Buzz
11
Fizz
13
14
FizzBuzz
16
17
Fizz
19
Buzz
Fizz
22
23
Fizz
Buzz
26
Fizz
28
29
FizzBuzz
31
32
Fizz
34
Buzz
Fizz
37
38
Fizz
Buzz
41
Fizz
43
44
FizzBuzz
46
47
Fizz
49
Buzz
Fizz
52
53
Fizz
Buzz
56
Fizz
58
59
FizzBuzz
61
62
Fizz
64
Buzz
Fizz
67
68
Fizz
Buzz
71
Fizz
73
74
FizzBuzz
76
77
Fizz
79
Buzz
Fizz
82
83
Fizz
Buzz
86
Fizz
88
89
FizzBuzz
91
92
Fizz
94
Buzz
Fizz
97
98
Fizz
Buzz
", sb.ToString());
        }
    }
}
