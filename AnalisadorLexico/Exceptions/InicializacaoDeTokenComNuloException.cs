using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorLexico.Exceptions
{
    public class InicializacaoDeTokenComNuloException : Exception
    {
        public InicializacaoDeTokenComNuloException() : base("Token não pode ser inicializado com campo nulo.")
        {
        }
    }
}
