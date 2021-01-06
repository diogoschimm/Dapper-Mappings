# Dapper-Mappings
Projeto de exemplo para mappings do dapper com QueryMultiple e Query ( 1 - N )

## Utilizando QueryMulitple

```c#
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
```

## Utilizando Mapping

```c#
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
```


## API

```c#
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {  
        [HttpGet("{id}")]
        public Cliente Get(int id, [FromServices]ClienteRepository clienteRepository)
        {
            return clienteRepository.GetByIdWithMapping(id);
        }
         
    }
```
