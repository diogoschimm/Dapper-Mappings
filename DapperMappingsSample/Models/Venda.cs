namespace DapperMappingsSample.Models
{
    public class Venda
    {
        public int IdVenda { get; set; }
        public decimal ValorTotal { get; set; }

        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
    }
}
