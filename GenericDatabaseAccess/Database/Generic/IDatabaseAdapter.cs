using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Languages.Interfaces;

namespace StockExchangeGame.Database.Generic
{
    public interface IDatabaseAdapter
    {
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void SetCurrentLanguage(ILanguage language);

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        ILanguage GetCurrentLanguage();

        // ReSharper disable once UnusedMemberInSuper.Global
        string GetDatabasePath();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        string GetConnectionString();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateBoughtTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateCompanyEndingsTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateCompanyNamesTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateDummyCompanyTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateMerchantTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateNamesTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateSoldTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateStockTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateStockHistoryTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateStockMarketTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateSurnamesTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateTaxesTable();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void CreateAllTables();

        // ReSharper disable once UnusedMember.Global
        List<T> Get<T>();

        // ReSharper disable once UnusedMember.Global
        T Get<T>(long id);

        // ReSharper disable once UnusedMember.Global
        ObservableCollection<T> Get<T, TValue>(Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, TValue>> orderBy = null);

        // ReSharper disable once UnusedMember.Global
        T Get<T>(Expression<Func<T, bool>> predicate);

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        int Insert<T>(T entity);

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        int Update<T>(T entity);

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        int Delete<T>(T entity);

        // ReSharper disable once UnusedMember.Global
        int Insert<T>(List<T>entities);

        // ReSharper disable once UnusedMember.Global
        int Update<T>(List<T> entities);

        // ReSharper disable once UnusedMember.Global
        int Delete<T>(List<T> entities);

        // ReSharper disable once UnusedMember.Global
        int Count<T>(Expression<Func<T, bool>> predicate = null);
		
		// ReSharper disable once UnusedMember.Global
		void Truncate<T>();
    }
}