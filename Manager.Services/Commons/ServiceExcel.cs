using Manager.Views.BusinessView;
using MongoDB.Bson;
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

    #region SalaryScale

    public Tuple<double[][], string[], long, int[]> ImportSalaryScale(Stream stream)
    {
      //string fullPath = @"c:/jms/SALARYSCALE4.xlsx";
      //var stream = new FileStream(fullPath, FileMode.Open);
      try
      {
        ISheet sheet;

        stream.Position = 0;
        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   


        IRow rowpass = sheet.GetRow(3);
        var pass = rowpass.GetCell(12).ToString();
        if (pass != "sheetimport")
          throw new Exception("not_sheet");


        long count = CountLines(sheet);
        string[] grades = new string[count];
        int[] workloads = new int[count];

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
          var workload = row.GetCell(1).ToString();
          int numworkload; bool isNumworkload = int.TryParse(workload, out numworkload);
          if (isNumworkload)
            workloads[i - 2] = int.Parse(workload);
          else
            throw new Exception("not_numeric_workload");

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
        return new Tuple<double[][], string[], long, int[]>(matriz, grades, count, workloads);
      }
      catch (Exception e)
      {
        throw e;
      }


    }
    //public string ExportSalaryScale(Tuple<double[][], string[], string[], string[], string[], string[], int[], long> tuple)
    public string ExportSalaryScale(Tuple<double[][], string[], string[], string[], string[], int[], long> tuple, string[] descriptionnameT, dynamic header)
    {
      try
      {
        ISheet sheet;
        var excel = tuple.Item1;

        //XSSFWorkbook hssfwb = new XSSFWorkbook(excel); //This will read 2007 Excel format  
        XSSFWorkbook hssfwb = new XSSFWorkbook(); //This will read 2007 Excel format  

        sheet = hssfwb.CreateSheet("salaryscale"); //get first sheet from workbook   

        long count = tuple.Item7;
        string[] occupations = tuple.Item2;
        string[] descritionoccupations = descriptionnameT;
        string[] grades = tuple.Item3;
        string[] groups = tuple.Item4;
        string[] spheres = tuple.Item5;
        int[] workloads = tuple.Item6;

        var font = hssfwb.CreateFont();
        font.Boldweight = (short)FontBoldWeight.Bold;

        // col i = 9 lin = 50
        double[][] matriz = tuple.Item1;
        IRow headerRowCompany = sheet.CreateRow(0);
        IRow headerRowName = sheet.CreateRow(1);
        IRow headerRowVersion = sheet.CreateRow(2);
        IRow headerRowDate = sheet.CreateRow(3);
        IRow headerspace = sheet.CreateRow(4);

        headerRowCompany.CreateCell(0).SetCellValue("EMPRESA");
        headerRowCompany.CreateCell(1).SetCellValue(header.Company);

        headerRowName.CreateCell(0).SetCellValue("TABELA SALARIAL");
        headerRowName.CreateCell(1).SetCellValue(header.Name);

        headerRowVersion.CreateCell(0).SetCellValue("VERSÃO");
        headerRowVersion.CreateCell(1).SetCellValue(header.Version);

        headerRowDate.CreateCell(0).SetCellValue("DATA EMISSÃO");
        headerRowDate.CreateCell(1).SetCellValue(header.Date);

        headerRowCompany.GetCell(0).CellStyle = hssfwb.CreateCellStyle();
        headerRowCompany.GetCell(0).CellStyle.SetFont(font);
        headerRowCompany.GetCell(1).CellStyle = hssfwb.CreateCellStyle();
        headerRowCompany.GetCell(1).CellStyle.SetFont(font);
        headerRowName.GetCell(0).CellStyle = hssfwb.CreateCellStyle();
        headerRowName.GetCell(0).CellStyle.SetFont(font);
        headerRowName.GetCell(1).CellStyle = hssfwb.CreateCellStyle();
        headerRowName.GetCell(1).CellStyle.SetFont(font);
        headerRowVersion.GetCell(0).CellStyle = hssfwb.CreateCellStyle();
        headerRowVersion.GetCell(0).CellStyle.SetFont(font);
        headerRowVersion.GetCell(1).CellStyle = hssfwb.CreateCellStyle();
        headerRowVersion.GetCell(1).CellStyle.SetFont(font);
        headerRowDate.GetCell(0).CellStyle = hssfwb.CreateCellStyle();
        headerRowDate.GetCell(0).CellStyle.SetFont(font);
        headerRowDate.GetCell(1).CellStyle = hssfwb.CreateCellStyle();
        headerRowDate.GetCell(1).CellStyle.SetFont(font);

        IRow headerRow = sheet.CreateRow(5); //Get Header Row
        headerRow.CreateCell(0).SetCellValue("CARGO");
        headerRow.CreateCell(1).SetCellValue("FUNÇÃO");
        headerRow.CreateCell(2).SetCellValue("GRUPO");
        headerRow.CreateCell(3).SetCellValue("ESFERA");
        headerRow.CreateCell(4).SetCellValue("CARGA HORÁRIA");
        headerRow.CreateCell(5).SetCellValue("GRADE");
        headerRow.CreateCell(6).SetCellValue("A");
        headerRow.CreateCell(7).SetCellValue("B");
        headerRow.CreateCell(8).SetCellValue("C");
        headerRow.CreateCell(9).SetCellValue("D");
        headerRow.CreateCell(10).SetCellValue("E");
        headerRow.CreateCell(11).SetCellValue("F");
        headerRow.CreateCell(12).SetCellValue("G");
        headerRow.CreateCell(13).SetCellValue("H");
        //headerRow.CreateCell(14).SetCellValue("I");

        for (var hd = 0; hd <= 13; hd++)
        {
          headerRow.GetCell(hd).CellStyle = hssfwb.CreateCellStyle();
          headerRow.GetCell(hd).CellStyle.SetFont(font);
        }


        long cellCount = count;

        for (int i = 6; i < count; i++) //Read Excel File
        {
          IRow row = sheet.CreateRow(i);

          row.CreateCell(0).SetCellValue(occupations[i - 5]);
          row.CreateCell(1).SetCellValue(descritionoccupations[i - 5]);
          row.CreateCell(2).SetCellValue(groups[i - 5]);
          row.CreateCell(3).SetCellValue(spheres[i - 5]);
          row.CreateCell(4).SetCellValue(workloads[i - 5]);
          row.CreateCell(5).SetCellValue(grades[i - 5]);

          for (int j = 6; j < 14; j++)
          {
            row.CreateCell(j).SetCellValue(matriz[i - 5][j - 6]);
          }
        }
        //FileStream sw = File.Create(ObjectId.GenerateNewId() + DateTime.Now.ToShortDateString().Replace("/", "") + ".xlsx");
        FileStream sw = File.Create(ObjectId.GenerateNewId() + DateTime.Now.ToShortDateString().Replace("/", "") + ".xlsx");

        hssfwb.Write(sw);
        //sw.Flush();
        //sw.Close();

        //getexcel.Close();

        return sw.Name;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ExportUpdateSalaryScale(Tuple<double[][], string[], int[], long> tuple)
    {
      try
      {
        ISheet sheet;
        var excel = tuple.Item1;

        //XSSFWorkbook hssfwb = new XSSFWorkbook(excel); //This will read 2007 Excel format  
        XSSFWorkbook hssfwb = new XSSFWorkbook(); //This will read 2007 Excel format  

        sheet = hssfwb.CreateSheet("salaryscale"); //get first sheet from workbook   

        long count = tuple.Item4 + 1;
        string[] grades = tuple.Item2;
        int[] workloads = tuple.Item3;

        // col i = 9 lin = 50
        double[][] matriz = tuple.Item1;


        IRow headerRow = sheet.CreateRow(0); //Get Header Row
        headerRow.CreateCell(0).SetCellValue("GRADE");
        headerRow.CreateCell(1).SetCellValue("CARGA HORÁRIA");
        headerRow.CreateCell(2).SetCellValue("");
        headerRow.CreateCell(3).SetCellValue("");
        headerRow.CreateCell(4).SetCellValue("");
        headerRow.CreateCell(5).SetCellValue("STEP");
        headerRow.CreateCell(6).SetCellValue("");
        headerRow.CreateCell(7).SetCellValue("");
        headerRow.CreateCell(8).SetCellValue("");
        headerRow.CreateCell(9).SetCellValue("");
        IRow stepRow = sheet.CreateRow(1); //Get Header Row
        stepRow.CreateCell(0).SetCellValue("");
        stepRow.CreateCell(1).SetCellValue("");
        stepRow.CreateCell(2).SetCellValue("A");
        stepRow.CreateCell(3).SetCellValue("B");
        stepRow.CreateCell(4).SetCellValue("C");
        stepRow.CreateCell(5).SetCellValue("D");
        stepRow.CreateCell(6).SetCellValue("E");
        stepRow.CreateCell(7).SetCellValue("F");
        stepRow.CreateCell(8).SetCellValue("G");
        stepRow.CreateCell(9).SetCellValue("H");
        //headerRow.CreateCell(10).SetCellValue("I");



        ICellStyle style = hssfwb.CreateCellStyle();
        style.BorderBottom = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;
        style.BorderTop = BorderStyle.Thin;
        style.WrapText = true;
        style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        style.FillPattern = FillPattern.SolidForeground;

        ICellStyle styleValues = hssfwb.CreateCellStyle();
        styleValues.BorderBottom = BorderStyle.Thin;
        styleValues.BorderLeft = BorderStyle.Thin;
        styleValues.BorderRight = BorderStyle.Thin;
        styleValues.BorderTop = BorderStyle.Thin;
        style.WrapText = true;
        styleValues.FillForegroundColor = IndexedColors.White.Index;
        styleValues.FillPattern = FillPattern.SolidForeground;

        for (var hd = 0; hd <= 9; hd++)
        {
          headerRow.GetCell(hd).CellStyle = style;
          stepRow.GetCell(hd).CellStyle = style;
        }


        ICellStyle styleTitle = hssfwb.CreateCellStyle();
        styleTitle.BorderBottom = BorderStyle.None;
        styleTitle.BorderLeft = BorderStyle.None;
        styleTitle.BorderTop = BorderStyle.Thin;
        styleTitle.WrapText = true;
        styleTitle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        styleTitle.FillPattern = FillPattern.SolidForeground;
        for (int j = 2; j < 10; j++)
        {
          headerRow.GetCell(j).CellStyle = styleTitle;
        }
        ICellStyle styleHead = hssfwb.CreateCellStyle();
        styleHead.BorderTop = BorderStyle.None;
        styleHead.BorderBottom = BorderStyle.None;
        styleHead.BorderLeft = BorderStyle.Thin;
        styleHead.BorderRight = BorderStyle.Thin;
        styleHead.WrapText = true;
        styleHead.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        styleHead.FillPattern = FillPattern.SolidForeground;

        headerRow.GetCell(0).CellStyle = styleHead;
        headerRow.GetCell(1).CellStyle = styleHead;
        stepRow.GetCell(0).CellStyle = styleHead;
        stepRow.GetCell(1).CellStyle = styleHead;



        long cellCount = count;

        for (int i = 2; i < count; i++) //Read Excel File
        {
          IRow row = sheet.CreateRow(i);

          row.CreateCell(0).SetCellValue(grades[i - 2]);
          row.CreateCell(1).SetCellValue(workloads[i - 2]);
          row.GetCell(0).CellStyle = style;
          row.GetCell(1).CellStyle = style;

          for (int j = 2; j < 10; j++)
          {
            row.CreateCell(j).SetCellValue(matriz[i - 2][j - 2]);
            row.GetCell(j).CellStyle = styleValues;
          }
        }

        IRow blocksheet = sheet.GetRow(3);
        blocksheet.CreateCell(12).SetCellValue("sheetimport");

        var font = hssfwb.CreateFont();
        font.Color = IndexedColors.White.Index;
        blocksheet.GetCell(12).CellStyle = hssfwb.CreateCellStyle();
        blocksheet.GetCell(12).CellStyle.SetFont(font);

        //FileStream sw = File.Create(ObjectId.GenerateNewId() + DateTime.Now.ToShortDateString().Replace("/", "") + ".xlsx");
        FileStream sw = File.Create(ObjectId.GenerateNewId() + DateTime.Now.ToShortDateString().Replace("/", "") + ".xlsx");

        hssfwb.Write(sw);


        //sw.Flush();
        //sw.Close();

        //getexcel.Close();

        return sw.Name;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Training
    public List<ViewImportTraining> ImportTraning(Stream stream)
    {
      //string fullPath = @"c:/jms/SALARYSCALE4.xlsx";
      //var stream = new FileStream(fullPath, FileMode.Open);
      try
      {
        //HISTORYTRAINING
        ISheet sheet;
        var list = new List<ViewImportTraining>();
        stream.Position = 0;
        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   


        IRow rowpass = sheet.GetRow(3);
        var pass = rowpass.GetCell(12).ToString();
        if (pass != "sheetimport")
          throw new Exception("not_sheet");


        long count = CountLines(sheet);


        IRow headerRow = sheet.GetRow(0); //Get Header Row
        int cellCount = headerRow.LastCellNum;

        for (int i = 1; i < (count + 1); i++) //Read Excel File
        {
          IRow row = sheet.GetRow(i);
          if (row == null) continue;
          if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

          var cpf = row.GetCell(0)?.ToString();
          var namecourse = row.GetCell(1)?.ToString();
          var content = row.GetCell(2)?.ToString();
          var peridiocity = row.GetCell(3)?.ToString();
          var nameeevent = row.GetCell(4)?.ToString();
          var workload = row.GetCell(5)?.ToString();
          var nameentity = row.GetCell(6)?.ToString();
          var datebegin = row.GetCell(7)?.ToString();
          var dateend = row.GetCell(8)?.ToString();

          int intParsed;
          decimal decimalParsed;
          DateTime dateParsed;

          try
          {
            if ((decimal.TryParse(workload.Trim(), out decimalParsed)) == false)
              throw new Exception("workload_incorret");
          }
          catch (Exception)
          {
            throw new Exception("workload_incorret");
          }

          if ((int.TryParse(peridiocity.Trim(), out intParsed)) == false)
            peridiocity = "0";

          try
          {
            if ((DateTime.TryParse(dateend.Trim(), out dateParsed)) == false)
              throw new Exception("dateend_incorret");
          }
          catch (Exception)
          {
            throw new Exception("dateend_incorret");
          }

          if ((DateTime.TryParse(datebegin.Trim(), out dateParsed)) == false)
            datebegin = dateend;

          var view = new ViewImportTraining()
          {
            Cpf = cpf,
            NameCourse = namecourse,
            Content = content,
            NameEvent = nameeevent,
            NameEntity = nameentity,
            Workload = (decimal.Parse(workload) * 60),
            Peridiocity = byte.Parse(peridiocity),
            DateBegin = DateTime.Parse(datebegin).AddDays(1),
            DateEnd = DateTime.Parse(dateend).AddDays(1)
          };
          list.Add(view);
        }
        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region private
    private long CountLines(ISheet sheet)
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
    #endregion


  }
}
