using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BarMode
{
    public class Pedido
    {
        [DataMember(Name = "id")]
        public Guid Id { get; private set; }

        [DataMember(Name = "produto")]
        public Produto Produto { get; private set; }
     
        [DataMember(Name = "clientes")]
        public IEnumerable<Cliente> Clientes
        {
            get { return _clientesPagamentos.Select(x => x.Cliente).ToList(); }
        }

        private readonly IList<ClientePagamento> _clientesPagamentos;

        public Pedido(Produto produto, IList<Cliente> clientes)
        {
            Produto = produto;

            if (!clientes.Any())
                throw new ArgumentException("O pedido deve ter pelo menos um cliente");

            _clientesPagamentos = clientes.Select(x => new ClientePagamento(x)).ToList();
            
            Id = Guid.NewGuid();
        }

        public decimal TotalPorCliente()
        {
            var totalPorCliente = Produto.Preco / _clientesPagamentos.Count;

            return totalPorCliente;

        }

        public void RegistrarPagamento(Cliente cliente)
        {
            _clientesPagamentos.First(x => x.Cliente.Equals(cliente)).pago = true;
        }
        
        public decimal GetTotalPago()
        {
            var totalPorCliente = TotalPorCliente();

            var qtdPagos = _clientesPagamentos.Count(x => x.pago);

            var totalPago = Produto.Preco - (qtdPagos * totalPorCliente);

            return totalPago;
        }

        private class ClientePagamento
        {
            internal readonly Cliente Cliente;
            internal bool pago;

            public ClientePagamento(Cliente cliente)
            {
                Cliente = cliente;
            }
        }
    }
}