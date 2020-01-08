using IntegrationClient.ModelTools;
using IntegrationService.Api;
using IntegrationService.Enumns;
using IntegrationService.Tools;
using Manager.Views.Integration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace IntegrationClient
{
  public partial class ImportarMapasExcel : Form
  {
    private Excel.Application excelApp;
    private Excel.Worksheet excelPln;
    private Excel.Workbook excelPst;

    private List<OccupationStatistic> occupations;
    private List<SkillStatistic> skills;
    private List<SchoolingStatistic> schoolings;
    private List<OccupationSkillStatistic> occupationSkills;
    private List<OccupationSchoolingStatistic> occupationSchoolings;

    #region Constructor
    public ImportarMapasExcel()
    {
      InitializeComponent();
    }
    private void ImportarMapasExcel_Load(object sender, EventArgs e)
    {
      txtPst.Text = Properties.Settings.Default.MapExcelPath;
    }
    #endregion

    #region Objects
    private void ChkAtu_CheckedChanged(object sender, EventArgs e)
    {
      if (chkAtu.Checked)
      {
        chkCom.Checked = true;
        chkCom.Enabled = false;
      }
      else
      {
        chkCom.Checked = false;
        chkCom.Enabled = true;
      }
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
        Properties.Settings.Default.MapExcelPath = txtPst.Text;
        Properties.Settings.Default.Save();
        if (File.Exists(Path.Combine(txtPst.Text, "Tabulacao.xlsx")))
        {
          File.Delete(Path.Combine(txtPst.Text, "Tabulacao.xlsx"));
          Directory.GetFiles(txtPst.Text, "*.log", SearchOption.AllDirectories).ToList().ForEach(f => File.Delete(f));
        }
        InfraIntegration infraIntegration = new InfraIntegration(Program.PersonLogin);
        IEnumerable<string> files;
        excelApp = new Excel.Application
        {
          DisplayAlerts = false,
          Visible = true
        };
        files = Directory.EnumerateFiles(txtPst.Text, "*.xls*", SearchOption.TopDirectoryOnly).OrderBy(filename => filename);
        occupations = new List<OccupationStatistic>();
        skills = new List<SkillStatistic>();
        schoolings = new List<SchoolingStatistic>();
        occupationSkills = new List<OccupationSkillStatistic>();
        occupationSchoolings = new List<OccupationSchoolingStatistic>();
        foreach (string file in files)
        {
          ImportFileExcel(file, infraIntegration);
        }
        FinalTabExcel();
        excelApp.Quit();
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
      dynamic work = excelPln.Range[string.Format("{0}{1}", column, line)].Value;
      return work == null ? string.Empty : work.ToString().Trim();
    }
    private string DictionarySkill(string item)
    {
      item = item.Replace((char)10, ' ').Replace((char)13, ' ').Trim().ToUpper();
      item = item.Replace("POLITICA", "POLÍTICA");
      item = item.Replace("LEGISLACAO", "LEGISLAÇÃO");
      item = item.Replace("TRIBUTARIA", "TRIBUTÁRIA");
      item = item.Replace("GESTAO", "GESTÃO");
      item = item.Equals("AUTOMAÇÃO CLP E PLC") ? "AUTOMAÇÃO INDUSTRIAL" : item;
      item = item.Equals("AUTOMAÇÃO INDUSTRIAL (CLP, PLC, ETC)") ? "AUTOMAÇÃO INDUSTRIAL" : item;
      item = item.Equals("CADEIA DE VALORES DOS NEGÓCIOS DO GRUPO") ? "CADEIA DE VALOR DOS NEGÓCIOS DO GRUPO" : item;
      item = item.Equals("CATALOG") ? "PROMOB CATALOG" : item;
      item = item.Equals("COMUNICAÇÃO INTEGRADA EM MARKETING") ? "COMUNICAÇÃO INTEGRADA DE MARKETING" : item;
      item = item.Equals("CONHECIMENTO DE PRODUTO") ? "PRODUTOS" : item;
      item = item.Equals("CONHECIMENTO DE PRODUTOS") ? "PRODUTOS" : item;
      item = item.Equals("CONTABILIDADE.") ? "CONTABILIDADE" : item;
      item = item.Equals("CONTROLE DE ESTOQUES") ? "CONTROLE DE ESTOQUE" : item;
      item = item.Equals("DICCÇÃO E ORATÓRIA") ? "DICÇÃO E ORATÓRIA" : item;
      item = item.Equals("EFICIÊNCIA PRODUTIVA") ? "EFICIÊNCIA PRODUTIVA/PRODUÇÃO ENXUTA/LEAN MANUFACTURING" : item;
      item = item.Equals("EFICIÊNCIA PRODUTIVA/PRODUÇÃO ENXUTA") ? "EFICIÊNCIA PRODUTIVA/PRODUÇÃO ENXUTA/LEAN MANUFACTURING" : item;
      item = item.Equals("ERGONOMIA APLICADA EM MEIO AMBIENTE") ? "ERGONOMIA APLICADA" : item;
      item = item.Equals("ERP(ENTERPRISE RESOURCE PLANNING)") ? "SISTEMAS INFORMATIZADOS DE GESTÃO" : item;
      item = item.Equals("ESTATÍTICA") ? "ESTATÍSTICA" : item;
      item = item.Equals("EXCEL  AVANÇADO") ? "EXCEL AVANÇADO" : item;
      item = item.Equals("FISCAL E TRIBUTÁRIA") ? "LEGISLAÇÃO FISCAL E TRIBUTÁRIA" : item;
      item = item.Equals("GESTÃO DE CONFLITO") ? "GESTÃO DE CONFLITOS" : item;
      item = item.Equals("GESTÃO CUSTOS") ? "GESTÃO DE CUSTOS" : item;
      item = item.Equals("GESTÃO POR INDICADORES") ? "GESTÃO DE INDICADORES" : item;
      item = item.Equals("HIDRAULICA") ? "HIDRÁULICA" : item;
      item = item.Equals("IDIOMA ESPANHOL") ? "LINGUA ESPANHOLA" : item;
      item = item.Equals("IMPOSTOS E OBRIGAÇÕES ACESSÓRIAS (SEFIP, RAIS, CAGED).") ? "IMPOSTOS E OBRIGAÇÕES ACESSÓRIAS (SEFIP, RAIS, CAGED, DIRF)" : item;
      item = item.Equals("IMPOSTOS E OBRIGAÇÕES ACESSÓRIAS (SEFIP, RAIS, CAGED, DIRF).") ? "IMPOSTOS E OBRIGAÇÕES ACESSÓRIAS (SEFIP, RAIS, CAGED, DIRF)" : item;
      item = item.Equals("INSTRUÇÕES/PROCEDIMENTOS E/OU ROTINAS DE TRABALHO") ? "INSTRUÇÕES, PROCEDIMENTOS E/OU ROTINAS DE TRABALHO" : item;
      item = item.Equals("INSTRUÇÕES/ PROCEDIMENTOS E / OU ROTINAS DE TRABALHO") ? "INSTRUÇÕES, PROCEDIMENTOS E/OU ROTINAS DE TRABALHO" : item;
      item = item.Equals("INSTRUMENTAÇÃO") ? "INSTRUMENTAÇÃO (PAQUÍMETROS, MICROMETROS)" : item;
      item = item.Equals("LEAN MANUFECTURING") ? "LEAN MANUFACTURING" : item;
      item = item.Equals("LEGISLAÇÃO FISCAL (SINTEGRA, SPED, DCTF, DIRF, ETC)") ? "LEGISLAÇÃO FISCAL E OBRIGAÇÕES (SINTEGRA, SPED, DCTF, DIRF, ETC)" : item;
      item = item.Equals("LEGISLAÇÃO FISCAL E OBRIGAÇÕES (SINTEGRA / SPED / DCTF / DIRF ETC.)") ? "LEGISLAÇÃO FISCAL E OBRIGAÇÕES (SINTEGRA, SPED, DCTF, DIRF, ETC)" : item;
      item = item.Equals("LEGISLAÇÃO E GESTÃO DE TERCEIROS") ? "GESTÃO DE TERCEIRIZADOS" : item;
      item = item.Equals("LEGISLAÇÃO TRIBUTÁRIA FISCAL") ? "LEGISLAÇÃO FISCAL E TRIBUTÁRIA" : item;
      item = item.Equals("LEGISLAÇÃO TRIBUTÁRIA, FISCAL E ADMINISTRATIVA") ? "LEGISLAÇÃO FISCAL E TRIBUTÁRIA" : item;
      item = item.Equals("LOGÍSTICA INTERNA (ALMOXARIFADO/EXPEDIÇÃO)") ? "LOGÍSTICA INTERNA" : item;
      item = item.Equals("LOGÍSTICA") ? "LOGÍSTICA INTERNA" : item;
      item = item.Equals("LID - LEITURA INTERPRETAÇÃO DE DESENHOS") ? "LEITURA E INTERPRETAÇÃO DE DESENHOS" : item;
      item = item.Equals("LID - LEITURA INTERPRETAÇÃO DE DESENHOS") ? "LEITURA E INTERPRETAÇÃO DE DESENHOS" : item;
      item = item.Equals("LÍNGUA ESTRANGEIRA (ESPANHOL E INGLÊS)") ? "INGLÊS E ESPANHOL" : item;
      item = item.Equals("MERCADO DE PLANEJADO") ? "MERCADO DE PLANEJADOS" : item;
      item = item.Equals("NR10") ? "NR 10" : item;
      item = item.Equals("NORMAIS INTERNACIONAIS") ? "NORMAS INTERNACIONAIS" : item;
      item = item.Equals("NORMAS TÉCNICAS APLICÁVEIS") ? "NORMAS TÉCNICAS APLICADAS AO PRODUTO" : item;
      item = item.Equals("OPERAÇÃO DE EMPILHADEIRA - NR11") ? "OPERAÇÃO DE EMPILHADEIRA - NR 11" : item;
      item = item.Equals("OPERADOR DE EMPILHADEIRA") ? "OPERAÇÃO DE EMPILHADEIRA - NR 11" : item;
      item = item.Equals("ORÇAMENTO") ? "ORÇAMENTO EMPRESARIAL" : item;
      item = item.Equals("PACOTE DE APLICATIVOS OFFICE") ? "APLICATIVOS OFFICE" : item;
      item = item.Equals("PACOTE DE APLICATIVO OFFICE") ? "APLICATIVOS OFFICE" : item;
      item = item.Equals("PACOTE OFFICE") ? "APLICATIVOS OFFICE" : item;
      item = item.Equals("PESQUISAS EM MARKETING") ? "PESQUISAS DE MARKETING" : item;
      item = item.Equals("POLÍTICAS  DA ORGANIZAÇÃO") ? "POLÍTICAS DA ORGANIZAÇÃO" : item;
      item = item.Equals("POLÍTICAS DE QUALIDADE INTERNAS") ? "POLÍTICA INTERNA DE QUALIDADE" : item;
      item = item.Equals("POLÍTICAS DE QUALIDADE INTERNA") ? "POLÍTICA INTERNA DE QUALIDADE" : item;
      item = item.Equals("POLÍTICAS INTERNAS DE QUALIDADE") ? "POLÍTICA INTERNA DE QUALIDADE" : item;
      item = item.Equals("PRODUTO") ? "PRODUTOS" : item;
      item = item.Equals("PRODUTOS E/OU SERVIÇOS DA EMPRESA") ? "PRODUTOS" : item;
      item = item.Equals("PRODUTO MOEIS DE AÇO") ? "PRODUTO MÓVEIS DE AÇO" : item;
      item = item.Equals("PROMOB") ? "PROMOB CATALOG" : item;
      item = item.Equals("REMUNERAÇÃO (SALÁRIOS E GESTÃO DE BENEFÍCIOS)") ? "REMUNERAÇÃO ESTRATÉGICA" : item;
      item = item.Equals("SEGURANÇA DE TRABALHO") ? "SEGURANÇA DO TRABALHO" : item;
      item = item.Equals("SEGURANÇA DO TRABALHO (NRS 10, SEP, 12...)") ? "SEGURANÇA DO TRABALHO" : item;
      item = item.Equals("SEGURANÇA DO TRABALHO/NRS") ? "SEGURANÇA DO TRABALHO" : item;
      item = item.Equals("SISTEMAS DE DESENHO") ? "SISTEMAS DE DESENHO (CAD)" : item;
      item = item.Equals("SISTEMAS INFORMATIZADOS DE GESTÃO (ERP)") ? "SISTEMAS INFORMATIZADOS DE GESTÃO" : item;
      item = item.Equals("SISTEMA FERRAMENTAS DE GESTÃO") ? "SISTEMAS E FERRAMENTAS DE GESTÃO" : item;
      item = item.Equals("SISTEMAS GESTÃO REVENDAS") ? "SISTEMA DE GESTÃO DE REVENDA" : item;
      item = item.Equals("SISTEMAS INFORMATIZADOS (EVISTA, PRIMUS)") ? "SISTEMAS INFORMATIZADOS DE GESTÃO" : item;
      item = item.Equals("SISTEMÁTICA DO COMÉRCIO EXTERIOR") ? "SISTEMÁTICA DE COMÉRCIO EXTERIOR" : item;
      item = item.Equals("SISTEMÁTICA DO COMÉRCIO EXTERIOR (DOCUMENTAÇÃO, PRAZOS, INCOTERMS, CÂMBIO...)") ? "SISTEMÁTICA DE COMÉRCIO EXTERIOR" : item;
      item = item.Equals("SISTEMAS  E FERRAMENTAS DE GESTÃO") ? "SISTEMAS E FERRAMENTAS DE GESTÃO" : item;
      item = item.Equals("SOFTWARE CATALOG") ? "PROMOB CATALOG" : item;
      item = item.Equals("SOFTWARE DE PROJETO") ? "SOFTWARE DE PROJETOS" : item;
      item = item.Equals("SUBSISTEMAS DE RECURSOS HUMANOS") ? "SUBSISTEMAS DE RH" : item;
      item = item.Equals("TÉCNICCAS DE NEGOCIAÇÃO") ? "TÉCNICAS DE NEGOCIAÇÃO" : item;
      item = item.Equals("TÉCNICAS E CONTROLE DE ESTOQUES") ? "TÉCNICAS DE CONTROLE E GESTÃO DE ESTOQUES" : item;
      item = item.Equals("TECNOLOGIA APLICADAS DE SEGURANÇA EM MÁQUINAS E EQUIPAMENTOS") ? "TECNOLOGIA APLICADA DE SEGURANÇA EM MÁQUINAS E EQUIPAMENTOS" : item;
      item = item.Equals("TRABALHO EM ALTURA") ? "TRABALHO EM ALTURA - NR 35" : item;
      item = item.Equals("TRATAMENTO DE RESSIDUOS E EFLENTES") ? "TRATAMENTO DE RESÍDUOS E EFLUNTES" : item;
      item = item.Equals("TRATAMENTO DE RESSIDUOS E EFLUNTES") ? "TRATAMENTO DE RESÍDUOS E EFLUNTES" : item;
      item = item.Equals("TRATAMENTO DE EFLUENTES") ? "TRATAMENTO DE RESSIDUOS E EFLUNTES" : item;
      return item;
    }
    private string DictionarySchooling(string item)
    {
      item = item.Replace((char)10, ' ').Replace((char)13, ' ').Trim().ToUpper();
      item = item.Replace("TECNICO", "TÉCNICO");
      item = item.Replace("MEDIO", "MÉDIO");
      item = item.Replace("ENSINO MÉDIO EM ANDAMENTO", "ENSINO MÉDIO INCOMPLETO");
      item = item.Replace("ENSINO SUPERIOR EM ANDAMENTO", "ENSINO SUPERIOR INCOMPLETO");
      item = item.Replace("ENSINO SUPERIOR COMPLETO OU EM ANDAMENTO", "ENSINO SUPERIOR INCOMPLETO");
      item = item.Replace("ENSINO SUPERIOR COMPLETO OU FALTANDO MENOS DE DOIS ANOS", "ENSINO SUPERIOR INCOMPLETO");
      item = item.Replace("ENSINO TÉCNICO EM ANDAMENTO", "TÉCNICO INCOMPLETO");
      item = item.Replace("ENSINO TÉCNICO COMPLETO", "TÉCNICO COMPLETO");
      item = item.Replace("ENSINO SUPERIOR COMPLETO NAS ÁREAS DE ENGENHARIA", "ENSINO SUPERIOR COMPLETO");
      item = item.Replace("ADM: PÓS - GRADUAÇÃO EM ANDAMENTO | OPERAÇÃO: ENSINO SUPERIOR COMPLETO", "PÓS GRADUAÇÃO INCOMPLETA");
      item = item.Replace("ADM: PÓS GRADUAÇÃO INCOMPLETA | OPERAÇÃO: ENSINO SUPERIOR COMPLETO", "PÓS GRADUAÇÃO INCOMPLETA");
      item = item.Replace("ADM: PÓS-GRADUAÇÃO EM ANDAMENTO | OPERAÇÃO: ENSINO SUPERIOR COMPLETO", "PÓS GRADUAÇÃO INCOMPLETA");
      item = item.Replace("PÓS-GRADUAÇÃO EM ANDAMENTO", "PÓS GRADUAÇÃO INCOMPLETA");
      item = item.Replace("PÓS-GRADUAÇÃO INCOMPLETA", "PÓS GRADUAÇÃO INCOMPLETA");
      item = item.Replace("PÓS-GRADUAÇÃO COMPLETA", "PÓS GRADUAÇÃO COMPLETA");
      item = item.Replace("PÓS-GRADUAÇÃO COMPLETO", "PÓS GRADUAÇÃO COMPLETA");
      item = item.Replace("PÓS - GRADUAÇÃO COMPLETA NAS ÁREAS DE INTERESSE DO NEGÓCIO", "PÓS GRADUAÇÃO COMPLETA");
      item = item.Replace("PÓS GRADUAÇÃO COMPLETA NAS ÁREAS DE INTERESSE DO NEGÓCIO", "PÓS GRADUAÇÃO COMPLETA");
      return item;
    }
    #endregion

    #region Excel
    private void ImportFileExcel(string file, InfraIntegration infraIntegration)
    {
      string cellName = "C5";
      // Utilizado para identificar a quebra de cargo por área (exemplo Bertolini)
      // Gerente Comercial - BSA
      //    Nome na celula C5: Gerente Comercial,
      //    Complemento na célula K5: - BSA, exatamente igual ao campo Description, inclusive com os espaços na frente
      string cellNameComplement = "K5";
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
      // Contabilização da importação
      ViewIntegrationProfileOccupation viewOccupation = new ViewIntegrationProfileOccupation
      {
        Messages = new List<string>(),
        // TODO: inicialização Level Two
        IdProcessLevelTwo = "5cc7790e28897595ddd49d5d",
        // TODO
        Activities = new List<string>(),
        Schooling = new List<string>(),
        SchoolingComplement = new List<string>(),
        Skills = new List<string>(),
        Update = chkAtu.Checked,
        UpdateSkill = chkCom.Checked,
        Name = string.Empty,
        Description = string.Empty,
        NameGroup = string.Empty,
        SpecificRequirements = string.Empty,
        _id = string.Empty
      };
      try
      {
        excelPst = excelApp.Workbooks.Open(file, false);
        excelPln = excelPst.Worksheets[1];
        excelPln.Activate();
        viewOccupation.Name = excelPln.Range[cellName].Value.ToString();
        viewOccupation.Description = excelPln.Range[cellNameComplement].Value ?? string.Empty;
        viewOccupation.NameGroup = excelPln.Range[cellGroup].Value.ToString();

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
            viewOccupation.Activities.Add(work);
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
          if (line > 200)
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
            viewOccupation.Skills.Add(DictionarySkill(work));
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
            viewOccupation.Schooling.Add(DictionarySchooling(work));
            viewOccupation.SchoolingComplement.Add(CellValue(formationCellColumnComplement, line));
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
        viewOccupation.SpecificRequirements = requirement;
        if (chkLjo.Checked)
        {
          string fileName = file.ToLower().Replace(".xlsx", ".log").Replace(".xls", ".log");
          FileClass.SaveLog(fileName, Program.PersonLogin.Token, EnumTypeLineOpportunityg.Register);
          FileClass.SaveLog(fileName, JsonConvert.SerializeObject(viewOccupation), EnumTypeLineOpportunityg.Register);
          viewOccupation = infraIntegration.IntegrationProfile(viewOccupation);
          FileClass.SaveLog(fileName, "Retorno", EnumTypeLineOpportunityg.Register);
          FileClass.SaveLog(fileName, JsonConvert.SerializeObject(viewOccupation), EnumTypeLineOpportunityg.Register);
        }
        else
        {
          viewOccupation = infraIntegration.IntegrationProfile(viewOccupation);
        }
        OccupationStatistic occupation = new OccupationStatistic()
        {
          FileName = file,
          GroupName = viewOccupation.NameGroup,
          Name = viewOccupation.Name,
          Description = viewOccupation.Description,
          Status = viewOccupation.Messages.Count > 0 ? "Erro" : string.IsNullOrEmpty(viewOccupation._id) ? "Não Atualizado" : "Ok"
        };
        if (occupations.FirstOrDefault(p => p.Name == viewOccupation.Name && p.Description == viewOccupation.Description) == null)
        {
          occupations.Add(occupation);
        }
        else
        {
          MessageBox.Show("Achei");
        }
        excelPst.Close(false);
        bool found;
        foreach (string item in viewOccupation.Skills)
        {
          int index = skills.FindIndex(p => p.Name.Equals(item));
          found = true;
          if (index == -1)
          {
            skills.Add(new SkillStatistic()
            {
              Name = item,
              Found = true,
              Used = 1
            });
          }
          else
          {
            skills[index].Used++;
            found = skills[index].Found;
          }
          occupationSkills.Add(new OccupationSkillStatistic()
          {
            FileName = file,
            SkillName = item,
            Found = found
          });
        }
        foreach (string item in viewOccupation.Schooling)
        {
          int index = schoolings.FindIndex(p => p.Name.ToUpper().Equals(item.ToUpper()));
          if (index == -1)
          {
            schoolings.Add(new SchoolingStatistic()
            {
              Name = item,
              Register = true,
              Found = true,
              Profile = true
            });
          }
          occupationSchoolings.Add(new OccupationSchoolingStatistic()
          {
            FileName = file,
            SchollingName = item,
            Profile = true
          });
        }
        foreach (string item in viewOccupation.Messages)
        {
          string[] itemAux = item.Split('@');
          if (itemAux[1].IndexOf("competência") != -1)
          {
            var index = skills.FindIndex(p => p.Name.Equals(itemAux[0]));
            if (index == -1)
            {
              skills.Add(new SkillStatistic()
              {
                Name = itemAux[0],
                Found = false,
                Used = 1
              });
            }
            else
            {
              skills[index].Used++;
            }
            occupationSkills.Add(new OccupationSkillStatistic()
            {
              FileName = file,
              SkillName = itemAux[0]
            });
          }
          else
          {
            if (itemAux[1].IndexOf("cadastrada") != -1)
            {
              schoolings.Add(new SchoolingStatistic()
              {
                Name = itemAux[0],
                Register = false,
                Found = false,
                Profile = false
              });
            }
            else
            {
              schoolings.Add(new SchoolingStatistic()
              {
                Name = itemAux[0],
                Register = true,
                Found = false,
                Profile = false
              });
            }
            occupationSchoolings.Add(new OccupationSchoolingStatistic()
            {
              FileName = file,
              SchollingName = itemAux[0],
              Profile = false
            });
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void FinalTabExcel()
    {
      try
      {
        excelPst = excelApp.Workbooks.Add();
        int qtdFinal = 6 - excelPst.Worksheets.Count;
        for (int i = excelPst.Worksheets.Count; i < qtdFinal; i++)
        {
          excelPst.Worksheets.Add();
        }
        // Planilha de Competências
        excelPln = excelPst.Worksheets[1];
        excelPln.Activate();
        excelPln.Name = "Competências";
        excelPln.Range["A1"].Value = "Competência";
        excelPln.Range["B1"].Value = "Cadastrada";
        excelPln.Range["C1"].Value = "Usada em Cargos";
        int line = 2;
        foreach (SkillStatistic item in skills)
        {
          excelPln.Range[string.Format("A{0}", line)].Value = item.Name;
          excelPln.Range[string.Format("B{0}", line)].Value = item.Found ? "Sim" : "Não";
          excelPln.Range[string.Format("C{0}", line)].Value = item.Used;
          line++;
        }
        // Planilha de Escolaridades
        excelPln = excelPst.Worksheets[2];
        excelPln.Activate();
        excelPln.Name = "Escolaridades";
        excelPln.Range["A1"].Value = "Escolaridade";
        excelPln.Range["B1"].Value = "Cadastrada";
        excelPln.Range["C1"].Value = "Localizada";
        excelPln.Range["D1"].Value = "Perfil";
        line = 2;
        foreach (SchoolingStatistic item in schoolings)
        {
          excelPln.Range[string.Format("A{0}", line)].Value = item.Name;
          excelPln.Range[string.Format("B{0}", line)].Value = item.Register ? "Sim" : "Não";
          excelPln.Range[string.Format("C{0}", line)].Value = item.Found ? "Sim" : "Não";
          excelPln.Range[string.Format("D{0}", line)].Value = item.Profile ? "Sim" : "Não";
          line++;
        }
        // Planilha de Cargos
        excelPln = excelPst.Worksheets[3];
        excelPln.Activate();
        excelPln.Name = "Cargos";
        excelPln.Range["A1"].Value = "Arquivo";
        excelPln.Range["B1"].Value = "Grupo";
        excelPln.Range["C1"].Value = "Cargo";
        excelPln.Range["D1"].Value = "Status";
        line = 2;
        foreach (OccupationStatistic item in occupations)
        {
          excelPln.Range[string.Format("A{0}", line)].Value = item.FileName;
          excelPln.Range[string.Format("B{0}", line)].Value = item.GroupName;
          excelPln.Range[string.Format("C{0}", line)].Value = string.IsNullOrEmpty(item.Description) ? item.Name : string.Concat(item.Name, item.Description);
          excelPln.Range[string.Format("D{0}", line)].Value = item.Status;
          line++;
        }
        // Planilha de Competências por cargo
        excelPln = excelPst.Worksheets[4];
        excelPln.Activate();
        excelPln.Name = "CargosCompetencias";
        excelPln.Range["A1"].Value = "Cargo";
        excelPln.Range["B1"].Value = "Competência";
        excelPln.Range["C1"].Value = "Cadastrada";
        line = 2;
        foreach (OccupationSkillStatistic item in occupationSkills)
        {
          excelPln.Range[string.Format("A{0}", line)].Value = item.FileName;
          excelPln.Range[string.Format("B{0}", line)].Value = item.SkillName;
          excelPln.Range[string.Format("C{0}", line)].Value = item.Found;
          line++;
        }
        // Planilha de Competências por cargo
        excelPln = excelPst.Worksheets[5];
        excelPln.Activate();
        excelPln.Name = "CargosEscolaridades";
        excelPln.Range["A1"].Value = "Cargo";
        excelPln.Range["B1"].Value = "Escolaridade";
        excelPln.Range["C1"].Value = "Perfil";
        line = 2;
        foreach (OccupationSchoolingStatistic item in occupationSchoolings)
        {
          excelPln.Range[string.Format("A{0}", line)].Value = item.FileName;
          excelPln.Range[string.Format("B{0}", line)].Value = item.SchollingName;
          excelPln.Range[string.Format("C{0}", line)].Value = item.Profile ? "Sim" : "Não";
          line++;
        }
        excelPst.Worksheets[1].Activate();
        excelPst.SaveAs(Path.Combine(txtPst.Text,"Tabulacao.xlsx"));
        excelPst.Close();
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
