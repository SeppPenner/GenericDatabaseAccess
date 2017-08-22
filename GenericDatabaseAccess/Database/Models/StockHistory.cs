using System;

namespace StockExchangeGame.Database.Models
{
    public class StockHistory : AbstractEntity
    {
        private DateTime _priceDate;

        private double _pricePerStock;

        private long _stockId;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public DateTime PriceDate
        {
            get => _priceDate;
            set
            {
                if (value.Equals(_priceDate))
                    return;
                _priceDate = value;
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
        public double PricePerStock
        {
            get => _pricePerStock;
            set
            {
                if (value.Equals(_pricePerStock))
                    return;
                _pricePerStock = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(ModifiedAt)}: {ModifiedAt}, {nameof(Deleted)}: {Deleted}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(_priceDate)}: {_priceDate}, {nameof(_pricePerStock)}: {_pricePerStock}, {nameof(_stockId)}: {_stockId}";
        }
    }
}