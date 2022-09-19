using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile File)
        {
            Console.WriteLine($"called");
            Stream fs = File.OpenReadStream();
            if (!checkXlsxMimeType(File))
                return BadRequest("Invalid File Type");
            XSSFWorkbook wb = new XSSFWorkbook(fs);
            await GetDataAsync(wb);
            return Ok();
        }

        public Task GetDataAsync(XSSFWorkbook wb)
        {
            return Task.Run(() =>
            {
                ISheet sheet = wb.GetSheetAt(0);
                int row = 1;
                List<object> list = new List<object>();
                List<string> errors = new List<string>();
                while (true)
                {
                    try
                    {
                        var record = sheet.GetRow(row);
                        if (record == null) break;
                        var name = record.GetCell(0).StringCellValue;
                        var available = record.GetCell(1).BooleanCellValue;
                        list.Add(new { Name = name, Available = available });
                        Console.WriteLine($"{name} - {available}");
                    }
                    catch (Exception)
                    {
                        errors.Add($"Error at line {row}");
                    }
                    row++;
                }
                foreach (var err in errors)
                {
                    Console.WriteLine(err);
                }
            });
        }

        private bool checkXlsxMimeType(IFormFile file)
        {
            string filename = file.FileName;
            string[] temp = filename.Split(".");
            string mimeType = temp[temp.Length - 1];
            return (
                mimeType.Equals("xlsx", StringComparison.OrdinalIgnoreCase)
                || mimeType.Equals("xls", StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
