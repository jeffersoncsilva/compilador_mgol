namespace AnalisadorLexico;

public enum Tipo
{
    INTEIRO, REAL, LITERAL, PALAVRA_RES, NULO,
    ERROR
}

public static class TipoExtensions
{
    public static string ToString(this Tipo t)
    {
        switch (t)
        {
            case Tipo.INTEIRO:
                return "int";
            case Tipo.REAL:
                return "double";
            case Tipo.LITERAL:
                return "literal";
            case Tipo.PALAVRA_RES:
                return "pl";
            case Tipo.ERROR:
                return "error";
            default:
                return "nulo";
        }
    }
}
