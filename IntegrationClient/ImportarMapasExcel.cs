using IntegrationService.Enumns;
using IntegrationService.Tools;
using Manager.Views.Integration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace IntegrationClient
{
  public partial class ImportarMapasExcel : Form
  {
    private ExcelPackage eppPlusApp;
    private ExcelWorkbook eppPlusPst;
    private ExcelWorksheet eppPlusPln;

    private Excel.Application excelApp;
    private Excel.Worksheet excelPln;
    private Excel.Workbook excelPst;

    #region Constructor
    public ImportarMapasExcel()
    {
      InitializeComponent();
    }
    #endregion

    #region Import
    private void BtnImpV2_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtPst.Text))
        {
          throw new Exception("Informe a pasta de origem");
        }
        IEnumerable<string> files;
        if (chkEppPlus.Checked)
        {
          files = Directory.EnumerateFiles(txtPst.Text, "*.xls", SearchOption.TopDirectoryOnly);
        }
        else
        {
          excelApp = new Excel.Application
          {
            DisplayAlerts = false,
            Visible = true
          };
          files = Directory.EnumerateFiles(txtPst.Text, "*.xls*", SearchOption.TopDirectoryOnly);
        }
        foreach (string file in files)
        {
          if (chkEppPlus.Checked)
          {
            ImportFileEppPlus(file);
          }
          else
          {
            ImportFileExcel(file);
          }
        }
        if (!chkEppPlus.Checked)
        {
          excelApp.Quit();
        }
        MessageBox.Show("Fim da importação!", "Importação de Mapas", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #region Commum
    private string CellValue(string column, int line)
    {
      if (chkEppPlus.Checked)
      {
        object work = eppPlusPln.Cells[string.Format("{0}{1}", column, line)].Value;
        return work == null ? string.Empty : work.ToString().Trim();
      }
      else
      {
        dynamic work = excelPln.Range[string.Format("{0}{1}", column, line)].Value;
        return work == null ? string.Empty : work.ToString().Trim();
      }
    }
    #endregion

    #region Excel
    private void ImportFileExcel(string file)
    {
      string cellName = "C5";
      string cellGroup = "C6";
      string cellColumnCheck = "A";
      // Responsabilidade
      int responsibilityCellLine = 15;
      string responsibilityCellColumn = "A";
      string responsibilityTextCheck = "RESPONSABILIDADES / ENTREGAS nos processos de atuação";
      // Técnicas
      string hardSkillTextCheck = "BLOCO DE COMPETÊNCIAS TÉCNICAS";
      string softSkillTextCheck = "BLOCO DE COMPETÊNCIAS COMPORTAMENTAIS";
      string hardSkillTextTypeCheck = "ESPECÍFICAS";
      string hardSkillCellColumn = "B";
      // Formação
      string formationTextCheck = "FORMAÇÃO ESCOLAR / ACADÊMICA";
      string formationCellColumn = "C";
      string formationCellColumnComplement = "F";
      // Requisitos
      string requirementTextCheck = "REQUISITOS NECESSÁRIOS (PARA CONTRATAÇÃO)";
      string requirement;
      // Linha de controle de leitura
      int line = 0;
      string work;
      // TODO: inicialização de idCompany e Level Two
      ViewIntegrationProfileOccupation occupation = new ViewIntegrationProfileOccupation
      {
        Messages = new List<string>(),
        IdProcessLevelTwo = "",
        IdCompany = "",
        Activities = new List<string>(),
        Schooling = new List<string>(),
        SchoolingComplement = new List<string>(),
        Skills = new List<string>(),
        Update = chkAtu.Checked
      };
      try
      {
        excelPst = excelApp.Workbooks.Open(file,false);
        excelPln = excelPst.Worksheets[1];
        excelPln.Activate();
        occupation.Name = excelPln.Range[cellName].Value.ToString();
        occupation.NameGroup = excelPln.Range[cellGroup].Value.ToString();
        if (!(excelPln.Range[string.Format("{0}{1}", responsibilityCellColumn, responsibilityCellLine)].Value).ToString().Trim().ToUpper().Equals(responsibilityTextCheck.ToUpper()))
        {
          throw new Exception("Não encontrei a primeira linha das entregas");
        }
        // Tratamento das entregas
        line = responsibilityCellLine + 1;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(hardSkillTextCheck))
          {
            break;
          }
          work = CellValue(responsibilityCellColumn, line);
          if (!string.IsNullOrEmpty(work))
          {
            occupation.Activities.Add(work);
          }
          line++;
        }
        // Encontrar a linha inicial das competências específicas
        line++;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(hardSkillTextTypeCheck))
          {
            break;
          }
          line++;
          if (line>200)
          {
            throw new Exception("Não encontrei as competências específicas");
          }
        }
        // Carregar as competências técnicas
        line++;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(softSkillTextCheck))
          {
            break;
          }
          work = CellValue(hardSkillCellColumn, line);
          if (!string.IsNullOrEmpty(work))
          {
            occupation.Skills.Add(work);
          }
          line++;
        }
        // Encontrar a formação
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(formationTextCheck))
          {
            break;
          }
          line++;
        }
        // Carregar formação
        line++;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(requirementTextCheck))
          {
            break;
          }
          work = CellValue(formationCellColumn, line);
          if (!string.IsNullOrEmpty(work))
          {
            occupation.Schooling.Add(work);
            occupation.SchoolingComplement.Add(CellValue(formationCellColumnComplement, line));
          }
          line++;
        }
        // Requisitos
        line++;
        requirement = string.Empty;
        while (true)
        {
          work = CellValue(cellColumnCheck, line);
          if (string.IsNullOrEmpty(work))
          {
            break;
          }
          requirement = string.Concat(requirement, work);
          line++;
        }
        occupation.SpecificRequirements = requirement;
        excelPst.Close(false);
        // TODO: Chamar API do servidor
        string jj = JsonConvert.SerializeObject(occupation);
        FileClass.SaveLog("aqui.txt", jj, EnumTypeLineOpportunityg.Register);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

    #region EppPlus
    private void ImportFileEppPlus(string file)
    {
      string cellName = "C5";
      string cellGroup = "C6";
      string cellColumnCheck = "A";
      // Responsabilidade
      int responsibilityCellLine = 15;
      string responsibilityCellColumn = "A";
      string responsibilityTextCheck = "RESPONSABILIDADES / ENTREGAS nos processos de atuação";
      // Técnicas
      string hardSkillTextCheck = "BLOCO DE COMPETÊNCIAS TÉCNICAS";
      string softSkillTextCheck = "BLOCO DE COMPETÊNCIAS COMPORTAMENTAIS";
      string hardSkillTextTypeCheck = "ESPECÍFICAS";
      string hardSkillCellColumn = "B";
      // Formação
      string formationTextCheck = "FORMAÇÃO ESCOLAR / ACADÊMICA";
      string formationCellColumn = "C";
      string formationCellColumnComplement = "F";
      // Requisitos
      string requirementTextCheck = "REQUISITOS NECESSÁRIOS (PARA CONTRATAÇÃO)";
      string requirement;
      // Linha de controle de leitura
      int line = 0;
      string work;
      // TODO: inicialização de idCompany e Level Two
      ViewIntegrationProfileOccupation occupation = new ViewIntegrationProfileOccupation
      {
        Messages = new List<string>(),
        IdProcessLevelTwo = "",
        IdCompany = "",
        Activities = new List<string>(),
        Schooling = new List<string>(),
        SchoolingComplement = new List<string>(),
        Skills = new List<string>()
      };
      try
      {
        FileInfo fileInfo = new FileInfo(file);
        eppPlusApp = new ExcelPackage(fileInfo);
        eppPlusPst = eppPlusApp.Workbook;
        eppPlusPln = eppPlusPst.Worksheets["Mapa"];
        occupation.Name = eppPlusPln.Cells[cellName].Value.ToString();
        occupation.NameGroup = eppPlusPln.Cells[cellGroup].Value.ToString();
        if (!(eppPlusPln.Cells[string.Format("{0}{1}", responsibilityCellColumn, responsibilityCellLine)].Value).ToString().Trim().ToUpper().Equals(responsibilityTextCheck.ToUpper()))
        {
          throw new Exception("Não encontrei a primeira linha das entregas");
        }
        // Tratamento das entregas
        line = responsibilityCellLine + 1;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(hardSkillTextCheck))
          {
            break;
          }
          work = CellValue(responsibilityCellColumn, line);
          if (!string.IsNullOrEmpty(work))
          {
            occupation.Activities.Add(work);
          }
          line++;
        }
        // Encontrar a linha inicial das competências específicas
        line++;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(hardSkillTextTypeCheck))
          {
            break;
          }
          line++;
        }
        // Carregar as competências técnicas
        line++;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(softSkillTextCheck))
          {
            break;
          }
          work = CellValue(hardSkillCellColumn, line);
          if (!string.IsNullOrEmpty(work))
          {
            occupation.Skills.Add(work);
          }
          line++;
        }
        // Encontrar a formação
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(formationTextCheck))
          {
            break;
          }
          line++;
        }
        // Carregar formação
        line++;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(requirementTextCheck))
          {
            break;
          }
          work = CellValue(formationCellColumn, line);
          if (!string.IsNullOrEmpty(work))
          {
            occupation.Schooling.Add(work);
            occupation.SchoolingComplement.Add(CellValue(formationCellColumnComplement, line));
          }
          line++;
        }
        // Requisitos
        line++;
        requirement = string.Empty;
        while (true)
        {
          work = CellValue(cellColumnCheck, line);
          if (string.IsNullOrEmpty(work))
          {
            break;
          }
          requirement = string.Concat(requirement, work);
          line++;
        }
        occupation.SpecificRequirements = requirement;
        MessageBox.Show(occupation.ToString());
        eppPlusPst.Dispose();
        eppPlusApp.Dispose();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

    #endregion

  }
}
