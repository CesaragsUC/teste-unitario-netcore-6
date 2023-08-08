using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.Pedido;
using NerdStore.BDD.Tests.Usuario;
using NerdStore.BDD.Tests.Voucher;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebTestsFixtureCollection))]
    public class Aplicar_VoucherStep
    {
        public readonly AutomacaoWebTestsFixture _testsFixture;
        public readonly PedidoTela _pedidoTela;
        public readonly LoginUsuarioTela _loginUsuarioTela;
        public readonly VoucherTela _voucherTela;
        private string _urlProduto;

        public Aplicar_VoucherStep(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _pedidoTela = new PedidoTela(_testsFixture.BrowserHelper);
            _loginUsuarioTela = new LoginUsuarioTela(_testsFixture.BrowserHelper);
            _voucherTela = new VoucherTela(_testsFixture.BrowserHelper);
        }

        [Given(@"O usuario esteja logado no site")]
        public void GivenOUsuarioEstejaLogadoNoSite()
        {
            // Arrange
            var usuario = new User
            {
                Email = "cesar@teste.com",
                Senha = "Teste@123"
            };
            _testsFixture.Usuario = usuario;

            // Act 
            var login = _loginUsuarioTela.Login(usuario);

            // Assert
            Assert.True(login);
        }


        [Given(@"O usuario adicona um item ao carrinho")]
        public void GivenOUsuarioAdiconaUmItemAoCarrinho()
        {
            // Arrange
            _pedidoTela.AcessarVitrineDeProdutos(); //acessa vitrine de produtos
            _pedidoTela.ObterDetalhesDoProduto(); //entra no detalhe do produto
            _urlProduto = _pedidoTela.ObterUrl(); //armazena a url desse produto detalhe contendo id
            _pedidoTela.ClicarEmComprarAgora(); //clica no botao comprar
        }

        [When(@"O usuario estiver no resumo da compra")]
        public void WhenOUsuarioEstiverNoResumoDaCompra()
        {

            _pedidoTela.ValidarSeEstaNoCarrinhoDeCompras();//redireciona para tela de resumo da compra
        }

        [When(@"O usuario colocar o codigo do voucher")]
        public void WhenOUsuarioColocarOCodigoDoVoucher()
        {
            _voucherTela.PreecnherCodigoDoVoucher();

        }

        [Then(@"Deve aplicar o desconto sobre o valor total")]
        public void ThenDeveAplicarODescontoSobreOValorTotal()
        {
            //Arreange
            _voucherTela.ClicarBotaoAplicarVoucher();

            var valorDecontoVoucher = _voucherTela.ObterValorDescontoVoucher();
            var valorTotal = _voucherTela.ObterValorTotal();
            var valorTotalCarrinho = _pedidoTela.ObterValorTotalCarrinho();
            var totalPagar = valorTotal - valorDecontoVoucher;

            //Assert
            Assert.Equal(totalPagar, valorTotalCarrinho);
        }
    }
}
