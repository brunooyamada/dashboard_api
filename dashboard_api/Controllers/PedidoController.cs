using dashboard_api.Models;
using dashboard_api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dashboard_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly string token = "Bearer asdfghjkl123456789";

        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }


        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> GetPedidos(int? pagina)
        {
            var request = HttpContext.Request;
            var headers = request.Headers;
          

            //if (! headers["token"].ToString().Equals(this.token))
            //{
            //    return StatusCode(403, new { erro = "Token inválido" });
            //}

            List<dynamic> result = await _pedidoRepository.GetPedido(pagina);
            var json = result;
            return Ok(json);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPedidosById(int id)
        {
            var request = HttpContext.Request;
            var headers = request.Headers;

            //if (!headers["token"].ToString().Equals(this.token))
            //{
            //    return StatusCode(403, new { erro = "Token inválido" });
            //}

            var result = await _pedidoRepository.GetPedidoById(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("inserir")]
        public async Task<IActionResult> Save(Pedido pedido)
        {
            var request = HttpContext.Request;

            //if (!headers["token"].ToString().Equals(this.token))
            //{
            //    return StatusCode(403, new { erro = "Token inválido" });
            //}

            pedido.data_criacao = DateTime.Now;
            
            var result = await _pedidoRepository.Save(pedido);
            if (result > 0)
            {
                return Ok(new { result = "Pedido inserido com sucesso" });
            } else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("Atualizar")]
        public async Task<IActionResult> UpdatePedido(Pedido pedido)
        {
            var request = HttpContext.Request;
            var headers = request.Headers;

            //if (!headers["token"].ToString().Equals(this.token))
            //{
            //    return StatusCode(403, new { erro = "Token inválido" });
            //}

            // Dados do body
            var stream = new StreamReader(request.Body);
            var body = stream.ReadToEndAsync();

            var json = body.Result;
            var jsonConvert = JsonConvert.DeserializeObject<dynamic>(json);
            // fim do body
            
            var result = await _pedidoRepository.UpdatePedido(pedido);
            if (result > 0)
            {
                return Ok(new {  result = "Pedido atualizado com sucesso" });
            } else
            {
                return NotFound();
            }
        }
    }
}
