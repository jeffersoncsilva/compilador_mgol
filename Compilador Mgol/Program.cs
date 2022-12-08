using AnalisadorLexico;

//Lexico lx = new("D://Projetos/mgol2.txt");
Lexico lx = new("D://Projetos/main_mgol.txt");
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
    Console.ForegroundColor= ConsoleColor.Red;
    Console.Write(tk.Tipo);
    Console.ReadLine();
}