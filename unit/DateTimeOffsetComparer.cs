// <copyright file="DateTimeOffsetComparer.cs" company="Cimpress, Inc.">
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
using System.Collections.Generic;
using static System.Math;

namespace Tiger.Clock.Tests
{
    /// <summary>
    /// Represents a <see cref="DateTimeOffset"/> comparison operation that
    /// uses specific tolerance rules.
    /// </summary>
    public abstract class DateTimeOffsetComparer
        : IComparer<DateTimeOffset>, IEqualityComparer<DateTimeOffset>
    {
        /// <summary>
        /// A <see cref="DateTimeOffsetComparer"/> object which performs an exact comparison.
        /// </summary>
        public static readonly DateTimeOffsetComparer Exact = new ExactComparer();

        /// <summary>
        /// Creates a <see cref="DateTimeOffsetComparer"/> object that performs a comparison
        /// with the specified tolerance.
        /// </summary>
        /// <param name="tolerance">
        /// The duration within which two <see cref="DateTimeOffset"/> values must be
        /// in order to be considered equal.
        /// </param>
        /// <returns>A <see cref="DateTimeOffsetComparer"/> object.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> cannot be used as a duration.</exception>
        public static DateTimeOffsetComparer Within(TimeSpan tolerance) => new WithinComparer(tolerance);

        /// <inheritdoc/>
        public abstract int Compare(DateTimeOffset x, DateTimeOffset y);

        /// <inheritdoc/>
        public abstract bool Equals(DateTimeOffset x, DateTimeOffset y);

        /// <inheritdoc/>
        public abstract int GetHashCode(DateTimeOffset obj);

        /// <summary>
        /// Represents a <see cref="DateTimeOffset"/> comparison operation that
        /// specifies lack of tolerance in comparison.
        /// </summary>
        sealed class ExactComparer
            : DateTimeOffsetComparer
        {
            /// <inheritdoc/>
            public override int Compare(DateTimeOffset x, DateTimeOffset y) => x.CompareTo(y);

            /// <inheritdoc/>
            public override bool Equals(DateTimeOffset x, DateTimeOffset y) => x.Equals(y);

            /// <inheritdoc/>
            public override int GetHashCode(DateTimeOffset obj) => obj.GetHashCode();
        }

        /// <summary>
        /// Represents a <see cref="DateTimeOffset"/> comparison operation that
        /// uses a specified tolerance value.
        /// </summary>
        sealed class WithinComparer
            : DateTimeOffsetComparer
        {
            readonly TimeSpan _tolerance;

            /// <summary>
            /// Initializes a new instance of the <see cref="WithinComparer"/> class.
            /// </summary>
            /// <param name="tolerance">
            /// The duration within which two <see cref="DateTimeOffset"/> values must be
            /// in order to be considered equal.
            /// </param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> cannot be used as a duration.</exception>
            public WithinComparer(TimeSpan tolerance)
            {
                if (tolerance == TimeSpan.MinValue)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(tolerance),
                        TimeSpan.MinValue,
                        "This value cannot be used as a duration.");
                }

                _tolerance = tolerance.Duration();
            }

            /// <inheritdoc/>
            public override int Compare(DateTimeOffset x, DateTimeOffset y)
            {
                var difference = x - y;
                if (difference == TimeSpan.MinValue)
                {
                    return -1;
                }

                if (difference.Duration() <= _tolerance)
                {
                    // note(cosborn) Being within tolerance is equality.
                    return 0;
                }

                return Sign(difference.Ticks);
            }

            /// <inheritdoc/>
            public override bool Equals(DateTimeOffset x, DateTimeOffset y)
            {
                var difference = x - y;
                if (difference == TimeSpan.MinValue)
                {
                    return false;
                }
                // note(cosborn) Being within tolerance is equality.
                return difference.Duration() <= _tolerance;
            }

            /// <inheritdoc/>
            public override int GetHashCode(DateTimeOffset obj) => obj.GetHashCode();
        }
    }
}
