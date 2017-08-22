using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Languages.Interfaces;

namespace StockExchangeGame.Database.Generic
{
    // ReSharper disable once UnusedMember.Global
    public interface IEntityController<T>
    {
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void SetCurrentLanguage(ILanguage language);

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        ILanguage GetCurrentLanguage();

        // ReSharper disable once UnusedMethodReturnValue.Global
        int CreateTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        List<T> Get();

        // ReSharper disable once UnusedMember.Global
        T Get(long id);

        // ReSharper disable once UnusedMember.Global
        ObservableCollection<T> Get<TValue>(Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, TValue>> orderBy = null);

        // ReSharper disable once UnusedMember.Global
        T Get(Expression<Func<T, bool>> predicate);

        // ReSharper disable once UnusedMember.Global
        int Insert(T entity);

        // ReSharper disable once UnusedMember.Global
        int Update(T entity);

        // ReSharper disable once UnusedMember.Global
        int Delete(T entity);

        // ReSharper disable once UnusedMember.Global
        int Count(Expression<Func<T, bool>> predicate = null);
		
		void Truncate();
    }
}