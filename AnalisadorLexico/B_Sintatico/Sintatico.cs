using CompiladorMgol.B_Sintatico;
using CompiladorMgol.A_Lexico;
using CompiladorMgol.Common;
using AnalisadorLexico;
using CompiladorMgol.C_Semantico;

namespace CompiladorMGol.B_Sintatico;

public class Sintatico
{
    private TabelaSintatica tb;
    private Lexico lexico;
    private AnalisadorSemantico semantico;
    private GerenciadorDoAlfabeto alfabeto;
    private Logging logSintatico;
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
        logSintatico = new();
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
                IniciaRotinaRecuperacaoDeErro(acao, estadoAtual_linha);
            }
        }
        semantico.FinalisaGeracaoArquivo();
    }

    private void RealizaAcaoDeShift(string acao)
    {        
        int.TryParse(ObtemEstadoDaAcao(acao), out int proximo_estado);
        pilha.Push(proximo_estado);
        pilhaSemantica.Push(tokenAtual);
        tokenAtual = lexico.Scanner();
    }

    private void RealizaAcaoDeReducao(string acao)
    {
        RegraAlfabeto acao_reducao = alfabeto.ObtemRegraDeReducao(acao);
        ImprimeAcaoReducao(acao_reducao);
        ImprimeTipoToken();
        DesempilhaSimbolos();
        var t = pilha.Peek(); 
        var nova_acao = tb.PegaAcao(t, acao_reducao.RecuperaPosicaoReducao());
        if (int.TryParse(nova_acao, out int novo_estado))
            pilha.Push(novo_estado);
        else
            throw new ArgumentException("Operação invalida detectada. Operação: " + nova_acao);
        try
        {
            semantico.AplicarRegraSemantica(acao_reducao, tokenAtual, lexico.Linha_sendo_lida, lexico.Coluna_sendo_lida);
        }catch(NotImplementedException ex)
        {
            Console.WriteLine("Regra semantica não implementada. REGRA: " + acao_reducao);
        }
        
        
        void DesempilhaSimbolos()
        {
            var quantidade_simbolos_desempilhar = acao_reducao.LadoDireito.Length; 

            for (int i = 0; i < quantidade_simbolos_desempilhar; i++)
                pilha.Pop();
        }
    }

    private void ImprimeTipoToken()
    {
        //Console.WriteLine("Tipo do token: " + tokenAtual.Tipo);
    }

    private void ImprimeAcaoReducao(RegraAlfabeto acao_reducao)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        //ImprimePilha();
        //Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(acao_reducao.ToString());
        //logSintatico.Log(acao_reducao.ToString());
    }

    private void AceitaEntrada()
    {
        logSintatico.Log("Entrada foi aceita.");
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
            case "E10":
                TrataErroEstado10();
                break;
            case "E12":
                TrataErroEstado12();
                break;
            case "E25":
                TrataErroEstado25();
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
        if (Classe.varinicio == tokenAtual.Classe)
        {
            pilha.Push(2);
            logSintatico.LogErrors($"Erro: token esperado: inicio - token encontrado: {tokenAtual.Classe} - Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
        }
        else
        {
            // avança ate encontrar o token varfim 
            while (Classe.varfim != tokenAtual.Classe && Classe.fim != tokenAtual.Classe && Classe.cifrao != tokenAtual.Classe)
                tokenAtual = lexico.Scanner();
            if (tokenAtual.Classe == Classe.varfim)
            {
                // 0 - 2 - 4 - 17
                pilha.Push(2);
                pilha.Push(4);
            }
            else if (tokenAtual.Classe == Classe.fim)
            {
                pilha.Push(2);
                pilha.Push(3);
            }
            logSintatico.LogErrors($"Erro: token não esperado encontrado: {tokenAtual.Classe} - Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
            //Console.WriteLine("O processo será interrompido.");
            //erroNaoFoiEncontrado = false;
        }
    }

    private void TrataErroEstado1()
    {
        logSintatico.LogErrors($"Token não esperado encontrado. {tokenAtual.ToString()} - Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
        erroNaoFoiEncontrado = false;
    }

    private void TrataErroEstado2()
    {
        if(Classe.varfim == tokenAtual.Classe ||
            Classe.inteiro == tokenAtual.Classe ||
            Classe.real == tokenAtual.Classe ||
            Classe.literal == tokenAtual.Classe)
        {
            pilha.Push(4);
            logSintatico.LogErrors($"Token - varinicio - não encontrado. Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
        }
    }
    
    private void TrataErroEstado10()
    {
        if(Classe.id != tokenAtual.Classe)
        {
            logSintatico.LogErrors($"Identificador esperado não encontrado. Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
            logSintatico.LogErrors("Identificador encontrado: " + tokenAtual.Classe);
            pilha.Push(25);
        }
    }

    private void TrataErroEstado12()
    {
        if (Classe.opr == tokenAtual.Classe)
        {
            logSintatico.LogErrors($"Identificador não reconhecido. Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
            pilha.Push(30);
        }
        else if (Classe.num == tokenAtual.Classe || Classe.id == tokenAtual.Classe)
        {
            logSintatico.LogErrors($"Identificador de atribuição faltando. Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
            pilha.Push(30);
        }
        else
        {
            ExecutaModoPanico();
        }
    }
    
    private void TrataErroEstado25()
    {
        if(Classe.pt_v != tokenAtual.Classe)
        {
            logSintatico.LogErrors($"Identificador ponto e vigula fantando. Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
            logSintatico.LogErrors("Identificador encontrado: " + tokenAtual.Classe);
            pilha.Push(40);
        }
    }

    private void ExecutaModoPanico()
    {
        logSintatico.LogErrors($"Token não esperado encontrado. Linha: {lexico.Linha_sendo_lida} - Coluna: {lexico.Coluna_sendo_lida}");
        bool estadoValido = false;
        string acao = "";
        while (!estadoValido)
        {
            var estadoAtual = pilha.Peek();
            tokenAtual = lexico.Scanner();
            //var tokenEntrada_coluna = tokenAtual.Classe;
            acao = tb.PegaAcao(estadoAtual, ((int)tokenAtual.Classe));
            estadoValido = AcaoEValida(acao);
        }

        if (estadoValido)
            RealizaAcaoDeShift(acao);

        bool AcaoEValida(string acao)
        {
            return acao.Contains("r") || acao.Contains("s");
        }
    }
    
   
}