namespace CompiladorMgol.A_Lexico;

internal struct CaracteresEspeciais
{
    public const char SOMA = '+',
    SUBTRACAO = '-',
    DIVISAO = '/',
    MULTIPLICACAO = '*',
    ABRE_PARENTESES = '(',
    FECHA_PARENTESES = ')',
    ATRIBUICAO = '=',
    MAIOR = '>',
    MENOR = '<',
    ASPAS_DUPLAS = '"',
    VIRGULA = ',',
    PONTO = '.',
    PONTO_VIRGULA = ';',
    DOIS_PONTOS = ':',
    ABRE_CHAVES = '{',
    FECHA_CHAVES = '}',
    DIFERENTE = '!',
    INTERROGACAO = '?',
    TABULACAO = '\t',
    QUEBRA_DE_LINHA = '\n',
    CARRIEGE_RETURN = '\r',
    BARRA_INVERTIDA = '\\',
    ESPACO = ' ';

    public static bool EhCaractereEspecial(char caracter)
    {
        return caracter == SOMA ||
        caracter == SUBTRACAO ||
        caracter == DIVISAO ||
        caracter == MULTIPLICACAO ||
        caracter == ABRE_PARENTESES ||
        caracter == FECHA_PARENTESES ||
        caracter == ATRIBUICAO ||
        caracter == MAIOR ||
        caracter == MENOR ||
        caracter == ASPAS_DUPLAS ||
        caracter == VIRGULA ||
        caracter == PONTO ||
        caracter == PONTO_VIRGULA ||
        caracter == DOIS_PONTOS ||
        caracter == ABRE_CHAVES ||
        caracter == FECHA_CHAVES ||
        caracter == DIFERENTE ||
        caracter == INTERROGACAO;
    }
}
