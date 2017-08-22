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
    public class StockMarketController : IEntityController<StockMarket>
    {
        private readonly SQLiteConnection _connection;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _currentLanguage;

        public StockMarketController(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            _log.Info(string.Format(_currentLanguage.GetWord("LanguageSet"), "StockMarket", language.Identifier));
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
            _log.Info(string.Format(_currentLanguage.GetWord("TableCreated"), "StockMarket", result));
            _connection.Close();
            return result;
        }

        public List<StockMarket> Get()
        {
            var list = new List<StockMarket>();
            var sql = "SELECT * FROM StockMarket";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var stockmarket = GetStockMarketFromReader(reader);
                        list.Add(stockmarket);
                    }
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGet"), "StockMarket", string.Join("; ", list)));
            _connection.Close();
            return list;
        }

        public StockMarket Get(long id)
        {
            StockMarket stockMarket = null;
            var sql = "SELECT * FROM StockMarket WHERE Id = @Id";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                PrepareCommandSelect(command, id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        stockMarket = GetStockMarketFromReader(reader);
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSingle"), "StockMarket", stockMarket));
            _connection.Close();
            return stockMarket;
        }

        public ObservableCollection<StockMarket> Get<TValue>(Expression<Func<StockMarket, bool>> predicate = null,
            Expression<Func<StockMarket, TValue>> orderBy = null)
        {
            if (predicate == null && orderBy == null)
                return GetNoPredicateNoOrderBy();
            if (predicate != null && orderBy == null)
                return GetPredicateOnly(predicate);
            return predicate == null ? GetOrderByOnly(orderBy) : GetPredicateAndOrderBy(predicate, orderBy);
        }

        private ObservableCollection<StockMarket> GetNoPredicateNoOrderBy()
        {
            var result = GetCollection(Get());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockMarket", null, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<StockMarket> GetPredicateOnly(Expression<Func<StockMarket, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockMarket", predicate,
                null, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<StockMarket> GetOrderByOnly<TValue>(
            Expression<Func<StockMarket, TValue>> orderBy = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockMarket", null,
                orderBy, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<StockMarket> GetPredicateAndOrderBy<TValue>(
            Expression<Func<StockMarket, bool>> predicate = null,
            Expression<Func<StockMarket, TValue>> orderBy = null)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockMarket", predicate,
                orderBy, string.Join(";", result)));
            return result;
        }

        public StockMarket Get(Expression<Func<StockMarket, bool>> predicate)
        {
            var result = GetQueryable().Where(predicate).FirstOrDefault();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSinglePredicate"), "StockMarket", predicate,
                string.Join(";", result)));
            return result;
        }

        public int Insert(StockMarket entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandInsert(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedInsert"), "StockMarket", entity, result));
            _connection.Close();
            return result;
        }

        public int Update(StockMarket entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandUpdate(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedUpdate"), "StockMarket", entity, result));
            _connection.Close();
            return result;
        }

        public int Delete(StockMarket entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareDeleteCommand(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedDelete"), "StockMarket", entity, result));
            _connection.Close();
            return result;
        }

        public int Count(Expression<Func<StockMarket, bool>> predicate = null)
        {
            return predicate == null ? CountNoPredicate() : CountPredicate(predicate);
        }

        private int CountNoPredicate()
        {
            var count = 0;
            const string sql = "SELECT COUNT(Id) FROM StockMarket";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader != null && reader.Read())

                        count = Convert.ToInt32(reader[0].ToString());
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "StockMarket", null, count));
            _connection.Close();
            return count;
        }

        private int CountPredicate(Expression<Func<StockMarket, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var count = GetQueryable().Where(predicate).Count();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "StockMarket", predicate, count));
            return count;
        }

        private string GetCreateTableSQL()
        {
            return "CREATE TABLE IF NOT EXISTS StockMarket (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE," +
                   "Name TEXT NOT NULL," +
                   "CreatedAt TEXT NOT NULL," +
                   "Deleted BOOLEAN NOT NULL," +
                   "ModifiedAt TEXT NOT NULL)";
        }

        private void PrepareCommandSelect(SQLiteCommand command, long id)
        {
            command.Prepare();
            command.Parameters.AddWithValue("@Id", id);
        }

        private StockMarket GetStockMarketFromReader(SQLiteDataReader reader)
        {
            return new StockMarket
            {
                Id = Convert.ToInt64(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                Deleted = Convert.ToBoolean(reader["Deleted"].ToString()),
                ModifiedAt = Convert.ToDateTime(reader["ModifiedAt"].ToString())
            };
        }

        private ObservableCollection<StockMarket> GetCollection(IEnumerable<StockMarket> oldList)
        {
            var collection = new ObservableCollection<StockMarket>();
            foreach (var item in oldList)
                collection.Add(item);
            return collection;
        }

        private void PrepareCommandInsert(SQLiteCommand command, StockMarket stockMarket)
        {
            command.CommandText = "INSERT INTO StockMarket (Id, Name, CreatedAt, Deleted, ModifiedAt) " +
                                  "VALUES (@Id, @Name, @CreatedAt, @Deleted, @ModifiedAt)";
            command.Prepare();
            AddParametersUpdateInsert(command, stockMarket);
        }

        private void AddParametersUpdateInsert(SQLiteCommand command, StockMarket stockMarket)
        {
            command.Parameters.AddWithValue("@Id", stockMarket.Id);
            command.Parameters.AddWithValue("@Name", stockMarket.Name);
            command.Parameters.AddWithValue("@CreatedAt", stockMarket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@Deleted", stockMarket.Deleted);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private void PrepareCommandUpdate(SQLiteCommand command, StockMarket stockMarket)
        {
            command.CommandText =
                "UPDATE StockMarket SET Name = @Name, CreatedAt = @CreatedAt, Deleted = @Deleted, " +
                "ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            AddParametersUpdateInsert(command, stockMarket);
        }

        private void PrepareDeleteCommand(SQLiteCommand command, StockMarket stockMarket)
        {
            command.CommandText = "UPDATE StockMarket SET Deleted = true, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            command.Parameters.AddWithValue("@Id", stockMarket.Id);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private IQueryable<StockMarket> GetQueryable()
        {
            return Get().AsQueryable();
        }
		
		public void Truncate()
		{
            const string sql = "DELETE FROM StockMarket";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedTruncate"), "StockMarket"));
            _connection.Close();
		}
    }
}