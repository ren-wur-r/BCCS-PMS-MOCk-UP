using System;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLibrary.Core.Atom.Extension;

/// <summary>核心-原子-服務集-Extensions</summary>
public static class CoAtomServiceCollectionExtensions
{
    /// <summary>加入-核心-原子</summary>
    public static IServiceCollection AddCoreAtom(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // common library

        // db

        // mongo

        // redis

        // memory

        // logical

        // initial

        return services;
    }

}
