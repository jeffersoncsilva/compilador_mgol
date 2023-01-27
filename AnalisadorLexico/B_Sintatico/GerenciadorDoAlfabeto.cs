
using System.Reflection;
using System.Text;

namespace CompiladorMgol.B_Sintatico;

public class GerenciadorDoAlfabeto
{
    private List<RegraAlfabeto> regras;

    public GerenciadorDoAlfabeto()
    {
        regras = new();
        CarregaAlfabetoReduzido();
    }

    private void CarregaAlfabetoReduzido()
    {
        using (var stream = ObtemStreamingAlfabetoReduzido())
        {
            int id = 1;
            while (!stream.EndOfStream)
            {
                var linha = stream.ReadLine();
                var valores = linha?.Split("->");
                RegraAlfabeto ra = new();
                ra.LadoEsquerdo = valores[0];
                ra.LadoDireito = valores[1].Trim().Split(' ');
                ra.Identificador = $"r{id++}";
                regras.Add(ra);
            }
        }
    }

    private StreamReader ObtemStreamingAlfabetoReduzido()
    {
        //var nomeCompletoResource = $"CompiladorMgol.Recursos.AlfabetoReduzido.txt";
        var nomeCompletoResource = $"CompiladorMgol.Recursos.AlfabetoCompleto.txt";
        var assembly = Assembly.GetExecutingAssembly();
        var resourceStream = assembly.GetManifestResourceStream(nomeCompletoResource);
        return new StreamReader(resourceStream);
    }

    public RegraAlfabeto ObtemRegraDeReducao(string acao)
    {
        foreach(var r in regras)
        {
            if (r.Identificador.Equals(acao.Trim()))
                return r;
        }
        
        throw new ArgumentNullException("Regra não encontrada. Regra: " + acao);        
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach(var ra in regras)
        {
            sb.Append(ra.ToString());
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
