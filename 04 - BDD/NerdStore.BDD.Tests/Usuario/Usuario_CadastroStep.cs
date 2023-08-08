using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.Pedido;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebTestsFixtureCollection))]
    public class Usuario_CadastroStep
    {
        public readonly AutomacaoWebTestsFixture _testsFixture;
        public readonly CadastroDeUsuarioTela _cadastroUsuario;

        public Usuario_CadastroStep(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _cadastroUsuario = new CadastroDeUsuarioTela(_testsFixture.BrowserHelper);
        }

        [When(@"Ele clicar em registrar")]
        public void WhenEleClicarEmRegistrar()
        {
            //Arragne
            _cadastroUsuario.ClicarNoLinkRegistrar();

            //Assert
            Assert.Contains(_testsFixture.Configuration.RegisterUrl, _cadastroUsuario.ObterUrl());
        }

        [When(@"Preencher os dados do formulario")]
        public void WhenPreencherOsDadosDoFormulario(Table table)
        {
            //Arragne
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;

            //Act
            _cadastroUsuario.PreencherFormularioRegistro(usuario);

            //Assert
            Assert.True(_cadastroUsuario.ValidarPreenchimentoFormularioRegistro(usuario));

        }

        [When(@"Clicar no botão registrar")]
        public void WhenClicarNoBotaoRegistrar()
        {
            _cadastroUsuario.ClicarNoBotaoRegistrar();
        }


        [When(@"Preencher os dados do formulario com uma senha sem maiusculas")]
        public void WhenPreencherOsDadosDoFormularioComUmaSenhaSemMaiusculas(Table table)
        {
            //Arragne
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;
            usuario.Senha = "teste@123";

            //Act
            _cadastroUsuario.PreencherFormularioRegistro(usuario);

            //Assert
            Assert.True(_cadastroUsuario.ValidarPreenchimentoFormularioRegistro(usuario));
        }

        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiuscula")]
        public void ThenEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmaLetraMaiuscula()
        {
            Assert.True(_cadastroUsuario
                            .ValidarMensagemDeErroFormulario("Passwords must have at least one uppercase ('A'-'Z')"));
        }

        [When(@"Preencher os dados do formulario com uma senha sem caractere especial")]
        public void WhenPreencherOsDadosDoFormularioComUmaSenhaSemCaractereEspecial(Table table)
        {
            //Arragne
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;
            usuario.Senha = "Teste123";

            //Act
            _cadastroUsuario.PreencherFormularioRegistro(usuario);

            //Assert
            Assert.True(_cadastroUsuario.ValidarPreenchimentoFormularioRegistro(usuario));
        }

        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial")]
        public void ThenEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmCaractereEspecial()
        {
            Assert.True(_cadastroUsuario
                .ValidarMensagemDeErroFormulario("Passwords must have at least one non alphanumeric character"));
        }
    }
}
