
namespace CompiladorMGol.Exceptions
{
    public class ArquivoFonteNaoEncontradoException : Exception
    {
        private const string MSG_DEFAULT = "Arquivo fonte não foi encontrado no caminho indicado.";

        public ArquivoFonteNaoEncontradoException(string message = MSG_DEFAULT) : base(message)
        {

        }
    }
}
