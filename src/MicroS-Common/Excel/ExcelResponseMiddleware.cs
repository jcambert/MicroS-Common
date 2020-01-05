using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MicroS_Common.Excel
{
    public sealed class ExcelResponseMiddleware
    {
        class UserInfo
        {
            public string UserName { get; set; }
            public int Age { get; set; }
        }
        private string[] accepted = new string[] { "xls", "xlsx", "excel" };
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcelResponseMiddleware> _logger;

        public ExcelResponseMiddleware(RequestDelegate next,
            ILogger<ExcelResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task Invoke(HttpContext context)
        {

            await _next(context);
            if (context.Request.Query.ContainsKey("response".ToLower()) && Array.IndexOf(accepted, context.Request.Query["response"].ToString().ToLower()) > -1)
            {

                _logger.LogInformation("Transform response to Excel");

                var list = new List<UserInfo>()
                    {
                        new UserInfo { UserName = "catcher", Age = 18 },
                        new UserInfo { UserName = "james", Age = 20 },
                    };
                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(list, true);
                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                //context.Response.ContentType = "application/octet-stream";
                context.Response.Body = stream;
                //return File(stream, "application/octet-stream", excelName);  
                //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
        }
    }
}
