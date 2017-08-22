using System;

namespace StockExchangeGame.Database.Models
{
    // ReSharper disable once UnusedMember.Global
    public class Bought : AbstractEntity
    {
        private long _amount;

        private DateTime _dateBought;

        private long _merchantId;

        private long _stockId;

        private double _valuePerStockInEuro;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public long MerchantId
        {
            get => _merchantId;
            set
            {
                if (value.Equals(_merchantId))
                    return;
                _merchantId = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public long StockId
        {
            get => _stockId;
            set
            {
                if (value.Equals(_stockId))
                    return;
                _stockId = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public long Amount
        {
            get => _amount;
            set
            {
                if (value.Equals(_amount))
                    return;
                _amount = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public DateTime DateBought
        {
            get => _dateBought;
            set
            {
                if (value.Equals(_dateBought))
                    return;
                _dateBought = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public double ValuePerStockInEuro
        {
            get => _valuePerStockInEuro;
            set
            {
                if (value.Equals(_valuePerStockInEuro))
                    return;
                _valuePerStockInEuro = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(ModifiedAt)}: {ModifiedAt}, {nameof(Deleted)}: {Deleted}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(_amount)}: {_amount}, {nameof(_dateBought)}: {_dateBought}, {nameof(_merchantId)}: {_merchantId}, {nameof(_stockId)}: {_stockId}, {nameof(_valuePerStockInEuro)}: {_valuePerStockInEuro}";
        }
    }
}