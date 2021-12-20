using BrokerAppTest.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrokerAppTest.Models
{
    public class Operation
    {
        public int Id { get; set; }

        [Required]
        public DateTime OperationDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal OperationSum => Quantity * Price;

        [Required]
        public bool IsSale { get; set; }

        [Required]
        public int BrokerInfoId { get; set; }

        public virtual BrokerInfo BrokerInfo { get; set; }

        [NotMapped]
        public BrokerInfo Broker
        {
            get
            {
                return DataAccess.GetBrokerById(BrokerInfoId);
            }
        }
    }
}
