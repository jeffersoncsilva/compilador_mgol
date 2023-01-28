using System.Reflection;
using System.Text;
using CompiladorMgol.Common;

namespace CompiladorMGol.Utilidades;

internal class LexicoResumido
{
    List<Token> tokens;

    int ultimo = 0;

    public LexicoResumido()
    {
        tokens = new List<Token>();
        LeTokens();
    }

    private void LeTokens()
    {        
        using (var reader = ObtemArquivoResumido())
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var conteudo = line.Split(' ');
                foreach (var l in conteudo)
                {
                  //  tokens.Add(new Token(l));
                }
                //tokens.Add(new Token("$"));
            }
        }
    }

    private StreamReader ObtemArquivoResumido()
    {
        var nomeCompletoResource = $"CompiladorMgol.Recursos.programa_resumido.txt";
        var assembly = Assembly.GetExecutingAssembly();
        var resourceStream = assembly.GetManifestResourceStream(nomeCompletoResource);
        return new StreamReader(resourceStream);
    }

    //public Token ProximoToken()
    //{
    //    return (tokens.Count > ultimo) ? tokens[ultimo++] : new Token("$");
    //}

    public bool Acabou() => ultimo == tokens.Count;

    public override string ToString()
    {
        StringBuilder sb = new();
        foreach (var token in tokens)
        {
            sb.Append(token.ToString());
            sb.Append('\n');
        }
        return sb.ToString();
    }

}