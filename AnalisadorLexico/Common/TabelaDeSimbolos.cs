using CompiladorMgol.Common;
using System.Text;

namespace AnalisadorLexico;

public class TabelaDeSimbolos
{
    private IDictionary<string, Token> tokens;

    public TabelaDeSimbolos()
    {
        tokens = new Dictionary<string, Token>();
        AdicionaPalavrasReservadasNaTabela();
    }

    public void InsereSimbolo(Token tk) => tokens.Add(tk.Lexema, tk);

    public void AtualizaSimbolo(Token token)
    {
        if (tokens.ContainsKey(token.Lexema))
        {
            tokens[token.Lexema] = token;
        }
    }

    public Token? BuscaSimbolo(Token tk)
    {
        if (tokens.ContainsKey(tk.Lexema))
        {
            return tokens[tk.Lexema];
        }
        return null;
    }

    private void AdicionaPalavrasReservadasNaTabela()
    {
        tokens.Add("inicio", new Token(Classe.inicio, "inicio", Tipo.PALAVRA_RES));
        tokens.Add("varinicio", new Token(Classe.varinicio, "varinicio", Tipo.PALAVRA_RES));
        tokens.Add("varfim", new Token(Classe.varfim, "varfim", Tipo.PALAVRA_RES));
        tokens.Add("escreva", new Token(Classe.escreva, "escreva", Tipo.PALAVRA_RES));
        tokens.Add("leia", new Token(Classe.leia, "leia", Tipo.PALAVRA_RES));
        tokens.Add("se", new Token(Classe.se, "se", Tipo.PALAVRA_RES));
        tokens.Add("entao", new Token(Classe.entao, "entao", Tipo.PALAVRA_RES));
        tokens.Add("fimse", new Token(Classe.fimse, "fimse", Tipo.PALAVRA_RES));
        tokens.Add("fim", new Token(Classe.fim, "fim", Tipo.PALAVRA_RES));
        tokens.Add("inteiro", new Token(Classe.inteiro, "inteiro", Tipo.PALAVRA_RES));
        tokens.Add("literal", new Token(Classe.literal, "literal", Tipo.PALAVRA_RES));
        tokens.Add("real", new Token(Classe.real, "real", Tipo.PALAVRA_RES));
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var t in tokens)
        {
            sb.Append(t.Value.ToString());
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
