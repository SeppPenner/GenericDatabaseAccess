using System;

namespace StockExchangeGame.Database.Models
{
    // ReSharper disable once UnusedMember.Global
    public class Taxes : AbstractEntity
    {
        private DateTime _dateTaxWasDue;

        private double _dueInEuro;

        private long _merchantId;

        private double _payedInEuro;

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
        public DateTime DateTaxWasDue
        {
            get => _dateTaxWasDue;
            set
            {
                if (value.Equals(_dateTaxWasDue))
                    return;
                _dateTaxWasDue = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public double DueInEuro
        {
            get => _dueInEuro;
            set
            {
                if (value.Equals(_dueInEuro))
                    return;
                _dueInEuro = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public double PayedInEuro
        {
            get => _payedInEuro;
            set
            {
                if (value.Equals(_payedInEuro))
                    return;
                _payedInEuro = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(ModifiedAt)}: {ModifiedAt}, {nameof(Deleted)}: {Deleted}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(_dateTaxWasDue)}: {_dateTaxWasDue}, {nameof(_dueInEuro)}: {_dueInEuro}, {nameof(_merchantId)}: {_merchantId}, {nameof(_payedInEuro)}: {_payedInEuro}";
        }
    }
}