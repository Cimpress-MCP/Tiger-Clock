// <copyright file="StandardClockTests.cs" company="Cimpress, Inc.">
//   Copyright 2020 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License") –
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>

using System;
using Xunit;
using static System.DateTimeOffset;
using static System.TimeSpan;
using static Tiger.Clock.Tests.DateTimeOffsetComparer;

namespace Tiger.Clock.Tests
{
    /// <summary>Tests related to <see cref="StandardClock"/>.</summary>
    public static class StandardClockTests
    {
        public static readonly TheoryData<TimeSpan> Tolerances = new()
        {
            FromSeconds(1),
            FromMilliseconds(500),
            FromMilliseconds(250),
            FromMilliseconds(100),
            FromMilliseconds(50),
            FromMilliseconds(10),
            FromMilliseconds(5)
        };

        [Theory(DisplayName = "Now returns the current time in the current offset.")]
        [MemberData(nameof(Tolerances))]
        public static void Clock_Now(TimeSpan tolerance)
        {
            var sut = new StandardClock();

            Assert.Equal(Now, sut.Now, Within(tolerance));
        }

        [Theory(DisplayName = "UtcNow returns the current time in the universal offset.")]
        [MemberData(nameof(Tolerances))]
        public static void Clock_UtcNow(TimeSpan tolerance)
        {
            var sut = new StandardClock();

            Assert.Equal(UtcNow, sut.UtcNow, Within(tolerance));
        }
    }
}
