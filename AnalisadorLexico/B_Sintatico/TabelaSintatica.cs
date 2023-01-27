using CompiladorMGol.Exceptions;
using System.Reflection;
using System.Text;

namespace CompiladorMGol.B_Sintatico;

public class TabelaSintatica
{
    /*
        0,id, *, +, $, E, T,F
        1,s4, 0, 0, 0, 1, 2,3
        2, 0, 0,s5,ac, 0, 0,0
        3, 0,s6,r3, 0, 0, 0,0
        4, 0,r5,r5,r5, 0, 0,0
        5, 0,r6,r6,r6, 0, 0,0
        6,s4, 0, 0, 0, 0, 7,3
        7,s4, 0, 0, 0, 0, 0,8
        8, 0,s6,r2,r2, 0, 0,0
        9, 0,r4,r4,r4, 0, 0,0 
     */

    private readonly string[,] table;
    private Dictionary<string, int> primeira_linha_tabela = new();

    public TabelaSintatica()
    {
        table = LeTabelaDoArquivoCsv();
        //if (table["0", "id"] == "0")
    }

    private string[,] LeTabelaDoArquivoCsv()
    {
        List<string[]> linhas = new List<string[]>();
        using (var reader = ObtemStreamLeituraTabelaSintatica())
        {
            var firstLine = reader.ReadLine();
            TratarPrimeiraLinha(firstLine);
            var linha = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line?.Split(',');
                for (int k = 0; k < values?.Length; k++)
                {
                    if (string.IsNullOrEmpty(values[k]))
                    {
                        values[k] = $"E{linha}".PadLeft(4);
                        //values[k] = "".PadLeft(4);
                    }else
                        values[k] = values[k].PadLeft(4, ' ');
                }
                linha++;
                if (values != null)
                    linhas.Add(values);
            }
        }
        int l = linhas.Count;
        int c = linhas[0].Length;
        var tab = new string[l, c];
        int i = 0;
        foreach (var linha in linhas)
        {
            for (int k = 0; k < c; k++)
            {
                tab[i, k] = linha[k];
            }
            i++;
        }

        return tab;
    }

    private StreamReader ObtemStreamLeituraTabelaSintatica()
    {
        //var nomeCompletoResource = $"CompiladorMgol.Recursos.sintatico_resumido.csv";
        var nomeCompletoResource = $"CompiladorMgol.Recursos.tabela_mgol_2.csv";
        var assembly = Assembly.GetExecutingAssembly();
        var resourceStream = assembly.GetManifestResourceStream(nomeCompletoResource);
        return new StreamReader(resourceStream);
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        int linhas = table.GetLength(0);
        int colunas = table.GetLength(1);
        for (int i = 0; i < linhas; i++)
        {
            for (int j = 0; j < colunas; j++)
                sb.Append(table[i, j] + " ");
            sb.Append("\n");
        }
        sb.Append("\n");

        return sb.ToString();
    }

    public string PegaAcao(int linha, string coluna)
    {
        //if (!int.TryParse(linha, out int line))
        //    return "";

        int col = primeira_linha_tabela[coluna];
        return table[linha, col];
        //if ("0".Equals(posicao.Trim()))
        //    return Acao.Erro;
        //if (EParaReduzir(posicao))
        //    return Acao.Reduzir;
        //if (EParaEmpilhar(posicao))
        //    return Acao.Empilhar;

        //return Acao.Erro;

    }

    private void TratarPrimeiraLinha(string? firstLine)
    {
        if (firstLine == null)
        {
            throw new TabelaSintaticaNaoEncontradaException("A tabela sintática não foi carregada corretamente.");
        }

        var lineVector = firstLine.Split(',');
        
        for (int k = 0; k < lineVector.Length; k++)
        {
            primeira_linha_tabela.Add(lineVector[k].Trim(), k);
        }
    }
}
