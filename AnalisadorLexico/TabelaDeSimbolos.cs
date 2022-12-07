using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorLexico
{
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

        }

        public Token? BuscaSimbolo()
        {
            return null;
        }

        private void AdicionaPalavrasReservadasNaTabela()
        {
            tokens.Add(new Token("inicio"));
            tokens.Add(new Token("varinicio"));
            tokens.Add(new Token("varfim"));
            tokens.Add(new Token("escreva"));
            tokens.Add(new Token("leia"));
            tokens.Add(new Token("se"));
            tokens.Add(new Token("entao"));
            tokens.Add(new Token("fimse"));
            tokens.Add(new Token("fim"));
            tokens.Add(new Token("inteiro"));
            tokens.Add(new Token("literal"));
            tokens.Add(new Token("real"));
        }
    }
}
