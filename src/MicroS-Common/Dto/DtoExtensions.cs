using Microsoft.AspNetCore.Builder;
using System;
using System.Reflection;
namespace MicroS_Common
{
    public static class DtoExtensions
    {
        public static void UseDto(this IApplicationBuilder app, Type type)
        {

        }

        public static void UseDto(this IApplicationBuilder app, Assembly[] assemblies)
        {

        }


    }
}
