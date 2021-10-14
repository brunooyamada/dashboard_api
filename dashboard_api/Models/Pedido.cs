using dashboard_api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dashboard_api
{
    public class Pedido
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime data_criacao { get; set; }
        public DateTime data_entrega { get; set; }
        public string endereco { get; set; }
        public string nome { get; set; }
        public string veiculo { get; set; }
        public string placa { get; set; }

        public List<dynamic> produtos { get; set; }
    }
}
