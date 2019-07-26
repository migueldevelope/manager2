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
    public Tuple<double[][], string[], long> ImportSalaryScale(Stream stream)
    {
      //string fullPath = @"c:/jms/SALARYSCALE4.xlsx";
      //var stream = new FileStream(fullPath, FileMode.Open);

      try
      {
        ISheet sheet;

        stream.Position = 0;
        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

        long count = CountLines(sheet);
        string[] grades = new string[count];
        // col i = 9 lin = 50
        double[][] matriz = new double[count][];
        for (int i = 0; i < count; i++)
          matriz[i] = new double[9];

        IRow headerRow = sheet.GetRow(0); //Get Header Row
        int cellCount = headerRow.LastCellNum;

        for (int i = 2; i < (count + 2); i++) //Read Excel File
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
              else if (value.Trim().Length > 0)
                throw new Exception("not_numeric");
            }
          }
        }
        return new Tuple<double[][], string[], long>(matriz, grades, count);
      }
      catch(Exception e)
      {
        throw e;
      }

      
    }


    public long CountLines(ISheet sheet)
    {
      long count = 0;
      byte blankline = 0;

      IRow headerRow = sheet.GetRow(0); //Get Header Row

      for (int i = 2; i <= sheet.LastRowNum; i++) //Read Excel File
      {
        IRow row = sheet.GetRow(i);
        if (row == null) continue;
        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
        blankline = 0;
        for (int j = 2; j < 10; j++)
        {
          if (row.GetCell(j) != null)
          {
            var value = row.GetCell(j).ToString();
            if (value.Trim().Length == 0)
              blankline += 1;
          }
        }
        if (blankline == 8)
          break;
        else
          count += 1;
      }
      return count;
    }

  }
}