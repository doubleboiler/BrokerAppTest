using BrokerAppTest.Data;
using BrokerAppTest.Models;
using BrokerAppTest.Mvvm;
using BrokerAppTest.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
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
        private int _playersStocks;
        private decimal _botsDepo;
        private int _quantity;
        private decimal _sum;

        #endregion

        public ObservableCollection<Operation> Operations
        {
            get { return _operations; }
            set { SetProperty(ref _operations, value); }
        }

        public decimal PlayersDepo
        {
            get { return _playersDepo; }
            set { SetProperty(ref _playersDepo, value); }
        }

        public int PlayersStocks
        {
            get { return _playersStocks; }
            set { SetProperty(ref _playersStocks, value); }
        }

        public decimal BotsDepo
        {
            get => _botsDepo;
            set { SetProperty(ref _botsDepo, value); }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                if (value > 0) Sum = Quantity * StockPrice;
                RaisePropertyChanged(nameof(Sum));
            }
        }

        public decimal Sum
        {
            get => _sum;
            set { SetProperty(ref _sum, value); }
        }

        public ICommand BuyCommand { get; private set; }
        public ICommand SellCommand { get; private set; }

        public decimal StockPrice => StockRateSimulator.GetStockPrice();

        public bool IsRise => StockRateSimulator.IsRise();
        

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DataAccess.LoadPlayers();
            Operations = new ObservableCollection<Operation>(DataAccess.GetAllOperations());
            PlayersDepo = DataAccess.LoadDepo("Player");
            BotsDepo = DataAccess.LoadDepo("Bot");
            PlayersStocks = DataAccess.LoadStocks("Player");

            BuyCommand = new RelayCommand(OnBuy, OnCanBuy);
            SellCommand = new RelayCommand(OnSell, OnCanSell);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _timer.Start();
            _timer.Tick += (o, e) => RaiseBindableProperties();
        }

        private bool OnCanSell()
        {
            return Quantity > 0 && Quantity <= PlayersStocks;
        }

        private void OnSell()
        {
            DataAccess.AddOperation("Player", true, StockPrice, Quantity);
            RaiseBindableProperties();
        }

        private bool OnCanBuy()
        {
            return Quantity > 0 && Sum <= PlayersDepo;
        }

        private void OnBuy()
        {
            DataAccess.AddOperation("Player", false, StockPrice, Quantity);
            RaiseBindableProperties();
        }

        private void RaiseBindableProperties()
        {
            RaisePropertyChanged(nameof(StockPrice));
            RaisePropertyChanged(nameof(IsRise));
            RaisePropertyChanged(nameof(Operations));
            RaisePropertyChanged(nameof(PlayersStocks));
            RaisePropertyChanged(nameof(PlayersDepo));
        }

    }
}