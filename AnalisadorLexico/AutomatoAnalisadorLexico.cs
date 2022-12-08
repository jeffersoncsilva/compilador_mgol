﻿namespace AnalisadorLexico;

internal class AutomatoAnalisadorLexico
{
    private int[,] afd = new int[25, 24];
    private int linha_atual = 0;
    public int EstadoAtual { get { return this.linha_atual; } }

    public AutomatoAnalisadorLexico()
    {
        InicializaValoresVaziosAfd();
        InicializaValoresDasColunas();
    }

    private void InicializaValoresDasColunas()
    {
        InicializaEstadoQ0();
        InicializaEstadoQ1();
        afd[2, 1] = 3;
        InicializaEstadoQ3();
        InicializaEstadoQ4();
        InicializaEstadoQ5();
        InicializaEstadoQ6();
        InicializaestadoQ7();
        InicializaEstadoQ11();
        InicializaEstadoQ14();
        InicializaEstadoQ17();
        InicializaEstadoQ20();

        void InicializaEstadoQ20()
        {
            afd[20, 1] = 20;
            afd[20, 2] = 20;
            afd[20, 3] = 20;
            afd[20, 4] = 20;
            afd[20, 5] = 20;
            afd[20, 6] = 20;
            afd[20, 7] = 20;
            afd[20, 8] = 20;
            afd[20, 9] = 20;
            afd[20, 10] = 20;
            afd[20, 11] = 20;
            afd[20, 12] = 20;
            afd[20, 13] = 20;
            afd[20, 14] = 20;
            afd[20, 15] = 20;
            afd[20, 16] = 21;
            afd[20, 17] = 20;
            afd[20, 18] = 20;
            afd[20, 19] = 20;
            afd[20, 20] = 20;
            afd[20, 21] = 20;
            afd[20, 22] = 20;
            afd[20, 23] = 20;

        }

        void InicializaEstadoQ14()
        {
            afd[14, 4] = 19;
            afd[14, 13] = 16;
            afd[14, 15] = 15;
        }

        void InicializaEstadoQ11()
        {
            afd[11, 1] = 11;
            afd[11, 2] = 11;
            afd[11, 3] = 11;
            afd[11, 4] = 11;
            afd[11, 5] = 11;
            afd[11, 6] = 11;
            afd[11, 7] = 11;
            afd[11, 8] = 11;
            afd[11, 9] = 11;
            afd[11, 10] = 11;
            afd[11, 11] = 11;
            afd[11, 12] = 12;
            afd[11, 13] = 11;
            afd[11, 14] = 11;
            afd[11, 15] = 11;
            afd[11, 16] = 11;
            afd[11, 17] = 11;
            afd[11, 18] = 11;
            afd[11, 19] = 11;
            afd[11, 20] = 11;
            afd[11, 21] = 11;
        }

        void InicializaestadoQ7()
        {
            afd[7, 1] = 7;
            afd[7, 2] = 7;
            afd[7, 8] = 7;
        }

        void InicializaEstadoQ17()
        {
            afd[17, 13] = 18;
        }

        void InicializaEstadoQ6()
        {
            afd[6, 1] = 6;
        }

        void InicializaEstadoQ5()
        {
            afd[5, 1] = 6;
        }

        void InicializaEstadoQ4()
        {
            afd[4, 1] = 6;
            afd[4, 3] = 5;
            afd[4, 4] = 5;
        }

        void InicializaEstadoQ3()
        {
            afd[3, 1] = 3;
            afd[3, 5] = 4;
            afd[3, 6] = 4;
        }

        void InicializaEstadoQ1()
        {
            afd[1, 1] = 1;
            afd[1, 5] = 4;
            afd[1, 6] = 4;
            afd[1, 7] = 2;
        }

        void InicializaEstadoQ0()
        {
            afd[0, 1] = 1;
            afd[0, 2] = 7;
            afd[0, 3] = 24;
            afd[0, 4] = 24;
            afd[0, 9] = 8;
            afd[0, 10] = 9;
            afd[0, 11] = 11;
            afd[0, 13] = 13;
            afd[0, 14] = 14;
            afd[0, 15] = 17;
            afd[0, 16] = 20;
            afd[0, 17] = 22;
            afd[0, 18] = 23;
            afd[0, 19] = 24;
            afd[0, 20] = 24;
        }
    }

    private void InicializaValoresVaziosAfd()
    {
        for (int i = 0; i < afd.GetLength(0); i++)
            for (int j = 0; j < afd.GetLength(1); j++)
                afd[i, j] = -1;
    }

    public bool CaractereValido(char caracterAtual)
    {
        if (char.IsDigit(caracterAtual))
            return TrataCaracteresEspeciais(1);
     
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
            return TrataCaracteresEspeciais(2);
        }

        switch (caracterAtual)
        {
            case '+':
                return TrataCaracteresEspeciais(3);
            case '-':
                return TrataCaracteresEspeciais(4);
            case '\\':
                return TrataCaracteresEspeciais(20);
            case '*':
                return TrataCaracteresEspeciais(25);
            case '(':
                return TrataCaracteresEspeciais(9);
            case ')':
                return TrataCaracteresEspeciais(10);
            case '=':
                return TrataCaracteresEspeciais(13);
            case '>':
                return TrataCaracteresEspeciais(15);
            case '<':
                return TrataCaracteresEspeciais(14);
            case '"':
                return TrataCaracteresEspeciais(16);
            case ',':
                return TrataCaracteresEspeciais(17);
            case '.':
                return TrataCaracteresEspeciais(7);
            case ';':
                return TrataCaracteresEspeciais(18);
            case ':':
                return TrataCaracteresEspeciais(21);
            case '{':
                return TrataCaracteresEspeciais(11);
            case '}':
                return TrataCaracteresEspeciais(12);
            case '!':
                return TrataCaracteresEspeciais(22);
            case '?':
                return TrataCaracteresEspeciais(23);
        }
        return false;
    }

    private bool NaoExisteTransicaoLinhaAtualParaColuna(int coluna) => afd[linha_atual, coluna] == -1;

    private bool TrataCaracteresEspeciais(int coluna_do_caractere)
    {
        if (NaoExisteTransicaoLinhaAtualParaColuna(coluna_do_caractere))
            return false;
        linha_atual = afd[linha_atual, coluna_do_caractere];
        return true;
    }
    
    public void ReiniciaEstado()
    {
        this.linha_atual = 0;
    }
}
