using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashboard_api.Repository
{
    public interface IPedidoRepository
    {
        Task<List<Pedido>> GetPedido();
        Task<Pedido> GetPedidoById(int id);
        Task<int> Save(Pedido pedido);
        Task<int> UpdatePedido(Pedido pedido);
        Task<int> Delete(int id);
    }
}
