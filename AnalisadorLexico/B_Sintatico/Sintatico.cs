using CompiladorMgol.B_Sintatico;
using CompiladorMgol.A_Lexico;
using CompiladorMgol.Common;
using AnalisadorLexico;
using CompiladorMgol.C_Semantico;
using Microsoft.VisualBasic;

namespace CompiladorMGol.B_Sintatico;

public class Sintatico
{
    private TabelaSintatica tb;
    private Lexico lexico;
    private AnalisadorSemantico semantico;
    private GerenciadorDoAlfabeto alfabeto;
    private Stack<int> pilha;
    private bool entradaNaoFoiAceita = true;
    private bool erroNaoFoiEncontrado = true;
    private Token tokenAtual;
    private Stack<Token> pilhaSemantica;

    public Sintatico()
    {
        tb = new TabelaSintatica();
        lexico = new Lexico();
        alfabeto = new GerenciadorDoAlfabeto();
        pilhaSemantica = new();
        semantico = new AnalisadorSemantico(lexico.TabelaDeSimbolos, pilhaSemantica);
        pilha = new Stack<int>();
    }

    public void IniciaAnalise()
    {
        pilha.Push(0);
        tokenAtual = lexico.Scanner();
        while (entradaNaoFoiAceita && erroNaoFoiEncontrado)
        {
            int estadoAtual_linha = pilha.Peek();
            //var tokenEntrada_coluna = tokenAtual.Classe;
            var acao = tb.PegaAcao(estadoAtual_linha, ((int)tokenAtual.Classe));
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
                Console.ForegroundColor = ConsoleColor.Red;
                GeradorDeCodigoFinal.HasError = true;
                IniciaRotinaRecuperacaoDeErro(acao, estadoAtual_linha);
                Console.ResetColor();
            }
        }
        if (erroNaoFoiEncontrado)
            semantico.FinalisaGeracaoArquivo();
    }

    private void RealizaAcaoDeShift(string acao)
    {
        var proximoEstado = ObtemEstadoDaAcao(acao);
        int proximo_estado = int.Parse(proximoEstado);
        pilha.Push(proximo_estado);
        pilhaSemantica.Push(tokenAtual);
        tokenAtual = lexico.Scanner();
    }

    private void RealizaAcaoDeReducao(string acao)
    {
        RegraAlfabeto acao_reducao = alfabeto.ObtemRegraDeReducao(acao);
        ImprimeAcaoReducao(acao_reducao);
        DesempilhaSimbolos();
        var t = pilha.Peek();
        var nova_acao = tb.PegaAcao(t, acao_reducao.RecuperaPosicaoReducao());
        int novo_estado = int.Parse(nova_acao);
        pilha.Push(novo_estado);
        semantico.AplicarRegraSemantica(acao_reducao, tokenAtual, lexico.Linha_sendo_lida, lexico.Coluna_sendo_lida);

        void DesempilhaSimbolos()
        {
            var quantidade_simbolos_desempilhar = acao_reducao.LadoDireito.Length;
            for (int i = 0; i < quantidade_simbolos_desempilhar; i++)
                pilha.Pop();
        }
    }

    private void ImprimeAcaoReducao(RegraAlfabeto acao_reducao)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(acao_reducao.ToString());
        Console.ResetColor();
    }

    private void AceitaEntrada()
    {
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
            case "E2":
                TrataErroEstado2();
                break;
            case "E10":
                TrataErroEstado10();
                break;
            case "E12":
                TrataErroEstado12();
                break;
            case "E25":
                TrataErroEstado25();
                break;
            case "E42":
                TrataErro42();
                break;
            default:
                PanicMode(estadoAtual);
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
        foreach (var p in pilha)
        {
            Console.Write($"{p} ");
        }
        Console.WriteLine();
    }

    private void GeraMensagemErroSintatico(string? tokenEsperado, string tokenAtual)
    {
        if(tokenEsperado != null)
        Console.WriteLine($"Erro: token não esperado encontrado. Token encontrado: {tokenAtual} - token esperado: {tokenEsperado} - Linha: {lexico.Linha_sendo_lida-1} - Coluna: {lexico.Coluna_sendo_lida}");
        else
            Console.WriteLine($"Erro: token não esperado encontrado. Token encontrado: {tokenAtual} - Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
    }

    private void TrataErroEstado0()
    {
        switch (tokenAtual.Classe)
        {
            case Classe.varinicio:
                pilha.Push(2);
                pilhaSemantica.Push(new Token(Classe.varinicio, "inicio", Tipo.NULO));
                GeraMensagemErroSintatico("varinicio", tokenAtual.Classe.ToString());
                break;
            case Classe.TIPO:
                pilha.Push(2);
                pilhaSemantica.Push(new Token(Classe.varinicio, "inicio", Tipo.NULO));
                pilha.Push(4);
                pilhaSemantica.Push(new Token(Classe.varfim, "varfim", Tipo.NULO));
                GeraMensagemErroSintatico("inicio", tokenAtual.Classe.ToString());
                break;
            default:
                PanicMode();
                break;

        }
    }

    private void TrataErroEstado2()
    {
        if (Classe.varfim == tokenAtual.Classe ||
            Classe.inteiro == tokenAtual.Classe ||
            Classe.real == tokenAtual.Classe ||
            Classe.literal == tokenAtual.Classe)
        {
            pilha.Push(4);
            GeraMensagemErroSintatico("varinicio", tokenAtual.Classe.ToString());
        }
        else
            PanicMode();
    }

    private void TrataErroEstado10()
    {
        if (Classe.id != tokenAtual.Classe)
        {
            GeraMensagemErroSintatico("id", tokenAtual.Classe.ToString());
            pilha.Push(25);
        }
        else
            PanicMode();
    }

    private void TrataErroEstado12()
    {
        if (Classe.opr == tokenAtual.Classe || Classe.num == tokenAtual.Classe || Classe.id == tokenAtual.Classe)
        {
            GeraMensagemErroSintatico("=", tokenAtual.Classe.ToString());
            pilha.Push(30);
        }
        else
        {
            PanicMode();
        }
    }

    private void TrataErroEstado25()
    {
        if (Classe.pt_v != tokenAtual.Classe)
        {
            GeraMensagemErroSintatico(";", tokenAtual.Classe.ToString());
            pilha.Push(40);
            
            
        }
        else
            PanicMode();
    }

    private void TrataErro42()
    {
        if (Classe.pt_v != tokenAtual.Classe)
        {
            GeraMensagemErroSintatico(";", tokenAtual.Classe.ToString());
            pilha.Push(42);
            pilhaSemantica.Push(new Token(Classe.pt_v, "pt_v", Tipo.PALAVRA_RES));
        }
        else
            PanicMode();
    }

    private void PanicMode(int estadoAtual = -1)
    {
        if (estadoAtual == -1)
            estadoAtual = pilha.Peek();
        var msg = ObtemMensagemDeErro(estadoAtual);
        GeraMensagemErroSintatico(msg, tokenAtual.Classe.ToString());

        var tokensSincorinizacao = ObtemTokensSincronizacao(estadoAtual);

        while (!tokensSincorinizacao.Contains(tokenAtual.Classe))
        {
            tokenAtual = lexico.Scanner();
            pilhaSemantica.Push(tokenAtual);
        }
        
        var acao = tb.PegaAcao(estadoAtual, ((int)tokenAtual.Classe));
        if(AcaoEValida(acao))
            RealizaAcaoDeShift(acao);


        //bool estadoValido = false;
        //string acao = "";
        //while (!estadoValido)
        //{
        //    estadoAtual = pilha.Peek();
        //    tokenAtual = lexico.Scanner();
        //    if (tokenAtual.Classe != Classe.pt_v)
        //        continue;
        //    acao = tb.PegaAcao(estadoAtual, ((int)tokenAtual.Classe));
        //    estadoValido = AcaoEValida(acao);
        //}

        //if (estadoValido)
        //    RealizaAcaoDeShift(acao);

        bool AcaoEValida(string acao)
        {
            return acao.Contains("r") || acao.Contains("s");
        }
    }

    private string ObtemMensagemDeErro(int estadoAtual)
    {
        string msg = "";
        switch (estadoAtual)
        {
            case 2:
                msg = "varinicio";
                break;
            case 42:
                msg = ";";
                break;
            case 58:
                msg = "identificador ou constante";
                break;
        }
        return msg;
    }

    private List<Classe> ObtemTokensSincronizacao(int estadoAtual)
    {
        List<Classe> tokens = new();

        switch (estadoAtual)
        {
            case 2:
                tokens.Add(Classe.varfim);                
                break;
            case 42:
                tokens.Add(Classe.pt_v);
                break;
            case 58:
                tokens.Add(Classe.pt_v);
                //tokens.Add(Classe.id);
                //tokens.Add(Classe.num);
                //tokens.Add(Classe.OPRD);
                break;
        }

        return tokens;
    }
}