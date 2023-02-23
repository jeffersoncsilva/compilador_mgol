using AnalisadorLexico;
using CompiladorMgol.B_Sintatico;
using CompiladorMgol.Common;
using System.Net.Http.Headers;
using System.Xml;

namespace CompiladorMgol.C_Semantico;

public class AnalisadorSemantico
{
    private TabelaDeSimbolos tabelaDeSimbolos;
    private GeradorDeCodigoFinal geradorCodigoFinal;
    private Stack<Token> pilhaSemantica;
    private int idx_variavel_temporaria = 0;

    public AnalisadorSemantico(TabelaDeSimbolos tabelaDeSimbolos, Stack<Token> pilhaSemantica)
    {
        this.tabelaDeSimbolos = tabelaDeSimbolos;
        this.pilhaSemantica = pilhaSemantica;
        geradorCodigoFinal = new();
    }

    public void AplicarRegraSemantica(RegraAlfabeto ra, Token tokenAtual, int linha, int coluna)
    {
        switch (ra.LadoEsquerdo.Trim())
        {
            case "P":
                AplicarRegra2();
                break;
            case "V":
                AplicarRegra3();
                break;
            case "LV":
                AplicaRegra4Ou5(ra);
                break;
            case "D":
                AplicaRegra6();
                break;
            case "L":
                AplicaRegra7Ou8(ra, tokenAtual);
                break;
            case "TIPO":
                AplicaRegra9Ou10Ou11(ra, tokenAtual);
                break;
            case "A":
                AplicaRegra12Ou18Ou24Ou32(ra);
                break;
            case "ES":
                AplicaRegra13Ou14(ra, linha, coluna);
                break;
            case "ARG":
                AplicaRegra15Ou16Ou17(ra, linha, coluna);
                break;
            case "CMD":
                AplciaRegra19(linha, coluna);
                break;
            case "LD":
                AplicaRegra20Ou21(ra, linha, coluna);
                break;
            case "OPRD":
                AplicaRegra22Ou23(ra, linha, coluna);
                break;
            case "COND":
                AplicaRegra25();
                break;
            case "CAB":
                AplicaRegra26();
                break;
            case "EXP_R":
                AplicaRegra27(linha, coluna);
                break;
            case "CP":
                AplicaRegra28Ou29Ou30Ou31(ra);
                break;
            default:
                MostraErroSemantico(linha, coluna);
                break;

        }
    }

    public void FinalisaGeracaoArquivo()
    {
        geradorCodigoFinal.GerarCodigoFinal();
    }

    private void AplicaRegra28Ou29Ou30Ou31(RegraAlfabeto ra)
    {
        bool eRegra28 = ra.LadoDireito.Contains("ES CP");
        bool eRegra29 = ra.LadoDireito.Contains("CMD CP");
        bool eRegra30 = ra.LadoDireito.Contains("COND CP");
        bool eRegra31 = ra.LadoDireito.Contains("fimse");
        if (eRegra28 || eRegra29 || eRegra30)
        {
            var tk1 = pilhaSemantica.Pop();
            var tk2 = pilhaSemantica.Pop();
            var cp2 = new Token(Classe.CP, "CP", tk2.Tipo);
            pilhaSemantica.Push(cp2);
        }
        else if (eRegra31)
        {
            var cp2 = new Token(Classe.CP, "CP", Tipo.NULO);
            pilhaSemantica.Push(cp2);
        }
    }

    private void AplicaRegra27(int linha, int coluna)
    {
        var oprd = pilhaSemantica.Pop();
        var opr = pilhaSemantica.Pop();
        var oprd2 = pilhaSemantica.Pop();
        var tx = $"t{idx_variavel_temporaria++}";
        geradorCodigoFinal.ImprimeVariaveisTemporarias($"{TipoExtensions.ToString(oprd.Tipo)} {tx};\n");
        var exp_r = new Token(Classe.EXP_R, tx, oprd.Tipo);
        pilhaSemantica.Push(exp_r);

        if (oprd.Tipo == oprd2.Tipo)
        {
            geradorCodigoFinal.ImprimeArquivoFinal($"{tx} = {oprd.Lexema} {opr.Lexema} {oprd2.Lexema};\n");
            geradorCodigoFinal.QuebraDeLinha();
        }
        else
        {
            geradorCodigoFinal.ImprimeErro($"Erro: Operandos com tipos incompatíveis: Linha: {linha} - Coluna: {coluna}");
        }

    }

    private void AplicaRegra26()
    {
        var entao = pilhaSemantica.Pop();
        var fc_p = pilhaSemantica.Pop();
        var ext_r = pilhaSemantica.Pop();
        var ab_p = pilhaSemantica.Pop();
        var se = pilhaSemantica.Pop();
        var cab = new Token(Classe.CAB, "CAB", ext_r.Tipo);
        pilhaSemantica.Push(cab);
        geradorCodigoFinal.ImprimeArquivoFinal($"if ({ext_r.Lexema}){{\n");
    }

    private void AplicaRegra25()
    {
        var cp = pilhaSemantica.Pop();
        var cab = pilhaSemantica.Pop();
        var cond = new Token(Classe.COND, "COND", cab.Tipo);
        geradorCodigoFinal.QuebraDeLinha();
        geradorCodigoFinal.ImprimeArquivoFinal("}\n");
        geradorCodigoFinal.QuebraDeLinha();
    }

    private void AplicaRegra22Ou23(RegraAlfabeto ra, int linha, int coluna)
    {
        bool eRegra22 = ra.LadoDireito.Contains("id");
        if (eRegra22)
        {
            var id = pilhaSemantica.Pop();
            var idt = tabelaDeSimbolos.BuscaSimbolo(id);
            if (idt != null)
            {
                var oprd = new Token(Classe.OPRD, idt.Lexema, idt.Tipo);
                pilhaSemantica.Push(oprd);
            }
            else
            {
                geradorCodigoFinal.ImprimeErroVariavelNaoDeclarada(linha, coluna);
            }
        }
        else
        {
            var num = pilhaSemantica.Pop();
            var oprd = new Token(Classe.OPRD, num.Lexema, num.Tipo);
            pilhaSemantica.Push(oprd);
        }
    }

    private void AplicaRegra20Ou21(RegraAlfabeto ra, int linha, int coluna)
    {
        bool eRegra20 = ra.LadoDireito.Contains("opa");
        if (eRegra20)
        {
            var oprd1 = pilhaSemantica.Pop();
            var opa = pilhaSemantica.Pop();
            var oprd2 = pilhaSemantica.Pop();
            var t = $"t{idx_variavel_temporaria++}";
            geradorCodigoFinal.ImprimeVariaveisTemporarias($"{TipoExtensions.ToString(oprd1.Tipo)} {t};\n");
            var ld = new Token(Classe.LD, t, oprd1.Tipo);
            if (oprd1.Tipo == oprd2.Tipo && oprd1.Tipo != Tipo.LITERAL)
            {
                pilhaSemantica.Push(ld);
                geradorCodigoFinal.ImprimeArquivoFinal($"{t} = {oprd1.Lexema} {opa.Lexema} {oprd2.Lexema};\n");
                geradorCodigoFinal.QuebraDeLinha();
            }
            else
            {
                geradorCodigoFinal.ImprimeErro($"Erro: Operandos com tipos incompatíveis. Linha: {linha} - Coluna: {coluna}");
            }

        }
        else
        {
            var oprd = pilhaSemantica.Pop();
            var ld = new Token(Classe.LD, oprd.Lexema, oprd.Tipo);
            pilhaSemantica.Push(ld);
        }

    }

    private void AplciaRegra19(int linha, int coluna)
    {
        var pt_v = pilhaSemantica.Pop();
        var ld = pilhaSemantica.Pop();
        var rcb = pilhaSemantica.Pop();
        var id = pilhaSemantica.Pop();
        if (id.Tipo != Tipo.NULO)
        {
            if (id.Tipo == ld.Tipo)
            {
                geradorCodigoFinal.ImprimeArquivoFinal($"{id.Lexema} {rcb.Lexema} {ld.Lexema};\n");
                geradorCodigoFinal.QuebraDeLinha();
            }
            else
            {
                geradorCodigoFinal.ImprimeErroTiposDiferentesAtribuicao(linha, coluna);
            }
            var cmd = new Token(Classe.CMD, "CMD", Tipo.NULO);
            pilhaSemantica.Push(cmd);
        }
        else
        {
            geradorCodigoFinal.ImprimeErroVariavelNaoDeclarada(linha, coluna);
        }
    }

    private void AplicaRegra15Ou16Ou17(RegraAlfabeto ra, int linha, int coluna)
    {
        bool eRegra15 = ra.LadoDireito.Contains("lit");
        bool eRegra16 = ra.LadoDireito.Contains("num");
        bool eRegra17 = ra.LadoDireito.Contains("id");

        if (eRegra15)
        {
            var lit = pilhaSemantica.Pop();
            var arg = new Token(Classe.ARG, lit.Lexema, lit.Tipo);
            pilhaSemantica.Push(arg);
        }
        else if (eRegra16)
        {
            var num = pilhaSemantica.Pop();
            var arg = new Token(Classe.ARG, num.Lexema, num.Tipo);
            pilhaSemantica.Push(arg);
        }
        else if (eRegra17)
        {
            var id = pilhaSemantica.Pop();
            if (id.Classe == Classe.id && id.Tipo != Tipo.NULO)
            {
                var arg = new Token(Classe.ARG, id.Lexema, id.Tipo);
                pilhaSemantica.Push(arg);
            }
            else
            {

                geradorCodigoFinal.ImprimeErroVariavelNaoDeclarada(linha, coluna);
            }
        }
    }

    private void AplicaRegra13Ou14(RegraAlfabeto ra, int linha, int coluna)
    {
        bool ERegra13() => ra.LadoDireito.Contains("leia");
        bool ERegra14() => ra.LadoDireito.Contains("escreva");
        if (ERegra13())
        {
            var ptv = pilhaSemantica.Pop();
            var id = pilhaSemantica.Pop();
            var leia = pilhaSemantica.Pop();
            var idt = tabelaDeSimbolos.BuscaSimbolo(id);
            if (idt != null)
            {
                if (idt.Tipo == Tipo.LITERAL)
                {
                    geradorCodigoFinal.ImprimeArquivoFinal($"scanf(\"%s\", {idt.Lexema});\n");
                }
                else if (idt.Tipo == Tipo.INTEIRO)
                {
                    geradorCodigoFinal.ImprimeArquivoFinal($"scanf(\"%d\", &{idt.Lexema});\n");
                }
                else if (idt.Tipo == Tipo.REAL)
                {
                    geradorCodigoFinal.ImprimeArquivoFinal($"scanf(\"%lf\", &{idt.Lexema});\n");
                }
                geradorCodigoFinal.QuebraDeLinha();
            }
            else
            {
                geradorCodigoFinal.ImprimeErroVariavelNaoDeclarada(linha, coluna);
            }
            var es = new Token(Classe.ES, "ES", id.Tipo);
            pilhaSemantica.Push(es);
        }
        else if (ERegra14())
        {
            var pt_v = pilhaSemantica.Pop();
            var arg = pilhaSemantica.Pop();
            var escreva = pilhaSemantica.Pop();
            var es = new Token(Classe.ES, "ES", arg.Tipo);
            pilhaSemantica.Push(es);
            if(arg.Tipo == Tipo.LITERAL)
                geradorCodigoFinal.ImprimeArquivoFinal($"printf(\"%s\", {arg.Lexema});\n");
            else if(arg.Tipo == Tipo.INTEIRO)
                geradorCodigoFinal.ImprimeArquivoFinal($"printf(\"%d\", {arg.Lexema});\n");
            else if(arg.Tipo == Tipo.REAL)
                geradorCodigoFinal.ImprimeArquivoFinal($"printf(\"%f\", {arg.Lexema});\n");
            else
                geradorCodigoFinal.ImprimeArquivoFinal($"printf({arg.Lexema});\n");
            geradorCodigoFinal.QuebraDeLinha();
        }
    }

    private void AplicaRegra12Ou18Ou24Ou32(RegraAlfabeto ra)
    {
        bool eRegra12 = ra.LadoDireito.Contains("ES A");
        bool eRegra18 = ra.LadoDireito.Contains("CMD A");
        bool eRegra24 = ra.LadoDireito.Contains("COND A");
        bool eRegra32 = ra.LadoDireito.Contains("fim");
        if (eRegra12)
        {
            var a = pilhaSemantica.Pop();
            var es = pilhaSemantica.Pop();
            var a2 = new Token(Classe.A, "A", es.Tipo);
            pilhaSemantica.Push(a2);
        }
        else if (eRegra18)
        {
            var a = pilhaSemantica.Pop();
            var cmd = pilhaSemantica.Pop();
            var a2 = new Token(Classe.A, "A", cmd.Tipo);
            pilhaSemantica.Push(a2);
        }
        else if (eRegra24)
        {
            var a = pilhaSemantica.Pop();
            var cmd = pilhaSemantica.Pop();
            var a2 = new Token(Classe.A, "A", cmd.Tipo);
            pilhaSemantica.Push(a2);
        }
        else if (eRegra32)
        {
            var a2 = new Token(Classe.A, "A", Tipo.NULO);
            pilhaSemantica.Pop();
            pilhaSemantica.Push(a2);            
        }
    }

    private void AplicaRegra9Ou10Ou11(RegraAlfabeto ra, Token tk)
    {
        //Token token = (Token)tabelaDeSimbolos.BuscaSimbolo(tk);
        Token? tipo = null;
        if (ERegra9())
        {
            tk.Tipo = Tipo.INTEIRO;
            pilhaSemantica.Pop();
            tipo = new Token(Classe.TIPO, "TIPO", Tipo.INTEIRO);
        }
        else if (ERegra10())
        {
            tk.Tipo = Tipo.REAL;
            pilhaSemantica.Pop();
            tipo = new Token(Classe.TIPO, "TIPO", Tipo.REAL);
        }
        else if (ERegra11())
        {
            tk.Tipo = Tipo.LITERAL;
            pilhaSemantica.Pop();
            tipo = new Token(Classe.TIPO, "TIPO", Tipo.LITERAL);
        }
        geradorCodigoFinal.ImprimeVariavel($"{TipoExtensions.ToString(tipo.Tipo)}  ");        
        tabelaDeSimbolos.AtualizaSimbolo(tk);
        pilhaSemantica.Push(tipo);        

        bool ERegra9()
        {
            return ra.LadoDireito.Contains("inteiro");
        }

        bool ERegra10()
        {
            return ra.LadoDireito.Contains("real");
        }

        bool ERegra11()
        {
            return ra.LadoDireito.Contains("literal");
        }
    }

    private void AplicaRegra7Ou8(RegraAlfabeto ra, Token tka)
    {
        if (ERegra7())
        {
            var l = pilhaSemantica.Pop();
            var vig = pilhaSemantica.Pop();
            var id = pilhaSemantica.Pop();
            id.Tipo = l.Tipo;
            tabelaDeSimbolos.AtualizaSimbolo(id);
            var l2 = new Token(Classe.L, "L", l.Tipo);
            pilhaSemantica.Push(l2);
            geradorCodigoFinal.ImprimeVariavelNoMeio($" {id.Lexema}, ");
        }
        else
        {
            var tipo = pilhaSemantica.Where(p => p.Classe == Classe.TIPO).FirstOrDefault();
            var id = pilhaSemantica.Pop();
            id.Tipo = tipo.Tipo;
            var l = new Token(Classe.L, "L", id.Tipo);
            geradorCodigoFinal.ImprimeVariavel(" "+id.Lexema + ";\n");
            tabelaDeSimbolos.AtualizaSimbolo(id);
            pilhaSemantica.Push(l);
        }

        bool ERegra7()
        {
            return ra.LadoDireito.Contains("vir");
        }
    }

    private void AplicaRegra6()
    {
        var pt_v = pilhaSemantica.Pop();
        var l = pilhaSemantica.Pop();
        var tipo = pilhaSemantica.Pop();
        var d = new Token(Classe.D, "D", tipo.Tipo);
        pilhaSemantica.Push(d);
        geradorCodigoFinal.QuebraDeLinhaVariavel();
    }

    private void AplicaRegra4Ou5(RegraAlfabeto ra)
    {
        bool eRegra4 = ra.LadoDireito.Contains("D LV");
        bool eRegra5 = ra.LadoDireito.Contains("varfim");

        if (eRegra4)
        {
            var lv = pilhaSemantica.Pop();
            var d = pilhaSemantica.Pop();
            var lv2 = new Token(Classe.LV, "LV", lv.Tipo);
        }
        else if (eRegra5)
        {
            var pt_v = pilhaSemantica.Pop();
            var varfim = pilhaSemantica.Pop();
            var lv = new Token(Classe.LV, "LV", varfim.Tipo);
            geradorCodigoFinal.ImprimeRegra5();
        }
    }

    private void AplicarRegra3()
    {
        var lv = pilhaSemantica.Pop();
        var varinicio = pilhaSemantica.Pop();
        var v = new Token(Classe.LV, "LV", lv.Tipo);
        pilhaSemantica.Push(v);
    }

    private void AplicarRegra2()
    {
        var a = pilhaSemantica.Pop();
        var v = pilhaSemantica.Pop();
        var inicio = pilhaSemantica.Pop();
        var p = new Token(Classe.P, "P", a.Tipo);
        pilhaSemantica.Push(p);
    }

    private void MostraErroSemantico(int linha, int coluna)
    {
        Console.WriteLine("Possivel erro semantico na linha {0} e coluna {1}", linha, coluna);
    }

    private void MostraToken(Token t)
    {
        Console.WriteLine($"Tipo.token: {t.Tipo}");
    }
}
