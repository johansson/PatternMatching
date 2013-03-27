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

namespace PatternMatching
{
    using System;

    internal interface IUnexecutable { }

    public class FutureWithPredicate<T>
    {
        public T Value { get; set; }
        public Func<T, bool> Predicate { get; set; }
        public bool Matched { get { return Predicate(Value); } }
    }

    public static class Extensions
    {
        public static Tuple<FutureWithPredicate<T>, bool> With<T>(this T value, Func<T, bool> predicate)
        {
            /*
            //
            // Is it worth the trouble? Doing this would prevent you wanting to use some tuples for your rule processing.
            // You already have a compiler error if you try to use similar lambdas (as before the Otherwise call) after an Otherwise call.
            //
            if (value.GetType() == typeof(Tuple) && value.GetType().IsGenericType && value.GetType().GetGenericArguments().Length == 3)
                throw new ArgumentException("Cannot do another operation after an otherwise operation.", "value");
             */

            return new Tuple<FutureWithPredicate<T>, bool>(new FutureWithPredicate<T>() { Value = value, Predicate = predicate }, false);
        }

        public static Tuple<T, bool> Do<T>(this Tuple<FutureWithPredicate<T>, bool> future, Action<T> lambda)
        {
            if (future.Item1.Matched)
                lambda(future.Item1.Value);

            return new Tuple<T, bool>(future.Item1.Value, future.Item1.Matched | future.Item2);
        }

        public static Tuple<FutureWithPredicate<T>, bool> With<T>(this Tuple<T, bool> value, Func<T, bool> predicate)
        {
            return new Tuple<FutureWithPredicate<T>, bool>(new FutureWithPredicate<T>() { Value = value.Item1, Predicate = predicate }, value.Item2);
        }

        public static Tuple<T, object> Otherwise<T>(this Tuple<T, bool> result, Action<T> lambda)
        {
            if(!result.Item2)
                lambda(result.Item1);

            return new Tuple<T, object>(result.Item1, new object());
        }
    }
}
