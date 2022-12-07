namespace AnalisadorLexico;

internal class AutomatoAnalisadorLexico
{
    private int[,] afd = new int[25, 21];
    private int linha_atual = 0;

    public AutomatoAnalisadorLexico()
    {
        InicializaValoresVaziosAfd();
        InicializaValoresDeCadaLinha();
        InicializaValoresDasColunas();
    }

    private void InicializaValoresDasColunas()
    {
        afd[0, 1] = 1;
        afd[0, 2] = 7;
        afd[0, 9] = 8;
        afd[0, 10] = 9;
        afd[1, 1] = 1;
        afd[1, 5] = 4;
        afd[1, 6] = 4;
        afd[1, 7] = 2;
        afd[2, 1] = 3;
        afd[3, 1] = 3;
        afd[3, 5] = 4;
        afd[3, 6] = 4;
        afd[4, 1] = 6;
        afd[4, 3] = 5;
        afd[4, 4] = 5;
        afd[5, 1] = 6;
        afd[6, 1] = 6;
        afd[7, 1] = 7;
        afd[7, 2] = 7;
        afd[7, 8] = 7;

        //q0
        //afd[1, 0] = (char)2;
        //afd[1, 1] = (char)9;
        //afd[1, 5] = (char)24;
        //afd[1, 6] = (char)24;
        //afd[1, 9] = (char)22;
        //afd[1, 10] = (char)24;
        //afd[1, 11] = (char)24;
        //afd[1, 12] = (char)14;
        //afd[1, 13] = (char)17;
        //afd[1, 14] = (char)19;
        //afd[1, 16] = (char)13;
        //afd[1, 18] = (char)18;
        //afd[1, 19] = (char)11;
        //afd[1, 20] = (char)25;

        ////q2
        //afd[0, 2] = (char)2;
        //afd[2, 2] = (char)3;


        //afd[0, 3] = (char)4;//q3
        //afd[3, 4] = (char)5;//q4
        //afd[4, 4] = (char)5;//q4


        //afd[0, 5] = (char)7;//q5
        //afd[5, 5] = (char)6;//q5
        //afd[6, 5] = (char)6;//q5

        //afd[0, 6] = (char)7;//q6
        //afd[0, 7] = (char)8;//q7

        //afd[0, 9] = (char)9;//q9
        //afd[1, 9] = (char)9;//q9
        //afd[7, 9] = (char)9;//q9

        //afd[2, 13] = (char)20;//q13

        //afd[13, 14] = (char)15;//q14
        //afd[14, 14] = (char)16;//q14
        //afd[15, 14] = (char)25;//q14

        //afd[14, 17] = (char)18;//q17

        //afd[8, 23] = (char)23;//q23

        //afd[0, 20] = (char)20;//q20
        //afd[1, 20] = (char)20;//q20
        //afd[2, 20] = (char)20;//q20
        //afd[3, 20] = (char)20;//q20
        //afd[4, 20] = (char)20;//q20
        //afd[5, 20] = (char)20;//q20
        //afd[6, 20] = (char)20;//q20
        //afd[7, 20] = (char)20;//q20
        //afd[8, 20] = (char)20;//q20
        //afd[9, 20] = (char)20;//q20
        //afd[10, 20] = (char)20;//q20
        //afd[11, 20] = (char)20;//q20
        //afd[12, 20] = (char)20;//q20
        //afd[13, 20] = (char)20;//q20
        //afd[14, 20] = (char)20;//q20
        //afd[15, 20] = (char)20;//q20
        //afd[16, 20] = (char)20;//q20
        //afd[17, 20] = (char)21;//q20

        //afd[1, 26] = (char)26;//q26
        //afd[2, 26] = (char)26;//q26
        //afd[3, 26] = (char)26;//q26
        //afd[4, 26] = (char)26;//q26
        //afd[5, 26] = (char)26;//q26
        //afd[6, 26] = (char)26;//q26
        //afd[7, 26] = (char)26;//q26
        //afd[8, 26] = (char)26;//q26
        //afd[9, 26] = (char)26;//q26
        //afd[10, 26] = (char)26;//q26
        //afd[11, 26] = (char)26;//q26
        //afd[12, 26] = (char)26;//q26
        //afd[13, 26] = (char)26;//q26
        //afd[14, 26] = (char)26;//q26
        //afd[15, 26] = (char)26;//q26
        //afd[16, 26] = (char)26;//q26
        //afd[17, 26] = (char)26;//q26
        //afd[18, 26] = (char)27;//q26
    }

    private void InicializaValoresDeCadaLinha()
    {
        //afd[0, 0] = 'd';
        //afd[1, 0] = 'l';
        //afd[2, 0] = '.';
        //afd[3, 0] = 'e';
        //afd[4, 0] = 'E';
        //afd[5, 0] = '+';
        //afd[6, 0] = '-';
        //afd[7, 0] = '_';
        //afd[8, 0] = ';';
        //afd[9, 0] = ',';
        //afd[10, 0] = '/';
        //afd[11, 0] = '*';
        //afd[12, 0] = '<';
        //afd[13, 0] = '>';
        //afd[14, 0] = '=';
        //afd[15, 0] = '-';
        //afd[16, 0] = '{';
        //afd[17, 0] = '}';
        //afd[18, 0] = '"';
        //afd[19, 0] = '(';
        //afd[20, 0] = ')';
    }

    private void InicializaValoresVaziosAfd()
    {
        for (int i = 0; i < afd.GetLength(0); i++)
        {
            for (int j = 0; j < afd.GetLength(1); j++)
            {
                afd[i, j] = -1;
            }
        }
    }

    internal bool CaractereValido(char caracterAtual)
    {
        if (char.IsDigit(caracterAtual))
        {
            if (afd[linha_atual, 1] == -1)
            {
                return false;
            }
            else
            {
                linha_atual = afd[linha_atual, 1];
                return true;
            }
        }
        else if (char.IsLetter(caracterAtual))
        {
            if (caracterAtual.ToString().ToLower().Equals("e"))
            {
                if (afd[linha_atual, 5] != -1)
                {
                    linha_atual = afd[linha_atual, 5];
                    return true;
                }
            }
            if (afd[linha_atual, 2] == -1)
            {
                return false;
            }
            linha_atual = afd[linha_atual, 2];
            return true;
        }
        //else if(caracterAtual == '\0' || char.IsWhiteSpace(caracterAtual))
        //{
        //    return false;
        //}
        else if (caracterAtual == '+' || caracterAtual == '-')
        {
            linha_atual = afd[linha_atual, 3];
            return true;
        }
        else if (caracterAtual == '(')
        {
            if (afd[linha_atual, 9] == -1)
                return false;
            linha_atual = afd[linha_atual, 9];
            return true;
        }
        else if (caracterAtual == ')')
        {
            if (afd[linha_atual, 10] == -1)
                return false;
            linha_atual = afd[linha_atual, 10];
            return true;
        }
        return false;
    }

    internal void ReiniciaEstado()
    {
        this.linha_atual = 0;
    }

    public int EstadoAtual { get { return this.linha_atual; } }

}
