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
    public Tuple<double[][], string[]> ImportSalaryScale(Stream stream)
    {
      //string fullPath = @"c:/jms/SALARYSCALE4.xlsx";
      //var stream = new FileStream(fullPath, FileMode.Open);

      string[] grades = new string[50];
      // col i = 9 lin = 50
      double[][] matriz = new double[50][];
      for (int i = 0; i < 50; i++)
        matriz[i] = new double[9];

      ISheet sheet;

      stream.Position = 0;
      XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
      sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

      IRow headerRow = sheet.GetRow(0); //Get Header Row
      int cellCount = headerRow.LastCellNum;

      for (int i = 2; i <= sheet.LastRowNum; i++) //Read Excel File
      {
        IRow row = sheet.GetRow(i);
        if (row == null) continue;
        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

        grades[i - 2] = row.GetCell(0).ToString();
        for (int j = 2; j < cellCount; j++)
        {
          if (row.GetCell(j) != null)
          {
            var value = row.GetCell(j).ToString();
            double num; bool isNum = double.TryParse(value, out num);
            if (isNum)
              matriz[i - 2][j - 2] = double.Parse(value);
            else if(value.Trim().Length > 0)
                throw new Exception("not_numeric");
          }
        }
      }
      return new Tuple<double[][], string[]>(matriz, grades);
    }

  }
}