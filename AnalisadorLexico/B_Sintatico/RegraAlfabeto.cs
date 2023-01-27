namespace CompiladorMgol.B_Sintatico;

public class RegraAlfabeto
{
    public string Identificador { get; set; }
    public string LadoEsquerdo { get; set; }
    public string[] LadoDireito { get; set; }


    public override string ToString()
    {
        var direito = LadoDireito.Aggregate ((frase, palavra)  => frase + " " + palavra);
        return $"{Identificador.PadLeft(3)} - {LadoEsquerdo.PadLeft(5)} --> {direito}";
    }



}
