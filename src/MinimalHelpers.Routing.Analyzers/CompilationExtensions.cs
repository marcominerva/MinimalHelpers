using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MinimalHelpers.Routing.Analyzers;

internal static class CompilationExtensions
{
    /// <summary>
    /// Checks whether a given compilation (assumed to be for C#) is using at least a given language version.
    /// </summary>
    /// <param name="compilation">The <see cref="Compilation"/> to consider for analysis.</param>
    /// <param name="languageVersion">The minimum language version to check.</param>
    /// <returns>Whether <paramref name="compilation"/> is using at least the specified language version.</returns>
    public static bool HasLanguageVersionAtLeastEqualTo(this Compilation compilation, LanguageVersion languageVersion)
        => ((CSharpCompilation)compilation).LanguageVersion >= languageVersion;
}
