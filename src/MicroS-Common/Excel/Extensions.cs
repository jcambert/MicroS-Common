using Microsoft.AspNetCore.Builder;

namespace MicroS_Common.Excel
{
    public static class Extensions
    {
        public static IApplicationBuilder UseExcelResponseMiddleware(this IApplicationBuilder builder)
           => builder.UseMiddleware<ExcelResponseMiddleware>();
    }
}
