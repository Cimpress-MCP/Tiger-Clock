using System;
using JetBrains.Annotations;

namespace Tiger.Clock
{
    /// <summary>Retrieves system time.</summary>
    [PublicAPI]
    public interface IClock
    {
        /// <summary>Gets current system time in the local time offset.</summary>
        DateTimeOffset Now { get; }

        /// <summary>Gets the current system time in UTC.</summary>
        DateTimeOffset UtcNow { get; }
    }
}
