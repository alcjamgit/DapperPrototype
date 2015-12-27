using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperMapper.Models
{
    public class SalesTransaction
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
