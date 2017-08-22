namespace StockExchangeGame.Database.Models
{
    // ReSharper disable once UnusedMember.Global
    public class Stock : AbstractEntity
    {
        private string _name;

        private long _stockMarketId;

        private long _total;

        private long _used;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        public string Name
        {
            get => _name;
            set
            {
                if (value.Equals(_name))
                    return;
                _name = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public long StockMarketId
        {
            get => _stockMarketId;
            set
            {
                if (value.Equals(_stockMarketId))
                    return;
                _stockMarketId = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public long Total
        {
            get => _total;
            set
            {
                if (value.Equals(_total))
                    return;
                _total = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public long Used
        {
            get => _used;
            set
            {
                if (value.Equals(_used))
                    return;
                _used = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once UnusedMember.Global
        public double GetQuotaUsed()
        {
            return _used / (double) _total * 100;
        }

        public override string ToString()
        {
            return
                $"{nameof(ModifiedAt)}: {ModifiedAt}, {nameof(Deleted)}: {Deleted}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(_name)}: {_name}, {nameof(_total)}: {_total}, {nameof(_used)}: {_used}, {nameof(_stockMarketId)}: {_stockMarketId}";
        }
    }
}