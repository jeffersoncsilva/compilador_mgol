
using CompiladorMgol.Common;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace CompiladorMgol.C_Semantico;

public class GeradorDeCodigoFinal
{
    public static bool HasError = false;
    private StreamWriter writer;
    private Logging log_erros;
    private Logging log_codigo;
    private Logging log_variaveis;
    private Logging log_temporarias;
    private Logging log_cabecalho;

    public GeradorDeCodigoFinal()
    {
        ObterArquivoEscritaFinal();
        log_erros = new();
        log_codigo = new();
        log_variaveis = new();
        log_cabecalho = new();
        log_temporarias = new();
    }

    private void ObterArquivoEscritaFinal()
    {
        var path = "C://Mgol";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        var filePath = Path.Combine(path, "codigo_final.txt");

        if (File.Exists(filePath))
            File.Delete(filePath);

        writer = new StreamWriter(File.Create(filePath));


    }

    public void ImprimeRegra5()
    {
        //log_variaveis.Log("\n");
        //log_variaveis.Log("\n");
        //log_variaveis.Log("\n");
    }

    public void ImprimeVariavel(string vari)
    {
        log_variaveis.LogMeioDaLinha(vari);
    }

    public void ImprimeVariavelNoMeio(string variaveis)
    {
        log_variaveis.LogMeioDaLinha(variaveis);
    }

    public void ImprimeArquivoFinal(string msg) => log_codigo.Log(msg);

    public void QuebraDeLinha() => log_codigo.LogQuebraDeLinha();

    public void QuebraDeLinhaVariavel() => log_variaveis.LogQuebraDeLinha();

    public void ImprimeVariaveisTemporarias(string t)
    {
        //log_variaveis.Log("\\****************************\\");
        log_temporarias.Log(t);
        log_temporarias.LogQuebraDeLinha();
    }

    public void ImprimeErroVariavelNaoDeclarada(int linha, int coluna)
    {
        log_erros.LogErrors($"ERRO: Variável não declarada. Linha: {linha} - Coluna: {coluna}");
    }

    public void ImprimeErroTiposDiferentesAtribuicao(int linha, int coluna)
    {
        log_erros.LogErrors($"ERRO: tipos diferentes para atribuição: Linha: {linha} - Coluna: {coluna}");
    }

    public void ImprimeErro(string erro_msg)
    {
        log_erros.LogErrors(erro_msg);
    }

    public void GerarCodigoFinal()
    {
        ImprimeCabecalho();
        ImprimeVariaveisTemporarias();
        ImprimeCorpoCodigo();
        ImprimeSaidaConsole();
        writer.Flush();
        writer.Close();
    }

    private void ImprimeCabecalho()
    {
        log_cabecalho.Log("#include <stdio.h>\n");
        log_cabecalho.LogQuebraDeLinha();
        log_cabecalho.Log("typedef char literal[256];\n");
        log_cabecalho.LogQuebraDeLinha();
        log_cabecalho.Log("void main(void) {\n");
        log_cabecalho.LogQuebraDeLinha();
        if (!HasError)
            writer.WriteLine(log_cabecalho.ToString());
        
    }

    private void ImprimeVariaveisTemporarias()
    {
            log_temporarias.Log("/****************************/ \n");
            log_temporarias.LogQuebraDeLinha();
        if (!HasError)
        {
            writer.WriteLine(log_temporarias.ToString());
            writer.WriteLine(log_variaveis.ToString());
            
        }
    }

    private void ImprimeCorpoCodigo()
    {
            log_codigo.Log("\n}");
            log_codigo.LogQuebraDeLinha();
        if (!HasError)
        {
            writer.Write(log_codigo.ToString());
        }
    }

    private void ImprimeSaidaConsole()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("-------------ERROS: ----");
        Console.WriteLine(log_erros.Linha);
        Console.ResetColor();

        Console.WriteLine("----->>>>> Código gerado: ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(log_cabecalho.ToString());
        Console.WriteLine(log_temporarias.ToString());
        Console.WriteLine(log_variaveis.ToString());
        Console.WriteLine(log_codigo.ToString());
    }
}
