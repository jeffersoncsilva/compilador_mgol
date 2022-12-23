﻿using AnalisadorLexico.Exceptions;
using System.Text;

namespace AnalisadorLexico
{
    public class Lexico
    {
        private const string EOF = "/EOF";
        private readonly string PATH_FONTE;
        private StreamReader reader;
        private AutomatoAnalisadorLexico afd;
        private TabelaDeSimbolos tabelaDeSimbolos;

        public bool EndOfFile { get; private set; }

        private string[] linha;
        private int numero_linha = 0;
        private string palavra_atual;
        private int idxPalavraParaLer = 0;
        private int idxCaractererPalavraAtual = 0;

        public Lexico(string filePath)
        {
            PATH_FONTE = filePath;
            InicializaLeituraArquivo();
            afd = new();
            tabelaDeSimbolos = new();
        }

        public Token Scanner()
        {
            StringBuilder caracteresLidos = new StringBuilder();
            if (JaLeuTodaLinha() && JaLiTodaPalavraAtual())
            {
                linha = LeProximaLinha();
            }
            if (JaLiTodaPalavraAtual())
                palavra_atual = LeProximaPalavra();
            if (ChegouAoFimDoArquivo())
            {
                EndOfFile = true;
                return new Token(Classe.NULO, EOF, Tipo.NULO);
            }

            while (idxCaractererPalavraAtual < palavra_atual.Length)
            {
                char caractere = palavra_atual[idxCaractererPalavraAtual];
                if (afd.CaractereValido(caractere))
                {
                    caracteresLidos.Append(caractere);
                    idxCaractererPalavraAtual++;
                }
                else
                {
                    if (caracteresLidos.ToString().Length == 0)
                    {
                        idxCaractererPalavraAtual++;
                        return RetornaTokenCriado($"{caractere}");
                    }
                    return RetornaTokenCriado(caracteresLidos.ToString());
                }
                if (idxCaractererPalavraAtual == palavra_atual.Length && EstaLendoLiteralOuComentario())
                {
                    palavra_atual = LeProximaPalavra();
                    caracteresLidos.Append(" ");
                    idxCaractererPalavraAtual = 0;
                }
            }
            return RetornaTokenCriado(caracteresLidos.ToString());

            bool EstaLendoLiteralOuComentario()
            {
                return (afd.EstadoAtual == 11 || afd.EstadoAtual == 20);
            }
        }

        private bool PalavraAtualEVaziaOuEspacoEmBranco(string palavra)
        {
            return palavra.Length == 0;
        }

        private string LeProximaPalavra()
        {
            if (idxPalavraParaLer >= linha.Length)
                linha = LeProximaLinha();
            idxCaractererPalavraAtual = 0;
            string word = linha[idxPalavraParaLer++];
            if (PalavraAtualEVaziaOuEspacoEmBranco(word))
                word = LeProximaPalavra();
            return word;
        }

        private bool ChegouAoFimDoArquivo()
        {
            return EOF.Equals(linha[0]);
        }
        
        private Token RetornaTokenCriado(string lexema)
        {
            Token tk2 = CriarToken(lexema);
            afd.ReiniciaEstado();
            try
            {
                Token tb_tk = tabelaDeSimbolos.BuscaSimbolo(tk2);
                return tb_tk;
            }catch(TokenNaoEncontradoException ex)
            {
                if(tk2.Classe == Classe.ID)
                    tabelaDeSimbolos.InsereSimbolo(tk2);
            }
            return tk2;
        }

        private bool JaLiTodaPalavraAtual()
        {
            if (palavra_atual == null)
                return true;
            if (idxCaractererPalavraAtual == palavra_atual.Length)
                return true;
            if (string.Empty.Equals(palavra_atual))
                return true;
            return false;
        }

        private bool JaLeuTodaLinha()
        {
            if (linha == null)
                return true;

            return idxPalavraParaLer == linha.Length;
        }

        private Token CriarToken(string lexema)
        {
            switch (afd.EstadoAtual)
            {
                case 2:
                    return ErroEntradaInvalida(lexema);
                case 1:
                case 3:
                case 6:
                    return new Token(Classe.NUM, lexema, Tipo.INTEIRO);
                case 7:
                    return new Token(Classe.ID, lexema, Tipo.NULO);
                case 8:
                    return new Token(Classe.AB_P, lexema, Tipo.NULO);
                case 9:
                    return new Token(Classe.FC_P, lexema, Tipo.NULO);
                case 12:
                    afd.ReiniciaEstado();
                    return Scanner();
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                    return new Token(Classe.OPR, lexema, Tipo.NULO);
                case 21:
                    return new Token(Classe.LIT, lexema, Tipo.NULO);
                case 22:
                    return new Token(Classe.VIR, lexema, Tipo.NULO);
                case 23:
                    return new Token(Classe.PT_V, lexema, Tipo.NULO);
                case 24:
                    return new Token(Classe.OPR, lexema, Tipo.NULO);
                
                default:
                    return MensagemDeErroPadrao(lexema);
            }
        }

        private Token MensagemDeErroPadrao(string lexema)
        {
            string str = $"{lexema} - Caractere não reconhecido, linha {numero_linha} coluna {idxCaractererPalavraAtual+1}";
            return new Token(Classe.ERRO, str, Tipo.ERROR);
        }

        private Token ErroEntradaInvalida(string lexema)
        {
            string str = $"{lexema} - caracteres mal formados, linha {numero_linha} coluna {idxCaractererPalavraAtual+1}";
            var tk = new Token(Classe.ERRO, str, Tipo.ERROR);          
            return tk;
        }

        private string[] LeProximaLinha()
        {
            var line = reader.ReadLine();
            idxPalavraParaLer = 0;
            numero_linha++;
            if (line == null)
            {
                EndOfFile = true;
                return new string[1] { "/EOF" };
            }
            return line.Split(' ');
        }

        private void InicializaLeituraArquivo()
        {
            if (!File.Exists(PATH_FONTE))
                throw new ArquivoFonteNaoEncontradoException($"Arquivo no caminho: {PATH_FONTE} não foi encontrado.");

            var stream = new FileStream(PATH_FONTE, FileMode.Open);
            this.reader = new StreamReader(stream);
        }

        public void ImprimeTabelaDeSimbolos()
        {
            Console.WriteLine("------------Tabela de Simbolos--------");
            Console.WriteLine(tabelaDeSimbolos.ToString());
            Console.WriteLine("---------------------------------------");
        }
    }
}