﻿using AnalisadorLexico.Exceptions;

namespace AnalisadorLexico
{
    public struct Token
    {
        public string Classe { get; set; }
        public string Lexema { get; set; }
        public string Tipo { get; set; }

        public Token(string classe, string lexema, string tipo)
        {
            if (classe == null || lexema == null || tipo == null)
                throw new InicializacaoDeTokenComNuloException();
            Classe = classe;
            Lexema = lexema;
            Tipo = tipo;
        }

        public Token(string? classe) : this(classe, classe, classe) { }

        public override string ToString() => $"Classe: {Classe} - Lexema: {Lexema} - Tipo: {Tipo}";
    }
}