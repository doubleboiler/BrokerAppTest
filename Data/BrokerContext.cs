using BrokerAppTest.Models;
using System.Data.Entity;

namespace BrokerAppTest.Data
{
    public class BrokerContext : DbContext
    {
        public BrokerContext() { }

        public DbSet<BrokerInfo> Brokers { get; set; }
        public DbSet<Operation> Operations { get; set; }

    }
}
