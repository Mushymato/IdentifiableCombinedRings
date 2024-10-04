using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StardewModdingAPI;

namespace IdentifiableCombinedRings.AtraStuff;

/// <summary>Extensions from AtraShared and AtraBase</summary>
public static class Extensions
{
    // AtraShared/Utils/Extensions/SMAPIHelperExtensions.cs
    /// <summary>
    /// Writes the config async.
    /// </summary>
    /// <typeparam name="TConfig">Type of the config model.</typeparam>
    /// <param name="helper">SMAPI helper.</param>
    /// <param name="monitor">SMAPI logger.</param>
    /// <param name="config">Config class.</param>
    public static void AsyncWriteConfig<TConfig>(this IModHelper helper, IMonitor monitor, TConfig config)
        where TConfig : class, new()
    {
        Task.Run(() => helper.WriteConfig(config))
            .ContinueWith(t =>
            {
                switch (t.Status)
                {
                    case TaskStatus.RanToCompletion:
                        monitor.Log("Configuration written successfully!");
                        break;
                    case TaskStatus.Faulted:
                        monitor.LogError("writing config file", t.Exception!);
                        break;
                }
            });
    }

    // AtraShared/Utils/Extensions/LogExtensions.cs
    /// <summary>
    /// Logs an exception.
    /// </summary>
    /// <param name="monitor">Logging instance to use.</param>
    /// <param name="action">The current actions being taken when the exception happened.</param>
    /// <param name="ex">The exception.</param>
    [DebuggerHidden]
    internal static void LogError(this IMonitor monitor, string action, Exception ex)
    {
        monitor.Log($"Mod failed while {action}, see log for details.", LogLevel.Error);
        monitor.Log(ex.ToString());
    }

    // AtraBase/Toolkit/Extensions/StringExtensions.cs
    /// <summary>
    /// Gets the index of the next whitespace character.
    /// </summary>
    /// <param name="chars">ReadOnlySpan to search in.</param>
    /// <returns>Index of the whitespace character, or -1 if not found.</returns>
    [Pure]
    [MethodImpl(TKConstants.Hot)]
    public static int GetIndexOfWhiteSpace(this ReadOnlySpan<char> chars)
    {
        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsWhiteSpace(chars[i]))
            {
                return i;
            }
        }
        return -1;
    }

    [Pure]
    [MethodImpl(TKConstants.Hot)]
    public static bool TrySplitOnce(this string str, char? deliminator, out ReadOnlySpan<char> first, out ReadOnlySpan<char> second)
    {
        first = default;
        second = default;
        if (str != null)
            return str.AsSpan().TrySplitOnce(deliminator, out first, out second);
        return false;
    }

    [Pure]
    [MethodImpl(TKConstants.Hot)]
    public static bool TrySplitOnce(this ReadOnlySpan<char> str, char? deliminator, out ReadOnlySpan<char> first, out ReadOnlySpan<char> second)
    {
        int idx = deliminator is null ? str.GetIndexOfWhiteSpace() : str.IndexOf(deliminator.Value);

        if (idx < 0)
        {
            first = second = ReadOnlySpan<char>.Empty;
            return false;
        }

        first = str[..idx];
        second = str[(idx + 1)..];
        return true;
    }
}
