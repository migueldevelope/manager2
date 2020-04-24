using IntegrationClient.ModelTools;
using IntegrationClient.Api;
using IntegrationClient.Enumns;
using IntegrationClient.Tools;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Newtonsoft.Json;
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
        if (occupations.Count == 0)
        {
          throw new Exception("Não tem cargos para exportar!");
        }
        // Preparar areas para LOs
        List<ExportOpportunityLine> exportOpportunityLines = new List<ExportOpportunityLine>();
        string pathExport = txtPst.Text;
        if (string.IsNullOrEmpty(pathExport))
        {
          pathExport = string.Format(@"{0}Export", AppDomain.CurrentDomain.BaseDirectory);
          if (!Directory.Exists(pathExport))
          {
            Directory.CreateDirectory(pathExport);
          }
        }
        string layoutFile = string.Format(@"{0}Layout\Layout_Exportacao_Mapa.xlsx", AppDomain.CurrentDomain.BaseDirectory);
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
          foreach (ViewListProcessLevelTwo subProcess in occupation.Process)
          {
            exportOpportunityLines.Add(new ExportOpportunityLine()
            {
              Area = subProcess.ProcessLevelOne.Area.Name,
              Group = occupation.Group,
              Process = subProcess.ProcessLevelOne,
              SubProcess = subProcess,
              OccupationName = occupation.Name,
              GroupColumn = string.Empty,
              SubProcessLine = 0
            });
          }
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
          // GOTO: talvez aqui
          foreach (ViewCrudSchoolingOccupation item in map.Schooling)
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
          // Linha de Oportunidade
          lineRef += 3;
          bool firstLine = true;
          string saveArea = null;
          string saveProcess = null;
          foreach (ViewListProcessLevelTwo item in occupation.Process
            .OrderBy(o => o.ProcessLevelOne.Area.Name).ThenBy(o1 => o1.ProcessLevelOne.Order).ThenBy(o2 => o2.Order).ToList())
          {
            if (!firstLine)
            {
              AddLineOpportunityLine(lineRef);
            }
            if (saveArea != item.ProcessLevelOne.Area.Name)
            {
              excelPln.Range[string.Format("A{0}", lineRef)].Value = item.ProcessLevelOne.Area.Name;
              excelPln.Range[string.Format("D{0}", lineRef)].Value = item.ProcessLevelOne.Name;
              saveArea = item.ProcessLevelOne.Area.Name;
              saveProcess = item.ProcessLevelOne.Name;
            }
            else
            {
              if (saveProcess != item.ProcessLevelOne.Name)
              {
                excelPln.Range[string.Format("D{0}", lineRef)].Value = item.ProcessLevelOne.Name;
                saveProcess = item.ProcessLevelOne.Name;
              }
            }
            excelPln.Range[string.Format("G{0}", lineRef)].Value = item.Name;
            lineRef++;
            firstLine = false;
          }
          // Salvar pasta e fechar
          excelPln.Range["A11"].Select();
          excelPst.SaveAs(string.Format(@"{0}\{1}.xlsx", pathExport, occupation.Name.Replace("/","_")));
          excelPst.Close();
          lineRef = 11;
        }
        //FileClass.SaveLog("juremir.txt", JsonConvert.SerializeObject(exportOpportunityLines), EnumTypeLineOpportunityg.Register);
        //exportOpportunityLines = JsonConvert.DeserializeObject<List<ExportOpportunityLine>>(FileClass.LoadLog("juremir.txt"));
        string areaName = null;
        List<ExportOpportunityLine> partOpportunityLines = new List<ExportOpportunityLine>();
        List<ViewListGroup> areaGroups = new List<ViewListGroup>();
        List<ViewListProcessLevelTwo> areaSubProcess = new List<ViewListProcessLevelTwo>();
        foreach (var item in exportOpportunityLines.OrderBy(o => o.Area).ToList())
        {
          if (areaName == null)
          {
            areaName = item.Area;
          }
          if (areaName != item.Area)
          {
            ExportOpportunityLine(areaName, areaGroups, areaSubProcess, partOpportunityLines, pathExport);
            areaGroups = new List<ViewListGroup>();
            areaSubProcess = new List<ViewListProcessLevelTwo>();
            partOpportunityLines = new List<ExportOpportunityLine>();
            areaName = item.Area;
          }
          if (areaGroups.Find(p => p._id == item.Group._id) == null)
          {
            areaGroups.Add(item.Group);
          }
          if (areaSubProcess.Find(p => p._id == item.SubProcess._id) == null)
          {
            areaSubProcess.Add(item.SubProcess);
          }
          partOpportunityLines.Add(item);
        }
        ExportOpportunityLine(areaName, areaGroups, areaSubProcess, partOpportunityLines, pathExport);
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
    private void AddLineOpportunityLine(long lineRef)
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
        excelPln.Range[string.Format("D{0}:F{0}", lineRef, lineRef)].Select();
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
        excelPln.Range[string.Format("G{0}:J{0}", lineRef, lineRef)].Select();
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
    private void ExportOpportunityLine(string areaName, List<ViewListGroup> areaGroups, List<ViewListProcessLevelTwo> areaSubProcess, List<ExportOpportunityLine> partOpportunityLines, string pathExport)
    {
      try
      {
        string layoutFile = string.Format(@"{0}Layout\Layout_Exportacao_LO.xlsx", AppDomain.CurrentDomain.BaseDirectory);
        excelPst = excelApp.Workbooks.Open(layoutFile);
        excelPln = excelPst.Worksheets["Linha de Oportunidade"];
        excelPln.Activate();
        excelPln.Range["C2"].Value = areaName;
        excelPln.Range["I2"].Value = DateTime.Now;
        // Posicionar os grupos de cargo
        if (areaGroups.Count > 8)
        {
          // Adicionar colunas
          for (int i = 8; i < areaGroups.Count; i++)
          {
            var oRng = excelPln.Range["E3"];
            oRng.EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight,
                    Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
          }
        }
        long column = 0;
        areaGroups = areaGroups.OrderByDescending(o => o.Sphere.TypeSphere).ThenByDescending(o => o.Axis.TypeAxis).ThenByDescending(o => o.Line).ToList();
        foreach (ViewListGroup item in areaGroups)
        {
          excelPln.Range[string.Format("{0}4", (char)(67 + column))].Value = item.Name;
          ExportOpportunityLine element = partOpportunityLines.Where(p => p.Group._id == item._id && string.IsNullOrEmpty(p.GroupColumn)).FirstOrDefault();
          while (element != null)
          {
            partOpportunityLines[partOpportunityLines.IndexOf(element)].GroupColumn = ((char)(67 + column)).ToString();
            element = partOpportunityLines.Where(p => p.Group._id == item._id && string.IsNullOrEmpty(p.GroupColumn)).FirstOrDefault();
          }
          column++;
        }
        // Posicionar os processos e subprocessos
        long lineRef = 6;
        string processName = null;
        Excel.Range orig = null;
        Excel.Range destiny = null;
        foreach (ViewListProcessLevelTwo item in areaSubProcess.OrderBy(o => o.ProcessLevelOne.Order).ThenBy(o1 => o1.Order))
        {
          lineRef++;
          if (processName != item.ProcessLevelOne.Name)
          {
            orig = excelPln.Rows["5:5"];
            destiny = excelPln.Rows[string.Format("{0}:{1}", lineRef, lineRef)];
            orig.Copy(destiny);
            excelPln.Range[string.Format("A{0}", lineRef)].Value = item.ProcessLevelOne.Name;
            processName = item.ProcessLevelOne.Name;
            lineRef++;
          }
          orig = excelPln.Rows["6:6"];
          destiny = excelPln.Rows[string.Format("{0}:{1}", lineRef, lineRef)];
          orig.Copy(destiny);
          excelPln.Range[string.Format("B{0}", lineRef)].Value = item.Name;
          ExportOpportunityLine element = partOpportunityLines.Where(p => p.SubProcess._id == item._id && p.SubProcessLine == 0).FirstOrDefault();
          while (element != null)
          {
            partOpportunityLines[partOpportunityLines.IndexOf(element)].SubProcessLine = lineRef;
            element = partOpportunityLines.Where(p => p.SubProcess._id == item._id && p.SubProcessLine == 0).FirstOrDefault();
          }
        }
        // Posicionar os cargos
        string saveColumn = null;
        long saveLine = 0;
        string value = string.Empty;
        foreach (var item in partOpportunityLines.OrderBy(o => o.GroupColumn).ThenBy(o => o.SubProcessLine))
        {
          if (saveColumn == null)
          {
            saveLine = item.SubProcessLine;
            saveColumn = item.GroupColumn;
            value = item.OccupationName;
          }
          else
          {
            if (saveLine != item.SubProcessLine || saveColumn != item.GroupColumn)
            {
              excelPln.Range[string.Format("{0}{1}", saveColumn, saveLine)].Value = value;
              saveLine = item.SubProcessLine;
              saveColumn = item.GroupColumn;
              value = item.OccupationName;
            }
            else
            {
              value = string.Concat(value, Environment.NewLine, item.OccupationName);
            }
          }
        }
        excelPln.Range[string.Format("{0}{1}", saveColumn, saveLine)].Value = value;
        excelPln.Range["6:6"].Select();
        excelApp.Selection.Delete(Excel.XlDirection.xlUp);
        excelPln.Range["5:5"].Select();
        excelApp.Selection.Delete(Excel.XlDirection.xlUp);
        excelPln.Range["A1"].Select();
        excelPst.SaveAs(string.Format(@"{0}\_LO_{1}.xlsx", pathExport, areaName));
        excelPst.Close();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
