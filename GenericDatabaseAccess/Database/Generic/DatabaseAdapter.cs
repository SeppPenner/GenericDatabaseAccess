using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Languages.Interfaces;
using StockExchangeGame.Database.Models;

namespace StockExchangeGame.Database.Generic
{
    // ReSharper disable once UnusedMember.Global
    public class DatabaseAdapter : IDatabaseAdapter
    {
        private const string SqlDbFileName = "StockGame.sqlite3";
        private IEntityController<Bought> _boughtController;
        private IEntityController<CompanyEndings> _companyEndingsController;
        private IEntityController<CompanyNames> _companyNamesController;
        private ILanguage _currentLanguage;
        private IEntityController<DummyCompany> _dummyCompanyController;
        private IEntityController<Merchant> _merchantController;
        private IEntityController<Names> _namesController;
        private IEntityController<Sold> _soldController;
        private IEntityController<Stock> _stockController;
        private IEntityController<StockHistory> _stockHistoryController;
        private IEntityController<StockMarket> _stockMarketController;
        private IEntityController<Surnames> _surnamesController;
        private IEntityController<Taxes> _taxesController;

        public DatabaseAdapter(ILanguage language)
        {
            InitializeControllers(GetConnectionString());
            SetCurrentLanguage(language);
            CreateDatabaseFileIfNotExists();
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            SetLanguages(language);
        }

        public ILanguage GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        public string GetConnectionString()
        {
            return @"Data Source=" + GetDatabasePath() + "; " +
                   @"Version=3; FailIfMissing=True; Foreign Keys=True;";
        }

        public string GetDatabasePath()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            return location != null
                ? Path.Combine(Directory.GetParent(location).FullName, SqlDbFileName)
                : string.Empty;
        }

        public void CreateBoughtTable()
        {
            _boughtController.CreateTable();
        }

        public void CreateCompanyEndingsTable()
        {
            _companyEndingsController.CreateTable();
        }

        public void CreateCompanyNamesTable()
        {
            _companyNamesController.CreateTable();
        }

        public void CreateDummyCompanyTable()
        {
            _dummyCompanyController.CreateTable();
        }

        public void CreateMerchantTable()
        {
            _merchantController.CreateTable();
        }

        public void CreateNamesTable()
        {
            _namesController.CreateTable();
        }

        public void CreateSoldTable()
        {
            _soldController.CreateTable();
        }

        public void CreateStockTable()
        {
            _stockController.CreateTable();
        }

        public void CreateStockHistoryTable()
        {
            _stockHistoryController.CreateTable();
        }

        public void CreateStockMarketTable()
        {
            _stockMarketController.CreateTable();
        }

        public void CreateSurnamesTable()
        {
            _surnamesController.CreateTable();
        }

        public void CreateTaxesTable()
        {
            _taxesController.CreateTable();
        }

        public void CreateAllTables()
        {
            CreateBoughtTable();
            CreateCompanyEndingsTable();
            CreateCompanyNamesTable();
            CreateDummyCompanyTable();
            CreateMerchantTable();
            CreateNamesTable();
            CreateSoldTable();
            CreateStockTable();
            CreateStockHistoryTable();
            CreateStockMarketTable();
            CreateSurnamesTable();
            CreateTaxesTable();
        }

        public List<T> Get<T>()
        {
            if (typeof(T) == typeof(Bought))
                return _boughtController.Get() as List<T>;
            if (typeof(T) == typeof(CompanyEndings))
                return _companyEndingsController.Get() as List<T>;
            if (typeof(T) == typeof(CompanyNames))
                return _companyNamesController.Get() as List<T>;
            if (typeof(T) == typeof(DummyCompany))
                return _dummyCompanyController.Get() as List<T>;
            if (typeof(T) == typeof(Merchant))
                return _merchantController.Get() as List<T>;
            if (typeof(T) == typeof(Names))
                return _namesController.Get() as List<T>;
            if (typeof(T) == typeof(Sold))
                return _soldController.Get() as List<T>;
            if (typeof(T) == typeof(Stock))
                return _stockController.Get() as List<T>;
            if (typeof(T) == typeof(StockHistory))
                return _stockHistoryController.Get() as List<T>;
            if (typeof(T) == typeof(StockMarket))
                return _stockMarketController.Get() as List<T>;
            if (typeof(T) == typeof(Surnames))
                return _surnamesController.Get() as List<T>;
            if (typeof(T) == typeof(Taxes))
                return _taxesController.Get() as List<T>;
            return null;
        }

        public T Get<T>(long id)
        {
            if (typeof(T) == typeof(Bought))
            {
                var value = _boughtController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(CompanyEndings))
            {
                var value = _companyEndingsController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(CompanyNames))
            {
                var value = _companyNamesController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(DummyCompany))
            {
                var value = _dummyCompanyController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Merchant))
            {
                var value = _merchantController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Names))
            {
                var value = _namesController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Sold))
            {
                var value = _soldController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Stock))
            {
                var value = _stockController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(StockHistory))
            {
                var value = _stockHistoryController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(StockMarket))
            {
                var value = _stockMarketController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Surnames))
            {
                var value = _surnamesController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) != typeof(Taxes)) return (T) Convert.ChangeType(null, typeof(T));
            {
                var value = _taxesController.Get(id);
                return (T) Convert.ChangeType(value, typeof(T));
            }
        }

        private void InitializeControllers(string connectionString)
        {
            _boughtController = new BoughtController(connectionString);
            _companyEndingsController = new CompanyEndingsController(connectionString);
            _companyNamesController = new CompanyNamesController(connectionString);
            _dummyCompanyController = new DummyCompanyController(connectionString);
            _merchantController = new MerchantController(connectionString);
            _namesController = new NamesController(connectionString);
            _soldController = new SoldController(connectionString);
            _stockController = new StockController(connectionString);
            _stockHistoryController = new StockHistoryController(connectionString);
            _stockMarketController = new StockMarketController(connectionString);
            _surnamesController = new SurnamesController(connectionString);
            _taxesController = new TaxesController(connectionString);
        }

        private void SetLanguages(ILanguage language)
        {
            _boughtController.SetCurrentLanguage(language);
            _boughtController.SetCurrentLanguage(language);
            _companyEndingsController.SetCurrentLanguage(language);
            _companyNamesController.SetCurrentLanguage(language);
            _dummyCompanyController.SetCurrentLanguage(language);
            _merchantController.SetCurrentLanguage(language);
            _namesController.SetCurrentLanguage(language);
            _soldController.SetCurrentLanguage(language);
            _stockController.SetCurrentLanguage(language);
            _stockHistoryController.SetCurrentLanguage(language);
            _stockMarketController.SetCurrentLanguage(language);
            _surnamesController.SetCurrentLanguage(language);
            _taxesController.SetCurrentLanguage(language);
        }

        private void CreateDatabaseFileIfNotExists()
        {
            var databasePath = GetDatabasePath();
            if (File.Exists(databasePath))
            {
                CreateAllTables();
                return;
            }
            CreateDatabaseAndTables(databasePath);
        }

        private void CreateDatabaseAndTables(string databasePath)
        {
            File.Create(databasePath);
            CreateAllTables();
        }

        public ObservableCollection<T> Get<T, TValue>(Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, TValue>> orderBy = null)
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            // ReSharper disable ExpressionIsAlwaysNull
            if (typeof(T) == typeof(Bought))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _boughtController.Get(predicate as Expression<Func<Bought, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<Bought, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(CompanyEndings))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _companyEndingsController.Get(predicate as Expression<Func<CompanyEndings, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<CompanyEndings, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(CompanyNames))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _companyNamesController.Get(predicate as Expression<Func<CompanyNames, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<CompanyNames, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(DummyCompany))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _dummyCompanyController.Get(predicate as Expression<Func<DummyCompany, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<DummyCompany, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(Merchant))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _merchantController.Get(predicate as Expression<Func<Merchant, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<Merchant, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(Names))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _namesController.Get(predicate as Expression<Func<Names, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<Names, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(Sold))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _soldController.Get(predicate as Expression<Func<Sold, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<Sold, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(Stock))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _stockController.Get(predicate as Expression<Func<Stock, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<Stock, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(StockHistory))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _stockHistoryController.Get(predicate as Expression<Func<StockHistory, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<StockHistory, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(StockMarket))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _stockMarketController.Get(predicate as Expression<Func<StockMarket, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<StockMarket, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(Surnames))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _surnamesController.Get(predicate as Expression<Func<Surnames, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<Surnames, TValue>>) as ObservableCollection<T>;
            if (typeof(T) == typeof(Taxes))
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                return _taxesController.Get(predicate as Expression<Func<Taxes, bool>>,
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    // ReSharper disable once ExpressionIsAlwaysNull
                    orderBy as Expression<Func<Taxes, TValue>>) as ObservableCollection<T>;
            return null;
        }

        public T Get<T>(Expression<Func<T, bool>> predicate)
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            // ReSharper disable ExpressionIsAlwaysNull
            if (typeof(T) == typeof(Bought))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _boughtController.Get(predicate as Expression<Func<Bought, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(CompanyEndings))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _companyEndingsController.Get(predicate as Expression<Func<CompanyEndings, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(CompanyNames))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _companyNamesController.Get(predicate as Expression<Func<CompanyNames, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(DummyCompany))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _dummyCompanyController.Get(predicate as Expression<Func<DummyCompany, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Merchant))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _merchantController.Get(predicate as Expression<Func<Merchant, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Names))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _namesController.Get(predicate as Expression<Func<Names, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Sold))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _soldController.Get(predicate as Expression<Func<Sold, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Stock))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _stockController.Get(predicate as Expression<Func<Stock, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(StockHistory))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _stockHistoryController.Get(predicate as Expression<Func<StockHistory, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(StockMarket))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _stockMarketController.Get(predicate as Expression<Func<StockMarket, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(Surnames))
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _surnamesController.Get(predicate as Expression<Func<Surnames, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) != typeof(Taxes)) return (T) Convert.ChangeType(null, typeof(T));
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                // ReSharper disable once SuspiciousTypeConversion.Global
                var value = _taxesController.Get(predicate as Expression<Func<Taxes, bool>>);
                return (T) Convert.ChangeType(value, typeof(T));
            }
        }

        public int Insert<T>(T entity)
        {
            if (typeof(T) == typeof(Bought))
                return _boughtController.Insert(entity as Bought);
            if (typeof(T) == typeof(CompanyEndings))
                return _companyEndingsController.Insert(entity as CompanyEndings);
            if (typeof(T) == typeof(CompanyNames))
                return _companyNamesController.Insert(entity as CompanyNames);
            if (typeof(T) == typeof(DummyCompany))
                return _dummyCompanyController.Insert(entity as DummyCompany);
            if (typeof(T) == typeof(Merchant))
                return _merchantController.Insert(entity as Merchant);
            if (typeof(T) == typeof(Names))
                return _namesController.Insert(entity as Names);
            if (typeof(T) == typeof(Sold))
                return _soldController.Insert(entity as Sold);
            if (typeof(T) == typeof(Stock))
                return _stockController.Insert(entity as Stock);
            if (typeof(T) == typeof(StockHistory))
                return _stockHistoryController.Insert(entity as StockHistory);
            if (typeof(T) == typeof(StockMarket))
                return _stockMarketController.Insert(entity as StockMarket);
            if (typeof(T) == typeof(Surnames))
                return _surnamesController.Insert(entity as Surnames);
            if (typeof(T) == typeof(Taxes))
                return _taxesController.Insert(entity as Taxes);
            return -1;
        }

        public int Update<T>(T entity)
        {
            if (typeof(T) == typeof(Bought))
                return _boughtController.Update(entity as Bought);
            if (typeof(T) == typeof(CompanyEndings))
                return _companyEndingsController.Update(entity as CompanyEndings);
            if (typeof(T) == typeof(CompanyNames))
                return _companyNamesController.Update(entity as CompanyNames);
            if (typeof(T) == typeof(DummyCompany))
                return _dummyCompanyController.Update(entity as DummyCompany);
            if (typeof(T) == typeof(Merchant))
                return _merchantController.Update(entity as Merchant);
            if (typeof(T) == typeof(Names))
                return _namesController.Update(entity as Names);
            if (typeof(T) == typeof(Sold))
                return _soldController.Update(entity as Sold);
            if (typeof(T) == typeof(Stock))
                return _stockController.Update(entity as Stock);
            if (typeof(T) == typeof(StockHistory))
                return _stockHistoryController.Update(entity as StockHistory);
            if (typeof(T) == typeof(StockMarket))
                return _stockMarketController.Update(entity as StockMarket);
            if (typeof(T) == typeof(Surnames))
                return _surnamesController.Update(entity as Surnames);
            if (typeof(T) == typeof(Taxes))
                return _taxesController.Update(entity as Taxes);
            return -1;
        }

        public int Delete<T>(T entity)
        {
            if (typeof(T) == typeof(Bought))
                return _boughtController.Delete(entity as Bought);
            if (typeof(T) == typeof(CompanyEndings))
                return _companyEndingsController.Delete(entity as CompanyEndings);
            if (typeof(T) == typeof(CompanyNames))
                return _companyNamesController.Delete(entity as CompanyNames);
            if (typeof(T) == typeof(DummyCompany))
                return _dummyCompanyController.Delete(entity as DummyCompany);
            if (typeof(T) == typeof(Merchant))
                return _merchantController.Delete(entity as Merchant);
            if (typeof(T) == typeof(Names))
                return _namesController.Delete(entity as Names);
            if (typeof(T) == typeof(Sold))
                return _soldController.Delete(entity as Sold);
            if (typeof(T) == typeof(Stock))
                return _stockController.Delete(entity as Stock);
            if (typeof(T) == typeof(StockHistory))
                return _stockHistoryController.Delete(entity as StockHistory);
            if (typeof(T) == typeof(StockMarket))
                return _stockMarketController.Delete(entity as StockMarket);
            if (typeof(T) == typeof(Surnames))
                return _surnamesController.Delete(entity as Surnames);
            if (typeof(T) == typeof(Taxes))
                return _taxesController.Delete(entity as Taxes);
            return -1;
        }

        public int Insert<T>(List<T> entities)
        {
            return entities.Sum(Insert);
        }

        public int Update<T>(List<T> entities)
        {
            return entities.Sum(Update);
        }

        public int Delete<T>(List<T> entities)
        {
            return entities.Sum(Delete);
        }

        public int Count<T>(Expression<Func<T, bool>> predicate = null)
        {
            // ReSharper disable ExpressionIsAlwaysNull
            // ReSharper disable SuspiciousTypeConversion.Global
            if (typeof(T) == typeof(Bought))
                return _boughtController.Count(predicate as Expression<Func<Bought, bool>>);
            if (typeof(T) == typeof(CompanyEndings))
                return _companyEndingsController.Count(predicate as Expression<Func<CompanyEndings, bool>>);
            if (typeof(T) == typeof(CompanyNames))
                return _companyNamesController.Count(predicate as Expression<Func<CompanyNames, bool>>);
            if (typeof(T) == typeof(DummyCompany))
                return _dummyCompanyController.Count(predicate as Expression<Func<DummyCompany, bool>>);
            if (typeof(T) == typeof(Merchant))
                return _merchantController.Count(predicate as Expression<Func<Merchant, bool>>);
            if (typeof(T) == typeof(Names))
                return _namesController.Count(predicate as Expression<Func<Names, bool>>);
            if (typeof(T) == typeof(Sold))
                return _soldController.Count(predicate as Expression<Func<Sold, bool>>);
            if (typeof(T) == typeof(Stock))
                return _stockController.Count(predicate as Expression<Func<Stock, bool>>);
            if (typeof(T) == typeof(StockHistory))
                return _stockHistoryController.Count(predicate as Expression<Func<StockHistory, bool>>);
            if (typeof(T) == typeof(StockMarket))
                return _stockMarketController.Count(predicate as Expression<Func<StockMarket, bool>>);
            if (typeof(T) == typeof(Surnames))
                return _surnamesController.Count(predicate as Expression<Func<Surnames, bool>>);
            if (typeof(T) == typeof(Taxes))
                return _taxesController.Count(predicate as Expression<Func<Taxes, bool>>);
            return -1;
        }
		
		public void Truncate<T>()
        {
            if (typeof(T) == typeof(Bought))
            {
                _boughtController.Truncate();
				return;
            }
            if (typeof(T) == typeof(CompanyEndings))
            {
                _companyEndingsController.Truncate();
				return;
            }
            if (typeof(T) == typeof(CompanyNames))
            {
                _companyNamesController.Truncate();
				return;
            }
            if (typeof(T) == typeof(DummyCompany))
            {
                _dummyCompanyController.Truncate();
				return;
            }
            if (typeof(T) == typeof(Merchant))
            {
                _merchantController.Truncate();
				return;
            }
            if (typeof(T) == typeof(Names))
            {
                _namesController.Truncate();
				return;
            }
            if (typeof(T) == typeof(Sold))
            {
                _soldController.Truncate();
				return;
            }
            if (typeof(T) == typeof(Stock))
            {
                _stockController.Truncate();
				return;
            }
            if (typeof(T) == typeof(StockHistory))
            {
                _stockHistoryController.Truncate();
				return;
            }
            if (typeof(T) == typeof(StockMarket))
            {
                _stockMarketController.Truncate();
				return;
            }
            if (typeof(T) == typeof(Surnames))
            {
                _surnamesController.Truncate();
				return;
            }
			if (typeof(T) == typeof(Taxes))
            {
                _taxesController.Truncate();
				return;
            }
        }
    }
}