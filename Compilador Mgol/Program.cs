using CompiladorMgol.Common;
using CompiladorMGol.B_Sintatico;
using System.Runtime.CompilerServices;


//TestaLexico();
TestaSintatico();

static void TestaLexico()
{
    Lexico lx = new();
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
        Console.WriteLine();

    }
}

static void TestaSintatico()
{
    Sintatico ass = new();
    ass.IniciaAnalise();
}