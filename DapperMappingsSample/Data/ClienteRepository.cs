using Dapper;
using DapperMappingsSample.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DapperMappingsSample.Data
{
    public class ClienteRepository
    {
        private readonly string strConexao;

        public ClienteRepository(string strConexao)
        {
            this.strConexao = strConexao;
        }

        public Cliente GetByIdWithMultiple(int idCliente)
        {
            using var conexao = new SqlConnection(this.strConexao);

            var queryCliente = @"select * from Cliente c where c.idCliente = @idCliente;";
            var queryVenda = @"select * from Venda v where v.idCliente = @idCliente;";

            var result = conexao.QueryMultiple(queryCliente + queryVenda, new { idCliente });

            var cliente = result.Read<Cliente>().FirstOrDefault();
            if (cliente != null)
                cliente.Vendas = result.Read<Venda>().ToList();

            return cliente;
        }

        public Cliente GetByIdWithMapping(int idCliente)
        {
            using var conexao = new SqlConnection(this.strConexao);

            var queryCliente = @"select * from cliente c
                                 join venda v on c.IdCliente = v.IdCliente
                                 where c.IdCliente = @idCliente";

            var resultSet = new Dictionary<int, Cliente>();

            var result = conexao.Query<Cliente, Venda, Cliente>(
                queryCliente,
                map: (clienteMap, vendaMap) =>
                {
                    if (!resultSet.TryGetValue(clienteMap.IdCliente, out Cliente cliente))
                        resultSet.Add(clienteMap.IdCliente, cliente = clienteMap);

                    if (cliente.Vendas == null)
                        cliente.Vendas = new List<Venda>();

                    if (vendaMap != null)
                    {
                        if (!cliente.Vendas.Any(v => v.IdVenda == vendaMap.IdVenda))
                            cliente.Vendas.Add(vendaMap);
                    }

                    return cliente;
                },
                splitOn: "idVenda",
                param: new { idCliente });

            return result.FirstOrDefault();
        }
    }
}
