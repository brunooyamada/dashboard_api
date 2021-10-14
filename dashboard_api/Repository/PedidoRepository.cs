using Dapper;
using dashboard_api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace dashboard_api.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private DbSession _db;
        
        public PedidoRepository(DbSession dbSession)
        {
            _db = dbSession;
        }

        public async Task<int> Delete(int id)
        {
            using (var conn = _db.Connection)
            {
                string query = @"DELETE FROM Pedido WHERE id = @id";
                var result = await conn.ExecuteAsync(sql: query, param: new { id });
                return result;
            }
        }

        public async Task<dynamic> GetPedido(int? pagina=1)
        {
            if (pagina == 0 || pagina is null) {
                pagina = 1;
            }

            using (var conn = _db.Connection)
            {
                string query = @"SELECT COUNT(1) AS registros, CEILING(CAST(COUNT(1) AS DECIMAL)/50) AS paginas FROM Pedidos; ";
                var contadores = await conn.QueryAsync(sql: query);

                query =        @"DECLARE @QtdPorPagina INT = 50,
                                         @pagina INT = @page;
                                 SELECT Pedidos.id, Pedidos.data_criacao, Pedidos.data_entrega, Pedidos.endereco
                                 FROM Pedidos
                                 ORDER BY Pedidos.data_criacao ASC 

                                 OFFSET (@pagina - 1) * @QtdPorPagina ROWS 
								 FETCH NEXT @QtdPorPagina ROWS ONLY; ";
                var param = new DynamicParameters();
                param.Add("@page", pagina, DbType.Int32, ParameterDirection.Input);

                var pedidos = await conn.QueryAsync<dynamic>(sql: query, param: param);

                query = @"SELECT Produtos.id, Produtos.nome, Produtos.descricao, Produtos.valor
                          FROM Pedidos_Produtos
                          INNER JOIN Produtos ON Produtos.id = Pedidos_Produtos.id_produto
                          WHERE Pedidos_Produtos.id_pedido = @id_pedido; ";

                foreach (var pedido in pedidos)
                {
                    param = new DynamicParameters();
                    param.Add("@id_pedido", pedido.id, DbType.String, ParameterDirection.Input);

                    var produtos = await conn.QueryAsync<dynamic>(sql: query, param: param);
                    pedido.produtos = produtos;
                }

                var resultado = new List<dynamic>();
                var qtdPaginas = contadores.FirstOrDefault().paginas;
                resultado.Add(new { registros = contadores.FirstOrDefault().registros, paginas = qtdPaginas });
                resultado.Add(new { pedidos = pedidos });

                return resultado;
            }
        }

        public async Task<Pedido> GetPedidoById(int id)
        {
            using (var conn = _db.Connection)
            {
                string query = "SELECT id, data_criacao, data_entrega, endereco FROM Pedidos WHERE id = @id";
                var pedido = await conn.QueryFirstOrDefaultAsync<Pedido>
                    (sql: query, param: new { id });

                query = @"SELECT Produtos.id, Produtos.nome, Produtos.descricao, Produtos.valor
                          FROM Pedidos_Produtos
                          INNER JOIN Produtos ON Produtos.id = Pedidos_Produtos.id_produto
                          WHERE Pedidos_Produtos.id_pedido = @id_pedido; ";

                var param = new DynamicParameters();
                param.Add("@id_pedido", pedido.id, DbType.String, ParameterDirection.Input);

                pedido.produtos = new List<dynamic>();

                var produtos = await conn.QueryAsync<dynamic>(sql: query, param: param);
                foreach(var produto in produtos)
                {
                    var prod = new Produto();
                    prod.id = produto.id;
                    prod.nome = produto.nome;
                    prod.descricao = produto.descricao;                    
                    prod.valor = produto.valor;

                    pedido.produtos.Add(prod);
                }
                
                return pedido;
            }
        }

        public async Task<int> Save(Pedido pedido)
        {
            using(var conn = _db.Connection)
            {
                string command = @"INSERT INTO Pedidos(data_criacao, data_entrega, endereco)
                                   VALUES(@data_criacao, null, @endereco);";
                DynamicParameters param = new DynamicParameters();
                param.Add("@data_criacao", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                param.Add("@endereco", pedido.endereco);

                var result = await conn.ExecuteAsync(sql: command, param: param);

                string sql = "SELECT @@IDENTITY AS identificador; ";
                var idPedido = conn.QueryFirstOrDefault<int>(sql);

                // Produtos
                command = @"INSERT INTO Produtos(nome, descricao, valor)
                            VALUES(@nome, @descricao, @valor);";
                param = new DynamicParameters();
                param.Add("@nome", pedido.produtos[0].nome, DbType.String, ParameterDirection.Input);
                param.Add("@descricao", pedido.produtos[0].descricao, DbType.String, ParameterDirection.Input);
                param.Add("@valor", pedido.produtos[0].valor, DbType.Double, ParameterDirection.Input);
                await conn.ExecuteAsync(sql: command, param: param);
                
                sql = "SELECT @@IDENTITY AS identificador; ";
                var idProduto = conn.QueryFirstOrDefault<int>(sql);

                command = "INSERT INTO Pedidos_Produtos(id_pedido, id_produto) VALUES(@id_pedido, @id_produto)";
                param = new DynamicParameters();
                param.Add("@id_pedido", idPedido, DbType.Int16, ParameterDirection.Input);
                param.Add("@id_produto", idProduto, DbType.Int32, ParameterDirection.Input);
                await conn.ExecuteAsync(sql: command, param: param);

                return result;
            }
        }

        public async Task<int> UpdatePedido(Pedido pedido)
        {
            using (var conn = _db.Connection)
            {
                string command = @"UPDATE Pedidos 
                                   SET nome = @nome,
                                       veiculo = @veiculo,
                                       placa = @placa,
                                       data_entrega = @data_entrega 
                                   WHERE id = @id";
                var result = await conn.ExecuteAsync(sql: command, param: pedido);
                return result;
            }
        }
    }
}
