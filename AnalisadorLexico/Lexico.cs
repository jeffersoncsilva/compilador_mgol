using AnalisadorLexico.Exceptions;
using System.Text;

namespace AnalisadorLexico
{
    public class Lexico
    {
        private readonly string PATH_FONTE;
        private StreamReader reader;
        private char[] linhaAtual;
        private int idxCaractereAtual = 0;
        private AutomatoAnalisadorLexico afd;
        private TabelaDeSimbolos tabelaDeSimbolos;

        public bool EndOfFile { get; private set; }

        public Lexico(string filePath)
        {
            PATH_FONTE= filePath;
            InicializaLeituraArquivo();
            afd = new();
            tabelaDeSimbolos = new();
        }

        public Token Scanner()
        {
            StringBuilder caracteresLidos = new StringBuilder();
            char caracterAtual;
            do
            {                
                caracterAtual = LeProximoCaractere();
                if (caracterAtual == '\0' || char.IsWhiteSpace(caracterAtual))
                {
                    idxCaractereAtual++;
                    continue;
                }

                if (afd.CaractereValido(caracterAtual))
                {
                    caracteresLidos.Append(caracterAtual);
                    idxCaractereAtual++;
                }
                else
                {
                    Token tk = CriarToken(caracteresLidos.ToString());
                    afd.ReiniciaEstado();
                    tabelaDeSimbolos.InsereSimbolo(tk);
                    return tk;
                }
            } while (!EndOfFile);
            return new("ERROR");
        }

        private Token CriarToken(string caracteres)
        {           
            switch (afd.EstadoAtual)
            {
                case 1:
                case 2:
                case 3:
                    return new Token("NUM", caracteres, "inteiro");
                case 7:
                    return new Token("id", caracteres, "NULO");
                case 8:
                    return new Token("AB_P", caracteres, "NULO");
                case 9:
                    return new Token("FC_P", caracteres, "NULO");
            }

            //if (afd.EstadoAtual == 1 || afd.EstadoAtual == 3 || afd.EstadoAtual == 6)
            //{
                
            //}
            //else if(afd.EstadoAtual == 7)
            //{
            //    return new Token("id", caracteres, "NULO");
            //}else if(afd.EstadoAtual == 8)
            //{

            //    return new Token("AB_P", caracteres, "NULO");
            //}
            return new("ERROR", caracteres, "NULO");
        }

        private char LeProximoCaractere()
        {
            //if(linhaAtual == null)
            //    linhaAtual = LeProximaLinha();

            //if (idxCaractereAtual == linhaAtual.Length) 
            //{ 
            //    idxCaractereAtual++;
            //    return '\0';
            //}

            //if(idxCaractereAtual >= linhaAtual.Length)
            //{
            //    idxCaractereAtual = 0;
            //    linhaAtual = LeProximaLinha();
            //}
            if (LinhaEstaVazia())
            {
                linhaAtual = LeProximaLinha();
                idxCaractereAtual = 0;
            }         
            return linhaAtual[idxCaractereAtual];
        }

        private bool LinhaEstaVazia()
        {
            if (linhaAtual == null)
                return true;
            if(idxCaractereAtual >= linhaAtual.Length) 
                return true;
            return false;
        }

        private char[] LeProximaLinha()
        {
            var line = reader.ReadLine();
            //if(line == null)
            //{
            //    EndOfFile = true;
            //    return new char[1] {'\n'};
            //}
            if(line == null)
            {
                EndOfFile = (line == null);
                return new char[1] {null};
            }
            if (line.Length == 0)
            {
                return new char[1] { '\n' };
            }
            return line.ToCharArray();
        }

        private void InicializaLeituraArquivo()
        {
            if (!File.Exists(PATH_FONTE))
                throw new ArquivoFonteNaoEncontradoException($"Arquivo no caminho: {PATH_FONTE} não foi encontrado.");

            var stream = new FileStream(PATH_FONTE, FileMode.Open);
            this.reader = new StreamReader(stream);
        }
    }
}