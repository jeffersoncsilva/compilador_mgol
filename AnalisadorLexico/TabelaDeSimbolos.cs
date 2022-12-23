using AnalisadorLexico.Exceptions;
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
        try
        {
            var tk = BuscaSimbolo(token);
            tokens.Remove(tk);
            tokens.Add(token);
        }catch(TokenNaoEncontradoException ex)
        {
            throw ex;
        }
    }

    public Token BuscaSimbolo(Token tk)
    {
        var token = tokens.Where(t => t.Lexema == tk.Lexema).FirstOrDefault();
        if (token.Lexema == null)
            throw new TokenNaoEncontradoException("O token buscado não está na tabela de simbolos.");
        return token;
    }

    private void AdicionaPalavrasReservadasNaTabela()
    {
        tokens.Add(new Token(Classe.PL_RESERVADA, "inicio", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "varinicio", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "varfim", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "escreva", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "leia", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "se", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "entao", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "fimse", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "fim", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "inteiro", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "literal", Tipo.PALAVRA_RES));
        tokens.Add(new Token(Classe.PL_RESERVADA, "real", Tipo.PALAVRA_RES));
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
