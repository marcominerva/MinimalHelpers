using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MinimalHelpers.Routing.Analyzers;

[Generator]
public class EndpointRouteHandlerGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register the interface during post-initialization.
        context.RegisterPostInitializationOutput(static context =>
        {
            var @interface = GetIEndpointRouteHandlerBuilderInterface();
            context.AddSource("IEndpointRouteHandlerBuilder.g.cs", SourceText.From(@interface, Encoding.UTF8));
        });

        // Find classes that implement IEndpointRouteHandlerBuilder in the current compilation.
        var currentCompilationEndpointClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax { BaseList: not null },
                transform: static (context, token) =>
                {
                    if (!context.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(LanguageVersion.CSharp11))
                    {
                        return null;
                    }

                    token.ThrowIfCancellationRequested();

                    var classDeclaration = (ClassDeclarationSyntax)context.Node;
                    var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration, token);

                    if (classSymbol is null)
                    {
                        return null;
                    }

                    // Check if it implements IEndpointRouteHandlerBuilder using semantic model.
                    var endpointRouteHandlerBuilderSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Routing.IEndpointRouteHandlerBuilder");
                    if (endpointRouteHandlerBuilderSymbol is null || !classSymbol.AllInterfaces.Contains(endpointRouteHandlerBuilderSymbol, SymbolEqualityComparer.Default))
                    {
                        return null;
                    }

                    return classSymbol;
                })
            .Where(static symbol => symbol is not null)
            .Collect();

        // Find classes from referenced assemblies via duck-typing: a public, non-abstract, non-generic
        // class with a public static void MapEndpoints(IEndpointRouteBuilder) method.
        // Duck-typing is necessary because the generated IEndpointRouteHandlerBuilder interface is
        // internal and therefore not visible across assembly boundaries.
        var externalEndpointClasses = context.CompilationProvider
            .SelectMany(static (compilation, token) =>
            {
                var results = ImmutableArray.CreateBuilder<INamedTypeSymbol>();

                foreach (var reference in compilation.References)
                {
                    token.ThrowIfCancellationRequested();

                    if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assemblySymbol)
                    {
                        CollectEndpointTypesFromNamespace(assemblySymbol.GlobalNamespace, results, token);
                    }
                }

                return results.ToImmutable();
            })
            .Collect();

        var allEndpointClasses = currentCompilationEndpointClasses.Combine(externalEndpointClasses);

        context.RegisterSourceOutput(allEndpointClasses, Execute);
    }

    private static void CollectEndpointTypesFromNamespace(INamespaceSymbol namespaceSymbol, ImmutableArray<INamedTypeSymbol>.Builder results, CancellationToken token)
    {
        foreach (var type in namespaceSymbol.GetTypeMembers())
        {
            token.ThrowIfCancellationRequested();

            if (type.TypeKind == TypeKind.Class && !type.IsAbstract && !type.IsGenericType
                && type.DeclaredAccessibility == Accessibility.Public
                && HasMapEndpointsMethod(type))
            {
                results.Add(type);
            }
        }

        foreach (var nestedNamespace in namespaceSymbol.GetNamespaceMembers())
        {
            CollectEndpointTypesFromNamespace(nestedNamespace, results, token);
        }
    }

    private static bool HasMapEndpointsMethod(INamedTypeSymbol typeSymbol)
        => typeSymbol.GetMembers("MapEndpoints")
            .OfType<IMethodSymbol>()
            .Any(static m =>
                m.IsStatic &&
                m.DeclaredAccessibility == Accessibility.Public &&
                m.ReturnsVoid &&
                m.Parameters.Length == 1 &&
                m.Parameters[0].Type.Name == "IEndpointRouteBuilder" &&
                m.Parameters[0].Type.ContainingNamespace?.ToDisplayString() == "Microsoft.AspNetCore.Routing");

    private static void Execute(SourceProductionContext context, (ImmutableArray<INamedTypeSymbol?> CurrentCompilation, ImmutableArray<INamedTypeSymbol> External) input)
    {
        //#if DEBUG
        //        if (!Debugger.IsAttached)
        //        {
        //            Debugger.Launch();
        //        }
        //#endif

        var validClasses = input.CurrentCompilation
            .Where(static symbol => symbol is not null)
            .Cast<INamedTypeSymbol>()
            .Concat(input.External)
            .ToArray();

        //if (validClasses.Length == 0)
        //{
        //    return;
        //}

        var prefixCode = """
            // <auto-generated />
            namespace Microsoft.AspNetCore.Routing;

            #nullable enable annotations
            #nullable disable warnings

            /// <summary>
            /// Provides extension methods for <see cref="IEndpointRouteBuilder" /> to add route handlers.
            /// </summary>
            internal static class EndpointRouteBuilderExtensions
            {
                /// <summary>
                /// Automatically registers all the route endpoints defined in classes that implement the <see cref="IEndpointRouteHandlerBuilder "/> interface.
                /// </summary>
                /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
                public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
                {            
            """;

        var suffixCode = """

                    return endpoints;
                }
            }
            """;

        var codeBuilder = new StringBuilder();
        codeBuilder.AppendLine(prefixCode);

        foreach (var classSymbol in validClasses)
        {
            var fullClassName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            codeBuilder.AppendLine($"        {fullClassName}.MapEndpoints(endpoints);");
        }

        codeBuilder.AppendLine(suffixCode);

        context.AddSource("EndpointRouteBuilderExtensions.g.cs", SourceText.From(codeBuilder.ToString(), Encoding.UTF8));
    }

    private static string GetIEndpointRouteHandlerBuilderInterface()
        => """
            // <auto-generated />
            namespace Microsoft.AspNetCore.Routing;

            #nullable enable annotations
            #nullable disable warnings                

            /// <summary>
            /// Defines a contract for a class that holds one or more route handlers that must be registered by the application.
            /// </summary>
            internal interface IEndpointRouteHandlerBuilder
            {
                /// <summary>
                /// Maps route endpoints to the corresponding handlers.
                /// </summary>
                static abstract void MapEndpoints(IEndpointRouteBuilder endpoints);
            }
            """;
}