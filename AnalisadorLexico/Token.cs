namespace AnalisadorLexico;

public struct Token
{
    public Classe Classe { get; set; }
    public Tipo Tipo { get; set; }
    public string Lexema { get; set; }

    public Token(Classe classe, string lexema, Tipo tipo)
    {
        Classe = classe;
        Lexema = lexema;
        Tipo = tipo;
    }
    
    public override string ToString() => $"Classe: {Classe} - Lexema: {Lexema} - Tipo: {Tipo}";
}
