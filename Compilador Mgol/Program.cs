using CompiladorMgol.A_Lexico;
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
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(tk.Classe.ToString().PadLeft(15));
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" LEXEMA: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(tk.Lexema.ToString().PadLeft(10));
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" TIPO: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(tk.Tipo.ToString().PadLeft(10));
        Console.WriteLine();
    }
}

static void TestaSintatico()
{
    Sintatico ass = new();
    ass.IniciaAnalise();
}