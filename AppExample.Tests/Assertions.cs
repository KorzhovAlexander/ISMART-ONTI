using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MediatR;
using Shouldly;

namespace AppExample.Tests
{
    using static Environment;
    using static Testing;

    public static class Assertions
    {
        public static TException Throws<TException>(this Action action) where TException : Exception
            => Should.Throw<TException>(action);

        public static void ShouldMatch<T>(this IEnumerable<T> actual, params T[] expected)
            => actual.ToArray().ShouldMatch(expected);

        public static void ShouldMatch<T>(this T actual, T expected)
        {
            if (Json(expected) != Json(actual))
                throw new MatchException(expected, actual);
        }

        public static void ShouldValidate<TResult>(this IRequest<TResult> message)
            => Validation(message).ShouldBeSuccessful();

        public static void ShouldNotValidate<TResult>(this IRequest<TResult> message, params string[] expectedErrors)
            => Validation(message).ShouldBeFailure(expectedErrors);
        
    }
}