using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
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
