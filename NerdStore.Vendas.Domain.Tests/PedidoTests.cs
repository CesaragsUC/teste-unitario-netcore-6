using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Add  Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void Add_Item_Pedido_Novo_Pedido_Vazio_Deve_Atualizar_Valor()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste",2,100);

            // Act

            pedido.AdicionarItem(pedidoItem);
            // Assert

            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Add  Item Novo Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void Add_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            // Arrange
            var pedido =  Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 100);
          
            // Act
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(x=> x.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Add  Item Pedido Acima do Permitido")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void Add_ItemAcimaDe15Unidades_DeveRetornarExcpetion()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 16, 100);

            // Act & Assert
            Assert.Throws<DomainException>(()=> pedido.AdicionarItem(pedidoItem));
            
        }

        [Fact(DisplayName = "Add  Somar item existente acima do permitido")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void AddItemPedido_ItemExixtenteSomaUnidadesAcimaDoPermitido_DeveRetornarExcpetion()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1, 100);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 100);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));

        }

        [Fact(DisplayName = "Atualizar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void AtualizarItemPedido_ItemNaoExistenteNaLista_DeveRetornarExcpetion()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1, 100);


            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItem));

        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Camiseta", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Camiseta", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            //Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Act & Assert
            Assert.Equal(novaQuantidade,pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Xbox Series S", 2, 500);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Camiseta", 2, 100);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            //altera quantidade do produto
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Camiseta", 6, 100);

            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                        pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

            //Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Act & Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);

        }


        [Fact(DisplayName = "Atualizar Item Pedido Quantidade Acima do Permitido")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void AtualizarItemPedido_ItenmUnidadeAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Xbox Series S", 3, 500);
            pedido.AdicionarItem(pedidoItemExistente1);

            //altera quantidade do produto
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Xbox Series S", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));

        }

        [Fact(DisplayName = "Remover Item Pedido Inixistente")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemNaoExistente = new PedidoItem(Guid.NewGuid(), "Xbox Series S", 3, 500);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemNaoExistente));

        }

        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
        [Trait("Categoria", "Vendas - Pedidos")]
        public void RemoverItemPedido_ItemExistente_DeveCalcularValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var Item1 = new PedidoItem(Guid.NewGuid(), "Xbox Series S", 2, 500);
            var Item2 = new PedidoItem(produtoId, "Camiseta", 2, 100);
            pedido.AdicionarItem(Item1);
            pedido.AdicionarItem(Item2);

            //soma total do item2 que nao foi removido
            var totalPedido = Item2.Quantidade * Item2.ValorUnitario;

            // Act
            pedido.RemoverItem(Item1);
            
            //  Assert
            Assert.Equal(totalPedido,pedido.ValorTotal);

        }
    }
}
