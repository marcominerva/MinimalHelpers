﻿using System.Collections.Immutable;
using System.Text;
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
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax classDeclaration && classDeclaration.HasOrPotentiallyHasBaseTypes(),
                static (context, token) =>
                {
                    if (!context.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(LanguageVersion.CSharp11))
                    {
                        return default;
                    }

                    token.ThrowIfCancellationRequested();

                    return (ClassDeclarationSyntax)context.Node;
                }
            ).Where(static c => c is not null);

        var compilation = context.CompilationProvider.Combine(provider.Collect());
        context.RegisterSourceOutput(compilation, Execute!);
    }

    private void Execute(SourceProductionContext context, (Compilation Compilation, ImmutableArray<ClassDeclarationSyntax> Classes) tuple)
    {
        //#if DEBUG
        //        if (!Debugger.IsAttached)
        //        {
        //            Debugger.Launch();
        //        }
        //#endif

        var (compilation, classes) = tuple;

        var @interface = GetIEndpointRouteHandlerBuilderInterface();
        context.AddSource("IEndpointRouteHandlerBuilder.g.cs", SourceText.From(@interface, Encoding.UTF8));

        var prefixCode = """
            // <auto-generated />
            namespace Microsoft.AspNetCore.Routing;

            #nullable enable annotations
            #nullable disable warnings

            /// <summary>
            /// Provides extension methods for <see cref="IEndpointRouteBuilder" /> to add route handlers.
            /// </summary>
            public static class EndpointRouteBuilderExtensions
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

        foreach (var @class in classes.Where(c => c.BaseList?.Types.Any(t => t.Type.ToString() == "IEndpointRouteHandlerBuilder") is true))
        {
            var @namespace = GetNamespace(@class);
            var fullClassName = $"{@namespace}.{@class.Identifier.Text}".TrimStart('.');

            codeBuilder.AppendLine($"        global::{fullClassName}.MapEndpoints(endpoints);");
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
            public interface IEndpointRouteHandlerBuilder
            {
                /// <summary>
                /// Maps route endpoints to the corresponding handlers.
                /// </summary>
                static abstract void MapEndpoints(IEndpointRouteBuilder endpoints);
            }
            """;

    // determine the namespace the class/enum/struct is declared in, if any
    // https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
    private static string GetNamespace(BaseTypeDeclarationSyntax syntax)
    {
        var @namespace = string.Empty;
        var potentialNamespaceParent = syntax.Parent;

        while (potentialNamespaceParent is not null and not NamespaceDeclarationSyntax and not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            @namespace = namespaceParent.Name.ToString();

            while (namespaceParent.Parent is NamespaceDeclarationSyntax parent)
            {
                @namespace = $"{namespaceParent.Name}.{@namespace}";
                namespaceParent = parent;
            }
        }

        return @namespace;
    }
}
