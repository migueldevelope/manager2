using IntegrationClient.ModelTools;
using IntegrationClient.Api;
using IntegrationClient.Enumns;
using IntegrationClient.Tools;
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
        if (File.Exists(Path.Combine(txtPst.Text, "Tabulacao.xlsx")))
        {
          File.Delete(Path.Combine(txtPst.Text, "Tabulacao.xlsx"));
          Directory.GetFiles(txtPst.Text, "*.log", SearchOption.AllDirectories).ToList().ForEach(f => File.Delete(f));
        }
        IntegrationApi infraIntegration = new IntegrationApi(Program.PersonLogin);
        IEnumerable<string> files;
        excelApp = new Excel.Application
        {
          DisplayAlerts = false,
          Visible = true
        };
        if (chkSub.Checked)
        {
          files = Directory.EnumerateFiles(txtPst.Text, "*.xls*", SearchOption.AllDirectories).OrderBy(filename => filename);
        }
        else
        {
          files = Directory.EnumerateFiles(txtPst.Text, "*.xls*", SearchOption.TopDirectoryOnly).OrderBy(filename => filename);
        }
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
      item = item.Replace("  ", " ");
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
      // PUCRS
      item = item.Equals("ANÁLISE ECONOMICO-FINANCEIRO") ? "ANÁLISE ECONÔMICA E FINANCEIRA" : item;
      item = item.Equals("ÁREA DE CONHECIMENTO IPR") ? "ÁREAS DE CONHECIMENTO DO IPR" : item;
      item = item.Equals("ÁREA DE CONHECIMENTO DO IPR") ? "ÁREAS DE CONHECIMENTO DO IPR" : item;
      item = item.Equals("AREAS DE CONHECIMENTO") ? "ÁREAS DE CONHECIMENTO" : item;
      item = item.Equals("ÁREAS DE CONHECIMENTO  ESCOLAS / CURSO") ? "ÁREAS DE CONHECIMENTO DA SUA ESCOLA / CURSO" : item;
      item = item.Equals("ÁREAS DE CONHECIMENTO DA SUA ESCOLA/CURSO") ? "ÁREAS DE CONHECIMENTO DA SUA ESCOLA / CURSO" : item;
      item = item.Equals("ÁREAS DE CONHECIMENTO DO EMPREENDEDORISMO") ? "ÁREAS DE CONHECIMENTO DO EMPREENDEDORISMO" : item;
      item = item.Equals("AVALIAÇÃO | IMPACTO") ? "AVALIAÇÃO E IMPACTO" : item;
      item = item.Equals("BOAS PRÁTICAS DE LABORATÓRIO") ? "BOAS PRÁTICAS DE LABORATÓRIO (BPL)" : item;
      item = item.Equals("C APTAÇÃO DE PROJETOS") ? "CAPTAÇÃO DE PROJETOS" : item;
      item = item.Equals("CÓDIGO DEFESA DO CONSUMIDOR") ? "CÓDIGO DE DEFESA DO CONSUMIDOR" : item;
      item = item.Equals("CONNHECIMENTO ESPECÍFICO") ? "CONHECIMENTO ESPECÍFICO" : item;
      item = item.Equals("CURSOS DE CUMPLIANCE") ? "CURSOS DE COMPLIANCE" : item;
      item = item.Equals("DEMOSTRAÇÕES FINANCEIRAS") ? "DEMONSTRAÇÕES FINANCEIRAS" : item;
      item = item.Equals("DESENVOLVER | WEB") ? "DESENVOLVIMENTO WEB" : item;
      item = item.Equals("DOMINIO PACOTE OFFICE") ? "APLICATIVOS OFFICE" : item;
      item = item.Equals("ELÉTRICA - AVANÇADA") ? "ELÉTRICA AVANÇADA" : item;
      item = item.Equals("ESTRATÉGIA E MARKETING") ? "ESTRATÉGIA DE MARKETING" : item;
      item = item.Equals("EXCELL AVANÇADO") ? "EXCEL AVANÇADO" : item;
      item = item.Equals("BI - BUSINESS INTELLIGENCE") ? "FERRAMENTAS BI - BUSINESS INTELLIGENCE" : item;
      item = item.Equals("FERRAMENTAS BI") ? "FERRAMENTAS BI - BUSINESS INTELLIGENCE" : item;
      item = item.Equals("FERAMENTAS GRÁFICAS") ? "FERRAMENTAS GRÁFICAS" : item;
      item = item.Equals("GESTÃO DE PESSOAL") ? "GESTÃO DE PESSOAS" : item;
      item = item.Equals("GESTÃO DE PROJETO") ? "GESTÃO DE PROJETOS" : item;
      item = item.Equals("GESTÃO PESSOAS") ? "GESTÃO DE PESSOAS" : item;
      item = item.Equals("GOVERNAÇA, DIRETRIZES E .....") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNAÇA, DIRETRIZES.....") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA  E DOCUMENTOS INSTITUCIONAIS") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA E DIRETRIZES") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETRIZES") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETRIZES E .....") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETRIZES E DOCUMENTOS") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETRIZES E DOCUMENTOS  INSTITUCIONAIS") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSITUCIONAIS") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS.") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUIONAIS") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("GOVERNANÇA, DIRETTRIZES....") ? "GOVERNANÇA, DIRETRIZES E DOCUMENTOS INSTITUCIONAIS" : item;
      item = item.Equals("HIDRAÚLICA") ? "HIDRÁULICA" : item;
      item = item.Equals("INFRAESTRUTURA DE EVENTOS ;") ? "INFRAESTRUTURA DE EVENTOS" : item;
      item = item.Equals("INGLES INTERMADIÁRIO") ? "INGLÊS INTERMEDIÁRIO" : item;
      item = item.Equals("INGLÊS TÉCNIC0") ? "INGLÊS TÉCNICO" : item;
      item = item.Equals("INGLÊS OU ESPANHOL INTERMEDIÁRIO") ? "INGLÊS/ESPANHOL INTERMEDIÁRIO" : item;
      item = item.Equals("LEGISLAÇÃO APLIACADA NA CONSTRUCAO CIVIL") ? "LEGISLAÇÃO APLICADA NA CONSTRUÇÃO CIVIL" : item;
      item = item.Equals("LEGISLAÇÃO APLICADA NA CONSTRUCAO CIVIL") ? "LEGISLAÇÃO APLICADA NA CONSTRUÇÃO CIVIL" : item;
      item = item.Equals("LEGISLAÇÃO CÍVEL") ? "LEGISLAÇÃO CIVEL" : item;
      item = item.Equals("LEGISLAÇÃO CÍVEL E PROCESSUAL CÍVEL") ? "LEGISLAÇÃO CIVEL" : item;
      item = item.Equals("LEGISLAÇÃO DOS ORÇÃOS DE FOMENTO, CONEP") ? "LEGISLAÇÃO DOS ÓRGÃOS DE FOMENTO, CONEP" : item;
      item = item.Equals("LEGISLAÇÃO SUBVENÇÕES") ? "LEGISLAÇÃO SUBVENÇÃO PÚBLICA" : item;
      item = item.Equals("LINGUA ESPANHOLA AVANÇADA") ? "ESPANHOL AVANÇADO" : item;
      item = item.Equals("LINGUA INGLESA") ? "INGLÊS BÁSICO" : item;
      item = item.Equals("LÍNGUA INGLESA") ? "INGLÊS BÁSICO" : item;
      item = item.Equals("LINGUA PORTUGUESA") ? "LÍNGUA PORTUGUESA" : item;
      item = item.Equals("LINGUAGEM DE PROGRAMAÇÃO") ? "LINGUAGENS DE PROGRAMAÇÃO" : item;
      item = item.Equals("MECÂNICA - AVANÇADA") ? "MECÂNICA AVANÇADA" : item;
      item = item.Equals("METODOLOGIA EM ANÁLISES CLÍNICAS") ? "METODOLOGIA DE ANÁLISES CLÍNICAS" : item;
      item = item.Equals("METODOLOGIAS DE ANÁLISES CLINICAS") ? "METODOLOGIA DE ANÁLISES CLÍNICAS" : item;
      item = item.Equals("METOLODOGIA DE TESTES") ? "METODOLOGIA DE TESTES" : item;
      item = item.Equals("METOLOGIAS ATIVAS") ? "METODOLOGIAS ATIVAS" : item;
      item = item.Equals("METODOLOGIA ATIVAS") ? "METODOLOGIAS ATIVAS" : item;
      item = item.Equals("NR-11") ? "NR 11" : item;
      item = item.Equals("NR35") ? "NR 35" : item;
      item = item.Equals("OFFICE") ? "APLICATIVOS OFFICE" : item;
      item = item.Equals("PACOTTE OFFICE") ? "APLICATIVOS OFFICE" : item;
      item = item.Equals("PACOTE OFFICE, PRINCIPALMENTE EXCEL") ? "APLICATIVOS OFFICE" : item;
      item = item.Equals("PACOTE OFFICE | WORKFLOW") ? "APLICATIVOS OFFICE | WORKFLOW" : item;
      item = item.Equals("PLATAFORMAS E ADMINISTRAÇÃO DE SUPORTE") ? "PLATAFORMAS DE ADMINISTRAÇÃO E SUPORTE" : item;
      item = item.Equals("PORTIFOLIO DE SERVIÇOS") ? "PORTIFÓLIO DE SERVIÇOS" : item;
      item = item.Equals("PRODUÇÕES AUDIOVISUAIS") ? "PRODUÇÃO AUDIOVISUAL" : item;
      item = item.Equals("PROSPECÇÂO E FIDELIZAÇÂO") ? "PROSPECÇÃO E FIDELIZAÇÃO" : item;
      item = item.Equals("PROTOTIPAÇÃO | DESENVOLVIMENTO") ? "PROTOTIPAÇÃO E DESENVOLVIMENTO" : item;
      item = item.Equals("REDAÇÃO  EMPRESARIAL") ? "REDAÇÃO EMPRESARIAL" : item;
      item = item.Equals("SISPROUNI") ? "SISTEMA SISPROUNI" : item;
      item = item.Equals("SISTEMAS DE ADM. PESSOAL") ? "SISTEMAS DE ADMINISTRAÇÃO DE PESSOAL" : item;
      item = item.Equals("SISTEMAS DE GESTÃO DE LABORATÓRIO") ? "SISTEMAS DE GESTÃO DE LABORATÓRIOS" : item;
      item = item.Equals("TECNOLOGIAS DA INFORMAÇÃO") ? "TECNOLOGIA DA INFORMAÇÃO" : item;
      item = item.Equals("TEORIA E TÉCNICAS") ? "TEORIA E TÉCNICA" : item;
      item = item.Equals("ANÁLISE DE NEGÓCIO") ? "ANÁLISE DE NEGÓCIOS" : item;
      item = item.Equals("ANÁLISE DOS RESULTADOS") ? "ANÁLISE DE RESULTADOS" : item;
      item = item.Equals("ARQUITETURA DE SISTEMAS") ? "ARQUITETURAS DE SISTEMAS DE INFORMAÇÃO" : item;
      item = item.Equals("GESTÃO DE CONHECIMENTO") ? "GESTÃO DO CONHECIMENTO" : item;
      item = item.Equals("INDICADOR E DEMONSTRATIVOS GERENCIAIS") ? "INDICADORES E DEMONSTRATIVOS GERENCIAIS" : item;
      item = item.Equals("INFORMÁTICA BÁSICA (PRECISA?)") ? "INFORMÁTICA BÁSICA" : item;
      item = item.Equals("LEGISLAÇÃO CONDOMÍNIO") ? "LEGISLAÇÃO DE CONDOMÍNIO" : item;
      item = item.Equals("LEGISLAÇÃO CONDOMINIOS") ? "LEGISLAÇÃO DE CONDOMÍNIO" : item;
      item = item.Equals("LEGISLAÇÃO CONDOMÍNIOS") ? "LEGISLAÇÃO DE CONDOMÍNIO" : item;
      item = item.Equals("LEGISLAÇÃO DE CONDOMÍNIOS") ? "LEGISLAÇÃO DE CONDOMÍNIO" : item;
      item = item.Equals("LEI DO INQUILINATO") ? "LEGISLAÇÃO DO INQUILINATO" : item;
      item = item.Equals("PADRÃO DE ATENDIMENTO GUARIDA") ? "PADRÕES DE ATENDIMENTO GUARIDA" : item;
      item = item.Equals("VISÃO DO NEGÓCIO") ? "VISÃO DE NEGÓCIO" : item;
      item = item.Equals("SISTEMA DE GESTÃO EMPRESARIAL") ? "SOFTWARE DE GESTÃO EMPRESARIAL" : item;
      item = item.Equals("SOFTWARE DE GESTÃO EMPRESARIAL SA") ? "SOFTWARE DE GESTÃO EMPRESARIAL" : item;
      item = item.Equals("DINAMISMO / AGILIDADE") ? "DINAMISMO" : item;
      item = item.Equals("INDICADORES E DEMONSTRATIVOS DE RESULTADOS") ? "INDICADORES E DEMONSTRATIVOS GERENCIAIS" : item;
      item = item.Equals("IMOBILIAR") ? "SISTEMA IMOBILIAR" : item;
      item = item.Equals("BUSINESS INTELLIGENCE") ? "SOFTWARE DE BI" : item;
      item = item.Equals("TECNICAS DE NEGOCIAÇÃO/VENDAS") ? "TECNICAS DE NEGOCIAÇÃO" : item;
      item = item.Equals("ÁREAS DE CONHECIMENTO ESCOLAS / CURSO") ? "ÁREAS DE CONHECIMENTO DA SUA ESCOLA / CURSO" : item;
      item = item.Equals("INGLES BÁSICO") ? "INGLÊS BÁSICO" : item;
      item = item.Equals("INGLES INTERMEDIÁRIO") ? "INGLÊS INTERMEDIÁRIO" : item;
      item = item.Equals("INGLÊS INTERMEDIÀRIO") ? "INGLÊS INTERMEDIÁRIO" : item;
      item = item.Equals("INGLES TÉCNICO") ? "INGLÊS TÉCNICO" : item;
      item = item.Equals("REDE") ? "REDES" : item;
      item = item.Equals("FERRAMENTA DE GESTÃO DE PESSOAS") ? "FERRAMENTAS DE GESTÃO DE PESSOAS" : item;
      item = item.Equals("") ? "" : item;
      return item;
    }
    private string DictionarySchooling(string item)
    {
      item = item.Replace((char)10, ' ').Replace((char)13, ' ').Trim().ToUpper();
      item = item.Replace("  ", " ");
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
      // PUCRS
      item = item.Equals("ESPECIALIZAÇÃO COMPLETO") ? "PÓS GRADUAÇÃO COMPLETA" : item;
      item = item.Equals("ESPECIALIZAÇÃO COMPLETA") ? "PÓS GRADUAÇÃO COMPLETA" : item;
      item = item.Equals("ESPECIALIZAÇÃO") ? "PÓS GRADUAÇÃO COMPLETA" : item;
      item = item.Equals("GRADUAÇÃO") ? "ENSINO SUPERIOR COMPLETO" : item;
      item = item.Equals("ENSINO FUNDAMENTAL") ? "ENSINO FUNDAMENTAL COMPLETO" : item;
      item = item.Equals("ENSINO TÉCNICO") ? "TÉCNICO COMPLETO" : item;
      item = item.Equals("ENSINO SUPERIOR") ? "ENSINO SUPERIOR COMPLETO" : item;
      item = item.Equals("MESTRADO") ? "MESTRADO COMPLETO" : item;
      item = item.Equals("ENSINO MÉDIO") ? "ENSINO MÉDIO COMPLETO" : item;
      item = item.Equals("ENSINO SUPERIOR IMCOMPLETO") ? "ENSINO SUPERIOR INCOMPLETO" : item;
      item = item.Equals("MESTRADO E/OU DOUTORADO.") ? "PÓS GRADUAÇÃO COMPLETA" : item;
      item = item.Equals("ENSINO SUPERIOR COMPLETO/ DOUTORADO") ? "PÓS GRADUAÇÃO COMPLETA" : item;
      return item;
    }
    private string DictionaryGroup(string item)
    {
      item = item.Replace((char)10, ' ').Replace((char)13, ' ').Trim();
      item = item.Replace("  ", " ");
      item = item.Replace("Assessor  Tec / Adm", "Especialista | Técnico Administrativo");
      item = item.Replace("Assessor Tec/Adm", "Especialista | Técnico Administrativo");
      item = item.Replace("Assessor Professor", "Especialista | Professor");
      item = item.Replace("Assessor de Reitoria", "Assessor Reitoria");
      return item;
    }
    #endregion

    #region Excel
    private void ImportFileExcel(string file, IntegrationApi infraIntegration)
    {
      lblFile.Text = file;
      Refresh();
      string cellName = "C5";
      // Utilizado para identificar a quebra de cargo por área (exemplo Bertolini)
      // Gerente Comercial - BSA
      //    Nome na celula C5: Gerente Comercial,
      //    Complemento na célula K5: - BSA, exatamente igual ao campo Description, inclusive com os espaços na frente
      // Se não existir complemento deixar em branco ""
      string cellNameComplement = "K5";
      string cellGroup = "H5";
      string cellColumnCheck = "A";
      // Responsabilidade
      int responsibilityCellLine = 9;
      string responsibilityCellColumn = "A";
      string responsibilityTextCheck = "ENTREGAS DO CARGO";
      // Técnicas
      string softSkillTextCheck = "COMPETÊNCIAS COMPORTAMENTAIS";
      string hardSkillTextCheck = "COMPETÊNCIAS ESPECÍFICAS";
      string hardSkillCellColumn = "B";
      // Formação
      string formationColumnCheck = "C";
      string formationTextCheck = "ESCOLARIDADE";
      string formationCellColumn = "C";
      string formationCellColumnComplement = "E";
      // Requisitos
      string requirementTextCheck = "PRÉ REQUESITOS PARA CONTRATAÇÃO";
      string requirement;
      // Linha de controle de leitura
      int line = 0;
      string work;
      // Contabilização da importação
      ViewIntegrationProfileOccupation viewOccupation = new ViewIntegrationProfileOccupation
      {
        Messages = new List<string>(),
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
        _id = string.Empty,
        Area = string.Empty,
        Process = string.Empty,
        SubProcess = string.Empty
      };
      if (chkSub.Checked)
      {
        if (file.Replace(txtPst.Text,string.Empty).Split('\\').Count() != 4)
        {
          throw new Exception("Estrutura de pastas não está no formato AREA/PROCESSO/SUBPROCESSO/MAPA.XLSX");
        }
        viewOccupation.Area = file.Replace(txtPst.Text, string.Empty).Split('\\')[0].Replace(" = ", " | ");
        viewOccupation.Process = file.Replace(txtPst.Text, string.Empty).Split('\\')[1].Replace(" = ", " | ");
        viewOccupation.SubProcess = file.Replace(txtPst.Text, string.Empty).Split('\\')[2].Replace(" = ", " | ");
      }
      else
      {
        viewOccupation.Area = "Integração com Folha de Pagamento";
        viewOccupation.Process = "Integração";
        viewOccupation.SubProcess = "Integração";
      }
      try
      {
        excelPst = excelApp.Workbooks.Open(file, false);
        try
        {
          excelPln = excelPst.Worksheets["Mapa Competências "];
        }
        catch (Exception)
        {
          try
          {
            excelPln = excelPst.Worksheets["Mapa Competências"];
          }
          catch (Exception)
          {
            excelPln = excelPst.Worksheets[1];
          }
        }
        excelPln.Activate();
        viewOccupation.Name = excelPln.Range[cellName].Value.ToString().Trim();
        if (!string.IsNullOrEmpty(cellNameComplement))
        {
          viewOccupation.Description = string.Format(" - {0}", excelPln.Range[cellNameComplement].Value ?? string.Empty);
          if (viewOccupation.Description.Equals(" - "))
          {
            viewOccupation.Description = string.Empty;
          }
        }
        viewOccupation.NameGroup = DictionaryGroup(excelPln.Range[cellGroup].Value.ToString());

        if (!(excelPln.Range[string.Format("{0}{1}", responsibilityCellColumn, responsibilityCellLine)].Value).ToString().Trim().ToUpper().Equals(responsibilityTextCheck.ToUpper()))
        {
          throw new Exception("Não encontrei a primeira linha das entregas");
        }
        // Tratamento das entregas
        line = responsibilityCellLine + 1;
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(softSkillTextCheck))
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
        line++;
        // Encontrar a linha inicial das competências específicas
        while (true)
        {
          work = CellValue(cellColumnCheck, line).ToUpper();
          if (work.Equals(hardSkillTextCheck) || work.Equals("COMPETÊNCIAS ESPECÍFICAS",StringComparison.InvariantCultureIgnoreCase)
            || work.Equals("SKILLS ESPECÍFICAS", StringComparison.InvariantCultureIgnoreCase))
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
        while (true)
        {
          work = CellValue(formationColumnCheck, line).ToUpper();
          if (work.Equals(formationTextCheck))
          {
            break;
          }
          work = CellValue(hardSkillCellColumn, line);
          if (!string.IsNullOrEmpty(work) && !work.Equals("NOME DA COMPETÊNCIA") && !work.Equals("COMPETÊNCIA"))
          {
            viewOccupation.Skills.Add(DictionarySkill(work));
          }
          line++;
        }
        // Encontrar a formação
        while (true)
        {
          work = CellValue(formationColumnCheck, line).ToUpper();
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
          if (!string.IsNullOrEmpty(work) && !work.Equals("ESCOLARIDADE"))
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
          break;
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
        if (viewOccupation.Messages.Count > 0)
        {
          if (viewOccupation.Messages[0].IndexOf("competência") == -1 && viewOccupation.Messages[0].IndexOf("escolaridade") == -1)
          {
            occupation.Status = viewOccupation.Messages[0];
          }
        }
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
          string key = string.Empty;
          if (itemAux.Count() == 2)
          {
            key = itemAux[1];
          }
          if (key.IndexOf("competência") != -1)
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
          if (key.IndexOf("escolaridade") != -1)
          {
            schoolings.Add(new SchoolingStatistic()
            {
              Name = itemAux[0],
              Register = false,
              Found = false,
              Profile = false
            });
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
