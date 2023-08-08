using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebTestsFixtureCollection))]
    public class CommonSteps
    {

        public readonly AutomacaoWebTestsFixture _testsFixture;
        public readonly CadastroDeUsuarioTela _cadastroUsuario;

        public CommonSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _cadastroUsuario = new CadastroDeUsuarioTela(_testsFixture.BrowserHelper);
        }


        [Given(@"Que o visitante está acessando o site da loja")]
        public void GivenQueOVisitanteEstaAcessandoOSiteDaLoja()
        {
            //Arragne
            _cadastroUsuario.AcessarSiteLoja();

            //Assert
            Assert.Contains(_testsFixture.Configuration.DomainUrl, _cadastroUsuario.ObterUrl());
        }


        [Then(@"Ele será redirecionado para a vitrine")]
        public void ThenEleSeraRedirecionadoParaAVitrine()
        {
            //Assert
            Assert.Contains(_testsFixture.Configuration.VitrineUrl, _cadastroUsuario.ObterUrl());
        }


        [Then(@"Uma saudação com seu e-mail será exibida no menu superior")]
        public void ThenUmaSaudacaoComSeuE_MailSeraExibidaNoMenuSuperior()
        {
            //Assert
            Assert.True(_cadastroUsuario.ValidarSaudacaoUsuarioLogado(_testsFixture.Usuario));
        }
    }
}
