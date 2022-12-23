namespace AnalisadorLexico.Exceptions;

public class TokenNaoEncontradoException : Exception
{
    public TokenNaoEncontradoException(string message) : base(message)
    {
    }
}
