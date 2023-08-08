using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain.Tests
{
    public  class PedidoItensTests
    {

        [Fact(DisplayName = "Novo Item Pedido com unidades abaixo do Permitido")]
        [Trait("Categoria", "Vendas- Pedido Item")]
        public void Add_ItemAbaixoDoPermitido_DeveRetornarExcpetion()
        {
            // Act & Assert & Arrange
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", -1, 100));

        }
    }
}
