using BrokerAppTest.Data;
using BrokerAppTest.Models;
using BrokerAppTest.Mvvm;
using BrokerAppTest.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using System.Windows.Threading;

namespace BrokerAppTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region private fields

        private ObservableCollection<Operation> _operations;
        private IEventAggregator _eventAggregator;
        private readonly DispatcherTimer _timer;
        private decimal _playersDepo;
        private decimal _stockPrice;
        private int _playersStocks;
        private int _botsStocks;
        private decimal _botsDepo;
        private int _quantity;
        private decimal _sum;

        #endregion

        public ObservableCollection<Operation> Operations
        {
            get => _operations;
            set => SetProperty(ref _operations, value);
        }

        public decimal PlayersDepo
        {
            get => _playersDepo;
            set => SetProperty(ref _playersDepo, value);
        }

        public int PlayersStocks
        {
            get => _playersStocks;
            set => SetProperty(ref _playersStocks, value);
        }

        public int BotsStocks
        {
            get => _botsStocks;
            set => SetProperty(ref _botsStocks, value);
        }

        public decimal BotsDepo
        {
            get => _botsDepo;
            set => SetProperty(ref _botsDepo, value);
        }

        [Range(0, int.MaxValue, ErrorMessage = "Число не может быть отрицательным.")]
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                Sum = Quantity * StockPrice;
                RaisePropertyChanged(nameof(Sum));
            }
        }

        public decimal Sum
        {
            get => _sum;
            set => SetProperty(ref _sum, value);
        }

        public ICommand BuyCommand { get; private set; }
        public ICommand SellCommand { get; private set; }

        public decimal StockPrice
        {
            get => _stockPrice;
            set => SetProperty(ref _stockPrice, value);
        }

        public bool IsRise => StockRateSimulator.IsRise();
        

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DataAccess.LoadPlayers();
            StockPrice = StockRateSimulator.GetStockPrice();
            LoadData();

            BuyCommand = new RelayCommand(OnBuy, OnCanBuy);
            SellCommand = new RelayCommand(OnSell, OnCanSell);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _timer.Start();
            _timer.Tick += (o, e) => RefreshStockPrice();
        }

        private bool OnCanSell()
        {
            return Quantity > 0 && Quantity <= PlayersStocks;
        }

        private void OnSell()
        {
            DataAccess.AddOperation("Player", true, StockPrice, Quantity);
            LoadData();
        }

        private bool OnCanBuy()
        {
            return Quantity > 0 && Sum <= PlayersDepo;
        }

        private void OnBuy()
        {
            DataAccess.AddOperation("Player", false, StockPrice, Quantity);
            LoadData();
        }

        private void LoadData()
        {
            Operations = new ObservableCollection<Operation>(DataAccess.GetAllOperations());
            PlayersDepo = DataAccess.LoadDepo("Player");
            BotsDepo = DataAccess.LoadDepo("Bot");
            PlayersStocks = DataAccess.LoadStocks("Player");
            BotsStocks = DataAccess.LoadStocks("Bot");
        }

        private void RefreshStockPrice()
        {
            StockPrice = StockRateSimulator.GetStockPrice();
            if(Quantity!=0) Sum = Quantity * StockPrice;

            BotTraider();

            RaisePropertyChanged(nameof(IsRise));
            RaisePropertyChanged(nameof(Quantity));
        }

        private void BotTraider()
        {
            if (!IsRise && (25 * StockPrice) <= BotsDepo)
            {
                if (StockPrice < 85)
                {
                    DataAccess.AddOperation("Bot", false, StockPrice, 25);
                }
                if (StockPrice < 95)
                {
                    DataAccess.AddOperation("Bot", false, StockPrice, 10);
                }
                if (StockPrice < 105)
                {
                    DataAccess.AddOperation("Bot", false, StockPrice, 5);
                }
            }

            if (IsRise && 10 <= BotsStocks)
            {
                if (StockPrice > 122)
                {
                    DataAccess.AddOperation("Bot", true, StockPrice, 10);
                }
                if (StockPrice > 110)
                {
                    DataAccess.AddOperation("Bot", true, StockPrice, 5);
                }
            }

            LoadData();
        }
    }
}