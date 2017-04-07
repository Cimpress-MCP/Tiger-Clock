using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using static System.Math;
// ReSharper disable CheckNamespace

namespace Tiger.Clock.Tests
{
    /// <summary>
    /// Represents a <see cref="DateTimeOffset"/> comparison operation that
    /// uses specific tolerance rules.
    /// </summary>
    [PublicAPI]
    public abstract class DateTimeOffsetComparer
        : IComparer<DateTimeOffset>, IEqualityComparer<DateTimeOffset>
    {
        /// <summary>
        /// Gets a <see cref="DateTimeOffsetComparer"/> object that performs an exact comparison.
        /// </summary>
        public static DateTimeOffsetComparer Exact { get; } = new ExactComparer();

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
        [NotNull]
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
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Unusable durations checked for in advance of use.")]
        sealed class WithinComparer
            : DateTimeOffsetComparer
        {
            readonly TimeSpan _tolerance;

            /// <summary>
            /// Initializes a new instance of the <see cref="DateTimeOffsetComparer.WithinComparer"/> class.
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
                        @"This value cannot be used as a duration.");
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
                return difference.Duration() <= _tolerance
                    ? 0 // note(cosborn) Being within tolerance is equality.
                    : Sign(difference.Ticks);
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