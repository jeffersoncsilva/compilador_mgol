using AnalisadorLexico;
using AnalisadorLexico.Exceptions;
using CompiladorMgol.Common;
using CompiladorMgol.Exceptions;
using CompiladorMGol.A_Lexico;
using System.Reflection;
using System.Text;

namespace CompiladorMgol.A_Lexico
{
    public class Lexico
    {
        private const string EOF = "/EOF";

        private StreamReader reader;
        private AutomatoAnalisadorLexico afd;
        private TabelaDeSimbolos tabelaDeSimbolos;

        public bool EndOfFile { get; private set; }

        //private string[] linha;
        //private int numero_linha = 0;
        //private string palavra_atual;
        //private int idxPalavraParaLer = 0;
        //private int idxCaractererPalavraAtual = 0;

        private int linha_sendo_lida;
        private int coluna_sendo_lida;
        private char caractereAtual;
        private bool jaConsumiuCaractereAtual = true;

        public Lexico()
        {
            InicializaLeituraArquivo();
            //linha_que_esta_sendo_lida = reader.ReadLine();
            linha_sendo_lida = 1;
            //coluna_sendo_lida = 0;
            afd = new();
            tabelaDeSimbolos = new();
        }

        public Token Scanner()
        {
            StringBuilder caracteresLidos = new StringBuilder();
            while (true)
            {
                if (reader.EndOfStream)
                {
                    if (caracteresLidos.Length > 0)
                        return RetornaTokenCriado(caracteresLidos.ToString());
                    EndOfFile = true;
                    return RetornaTokenCriado(EOF);
                }
                
                if(jaConsumiuCaractereAtual)
                    caractereAtual = LeProximoCaractere();
                
                bool erro = false;
                
                if (EspacoEmBrancoOuTabulacao(caractereAtual) && afd.EstadoAtual != 20 && afd.EstadoAtual != 11)
                {
                    jaConsumiuCaractereAtual = true;
                    if (caracteresLidos.Length > 0)
                        return RetornaTokenCriado(caracteresLidos.ToString());
                    continue;
                }
                
                if (ECarriegeReturn(caractereAtual))
                {
                    jaConsumiuCaractereAtual = true;
                    continue;
                }
                
                if (EQuebraDeLinha(caractereAtual))
                {
                    jaConsumiuCaractereAtual = true;
                    linha_sendo_lida++;
                    coluna_sendo_lida = 0;
                    if (caracteresLidos.Length > 0)
                        return RetornaTokenCriado(caracteresLidos.ToString());
                    continue;
                }
                
                if (afd.CaractereValido(caractereAtual, ref erro))
                {
                    caracteresLidos.Append(caractereAtual);
                    jaConsumiuCaractereAtual = true;
                    continue;
                }
                else if (erro)
                {
                    if (!CaracteresEspeciais.EhCaractereEspecial(caractereAtual))
                    {
                        ImprimeMensagemErroLexico(caractereAtual);
                    }
                }
                if (caracteresLidos.Length == 0 && CaracteresEspeciais.EhCaractereEspecial(caractereAtual))
                {
                    jaConsumiuCaractereAtual = true;
                    return RetornaTokenCriado($"{caractereAtual}");
                }
                if (caracteresLidos.Length > 0)
                {
                    jaConsumiuCaractereAtual = false;
                    return RetornaTokenCriado(caracteresLidos.ToString());
                }
            }
        }
        
        private bool ECarriegeReturn(char c) => '\r'.Equals(c);
       
        private bool EQuebraDeLinha(char caracter) => '\n'.Equals(caracter);

        private bool EspacoEmBrancoOuTabulacao(char caracter) => ' '.Equals(caracter) || '\t'.Equals(caracter);

        private void ImprimeMensagemErroLexico(char caracter)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Caracter não esperado encontrado: " + caracter + " - Linha: " + linha_sendo_lida + " - Coluna: " + coluna_sendo_lida);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ImprimeMensagemErroLexico(string caracter)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            switch (afd.EstadoAtual)
            {
                case 2:
                    Console.WriteLine($"Caracterer mal formatado encontrado. Caractere: {caracter} - Linha: {linha_sendo_lida} - Coluna: {coluna_sendo_lida}");
                    break;
                case 12:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Comentario identificado e ignorado. Comentario: " + caracter);
                    break;
                default:
                    Console.WriteLine($"Caracter não esperado encontrado: {caracter} - Linha: {linha_sendo_lida} - Coluna: {coluna_sendo_lida}");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private char LeProximoCaractere()
        {
            var caracter = (char)reader.Read();
            coluna_sendo_lida++;
            return caracter;
        }

        private void FinalizaLeituraDoArquivoFonte()
        {
            //reader.Dispose();
            Console.WriteLine("Fim de leitura de aquivo fonte...");
        }

        private bool PalavraAtualEVaziaOuEspacoEmBranco(string palavra)
        {
            return palavra.Length == 0;
        }
        
        private Token RetornaTokenCriado(string lexema)
        {
            try
            {
                var tk = CriarToken(lexema);
                afd.ReiniciaEstado();
                var tk2 = tabelaDeSimbolos.BuscaSimbolo(tk);
                if (tk2 == null)
                {
                    tabelaDeSimbolos.InsereSimbolo(tk);
                    return tk;
                }
                return (Token)tk2;
            }
            catch (TokenNaoReconhecidoException ex)
            {
                //Console.WriteLine($"Erro: {ex.Message} - linha {linha_sendo_lida} coluna {coluna_sendo_lida}");
                ImprimeMensagemErroLexico(lexema);
                afd.ReiniciaEstado();
                return Scanner();
            }
            //finally
            //{
            //    afd.ReiniciaEstado();
            //}
        }
        
        private Token CriarToken(string lexema)
        {
            switch (afd.EstadoAtual)
            {
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
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    return new Token(Classe.OPR, lexema, Tipo.NULO);
                case 19:
                    return new Token(Classe.ATR, lexema, Tipo.NULO);
                case 21:
                    return new Token(Classe.LIT, lexema, Tipo.NULO);
                case 22:
                    return new Token(Classe.VIR, lexema, Tipo.NULO);
                case 23:
                    return new Token(Classe.PT_V, lexema, Tipo.NULO);
                case 24:
                    return new Token(Classe.OPA, lexema, Tipo.NULO);
            }
            if (lexema.Equals(EOF))
            {
                return new Token(Classe.PL_RESERVADA, "$", Tipo.NULO);   
            }
            throw new TokenNaoReconhecidoException();
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