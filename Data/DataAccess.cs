using BrokerAppTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace BrokerAppTest.Data
{
    public static class DataAccess
    {
        public static List<Operation> GetAllOperations()
        {
            using (BrokerContext db = new BrokerContext())
            {
                var result = db.Operations.Include("BrokerInfo").ToList();
                return result;
            }
        }

        public static BrokerInfo GetBrokerById(int brokerId)
        {
            using (BrokerContext db = new BrokerContext())
            {
                BrokerInfo pos = db.Brokers.FirstOrDefault(p => p.Id == brokerId);
                return pos;
            }
        }

        public static void LoadPlayers()
        {
            using (BrokerContext db = new BrokerContext())
            {
                if (db.Brokers.Count() != 2)
                {
                    db.Brokers.Add(new BrokerInfo() {OrgName  = "Player", Deposit = 100000, StocksQuantity = 0, Id = 1});
                    db.Brokers.Add(new BrokerInfo() {OrgName  = "Bot", Deposit = 100000, StocksQuantity = 0, Id = 2});
                    db.SaveChanges();
                }
            }
        }

        public static decimal LoadDepo(string name)
        {
            using (BrokerContext db = new BrokerContext())
            {
                var broker = db.Brokers.FirstOrDefault(e => e.OrgName == name);

                if (broker != null) return broker.Deposit;
                else throw new System.Exception($"Брокер {name} не найден"); ;
            }
        }

        public static int LoadStocks(string brokerName)
        {
            using (BrokerContext db = new BrokerContext())
            {
                var broker = db.Brokers.FirstOrDefault(e => e.OrgName == brokerName);

                if (broker != null) return broker.StocksQuantity;
                else throw new System.Exception($"Брокер {brokerName} не найден"); ;
            }
        }

        public static void AddOperation(string brokerName, bool isSale, decimal price, int quantity)
        {
            using (BrokerContext db = new BrokerContext())
            {
                var broker = db.Brokers.FirstOrDefault(e => e.OrgName == brokerName);

                Operation op = new Operation()
                {
                    BrokerInfoId = broker.Id,
                    IsSale = isSale,
                    OperationDate = System.DateTime.Now,
                    Price = price,
                    Quantity = quantity
                };

                db.Operations.Add(op);

                if (isSale)
                {
                    broker.StocksQuantity -= quantity;
                    broker.Deposit += quantity * price;
                }
                else
                {
                    broker.StocksQuantity += quantity;
                    broker.Deposit -= quantity * price;
                }

                db.SaveChanges();
            }
        }
    }
}
