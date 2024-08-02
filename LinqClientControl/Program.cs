using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqAdvancedDemo
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }

    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var pedidos = new List<Pedido>
            {
                new Pedido { PedidoId = 1, ClienteId = 1, Valor = 1500, Data = new DateTime(2024, 1, 15) },
                new Pedido { PedidoId = 2, ClienteId = 4, Valor = 200, Data = new DateTime(2024, 4, 20) },
                new Pedido { PedidoId = 3, ClienteId = 5, Valor = 300, Data = new DateTime(2024, 6, 19) },
                new Pedido { PedidoId = 4, ClienteId = 2, Valor = 450, Data = new DateTime(2024, 2, 08) },
                new Pedido { PedidoId = 5, ClienteId = 3, Valor = 100, Data = new DateTime(2024, 10, 24) }
            };

            var clientes = new List<Cliente>
            {
                new Cliente { ClienteId = 1, Nome = "Jeff" },
                new Cliente { ClienteId = 2, Nome = "Luis" },
                new Cliente { ClienteId = 3, Nome = "Cleber" },
                new Cliente { ClienteId = 4, Nome = "Ana" },
                new Cliente { ClienteId = 5, Nome = "Beatriz" }
            };

            var resultados = pedidos
                .Where(p => p.Valor > 100)
                .GroupBy(p => p.ClienteId)
                .Select(g => new
                {
                    ClienteId = g.Key,
                    TotalValor = g.Sum(p => p.Valor),
                    QuantidadePedidos = g.Count(),
                    UltimoPedido = g.Max(p => p.Data)
                })
                .Join(
                    clientes,
                    resultado => resultado.ClienteId,
                    cliente => cliente.ClienteId,
                    (resultado, cliente) => new
                    {
                        ClienteNome = cliente.Nome,
                        TotalValor = resultado.TotalValor,
                        QuantidadePedidos = resultado.QuantidadePedidos,
                        UltimoPedido = resultado.UltimoPedido
                    }
                )
                .OrderByDescending(r => r.TotalValor)
                .ToList();

            foreach (var resultado in resultados)
            {
                Console.WriteLine($"Cliente: {resultado.ClienteNome}");
                Console.WriteLine($"Total Valor: {resultado.TotalValor:C}");
                Console.WriteLine($"Quantidade: {resultado.QuantidadePedidos}");
                Console.WriteLine($"Data Último Pedido: {resultado.UltimoPedido:dd/MM/yyyy}");
                Console.WriteLine();
            }
        }
    }
}
