using AnalisadorLexico;
using System.Collections.Generic;
using System;

namespace CompiladorMgol.B_Sintatico;

public class RegraAlfabeto
{
    public string Identificador { get; set; }
    public string LadoEsquerdo { get; set; }
    public string[] LadoDireito { get; set; }


    public override string ToString()
    {
        var direito = LadoDireito.Aggregate((frase, palavra) => frase + " " + palavra);
        return $"{Identificador.PadLeft(3)} - {LadoEsquerdo.PadLeft(5)} --> {direito}";
    }

    public int RecuperaPosicaoReducao()
    {
        switch (LadoEsquerdo.Trim())
        {
            case "literal":
                return (int)Classe.literal;
            case "inicio":
                return (int)Classe.inicio;
            case "fim":
                return (int)Classe.fim;
            case "varinicio":
                return (int)Classe.varinicio;
            case "varfim":
                return (int)Classe.varfim;
            case "inteiro":
                return (int)Classe.inteiro;
            case "real":
                return (int)Classe.real;
            case "leia":
                return (int)Classe.leia;
            case "escreva":
                return (int)Classe.escreva;
            case "se":
                return (int)Classe.se;
            case "fimse":
                return (int)Classe.fimse;
            case "pt_v":
                return (int)Classe.pt_v;
            case "id":
                return (int)Classe.id;
            case "vir":
                return (int)Classe.vir;
            case "lit":
                return (int)Classe.lit;
            case "num":
                return (int)Classe.num;
            case "atr":
                return (int)Classe.atr;
            case "opa":
                return (int)Classe.opa;
            case "ab_p":
                return (int)Classe.ab_p;
            case "fc_p":
                return (int)Classe.fc_p;
            case "entao":
                return (int)Classe.entao;
            case "opr":
                return (int)Classe.opr;
            case "cifrao":
                return (int)Classe.cifrao;
            case "P":
                return (int)Classe.P;
            case "V":
                return (int)Classe.V;
            case "A":
                return (int)Classe.A;
            case "L":
                return (int)Classe.L;
            case "LV":
                return (int)Classe.LV;
            case "D":
                return (int)Classe.D;
            case "TIPO":
                return (int)Classe.TIPO;
            case "ES":
                return (int)Classe.ES;
            case "ARG":
                return (int)Classe.ARG;
            case "CMD":
                return (int)Classe.CMD;
            case "LD":
                return (int)Classe.LD;
            case "OPRD":
                return (int)Classe.OPRD;
            case "COND":
                return (int)Classe.COND;
            case "CAB":
                return (int)Classe.CAB;
            case "CP":
                return (int)Classe.CP;
            case "EXP_R":
                return (int)Classe.EXP_R;
            default:
                return -1;
        }
    }



}
