using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar Voucher Tipo Valor Valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);

            // Act

            var result = voucher.ValidarSeAplicavel();
            // Assert

            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Tipo Invalido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInValido()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

            // Act

            var result = voucher.ValidarSeAplicavel();
            // Assert

            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count); //espera-se 6 erros de validação 
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(m => m.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(m => m.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, result.Errors.Select(m => m.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(m => m.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(m => m.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.ValorDescontoErroMsg, result.Errors.Select(m => m.ErrorMessage));
        }

        [Fact(DisplayName = "Validar Voucher Porcentagem Valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-OFF", 15, null, 1, TipoDescontoVoucher.Porcentagem,
                DateTime.Now.AddDays(1), true, false);

            // Act

            var result = voucher.ValidarSeAplicavel();
            // Assert

            Assert.True(result.IsValid);
        }


        [Fact(DisplayName = "Validar Voucher Porcentagem Invalido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarInValido()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(-1), false, true);

            // Act

            var result = voucher.ValidarSeAplicavel();
            // Assert

            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher Valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_AplicarVoucherValido_DeveRetornarSemErros()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);

            // Act

            var result = pedido.AplicarVoucher(voucher);

            // Assert

            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher InValido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_AplicarVoucherInValido_DeveRetornarErros()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("", null, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(-1), false, true);

            // Act

            var result = pedido.AplicarVoucher(voucher);

            // Assert

            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher tipo valor desconto")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var item1 = new PedidoItem(Guid.NewGuid(), "Vaper", 2, 300);
            var item2 = new PedidoItem(Guid.NewGuid(), "Mouse", 1, 150);

            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);

            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);
            var valorTotal = pedido.ValorTotal - voucher.ValorDesconto;

            // Act

            pedido.AplicarVoucher(voucher);

            // Assert

            Assert.Equal(valorTotal, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var item1 = new PedidoItem(Guid.NewGuid(), "Vaper", 2, 300);
            var item2 = new PedidoItem(Guid.NewGuid(), "Mouse", 1, 150);

            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);

            var voucher = new Voucher("PROMO-15-OFF", 15, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(1), true, false);

            var valorDesconto = (pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorTotal = pedido.ValorTotal - valorDesconto;

            // Act

             pedido.AplicarVoucher(voucher);

            // Assert

            Assert.Equal(valorTotal, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher desconto excede valor total")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void AplicarVoucher_DescontoExecdeValorTotalPedido_PedidoveTerValorZerado()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var item1 = new PedidoItem(Guid.NewGuid(), "Vaper", 1, 100);
            var item2 = new PedidoItem(Guid.NewGuid(), "Mouse", 1, 100);

            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);

            var voucher = new Voucher("PROMO-300-REAIS", null, 300, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);
            //var valorTotal = pedido.ValorTotal - voucher.ValorDesconto;

            // Act

            pedido.AplicarVoucher(voucher);

            // Assert

            Assert.Equal(0, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher recaulcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var item1 = new PedidoItem(Guid.NewGuid(), "Vaper", 2, 100);
            var item2 = new PedidoItem(Guid.NewGuid(), "Mouse", 1, 100);

            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);

            var voucher = new Voucher("PROMO-300-REAIS", null, 50, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(1), true, false);

            // Act

            pedido.AplicarVoucher(voucher);

            var totalEsperado = pedido.PedidoItems.Sum(p => p.Quantidade * p.ValorUnitario) - voucher.ValorDesconto;

            // Assert

            Assert.Equal(totalEsperado, pedido.ValorTotal);
        }
    }
}
