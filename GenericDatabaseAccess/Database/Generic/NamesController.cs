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
    public class NamesController : IEntityController<Names>
    {
        private readonly SQLiteConnection _connection;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _currentLanguage;

        public NamesController(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            _log.Info(string.Format(_currentLanguage.GetWord("LanguageSet"), "Names", language.Identifier));
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
            _log.Info(string.Format(_currentLanguage.GetWord("TableCreated"), "Names", result));
            _connection.Close();
            return result;
        }

        public List<Names> Get()
        {
            var list = new List<Names>();
            var sql = "SELECT * FROM Names";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var names = GetNamesFromReader(reader);
                        list.Add(names);
                    }
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGet"), "Names", string.Join("; ", list)));
            _connection.Close();
            return list;
        }

        public Names Get(long id)
        {
            Names name = null;
            var sql = "SELECT * FROM Names WHERE Id = @Id";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                PrepareCommandSelect(command, id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        name = GetNamesFromReader(reader);
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSingle"), "Names", name));
            _connection.Close();
            return name;
        }

        public ObservableCollection<Names> Get<TValue>(Expression<Func<Names, bool>> predicate = null,
            Expression<Func<Names, TValue>> orderBy = null)
        {
            if (predicate == null && orderBy == null)
                return GetNoPredicateNoOrderBy();
            if (predicate != null && orderBy == null)
                return GetPredicateOnly(predicate);
            return predicate == null ? GetOrderByOnly(orderBy) : GetPredicateAndOrderBy(predicate, orderBy);
        }

        private ObservableCollection<Names> GetNoPredicateNoOrderBy()
        {
            var result = GetCollection(Get());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Names", null, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Names> GetPredicateOnly(Expression<Func<Names, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Names", predicate, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Names> GetOrderByOnly<TValue>(Expression<Func<Names, TValue>> orderBy = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Names", null, orderBy,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<Names> GetPredicateAndOrderBy<TValue>(
            Expression<Func<Names, bool>> predicate = null,
            Expression<Func<Names, TValue>> orderBy = null)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "Names", predicate,
                orderBy, string.Join(";", result)));
            return result;
        }

        public Names Get(Expression<Func<Names, bool>> predicate)
        {
            var result = GetQueryable().Where(predicate).FirstOrDefault();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSinglePredicate"), "Names", predicate,
                string.Join(";", result)));
            return result;
        }

        public int Insert(Names entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandInsert(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedInsert"), "Names", entity, result));
            _connection.Close();
            return result;
        }

        public int Update(Names entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandUpdate(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedUpdate"), "Names", entity, result));
            _connection.Close();
            return result;
        }

        public int Delete(Names entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareDeleteCommand(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedDelete"), "Names", entity, result));
            _connection.Close();
            return result;
        }

        public int Count(Expression<Func<Names, bool>> predicate = null)
        {
            return predicate == null ? CountNoPredicate() : CountPredicate(predicate);
        }

        private int CountNoPredicate()
        {
            var count = 0;
            const string sql = "SELECT COUNT(Id) FROM Names";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader != null && reader.Read())

                        count = Convert.ToInt32(reader[0].ToString());
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "Names", null, count));
            _connection.Close();
            return count;
        }

        private int CountPredicate(Expression<Func<Names, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var count = GetQueryable().Where(predicate).Count();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "Names", predicate, count));
            return count;
        }

        private string GetCreateTableSQL()
        {
            return "CREATE TABLE IF NOT EXISTS Names (" +
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

        private Names GetNamesFromReader(SQLiteDataReader reader)
        {
            return new Names
            {
                Id = Convert.ToInt64(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                Deleted = Convert.ToBoolean(reader["Deleted"].ToString()),
                ModifiedAt = Convert.ToDateTime(reader["ModifiedAt"].ToString())
            };
        }

        private ObservableCollection<Names> GetCollection(IEnumerable<Names> oldList)
        {
            var collection = new ObservableCollection<Names>();
            foreach (var item in oldList)
                collection.Add(item);
            return collection;
        }

        private void PrepareCommandInsert(SQLiteCommand command, Names names)
        {
            command.CommandText = "INSERT INTO Names (Id, Name, CreatedAt, Deleted, ModifiedAt) " +
                                  "VALUES (@Id, @Name, @CreatedAt, @Deleted, @ModifiedAt)";
            command.Prepare();
            AddParametersUpdateInsert(command, names);
        }

        private void AddParametersUpdateInsert(SQLiteCommand command, Names names)
        {
            command.Parameters.AddWithValue("@Id", names.Id);
            command.Parameters.AddWithValue("@Name", names.Name);
            command.Parameters.AddWithValue("@CreatedAt", names.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@Deleted", names.Deleted);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private void PrepareCommandUpdate(SQLiteCommand command, Names names)
        {
            command.CommandText =
                "UPDATE Names SET Name = @Name, CreatedAt = @CreatedAt, Deleted = @Deleted, " +
                "ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            AddParametersUpdateInsert(command, names);
        }

        private void PrepareDeleteCommand(SQLiteCommand command, Names names)
        {
            command.CommandText = "UPDATE Names SET Deleted = true, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            command.Parameters.AddWithValue("@Id", names.Id);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private IQueryable<Names> GetQueryable()
        {
            return Get().AsQueryable();
        }
		
		public void Truncate()
		{
            const string sql = "DELETE FROM Names";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedTruncate"), "Names"));
            _connection.Close();
		}
    }
}