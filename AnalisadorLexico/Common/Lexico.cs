using CompiladorMGol.A_Lexico;
using CompiladorMGol.Exceptions;
using System.Reflection;
using System.Text;

namespace CompiladorMgol.Common
{
    public class Lexico
    {
        private const string EOF = "/EOF";
        
        private StreamReader reader;
        private AutomatoAnalisadorLexico afd;
        private TabelaDeSimbolos tabelaDeSimbolos;

        public bool EndOfFile { get; private set; }

        private string[] linha;
        private int numero_linha = 0;
        private string palavra_atual;
        private int idxPalavraParaLer = 0;
        private int idxCaractererPalavraAtual = 0;

        public Lexico()
        {
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
                return new Token("$");
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
                return afd.EstadoAtual == 11 || afd.EstadoAtual == 20;
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
                    return new Token("num", lexema, "inteiro");
                case 7:
                    return new Token(Classe.ID, lexema, Tipo.NULO);
                case 8:
                    return new Token("ab_p", lexema, "NULO");
                case 9:
                    return new Token("fc_p", lexema, "NULO");
                case 12:
                    afd.ReiniciaEstado();
                    return Scanner();
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    return new Token("opr", lexema, "NULO");
                case 19:
                    return new Token("atr", lexema, "NULO");
                case 21:
                    return new Token("lit", lexema, "NULO");
                case 22:
                    return new Token("vir", lexema, "NULO");
                case 23:
                    return new Token("pt_v", lexema, "NULO");
                case 24:
                    return new Token("opa", lexema, "NULO");

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
            var nomeArquivoCompleto = "CompiladorMgol.Recursos.programa_mgol.txt";
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream(nomeArquivoCompleto);
            reader = new StreamReader(resourceStream);
        }

        public void ImprimeTabelaDeSimbolos()
        {
            Console.WriteLine("------------Tabela de Simbolos--------");
            Console.WriteLine(tabelaDeSimbolos.ToString());
            Console.WriteLine("---------------------------------------");
        }
    }
}