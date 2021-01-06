using DapperMappingsSample.Data;
using DapperMappingsSample.Models;
using Microsoft.AspNetCore.Mvc;


namespace DapperMappingsSample.Controllers
{
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
}
