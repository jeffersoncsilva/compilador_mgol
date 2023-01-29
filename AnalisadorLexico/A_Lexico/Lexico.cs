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

        public int Linha_sendo_lida { get; private set; }
        public int Coluna_sendo_lida { get; private set; }
        private char caractereAtual;
        private bool jaConsumiuCaractereAtual = true;

        public Lexico()
        {
            InicializaLeituraArquivo();
            Linha_sendo_lida = 1;
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
                if (afd.CaractereValido(caractereAtual, ref erro))
                {
                    jaConsumiuCaractereAtual = true;
                    if ('\t' == caractereAtual || '\n' == caractereAtual || '\r' == caractereAtual)
                        continue;
                    caracteresLidos.Append(caractereAtual);
                    continue;
                }
                if (erro || '\\'==caractereAtual)
                {
                    if (!CaracteresEspeciais.EhCaractereEspecial(caractereAtual))
                    {
                        ImprimeMensagemErroLexico(caractereAtual);
                    }
                    if(caracteresLidos.Length != 0)
                    {
                        jaConsumiuCaractereAtual = true;
                        return RetornaTokenCriado(caracteresLidos.ToString());
                    }
                    
                }
                if (caracteresLidos.Length > 0)
                {
                    jaConsumiuCaractereAtual = false;
                    return RetornaTokenCriado(caracteresLidos.ToString());
                }
            }
        }

        private void ImprimeMensagemErroLexico(char caracter)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Caracter não esperado encontrado: " + caracter + " - Linha: " + Linha_sendo_lida + " - Coluna: " + Coluna_sendo_lida);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ImprimeMensagemErroLexico(string caracter)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            switch (afd.EstadoAtual)
            {
                case 2:
                    Console.WriteLine($"Caracterer mal formatado encontrado. Caractere: {caracter} - Linha: {Linha_sendo_lida} - Coluna: {Coluna_sendo_lida}");
                    break;
                case 12:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Comentario identificado e ignorado. Comentario: " + caracter);
                    break;
                case 11:
                    Console.WriteLine($"Caracter faltante encontrado: caracterer: '}}' - Linha: {Linha_sendo_lida} - Coluna: {Coluna_sendo_lida}");
                    break;
                case 20:
                    Console.WriteLine($"Caracter faltante encontrado: caracterer: '\"' - Linha: {Linha_sendo_lida} - Coluna: {Coluna_sendo_lida}");
                    break;
                default:
                    Console.WriteLine($"Caracter não esperado encontrado: {caracter} - Linha: {Linha_sendo_lida} - Coluna: {Coluna_sendo_lida}");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private char LeProximoCaractere()
        {
            var caracter = (char)reader.Read();
            Coluna_sendo_lida++;
            if('\r' == caracter)
            {
                Coluna_sendo_lida = 0;
                Linha_sendo_lida++;
            }
            return caracter;
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
                    return new Token(Classe.num, lexema, Tipo.INTEIRO);
                case 7:
                    return new Token(Classe.id, lexema, Tipo.NULO);
                case 8:
                    return new Token(Classe.ab_p, lexema, Tipo.NULO);
                case 9:
                    return new Token(Classe.fc_p, lexema, Tipo.NULO);
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    return new Token(Classe.opr, lexema, Tipo.NULO);
                case 19:
                    return new Token(Classe.atr, lexema, Tipo.NULO);
                case 21:
                    return new Token(Classe.lit, lexema, Tipo.NULO);
                case 22:
                    return new Token(Classe.vir, lexema, Tipo.NULO);
                case 23:
                    return new Token(Classe.pt_v, lexema, Tipo.NULO);
                case 24:
                    return new Token(Classe.opa, lexema, Tipo.NULO);
            }
            if (lexema.Equals(EOF))
            {
                return new Token(Classe.cifrao, "$", Tipo.NULO);   
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