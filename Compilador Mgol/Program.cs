using AnalisadorLexico;

Lexico lx = new("D://Projetos/mgol2.txt");
while (!lx.EndOfFile)
{
    var tk = lx.Scanner();
    Console.WriteLine(tk.ToString());
    Console.ReadLine();
}

