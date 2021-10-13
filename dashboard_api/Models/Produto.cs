using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashboard_api.Models
{
    public class Produto
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public float valor { get; set; }
    }
}
