namespace StockExchangeGame.Database.Models
{
    // ReSharper disable once UnusedMember.Global
    public class Merchant : AbstractEntity
    {
        private double _liquidFundsInEuro;
        private string _name;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
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
        public double LiquidFundsInEuro
        {
            get => _liquidFundsInEuro;
            set
            {
                if (value.Equals(_liquidFundsInEuro))
                    return;
                _liquidFundsInEuro = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(ModifiedAt)}: {ModifiedAt}, {nameof(Deleted)}: {Deleted}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(_liquidFundsInEuro)}: {_liquidFundsInEuro}, {nameof(_name)}: {_name}";
        }
    }
}