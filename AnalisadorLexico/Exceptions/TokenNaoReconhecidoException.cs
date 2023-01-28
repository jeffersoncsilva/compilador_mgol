namespace CompiladorMgol.Exceptions;

public class TokenNaoReconhecidoException : Exception
{
    public TokenNaoReconhecidoException() : base("Token não foi reconhecido.")
    {
    }
}
