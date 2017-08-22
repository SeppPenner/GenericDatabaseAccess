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
    public class BoughtController : IEntityController<Bought>
    {
        private readonly SQLiteConnection _connection;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _currentLanguage;

        public BoughtController(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            _log.Info(string.Format(_currentLanguage.GetWord("LanguageSet"), "Bought", language.Identifier));
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
            _log.Info(string.Format(_currentLanguage.GetWord("TableCreated"), "Bought", result));
            _connection.Close();
            return result;
        }

        public List<Bought> Get()
        {
            var list = new List<Bought>();
            var sql = "SELECT * FROM Bought";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var bought = GetBoughtFromReader(reader);
                        list.Add(bought);
                    }
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGet"), "Bought", string.Join("; ", list)));
            _connection.Close();
            return list;
        }

        public Bought Get(long id)
        {
            Bought bought = null;
            var sql = "SELECT * FROM Bought WHERE Id = @Id";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                PrepareCommandSelect(command, id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        bought = GetBoughtFromReader(reader);
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSingle"), "Bought", bought));
            _connection.Close();
            return bought;
        }

        public ObservableCollection<Bought> Get<TValue>(Expression<Func<Bought, bool>> predicate = null,
            Expression<Func<Bought, TValue>> orderBy = null)
        {
            if (predicate == null && orderBy == null)
                return GetNoPredicateNoOrderBy();
            if (predicate != null && orderBy == null)
                return GetPredicateOnly(predicate);
            return predicate == null ? GetOrderByOnly(orderBy) : GetPredicateAndOrderBy(predicate, orderBy);
        }

        private ObservableCollection<Bought> GetNoPredicateNoOrderBy()
        {
            var result = GetCollection(Get());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Bought", null, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Bought> GetPredicateOnly(Expression<Func<Bought, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Bought", predicate, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Bought> GetOrderByOnly<TValue>(Expression<Func<Bought, TValue>> orderBy = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Bought", null, orderBy,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Bought> GetPredicateAndOrderBy<TValue>(
            Expression<Func<Bought, bool>> predicate = null,
            Expression<Func<Bought, TValue>> orderBy = null)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Bought", predicate,
                orderBy, string.Join(";", result)));
            return result;
        }

        public Bought Get(Expression<Func<Bought, bool>> predicate)
        {
            var result = GetQueryable().Where(predicate).FirstOrDefault();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSinglePredicate"), "Bought", predicate,
                string.Join(";", result)));
            return result;
        }

        public int Insert(Bought entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandInsert(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedInsert"), "Bought", entity, result));
            _connection.Close();
            return result;
        }

        public int Update(Bought entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandUpdate(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedUpdate"), "Bought", entity, result));
            _connection.Close();
            return result;
        }

        public int Delete(Bought entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareDeleteCommand(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedDelete"), "Bought", entity, result));
            _connection.Close();
            return result;
        }

        public int Count(Expression<Func<Bought, bool>> predicate = null)
        {
            return predicate == null ? CountNoPredicate() : CountPredicate(predicate);
        }

        private int CountNoPredicate()
        {
            var count = 0;
            const string sql = "SELECT COUNT(Id) FROM Bought";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader != null && reader.Read())

                        count = Convert.ToInt32(reader[0].ToString());
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "Bought", null, count));
            _connection.Close();
            return count;
        }

        private int CountPredicate(Expression<Func<Bought, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var count = GetQueryable().Where(predicate).Count();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "Bought", predicate, count));
            return count;
        }

        private string GetCreateTableSQL()
        {
            return "CREATE TABLE IF NOT EXISTS Bought (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE," +
                   "Amount INTEGER NOT NULL," +
                   "CreatedAt TEXT NOT NULL," +
                   "DateBought TEXT NOT NULL," +
                   "Deleted BOOLEAN NOT NULL," +
                   "MerchantId INTEGER NOT NULL," +
                   "ModifiedAt TEXT NOT NULL," +
                   "StockId INTEGER NOT NULL," +
                   "ValuePerStockInEuro DOUBLE NOT NULL)";
        }

        private void PrepareCommandSelect(SQLiteCommand command, long id)
        {
            command.Prepare();
            command.Parameters.AddWithValue("@Id", id);
        }

        private Bought GetBoughtFromReader(SQLiteDataReader reader)
        {
            return new Bought
            {
                Id = Convert.ToInt64(reader["Id"].ToString()),
                Amount = Convert.ToInt64(reader["Amount"].ToString()),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                DateBought = Convert.ToDateTime(reader["DateBought"].ToString()),
                Deleted = Convert.ToBoolean(reader["Deleted"].ToString()),
                MerchantId = Convert.ToInt64(reader["MerchantId"].ToString()),
                ModifiedAt = Convert.ToDateTime(reader["ModifiedAt"].ToString()),
                StockId = Convert.ToInt64(reader["StockId"].ToString()),
                ValuePerStockInEuro = Convert.ToDouble(reader["ValuePerStockInEuro"].ToString())
            };
        }

        private ObservableCollection<Bought> GetCollection(IEnumerable<Bought> oldList)
        {
            var collection = new ObservableCollection<Bought>();
            foreach (var item in oldList)
                collection.Add(item);
            return collection;
        }

        private void PrepareCommandInsert(SQLiteCommand command, Bought bought)
        {
            command.CommandText = "INSERT INTO Bought (Id, Amount, CreatedAt, DateBought, Deleted, MerchantId, " +
                                  "ModifiedAt, StockId, ValuePerStockInEuro) VALUES (@Id, @Amount, @CreatedAt, " +
                                  "@DateBought, @Deleted, @MerchantId, @ModifiedAt, @StockId, @ValuePerStockInEuro)";
            command.Prepare();
            AddParametersUpdateInsert(command, bought);
        }

        private void AddParametersUpdateInsert(SQLiteCommand command, Bought bought)
        {
            command.Parameters.AddWithValue("@Id", bought.Id);
            command.Parameters.AddWithValue("@Amount", bought.Amount);
            command.Parameters.AddWithValue("@CreatedAt", bought.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@DateBought", bought.DateBought.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@Deleted", bought.Deleted);
            command.Parameters.AddWithValue("@MerchantId", bought.MerchantId);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@StockId", bought.StockId);
            command.Parameters.AddWithValue("@ValuePerStockInEuro", bought.ValuePerStockInEuro);
        }

        private void PrepareCommandUpdate(SQLiteCommand command, Bought bought)
        {
            command.CommandText =
                "UPDATE Bought SET Amount = @Amount, CreatedAt = @CreatedAt, DateBought = @DateBought," +
                " Deleted = @Deleted, MerchantId = @MerchantId, ModifiedAt = @ModifiedAt, StockId = @StockId, " +
                "ValuePerStockInEuro = @ValuePerStockInEuro WHERE Id = @Id";
            command.Prepare();
            AddParametersUpdateInsert(command, bought);
        }

        private void PrepareDeleteCommand(SQLiteCommand command, Bought bought)
        {
            command.CommandText = "UPDATE Bought SET Deleted = true, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            command.Parameters.AddWithValue("@Id", bought.Id);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private IQueryable<Bought> GetQueryable()
        {
            return Get().AsQueryable();
        }
		
		public void Truncate()
		{
            const string sql = "DELETE FROM Bought";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedTruncate"), "Bought"));
            _connection.Close();
		}
    }
}