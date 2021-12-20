using System;

namespace BrokerAppTest.Services
{
    public static class StockRateSimulator
    {
        private static int _volatility = 5;
        private static decimal _old_price = 100;
        private static decimal _new_price;

        public static bool IsRise()
        {
            return _new_price > _old_price;
        }

        public static decimal GetStockPrice()
        {
            if (_new_price == 0) _new_price = _old_price;
            else _old_price = _new_price;

            var rnd = GetRandomNumber(0, 1);
            var change = 2 * _volatility * (decimal)rnd;
            if (change > _volatility)
            {
                change -= 2 * _volatility;
            }

            _new_price += change;

            return _new_price;
        }

        private static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}
