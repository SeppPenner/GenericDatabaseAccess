using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using log4net;
using Languages.Interfaces;
using StockExchangeGame.Database.Models;

namespace StockExchangeGame.Database.Generic
{
    // ReSharper disable once UnusedMember.Global
    public class StockController : IEntityController<Stock>
    {
        private readonly SQLiteConnection _connection;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _currentLanguage;

        public StockController(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            _log.Info(string.Format(_currentLanguage.GetWord("LanguageSet"), "Stock", language.Identifier));
        }

        public ILanguage GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        public int CreateTable()
        {
            int result;
            var sql = GetCreateTableSQL();
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("TableCreated"), "Stock", result));
            _connection.Close();
            return result;
        }

        public List<Stock> Get()
        {
            var list = new List<Stock>();
            var sql = "SELECT * FROM Stock";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var stock = GetStockFromReader(reader);
                        list.Add(stock);
                    }
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGet"), "Stock", string.Join("; ", list)));
            _connection.Close();
            return list;
        }

        public Stock Get(long id)
        {
            Stock stock = null;
            var sql = "SELECT * FROM Stock WHERE Id = @Id";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                PrepareCommandSelect(command, id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        stock = GetStockFromReader(reader);
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSingle"), "Stock", stock));
            _connection.Close();
            return stock;
        }

        public ObservableCollection<Stock> Get<TValue>(Expression<Func<Stock, bool>> predicate = null,
            Expression<Func<Stock, TValue>> orderBy = null)
        {
            if (predicate == null && orderBy == null)
                return GetNoPredicateNoOrderBy();
            if (predicate != null && orderBy == null)
                return GetPredicateOnly(predicate);
            return predicate == null ? GetOrderByOnly(orderBy) : GetPredicateAndOrderBy(predicate, orderBy);
        }

        private ObservableCollection<Stock> GetNoPredicateNoOrderBy()
        {
            var result = GetCollection(Get());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Stock", null, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Stock> GetPredicateOnly(Expression<Func<Stock, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Stock", predicate, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Stock> GetOrderByOnly<TValue>(Expression<Func<Stock, TValue>> orderBy = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Stock", null, orderBy,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Stock> GetPredicateAndOrderBy<TValue>(
            Expression<Func<Stock, bool>> predicate = null,
            Expression<Func<Stock, TValue>> orderBy = null)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Stock", predicate,
                orderBy, string.Join(";", result)));
            return result;
        }

        public Stock Get(Expression<Func<Stock, bool>> predicate)
        {
            var result = GetQueryable().Where(predicate).FirstOrDefault();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSinglePredicate"), "Stock", predicate,
                string.Join(";", result)));
            return result;
        }

        public int Insert(Stock entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandInsert(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedInsert"), "Stock", entity, result));
            _connection.Close();
            return result;
        }

        public int Update(Stock entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandUpdate(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedUpdate"), "Stock", entity, result));
            _connection.Close();
            return result;
        }

        public int Delete(Stock entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareDeleteCommand(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedDelete"), "Stock", entity, result));
            _connection.Close();
            return result;
        }

        public int Count(Expression<Func<Stock, bool>> predicate = null)
        {
            return predicate == null ? CountNoPredicate() : CountPredicate(predicate);
        }

        private int CountNoPredicate()
        {
            var count = 0;
            const string sql = "SELECT COUNT(Id) FROM Stock";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader != null && reader.Read())

                        count = Convert.ToInt32(reader[0].ToString());
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "Stock", null, count));
            _connection.Close();
            return count;
        }

        private int CountPredicate(Expression<Func<Stock, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var count = GetQueryable().Where(predicate).Count();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "Stock", predicate, count));
            return count;
        }

        private string GetCreateTableSQL()
        {
            return "CREATE TABLE IF NOT EXISTS Stock (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE," +
                   "Name TEXT NOT NULL," +
                   "CreatedAt TEXT NOT NULL," +
                   "Total INTEGER NOT NULL," +
                   "Deleted BOOLEAN NOT NULL," +
                   "Used INTEGER NOT NULL," +
                   "ModifiedAt TEXT NOT NULL," +
                   "StockMarketId INTEGER NOT NULL)";
        }

        private void PrepareCommandSelect(SQLiteCommand command, long id)
        {
            command.Prepare();
            command.Parameters.AddWithValue("@Id", id);
        }

        private Stock GetStockFromReader(SQLiteDataReader reader)
        {
            return new Stock
            {
                Id = Convert.ToInt64(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                Total = Convert.ToInt64(reader["Total"].ToString()),
                Deleted = Convert.ToBoolean(reader["Deleted"].ToString()),
                Used = Convert.ToInt64(reader["Used"].ToString()),
                ModifiedAt = Convert.ToDateTime(reader["ModifiedAt"].ToString()),
                StockMarketId = Convert.ToInt64(reader["StockMarketId"].ToString())
            };
        }

        private ObservableCollection<Stock> GetCollection(IEnumerable<Stock> oldList)
        {
            var collection = new ObservableCollection<Stock>();
            foreach (var item in oldList)
                collection.Add(item);
            return collection;
        }

        private void PrepareCommandInsert(SQLiteCommand command, Stock stock)
        {
            command.CommandText = "INSERT INTO Stock (Id, Name, CreatedAt, Total, Deleted, Used, " +
                                  "ModifiedAt, StockMarketId) VALUES (@Id, @Name, @CreatedAt, @Total, @Deleted, " +
                                  "@Used, @ModifiedAt, @StockMarketId)";
            command.Prepare();
            AddParametersUpdateInsert(command, stock);
        }

        private void AddParametersUpdateInsert(SQLiteCommand command, Stock stock)
        {
            command.Parameters.AddWithValue("@Id", stock.Id);
            command.Parameters.AddWithValue("@Name", stock.Name);
            command.Parameters.AddWithValue("@CreatedAt", stock.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@Total", stock.Total);
            command.Parameters.AddWithValue("@Deleted", stock.Deleted);
            command.Parameters.AddWithValue("@Used", stock.Used);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@StockMarketId", stock.StockMarketId);
        }

        private void PrepareCommandUpdate(SQLiteCommand command, Stock stock)
        {
            command.CommandText =
                "UPDATE Stock SET Name = @Name, CreatedAt = @CreatedAt, Total = @Total," +
                " Deleted = @Deleted, Used = @Used, ModifiedAt = @ModifiedAt, StockMarketId " +
                "= @StockMarketId WHERE Id = @Id";
            command.Prepare();
            AddParametersUpdateInsert(command, stock);
        }

        private void PrepareDeleteCommand(SQLiteCommand command, Stock stock)
        {
            command.CommandText = "UPDATE Stock SET Deleted = true, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            command.Parameters.AddWithValue("@Id", stock.Id);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private IQueryable<Stock> GetQueryable()
        {
            return Get().AsQueryable();
        }
		
		public void Truncate()
		{
            const string sql = "DELETE FROM Stock";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedTruncate"), "Stock"));
            _connection.Close();
		}
    }
}