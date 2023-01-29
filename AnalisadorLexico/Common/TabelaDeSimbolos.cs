using AnalisadorLexico.Exceptions;
using CompiladorMgol.Common;
using System.Text;

namespace AnalisadorLexico;

public class TabelaDeSimbolos
{
    private List<Token> tokens;

    public TabelaDeSimbolos()
    {
        tokens = new List<Token>();
        AdicionaPalavrasReservadasNaTabela();
    }

    public void InsereSimbolo(Token tk) => tokens.Add(tk);        
    
    public void AtualizaSimbolo(Token token)
    {
    //    try
    //    {
    //        var tk = BuscaSimbolo(token);
    //        tokens.Remove(tk);
    //        tokens.Add(token);
    //    }catch(TokenNaoEncontradoException ex)
    //    {
    //        throw ex;
    //    }
    }

    public Token? BuscaSimbolo(Token tk)
    {
        var token = tokens.Where(t => t.Lexema == tk.Lexema).FirstOrDefault();
        if (token.Lexema == null)
            return null;
        return token;
    }

    private void AdicionaPalavrasReservadasNaTabela()
    {
        tokens.Add(new Token(Classe.inicio, "inicio", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.varinicio, "varinicio", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.varfim, "varfim", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.escreva, "escreva", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.leia, "leia", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.se, "se", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.entao, "entao", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.fimse, "fimse", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.fim, "fim", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.inteiro, "inteiro", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.literal, "literal", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.real, "real", Tipo.PALAVRA_RES));
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach(Token t in tokens)
        {
            sb.Append(t.ToString());
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
