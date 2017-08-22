using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StockExchangeGame.Database.Attributes;

namespace StockExchangeGame.Database.Models
{
    public class AbstractEntity : INotifyPropertyChanged
    {
        private DateTime _createdAt;

        private bool _deleted;

        private DateTime _modifiedAt;

        [PrimaryKey]
        [AutoIncrement]
        // ReSharper disable once UnusedMember.Global
        public long Id { get; set; }

        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (value.Equals(_createdAt))
                    return;
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedMember.Global
        public DateTime ModifiedAt
        {
            get => _modifiedAt;
            set
            {
                if (value.Equals(_modifiedAt))
                    return;
                _modifiedAt = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool Deleted
        {
            get => _deleted;
            set
            {
                if (value.Equals(_deleted))
                    return;
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeProtected.Global
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            ModifiedAt = DateTime.Now;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}