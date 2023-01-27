using CompiladorMGol.Utilidades;
using CompiladorMgol.Common;
using System.Runtime.CompilerServices;
using CompiladorMgol.B_Sintatico;
using System.Net;

namespace CompiladorMGol.B_Sintatico;

struct Producao
{
    string lado_esquerdo;
    string lado_direito;
}

public class Sintatico
{
    private TabelaSintatica tb;
    private Lexico lexico;
    private GerenciadorDoAlfabeto alfabeto;
    private Stack<int> pilha;
    private bool entradaNaoFoiAceita = true;
    private bool erroNaoFoiEncontrado = true;
    private Token tokenAtual;

    public Sintatico()
    {
        tb = new TabelaSintatica();
        lexico = new Lexico();
        alfabeto = new GerenciadorDoAlfabeto();
        pilha = new Stack<int>();
    }

    public void IniciaAnalise()
    {
        pilha.Push(0);
        tokenAtual = lexico.Scanner();
        while (entradaNaoFoiAceita && erroNaoFoiEncontrado)
        {
            int estadoAtual_linha = pilha.Peek();
            var tokenEntrada_coluna = tokenAtual.Classe;
            var acao = tb.PegaAcao(estadoAtual_linha, tokenEntrada_coluna);
            if (EParaEmpilhar(acao))
            {
                RealizaAcaoDeShift(acao);
            }
            else if (EParaReduzir(acao))
            {
                RealizaAcaoDeReducao(acao);
            }
            else if (AceitarEntrada(acao))
            {
                AceitaEntrada();
            }
            else
            {
                IniciaRotinaRecuperacaoDeErro(acao, estadoAtual_linha);
            }
        }
    }

    private void RealizaAcaoDeShift(string acao)
    {        
        int.TryParse(ObtemEstadoDaAcao(acao), out int proximo_estado);
        pilha.Push(proximo_estado);
        tokenAtual = lexico.Scanner();
    }

    private void RealizaAcaoDeReducao(string acao)
    {
        /*
        1 - qual ação de reduzir?
        2 - quantos simbolos do alfabeto de entrada devem ser desempilhados?
        3 - remove os simbolos da pilha
        4 - atualizar estado atual (pega o elemento do topo da pilha)
        5 - empilhar o não terminal correspondente a ação de redução
        6 - olhar na tabela o desvio para o estado correspondente ao não terminal
        6.1 - verifica se o simbolo retornado e um estado valido.
        6.2 - se não for estado valido, lança erro.
        7 - empilhar o desvio na pilha
        */
        RegraAlfabeto acao_reducao = alfabeto.ObtemRegraDeReducao(acao);// 1
        ImprimeAcaoReducao(acao_reducao);

        DesempilhaSimbolos(); //3

        var t = pilha.Peek(); // 4
        var nova_acao = tb.PegaAcao(t, acao_reducao.LadoEsquerdo.Trim());// 6
        if (int.TryParse(nova_acao, out int novo_estado))
            pilha.Push(novo_estado);
        else
            throw new ArgumentException("Operação invalida detectada. Operação: " + nova_acao);
        void DesempilhaSimbolos()
        {
            var quantidade_simbolos_desempilhar = acao_reducao.LadoDireito.Length; // 2

            for (int i = 0; i < quantidade_simbolos_desempilhar; i++)
                pilha.Pop();
        }
    }

    private void ImprimeAcaoReducao(RegraAlfabeto acao_reducao)
    {
        //Console.ForegroundColor = ConsoleColor.Blue;
        //ImprimePilha();
        //Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(acao_reducao);
    }

    private void AceitaEntrada()
    {
        Console.WriteLine("Entrada foi aceita.");
        entradaNaoFoiAceita = false;
    }

    private string ObtemEstadoDaAcao(string acao)
    {
        // Retorna apenas a parte númerica de s3 ou r2.
        return acao.Trim().Remove(0, 1);
    }

    private void IniciaRotinaRecuperacaoDeErro(string acao, int estadoAtual)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        switch (acao.Trim())
        {
            case "E0":
                TrataErroEstado0();
                break;
            case "E1":
                TrataErroEstado1();
                break;
            case "E2":
                TrataErroEstado2();
                break;
            case "E12":
                TrataErroEstado12();
                break;
            default:
                ExecutaModoPanico();
                break;
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    private bool EParaEmpilhar(string posicao)
    {
        var letra = posicao.Trim().ToCharArray()[0];
        return 's'.Equals(letra);
    }

    private bool EParaReduzir(string posicao)
    {
        var letra = posicao.Trim().ToCharArray()[0];
        return 'r'.Equals(letra);
    }

    private bool AceitarEntrada(string entrada)
    {
        return "acc".Equals(entrada.Trim().ToLower()) || "ac".Equals(entrada.Trim().ToLower());
    }

    private void ImprimePilha()
    {
        Console.Write("Pilha: ");
        foreach(var p in pilha)
        {
            Console.Write($"{p} ");
        }
        Console.WriteLine();
    }
     
    private void TrataErroEstado0()
    {
        if ("varinicio".Equals(tokenAtual.Classe))
        {
            pilha.Push(2);
            Console.WriteLine("Erro: token esperado: inicio - token encontrado: " + tokenAtual.Classe);
        }
        else
        {
            // avança ate encontrar o token varfim 
            while (!"varfim".Equals(tokenAtual.Classe) &&  !"fim".Equals(tokenAtual.Classe) && !"$".Equals(tokenAtual.Classe))
                tokenAtual = lexico.Scanner();
            if(tokenAtual.Classe == "varfim")
            {
                // 0 - 2 - 4 - 17
                pilha.Push(2);
                pilha.Push(4);
            }
            else if(tokenAtual.Classe == "fim")
            {
                pilha.Push(2);
                pilha.Push(3);
            }
            Console.WriteLine("Erro: token não esperado encontrado: " + tokenAtual.Classe);
            //Console.WriteLine("O processo será interrompido.");
            //erroNaoFoiEncontrado = false;
        }
    }

    private void TrataErroEstado1()
    {
        Console.WriteLine("Token não esperado encontrado. " + tokenAtual.ToString());
        erroNaoFoiEncontrado = false;
    }

    private void TrataErroEstado2()
    {
        if("varfim".Equals(tokenAtual.Classe) ||
            "inteiro".Equals(tokenAtual.Classe) ||
            "real".Equals(tokenAtual.Classe) ||
            "literal".Equals(tokenAtual.Classe))
        {
            pilha.Push(4);
            Console.WriteLine("Token - varinicio - não encontrado.");
        }
    }
    
    private void TrataErroEstado12()
    {
        if ("opr".Equals(tokenAtual.Classe.Trim()))
        {
            Console.WriteLine("Identificador não reconhecido.");
            pilha.Push(30);
        }
        else if ("num".Equals(tokenAtual.Classe.Trim()) || "id".Equals(tokenAtual.Classe.Trim()))
        {
            Console.WriteLine("Identificador de atribuição faltando.");
            pilha.Push(30);
        }
        else
        {
            ExecutaModoPanico();
        }
    }
    private void ExecutaModoPanico()
    {
        Console.WriteLine("Token não esperado encontrado.");
        bool estadoValido = false;
        while (!estadoValido)
        {
            var estadoAtual = pilha.Peek();
            tokenAtual = lexico.Scanner();
            var tokenEntrada_coluna = tokenAtual.Classe;
            var acao = tb.PegaAcao(estadoAtual, tokenEntrada_coluna);
            estadoValido = AcaoEValida(acao);
        }

        bool AcaoEValida(string acao)
        {
            return acao.Contains("r") || acao.Contains("s");
        }
    }
   
}
