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
    public class CompanyNamesController : IEntityController<CompanyNames>
    {
        private readonly SQLiteConnection _connection;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _currentLanguage;

        public CompanyNamesController(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            _log.Info(string.Format(_currentLanguage.GetWord("LanguageSet"), "CompanyNames", language.Identifier));
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
            _log.Info(string.Format(_currentLanguage.GetWord("TableCreated"), "CompanyNames", result));
            _connection.Close();
            return result;
        }

        public List<CompanyNames> Get()
        {
            var list = new List<CompanyNames>();
            var sql = "SELECT * FROM CompanyNames";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var companyNames = GetCompanyNamesFromReader(reader);
                        list.Add(companyNames);
                    }
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGet"), "CompanyNames", string.Join("; ", list)));
            _connection.Close();
            return list;
        }

        public CompanyNames Get(long id)
        {
            CompanyNames companyName = null;
            var sql = "SELECT * FROM CompanyNames WHERE Id = @Id";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                PrepareCommandSelect(command, id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        companyName = GetCompanyNamesFromReader(reader);
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSingle"), "CompanyNames", companyName));
            _connection.Close();
            return companyName;
        }

        public ObservableCollection<CompanyNames> Get<TValue>(Expression<Func<CompanyNames, bool>> predicate = null,
            Expression<Func<CompanyNames, TValue>> orderBy = null)
        {
            if (predicate == null && orderBy == null)
                return GetNoPredicateNoOrderBy();
            if (predicate != null && orderBy == null)
                return GetPredicateOnly(predicate);
            return predicate == null ? GetOrderByOnly(orderBy) : GetPredicateAndOrderBy(predicate, orderBy);
        }

        private ObservableCollection<CompanyNames> GetNoPredicateNoOrderBy()
        {
            var result = GetCollection(Get());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyNames", null, null,
                string.Join(";", result)));
            return result;
        }

        private ObservableCollection<CompanyNames> GetPredicateOnly(
            Expression<Func<CompanyNames, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyNames", predicate,
                null, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<CompanyNames> GetOrderByOnly<TValue>(
            Expression<Func<CompanyNames, TValue>> orderBy = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyNames", null,
                orderBy, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<CompanyNames> GetPredicateAndOrderBy<TValue>(
            Expression<Func<CompanyNames, bool>> predicate = null,
            Expression<Func<CompanyNames, TValue>> orderBy = null)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyNames", predicate,
                orderBy, string.Join(";", result)));
            return result;
        }

        public CompanyNames Get(Expression<Func<CompanyNames, bool>> predicate)
        {
            var result = GetQueryable().Where(predicate).FirstOrDefault();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSinglePredicate"), "CompanyNames", predicate,
                string.Join(";", result)));
            return result;
        }

        public int Insert(CompanyNames entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandInsert(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedInsert"), "CompanyNames", entity, result));
            _connection.Close();
            return result;
        }

        public int Update(CompanyNames entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandUpdate(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedUpdate"), "CompanyNames", entity, result));
            _connection.Close();
            return result;
        }

        public int Delete(CompanyNames entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareDeleteCommand(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedDelete"), "CompanyNames", entity, result));
            _connection.Close();
            return result;
        }

        public int Count(Expression<Func<CompanyNames, bool>> predicate = null)
        {
            return predicate == null ? CountNoPredicate() : CountPredicate(predicate);
        }

        private int CountNoPredicate()
        {
            var count = 0;
            const string sql = "SELECT COUNT(Id) FROM CompanyNames";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader != null && reader.Read())

                        count = Convert.ToInt32(reader[0].ToString());
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "CompanyNames", null, count));
            _connection.Close();
            return count;
        }

        private int CountPredicate(Expression<Func<CompanyNames, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var count = GetQueryable().Where(predicate).Count();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "CompanyNames", predicate, count));
            return count;
        }

        private string GetCreateTableSQL()
        {
            return "CREATE TABLE IF NOT EXISTS CompanyNames (" +
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

        private CompanyNames GetCompanyNamesFromReader(SQLiteDataReader reader)
        {
            return new CompanyNames
            {
                Id = Convert.ToInt64(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                Deleted = Convert.ToBoolean(reader["Deleted"].ToString()),
                ModifiedAt = Convert.ToDateTime(reader["ModifiedAt"].ToString())
            };
        }

        private ObservableCollection<CompanyNames> GetCollection(IEnumerable<CompanyNames> oldList)
        {
            var collection = new ObservableCollection<CompanyNames>();
            foreach (var item in oldList)
                collection.Add(item);
            return collection;
        }

        private void PrepareCommandInsert(SQLiteCommand command, CompanyNames companyNames)
        {
            command.CommandText = "INSERT INTO CompanyNames (Id, Name, CreatedAt, Deleted, ModifiedAt) " +
                                  "VALUES (@Id, @Name, @CreatedAt, @Deleted, @ModifiedAt)";
            command.Prepare();
            AddParametersUpdateInsert(command, companyNames);
        }

        private void AddParametersUpdateInsert(SQLiteCommand command, CompanyNames companyNames)
        {
            command.Parameters.AddWithValue("@Id", companyNames.Id);
            command.Parameters.AddWithValue("@Name", companyNames.Name);
            command.Parameters.AddWithValue("@CreatedAt", companyNames.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@Deleted", companyNames.Deleted);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private void PrepareCommandUpdate(SQLiteCommand command, CompanyNames companyNames)
        {
            command.CommandText =
                "UPDATE CompanyNames SET Name = @Name, CreatedAt = @CreatedAt, Deleted = @Deleted, " +
                "ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            AddParametersUpdateInsert(command, companyNames);
        }

        private void PrepareDeleteCommand(SQLiteCommand command, CompanyNames companyNames)
        {
            command.CommandText = "UPDATE CompanyNames SET Deleted = true, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            command.Parameters.AddWithValue("@Id", companyNames.Id);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private IQueryable<CompanyNames> GetQueryable()
        {
            return Get().AsQueryable();
        }
		
		public void Truncate()
		{
            const string sql = "DELETE FROM CompanyNames";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedTruncate"), "CompanyNames"));
            _connection.Close();
		}
    }
}