using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrokerAppTest.Models
{
    public class BrokerInfo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrgName { get; set; }

        [Required]
        public decimal Deposit { get; set; }

        [Required]
        public int StocksQuantity { get; set; }

        public List<Operation> Operations { get; set; } = new List<Operation>();
    }
}
