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
    public class CompanyEndingsController : IEntityController<CompanyEndings>
    {
        private readonly SQLiteConnection _connection;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _currentLanguage;

        public CompanyEndingsController(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public void SetCurrentLanguage(ILanguage language)
        {
            _currentLanguage = language;
            _log.Info(string.Format(_currentLanguage.GetWord("LanguageSet"), "CompanyEndings", language.Identifier));
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
            _connection.Close();
            _log.Info(string.Format(_currentLanguage.GetWord("TableCreated"), "CompanyEndings", result));
            return result;
        }

        public List<CompanyEndings> Get()
        {
            var list = new List<CompanyEndings>();
            var sql = "SELECT * FROM CompanyEndings";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var companyEndings = GetCompanyEndingsFromReader(reader);
                        list.Add(companyEndings);
                    }
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGet"), "CompanyEndings",
                string.Join("; ", list)));
            _connection.Close();
            return list;
        }

        public CompanyEndings Get(long id)
        {
            CompanyEndings companyEnding = null;
            var sql = "SELECT * FROM CompanyEndings WHERE Id = @Id";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                PrepareCommandSelect(command, id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        companyEnding = GetCompanyEndingsFromReader(reader);
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSingle"), "CompanyEndings", companyEnding));
            _connection.Close();
            return companyEnding;
        }

        public ObservableCollection<CompanyEndings> Get<TValue>(Expression<Func<CompanyEndings, bool>> predicate = null,
            Expression<Func<CompanyEndings, TValue>> orderBy = null)
        {
            if (predicate == null && orderBy == null)
                return GetNoPredicateNoOrderBy();
            if (predicate != null && orderBy == null)
                return GetPredicateOnly(predicate);
            return predicate == null ? GetOrderByOnly(orderBy) : GetPredicateAndOrderBy(predicate, orderBy);
        }

        private ObservableCollection<CompanyEndings> GetNoPredicateNoOrderBy()
        {
            var result = GetCollection(Get());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyEndings", null,
                null, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<CompanyEndings> GetPredicateOnly(
            Expression<Func<CompanyEndings, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyEndings",
                predicate, null, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<CompanyEndings> GetOrderByOnly<TValue>(
            Expression<Func<CompanyEndings, TValue>> orderBy = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyEndings", null,
                orderBy, string.Join(";", result)));
            return result;
        }

        private ObservableCollection<CompanyEndings> GetPredicateAndOrderBy<TValue>(
            Expression<Func<CompanyEndings, bool>> predicate = null,
            Expression<Func<CompanyEndings, TValue>> orderBy = null)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var result = GetCollection(GetQueryable().Where(predicate).OrderBy(orderBy).ToList());
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetPredicateOrderBy"), "CompanyEndings",
                predicate, orderBy, string.Join(";", result)));
            return result;
        }

        public CompanyEndings Get(Expression<Func<CompanyEndings, bool>> predicate)
        {
            var result = GetQueryable().Where(predicate).FirstOrDefault();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedGetSinglePredicate"), "CompanyEndings", predicate,
                string.Join(";", result)));
            return result;
        }

        public int Insert(CompanyEndings entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandInsert(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedInsert"), "CompanyEndings", entity, result));
            _connection.Close();
            return result;
        }

        public int Update(CompanyEndings entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareCommandUpdate(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedUpdate"), "CompanyEndings", entity, result));
            _connection.Close();
            return result;
        }

        public int Delete(CompanyEndings entity)
        {
            int result;
            _connection.Open();
            using (var command = new SQLiteCommand(_connection))
            {
                PrepareDeleteCommand(command, entity);
                result = command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedDelete"), "CompanyEndings", entity, result));
            _connection.Close();
            return result;
        }

        public int Count(Expression<Func<CompanyEndings, bool>> predicate = null)
        {
            return predicate == null ? CountNoPredicate() : CountPredicate(predicate);
        }

        private int CountNoPredicate()
        {
            var count = 0;
            const string sql = "SELECT COUNT(Id) FROM CompanyEndings";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader != null && reader.Read())

                        count = Convert.ToInt32(reader[0].ToString());
                }
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "CompanyEndings", null, count));
            _connection.Close();
            return count;
        }

        private int CountPredicate(Expression<Func<CompanyEndings, bool>> predicate = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var count = GetQueryable().Where(predicate).Count();
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedCount"), "CompanyEndings", predicate, count));
            return count;
        }

        private string GetCreateTableSQL()
        {
            return "CREATE TABLE IF NOT EXISTS CompanyEndings (" +
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

        private CompanyEndings GetCompanyEndingsFromReader(SQLiteDataReader reader)
        {
            return new CompanyEndings
            {
                Id = Convert.ToInt64(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                Deleted = Convert.ToBoolean(reader["Deleted"].ToString()),
                ModifiedAt = Convert.ToDateTime(reader["ModifiedAt"].ToString())
            };
        }

        private ObservableCollection<CompanyEndings> GetCollection(IEnumerable<CompanyEndings> oldList)
        {
            var collection = new ObservableCollection<CompanyEndings>();
            foreach (var item in oldList)
                collection.Add(item);
            return collection;
        }

        private void PrepareCommandInsert(SQLiteCommand command, CompanyEndings companyEndings)
        {
            command.CommandText = "INSERT INTO CompanyEndings (Id, Name, CreatedAt, Deleted, ModifiedAt) " +
                                  "VALUES (@Id, @Name, @CreatedAt, @Deleted, @ModifiedAt)";
            command.Prepare();
            AddParametersUpdateInsert(command, companyEndings);
        }

        private void AddParametersUpdateInsert(SQLiteCommand command, CompanyEndings companyEndings)
        {
            command.Parameters.AddWithValue("@Id", companyEndings.Id);
            command.Parameters.AddWithValue("@Name", companyEndings.Name);
            command.Parameters.AddWithValue("@CreatedAt", companyEndings.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            command.Parameters.AddWithValue("@Deleted", companyEndings.Deleted);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private void PrepareCommandUpdate(SQLiteCommand command, CompanyEndings companyEndings)
        {
            command.CommandText =
                "UPDATE CompanyEndings SET Name = @Name, CreatedAt = @CreatedAt, Deleted = @Deleted, " +
                "ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            AddParametersUpdateInsert(command, companyEndings);
        }

        private void PrepareDeleteCommand(SQLiteCommand command, CompanyEndings companyEndings)
        {
            command.CommandText = "UPDATE CompanyEndings SET Deleted = true, ModifiedAt = @ModifiedAt WHERE Id = @Id";
            command.Prepare();
            command.Parameters.AddWithValue("@Id", companyEndings.Id);
            command.Parameters.AddWithValue("@ModifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        private IQueryable<CompanyEndings> GetQueryable()
        {
            return Get().AsQueryable();
        }
		
		public void Truncate()
		{
            const string sql = "DELETE FROM CompanyEndings";
            _connection.Open();
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _log.Info(string.Format(_currentLanguage.GetWord("ExecutedTruncate"), "CompanyEndings"));
            _connection.Close();
		}
    }
}