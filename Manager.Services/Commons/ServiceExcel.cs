using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Manager.Services.Commons
{
  public class ServiceExcel
  {
    public string ImportSalaryScale()
    {
      //IFormFile file = Request.Form.Files[0];
      /*string folderName = "Upload";
      string webRootPath = _hostingEnvironment.WebRootPath;
      string newPath = Path.Combine(webRootPath, folderName);*/

      string fullPath = @"Models/SALARYSCALE.xlsx";

      StringBuilder sb = new StringBuilder();
      ISheet sheet;
      using (var stream = new FileStream(fullPath, FileMode.Open))
      {
        stream.Position = 0;
        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

        IRow headerRow = sheet.GetRow(0); //Get Header Row
        int cellCount = headerRow.LastCellNum;
        sb.Append("<table class='table'><tr>");
        for (int j = 0; j < cellCount; j++)
        {
          ICell cell = headerRow.GetCell(j);
          if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
          sb.Append("<th>" + cell.ToString() + "</th>");
        }
        sb.Append("</tr>");
        sb.AppendLine("<tr>");
        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
        {
          IRow row = sheet.GetRow(i);
          if (row == null) continue;
          if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
          for (int j = row.FirstCellNum; j < cellCount; j++)
          {
            if (row.GetCell(j) != null)
              sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
          }
          sb.AppendLine("</tr>");
        }
        sb.Append("</table>");

      }
      return sb.ToString();
    }
  }
}
