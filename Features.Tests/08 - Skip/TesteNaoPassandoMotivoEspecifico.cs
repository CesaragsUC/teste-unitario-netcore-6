using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.Tests
{
    public class TesteNaoPassandoMotivoEspecifico
    {
        [Fact(DisplayName = "Novo Cliente 2.0", Skip = "Este teste não é compativel com a Nova versão 2.0")]
        [Trait("Categoria", "Escapando dos Testes")]
        public void Teste_NaoEstaPassando_VersaoNovaNaoCompativel()
        {
            Assert.True(false);
        }
    }
}
