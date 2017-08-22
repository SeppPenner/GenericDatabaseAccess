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
    public class StockHistoryController : IEntityController<StockHistory>
    {
        private readonly SQLiteConnection _connection;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _currentLanguage;

        public StockHistoryController(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            _log.Info(string.Format(_currentLanguage.GetWord("LanguageSet"), "StockHistory", language.Identifier));
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
            _log.Info(string.Format(_currentLanguage.GetWord("TableCreated"), "StockHistory", result));
            _connection.Close();
            return result;
        }

        public List<StockHistory> Get()
        {
            var list = new List<StockHistory>();
            var sql = "SELECT * FROM StockHistory";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var stockhistory = GetStockHistoryFromReader(reader);
                        list.Add(stockhistory);
                    }
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGet"), "StockHistory", string.Join("; ", list)));
            _connection.Close();
            return list;
        }

        public StockHistory Get(long id)
        {
            StockHistory stockHistory = null;
            var sql = "SELECT * FROM StockHistory WHERE Id = @Id";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                PrepareCommandSelect(command, id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        stockHistory = GetStockHistoryFromReader(reader);
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSingle"), "StockHistory", stockHistory));
            _connection.Close();
            return stockHistory;
        }

        public ObservableCollection<StockHistory> Get<TValue>(Expression<Func<StockHistory, bool>> predicate = null,
            Expression<Func<StockHistory, TValue>> orderBy = null)
        {
            if (predicate == null && orderBy == null)
                return GetNoPredicateNoOrderBy();
            if (predicate != null && orderBy == null)
                return GetPredicateOnly(predicate);
            return predicate == null ? GetOrderByOnly(orderBy) : GetPredicateAndOrderBy(predicate, orderBy);
        }

        private ObservableCollection<StockHistory> GetNoPredicateNoOrderBy()
        {
            var result = GetCollection(Get());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockHistory", null, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<StockHistory> GetPredicateOnly(
            Expression<Func<StockHistory, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockHistory", predicate,
                null, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<StockHistory> GetOrderByOnly<TValue>(
            Expression<Func<StockHistory, TValue>> orderBy = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockHistory", null,
                orderBy, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<StockHistory> GetPredicateAndOrderBy<TValue>(
            Expression<Func<StockHistory, bool>> predicate = null,
            Expression<Func<StockHistory, TValue>> orderBy = null)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "StockHistory", predicate,
                orderBy, string.Join(";", result)));
            return result;
        }

        public StockHistory Get(Expression<Func<StockHistory, bool>> predicate)
        {
            var result = GetQueryable().Where(predicate).FirstOrDefault();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSinglePredicate"), "StockHistory", predicate,
                string.Join(";", result)));
            return result;
        }

        public int Insert(StockHistory entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandInsert(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedInsert"), "StockHistory", entity, result));
            _connection.Close();
            return result;
        }

        public int Update(StockHistory entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandUpdate(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedUpdate"), "StockHistory", entity, result));
            _connection.Close();
            return result;
        }

        public int Delete(StockHistory entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareDeleteCommand(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedDelete"), "StockHistory", entity, result));
            _connection.Close();
            return result;
        }

        public int Count(Expression<Func<StockHistory, bool>> predicate = null)
        {
            return predicate == null ? CountNoPredicate() : CountPredicate(predicate);
        }

        private int CountNoPredicate()
        {
            var count = 0;
            const string sql = "SELECT COUNT(Id) FROM StockHistory";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader != null && reader.Read())

                        count = Convert.ToInt32(reader[0].ToString());
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "StockHistory", null, count));
            _connection.Close();
            return count;
        }

        private int CountPredicate(Expression<Func<StockHistory, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var count = GetQueryable().Where(predicate).Count();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "StockHistory", predicate, count));
            return count;
        }

        private string GetCreateTableSQL()
        {
            return "CREATE TABLE IF NOT EXISTS StockHistory (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE," +
                   "PriceDate TEXT NOT NULL," +
                   "CreatedAt TEXT NOT NULL," +
                   "PricePerStock DOUBLE NOT NULL," +
                   "Deleted BOOLEAN NOT NULL," +
                   "StockId INTEGER NOT NULL," +
                   "ModifiedAt TEXT NOT NULL)";
        }

        private void PrepareCommandSelect(SQLiteCommand command, long id)
        {
            command.Prepare();
            command.Parameters.AddWithValue("@Id", id);
        }

        private StockHistory GetStockHistoryFromReader(SQLiteDataReader reader)
        {
            return new StockHistory
            {
                Id = Convert.ToInt64(reader["Id"].ToString()),
                PriceDate = Convert.ToDateTime(reader["PriceDate"].ToString()),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                PricePerStock = Convert.ToDouble(reader["PricePerStock"].ToString()),
                Deleted = Convert.ToBoolean(reader["Deleted"].ToString()),
                StockId = Convert.ToInt64(reader["StockId"].ToString()),
                ModifiedAt = Convert.ToDateTime(reader["ModifiedAt"].ToString())
            };
        }

        private ObservableCollection<StockHistory> GetCollection(IEnumerable<StockHistory> oldList)
        {
            var collection = new ObservableCollection<StockHistory>();
            foreach (var item in oldList)
                collection.Add(item);
            return collection;
        }

        private void PrepareCommandInsert(SQLiteCommand command, StockHistory stockHistory)
        {
            command.CommandText =
                "INSERT INTO StockHistory (Id, PriceDate, CreatedAt, PricePerStock, Deleted, StockId, " +
                "ModifiedAt) VALUES (@Id, @PriceDate, @CreatedAt, @PricePerStock, @Deleted, " +
                "@StockId, @ModifiedAt)";
            command.Prepare();
            AddParametersUpdateInsert(command, stockHistory);
        }

        private void AddParametersUpdateInsert(SQLiteCommand command, StockHistory stockHistory)
        {
            command.Parameters.AddWithValue("@Id", stockHistory.Id);
            command.Parameters.AddWithValue("@PriceDate", stockHistory.PriceDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@CreatedAt", stockHistory.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@PricePerStock", stockHistory.PricePerStock);
            command.Parameters.AddWithValue("@Deleted", stockHistory.Deleted);
            command.Parameters.AddWithValue("@StockId", stockHistory.StockId);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private void PrepareCommandUpdate(SQLiteCommand command, StockHistory stockHistory)
        {
            command.CommandText =
                "UPDATE StockHistory SET PriceDate = @PriceDate, CreatedAt = @CreatedAt, PricePerStock = @PricePerStock," +
                " Deleted = @Deleted, StockId = @StockId, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            AddParametersUpdateInsert(command, stockHistory);
        }

        private void PrepareDeleteCommand(SQLiteCommand command, StockHistory stockHistory)
        {
            command.CommandText = "UPDATE StockHistory SET Deleted = true, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            command.Parameters.AddWithValue("@Id", stockHistory.Id);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private IQueryable<StockHistory> GetQueryable()
        {
            return Get().AsQueryable();
        }
		
		public void Truncate()
		{
            const string sql = "DELETE FROM StockHistory";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedTruncate"), "StockHistory"));
            _connection.Close();
		}
    }
}