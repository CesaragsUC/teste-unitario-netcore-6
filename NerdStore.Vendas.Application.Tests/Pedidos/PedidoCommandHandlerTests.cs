using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        private readonly Guid _clientId;
        private readonly Guid _produtoId;
        private readonly Pedido _pedido;
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _pedidoHandler;
        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _pedidoHandler = _mocker.CreateInstance<PedidoCommandHandler>();
            _clientId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();
            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clientId);
            //_pedido = Pedido.PedidoFactory.NovoPedidoRascunhoComUmItemJaAdicionado();
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido com sucesso")]
        [Trait("Categoria", "Vendas - Pedido CommandHandler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Vaper", 2, 100);


            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act

            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            //verifica se a entidade Pedido foi chamada ao menos 1 vez ao tentar criar novo pedido
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            //verifica se o UnitOfWork foi chamado ao menos 1 vez, se sim então deu boa
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            // mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido Rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido CommandHandler")]
        public async Task AdicionarItem_NovoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange

            var itemExistente = new PedidoItem(Guid.NewGuid(), "Produto xpto", 2, 100);
            _pedido.AdicionarItem(itemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clientId, Guid.NewGuid(), "Vaper", 2, 100);


            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clientId)).Returns(Task.FromResult(_pedido));

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act

            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            //verifica se Atualizar e Adicionar foram chamados ao menos 1 vez ao tentar criar novo pedido
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            //verifica se o UnitOfWork foi chamado ao menos 1 vez, se sim então deu boa
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);

        }


        [Fact(DisplayName = "Adicionar Item Existente ao Pedido Rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido CommandHandler")]
        public async Task AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange

            var itemExistente = new PedidoItem(_produtoId, "Produto xpto", 2, 100);
            _pedido.AdicionarItem(itemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clientId, _produtoId, "Produto xpto", 2, 100);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clientId)).Returns(Task.FromResult(_pedido));

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act

            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            //verifica se Atualizar e Adicionar foram chamados ao menos 1 vez ao tentar criar novo pedido
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            //verifica se o UnitOfWork foi chamado ao menos 1 vez, se sim então deu boa
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);

        }


        [Fact(DisplayName = "Adicionar Item comando invalido")]
        [Trait("Categoria", "Vendas - Pedido CommandHandler")]
        public async Task AdicionarItem_ComandoInvalido_DeveRetornarFalsoElancarEventoDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act

            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            //verifica se Atualizar e Adicionar foram chamados ao menos 1 vez ao tentar criar novo pedido
            _mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));


        }
    }
}
