﻿// ReSharper disable All

using System;
using Xunit;
using static System.DateTimeOffset;
using static Tiger.Clock.Tests.DateTimeOffsetComparer;

namespace Tiger.Clock.Tests
{
    /// <summary>Tests related to <see cref="StandardClock"/>.</summary>
    public class ClockTests
    {
        public static readonly TheoryData<TimeSpan> Tolerances = new TheoryData<TimeSpan>
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromMilliseconds(250),
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(50),
            TimeSpan.FromMilliseconds(10),
            TimeSpan.FromMilliseconds(5)
        };

        [Theory(DisplayName = "Now returns the current time in the current offset.")]
        [MemberData(nameof(Tolerances))]
        public void Clock_Now(TimeSpan tolerance)
        {
            // arrange 
            var sut = new StandardClock();

            // act
            var expected = Now;
            var actual = sut.Now;

            // assert
            Assert.Equal(expected, actual, Within(tolerance));
        }

        [Theory(DisplayName = "UtcNow returns the current time in the universal offset.")]
        [MemberData(nameof(Tolerances))]
        public void Clock_UtcNow(TimeSpan tolerance)
        {
            // arrange 
            var sut = new StandardClock();

            // act
            var expected = UtcNow;
            var actual = sut.UtcNow;

            // assert
            Assert.Equal(expected, actual, Within(tolerance));
        }
    }
}