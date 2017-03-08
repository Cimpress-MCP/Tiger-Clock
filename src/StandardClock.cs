using System;

namespace Tiger.Clock
{
    /// <summary>Forwards system time from standard means.</summary>
    public sealed class StandardClock
        : IClock
    {
        /// <inheritdoc/>
        public DateTimeOffset Now => DateTimeOffset.Now;

        /// <inheritdoc/>
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
