using IntegrationService.Api;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace IntegrationClient
{
  public partial class ExportarMapas : Form
  {
    private Excel.Application excelApp;
    private Excel.Worksheet excelPln;
    private Excel.Workbook excelPst;

    public ExportarMapas()
    {
      InitializeComponent();
    }

    private void BtnExp_Click(object sender, EventArgs e)
    {
      try
      {
        IntegrationApi IntegrationApi = new IntegrationApi(Program.PersonLogin);
        List<ViewListOccupation> occupations = IntegrationApi.ExportOccupationList();
        string pathExport = txtPst.Text;
        if (string.IsNullOrEmpty(pathExport))
        {
          pathExport = string.Format(@"{0}Export", AppDomain.CurrentDomain.BaseDirectory);
          if (!Directory.Exists(pathExport))
          {
            Directory.CreateDirectory(pathExport);
          }
        }
        string layoutFile = string.Format(@"{0}Layout\Map.xlsx", AppDomain.CurrentDomain.BaseDirectory);
        lblPrb.Text = string.Format("Exportando 0 de {0} mapas", occupations.Count);
        prb.Maximum = occupations.Count;
        prb.Minimum = 0;
        Refresh();
        excelApp = new Excel.Application
        {
          DisplayAlerts = false,
          Visible = true
        };
        long lineRef = 11;
        List<string> nameRefs = new List<string>()
        {
          "Escopo do Grupo", "Entregas", "Competências Essenciais", "Competências do Grupo",
          "Competências Específicas", "Formação", "Requisitos Específicos"
        };
        List<string> nameTypeSkill = new List<string>()
        {
          "Técnica", "Comportamental", "Outros"
        };
        foreach (ViewListOccupation occupation in occupations)
        {
          prb.Minimum++;
          lblPrb.Text = string.Format("Exportando {0} de {1} mapas", prb.Minimum, occupations.Count);
          Refresh();
          ViewMapOccupation map = IntegrationApi.ExportOccupationProfile(occupation._id);
          excelPst = excelApp.Workbooks.Open(layoutFile);
          excelPln = excelPst.Worksheets["Mapa"];
          excelPln.Activate();
          excelPln.Range["C5"].Value = Program.PersonLogin.NameAccount;
          excelPln.Range["C7"].Value = occupation.Name;
          excelPln.Range["H7"].Value = string.Format("{0}/{1}", map.Group.Sphere.Name, map.Group.Axis.Name);
          excelPln.Range["C9"].Value = occupation.Group.Name;
          excelPln.Range["H9"].Value = DateTime.Now;
          // Escopo
          foreach (ViewListScope item in map.ScopeGroup)
          {
            lineRef++;
            if (excelPln.Range[string.Format("A{0}", lineRef)].Value == nameRefs[1])
            {
              AddLine(lineRef);
            }
            excelPln.Range[string.Format("A{0}", lineRef)].Value = item.Name;
          }
          while (excelPln.Range[string.Format("A{0}", lineRef)].Value != nameRefs[1])
          {
            lineRef++;
          }
          // Entregas
          foreach (ViewListActivitie item in map.Activities)
          {
            lineRef++;
            if (excelPln.Range[string.Format("A{0}", lineRef)].Value == nameRefs[2])
            {
              AddLine(lineRef);
            }
            excelPln.Range[string.Format("A{0}", lineRef)].Value = item.Name;
          }
          while (excelPln.Range[string.Format("A{0}", lineRef)].Value != nameRefs[2])
          {
            lineRef++;
          }
          // Competências Essencias
          foreach (ViewListSkill item in map.SkillsCompany)
          {
            lineRef++;
            if (excelPln.Range[string.Format("A{0}", lineRef)].Value == nameRefs[3])
            {
              AddLineSkill(lineRef);
            }
            excelPln.Range[string.Format("A{0}", lineRef)].Value = item.Name;
            excelPln.Range[string.Format("E{0}", lineRef)].Value = item.Concept;
            switch (item.TypeSkill)
            {
              case Manager.Views.Enumns.EnumTypeSkill.Hard:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[0];
                break;
              case Manager.Views.Enumns.EnumTypeSkill.Soft:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[1];
                break;
              default:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[2];
                break;
            }
          }
          while (excelPln.Range[string.Format("A{0}", lineRef)].Value != nameRefs[3])
          {
            lineRef++;
          }
          // Competências do Grupo
          foreach (ViewListSkill item in map.SkillsGroup)
          {
            lineRef++;
            if (excelPln.Range[string.Format("A{0}", lineRef)].Value == nameRefs[4])
            {
              AddLineSkill(lineRef);
            }
            excelPln.Range[string.Format("A{0}", lineRef)].Value = item.Name;
            excelPln.Range[string.Format("E{0}", lineRef)].Value = item.Concept;
            switch (item.TypeSkill)
            {
              case Manager.Views.Enumns.EnumTypeSkill.Hard:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[0];
                break;
              case Manager.Views.Enumns.EnumTypeSkill.Soft:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[1];
                break;
              default:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[2];
                break;
            }
          }
          while (excelPln.Range[string.Format("A{0}", lineRef)].Value != nameRefs[4])
          {
            lineRef++;
          }
          // Competências Específicas
          foreach (ViewListSkill item in map.Skills)
          {
            lineRef++;
            if (excelPln.Range[string.Format("A{0}", lineRef)].Value == nameRefs[5])
            {
              AddLineSkill(lineRef);
            }
            excelPln.Range[string.Format("A{0}", lineRef)].Value = item.Name;
            excelPln.Range[string.Format("E{0}", lineRef)].Value = item.Concept;
            switch (item.TypeSkill)
            {
              case Manager.Views.Enumns.EnumTypeSkill.Hard:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[0];
                break;
              case Manager.Views.Enumns.EnumTypeSkill.Soft:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[1];
                break;
              default:
                excelPln.Range[string.Format("J{0}", lineRef)].Value = nameTypeSkill[2];
                break;
            }
          }
          while (excelPln.Range[string.Format("A{0}", lineRef)].Value != nameRefs[5])
          {
            lineRef++;
          }
          // Formação
          foreach (var item in map.Schooling)
          {
            lineRef++;
            if (excelPln.Range[string.Format("A{0}", lineRef)].Value == nameRefs[6])
            {
              AddLineFormation(lineRef);
            }
            excelPln.Range[string.Format("A{0}", lineRef)].Value = item.Name;
            excelPln.Range[string.Format("D{0}", lineRef)].Value = item.Complement;
          }
          while (excelPln.Range[string.Format("A{0}", lineRef)].Value != nameRefs[6])
          {
            lineRef++;
          }
          lineRef++;
          // Requisitos Específicos
          excelPln.Range[string.Format("A{0}", lineRef)].Value = map.SpecificRequirements ?? string.Empty;
          // Salvar pasta e fechar
          excelPln.Range["A11"].Select();
          excelPst.SaveAs(string.Format(@"{0}\{1}.xlsx", pathExport, occupation.Name));
          excelPst.Close();
          lineRef = 11;
        }
        excelApp.Quit();
        MessageBox.Show("Fim de exportação!", "Exportação de Mapas", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exportação de Mapas", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AddLine(long lineRef)
    {
      try
      {
        excelPln.Rows[lineRef].Select();
        excelApp.Selection.Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        excelPln.Range[string.Format("A{0}:J{0}",lineRef,lineRef)].Select();
        excelApp.Selection.Merge();
        excelApp.Selection.HorizontalAlignment = Excel.Constants.xlLeft;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void AddLineSkill(long lineRef)
    {
      try
      {
        excelPln.Rows[lineRef].Select();
        excelApp.Selection.Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        excelPln.Range[string.Format("A{0}:D{0}", lineRef, lineRef)].Select();
        excelApp.Selection.Merge();
        excelApp.Selection.HorizontalAlignment = Excel.Constants.xlLeft;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
        excelPln.Range[string.Format("A{0}:D{0}", lineRef, lineRef)].Select();
        excelApp.Selection.Merge();
        excelApp.Selection.HorizontalAlignment = Excel.Constants.xlLeft;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
        excelPln.Range[string.Format("E{0}:I{0}", lineRef, lineRef)].Select();
        excelApp.Selection.Merge();
        excelApp.Selection.HorizontalAlignment = Excel.Constants.xlLeft;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void AddLineFormation(long lineRef)
    {
      try
      {
        excelPln.Rows[lineRef].Select();
        excelApp.Selection.Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        excelPln.Range[string.Format("A{0}:C{0}", lineRef, lineRef)].Select();
        excelApp.Selection.Merge();
        excelApp.Selection.HorizontalAlignment = Excel.Constants.xlLeft;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
        excelPln.Range[string.Format("D{0}:J{0}", lineRef, lineRef)].Select();
        excelApp.Selection.Merge();
        excelApp.Selection.HorizontalAlignment = Excel.Constants.xlLeft;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
        excelApp.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
