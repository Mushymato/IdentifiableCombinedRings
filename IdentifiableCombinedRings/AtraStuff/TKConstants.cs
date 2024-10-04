using System.Runtime.CompilerServices;
// AtraBase/Toolkit/TKConstants.cs

namespace IdentifiableCombinedRings.AtraStuff;

/// <summary>
/// A class that contains useful constants.
/// </summary>
public class TKConstants
{
    /// <summary>
    /// For use when asking the compiler to both inline and aggressively optimize.
    /// </summary>
    public const MethodImplOptions Hot = MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

    /// <summary>
    /// For use when asking the compiler to forgo optimization as much as possible.
    /// </summary>
    public const MethodImplOptions Cold = MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization;
}