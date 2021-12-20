using BrokerAppTest.Models;
using BrokerAppTest.Data;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Prism.Events;
using BrokerAppTest.Services;
using System.Windows.Threading;
using System;
using System.Windows.Input;
using Prism.Commands;

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
            set
            {
                _operations = value;
                RaisePropertyChanged(nameof(Operations));
            }
        }

        public decimal PlayersDepo
        {
            get { return _playersDepo; }
            set
            {
                _playersDepo = value; 
                RaisePropertyChanged(nameof(PlayersDepo));
                RaisePropertyChanged(nameof(BuyCommand));
            }
        }

        public int PlayersStocks
        {
            get { return _playersStocks; }
            set
            {
                _playersStocks = value;
                RaisePropertyChanged(nameof(PlayersStocks));
                RaisePropertyChanged(nameof(SellCommand));
            }
        }

        public decimal BotsDepo
        {
            get => _botsDepo;
            set { _botsDepo = value; RaisePropertyChanged(nameof(BotsDepo)); }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                if (value > 0) Sum = Quantity * StockPrice;
                RaisePropertyChanged(nameof(Quantity));
                RaisePropertyChanged(nameof(Sum));
                RaisePropertyChanged(nameof(BuyCommand));
                RaisePropertyChanged(nameof(SellCommand));
            }
        }

        public decimal Sum
        {
            get => _sum;
            set { _sum = value; RaisePropertyChanged(nameof(Sum)); }
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

            BuyCommand = new DelegateCommand(OnBuy, OnCanBuy);
            SellCommand = new DelegateCommand(OnSell, OnCanSell);

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
            DataAccess.AddOperation(1, true, StockPrice, Quantity);
            RaisePropertyChanged(nameof(Operations));
        }

        private bool OnCanBuy()
        {
            return Quantity > 0 && Sum <= PlayersDepo;
        }

        private void OnBuy()
        {
            DataAccess.AddOperation(1, false, StockPrice, Quantity);
            RaisePropertyChanged(nameof(Operations));
        }

        private void RaiseBindableProperties()
        {
            RaisePropertyChanged(nameof(StockPrice));
            RaisePropertyChanged(nameof(IsRise));
        }
    }
}