using AnalisadorLexico;
using AnalisadorLexico.Exceptions;

Lexico lx;// = new("D://Projetos/mgol2.txt");

try
{
    lx = new("D://Projetos/mgol2.txt");
    //lx = new("D://Projetos/main_mgol.txt");
    ImprimeTokens(lx);
    ImprimeTabelaSimbolos(lx);
}catch(ArquivoFonteNaoEncontradoException ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadLine();
    Environment.Exit(0);
}

void ImprimeTabelaSimbolos(Lexico lx)
{
    lx.ImprimeTabelaDeSimbolos();
}

static void ImprimeTokens(Lexico lx)
{
    while (!lx.EndOfFile)
    {
        var tk = lx.Scanner();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Classe: ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(tk.Classe);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" LEXEMA: ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(tk.Lexema);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" TIPO: ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(tk.Tipo);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        //Console.ReadLine();
    }
}