using AnalisadorLexico;
using AnalisadorLexico.Exceptions;

namespace TestesAnalisadorLexico
{
    [TestClass]
    public class LexicoTeste
    {
        //[TestMethod]
        //public void TestaCriacaoObjetoLexico_QuandoOArquivoExiste()
        //{
        //    var lexico = new Lexico("");
        //    Assert.DoesNotThrow(new Lexico(""));
        //}

        [TestMethod]
        public void TestaCriacaoObjetoLexico_QuandoOArquivoNaoExiste()
        {
            Assert.ThrowsException<ArquivoFonteNaoEncontradoException>(() =>
            {
                var lexico = new Lexico("");
            });
        }
    }
}