using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorMgol.Common
{
    public class Logging
    {
        private StringBuilder linha;
        private StringBuilder completo;

        public Logging()
        {
            this.linha = new();
            this.completo = new();
        }

        public void Log(string msg) => linha.Append(" " +msg);
        
        public void LogQuebraDeLinha()
        {
            completo.Append(linha.ToString().TrimStart());
            linha.Clear();
        }

        internal void LogInicio(string v)
        {
            linha.Insert(0, " "+v);
        }

        public void LogErrors(string msg)
        {
            
        }

        /// <summary>
        /// Imprime uma variável no meio da linha. Util em declaração de variáveis.
        /// </summary>
        /// <param name="vari">Variavel que deseja imprimir no meio da linha.</param>
        internal void LogMeioDaLinha(string vari)
        {
            var posicao = linha.ToString().IndexOf(" ");
            var p = posicao >= 0 ? posicao : 0;
            linha.Insert(p, ""+vari);
        }
        
        public override string ToString()
        {
            return completo.ToString();
        }
    }
}
