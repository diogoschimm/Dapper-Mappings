using System.Collections.Generic;

namespace DapperMappingsSample.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }

        public ICollection<Venda> Vendas { get; set; }
    }
}
