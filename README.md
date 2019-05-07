GenericDatabaseAccess
====================================

GenericDatabaseAccess is an assembly on how to access databases in a generic way and using C#'s LINQ.
The assembly was written and tested in .Net 4.8.

[![Build status](https://ci.appveyor.com/api/projects/status/xwfx8q48taebdx2e?svg=true)](https://ci.appveyor.com/project/SeppPenner/genericdatabaseaccess)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/GenericDatabaseAccess.svg)](https://github.com/SeppPenner/GenericDatabaseAccess/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/GenericDatabaseAccess.svg)](https://github.com/SeppPenner/GenericDatabaseAccess/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/GenericDatabaseAccess.svg)](https://github.com/SeppPenner/GenericDatabaseAccess/stargazers)
[![GitHub license](https://img.shields.io/badge/license-AGPL-blue.svg)](https://raw.githubusercontent.com/SeppPenner/GenericDatabaseAccess/master/License.txt)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/GenericDatabaseAccess/badge.svg)](https://snyk.io/test/github/SeppPenner/GenericDatabaseAccess)


## Example initialization:

```csharp
using System;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Languages.Implementation;
using Languages.Interfaces;
using GenericDatabaseAccess.Database.Generic;
using GenericDatabaseAccess.Exceptions;

namespace ExampleUsage
{
    public partial class Main : Form
    {
        private IDatabaseAdapter _databaseAdapter;
        private readonly ILanguageManager _lm = new LanguageManager();
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILanguage _lang;

        public Main()
        {
            InitializeComponent();
            InitializeLanguageManager();
            LoadLanguagesToCombo();
            InitDatabase();
        }

        private void InitDatabase()
        {
            try
            {
                _databaseAdapter = new DatabaseAdapter(_lang);
            }
            catch (Exception ex)
            {
                LogDatabaseInitializationException(ex);
            }
        }

        private void LogDatabaseInitializationException(Exception exception)
        {
            var ex = new InitializationException(_lang.GetWord("ErrorInDatabaseInit"), exception);
            LogError(ex);
        }

        private void InitializeLanguageManager()
        {
            _lm.SetCurrentLanguage("de-DE");
            _lm.OnLanguageChanged += OnLanguageChanged;
        }

        private void LoadLanguagesToCombo()
        {
            foreach (var lang in _lm.GetLanguages())
                comboBoxLanguage.Items.Add(lang.Name);
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void ComboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lm.SetCurrentLanguageFromName(comboBoxLanguage.SelectedItem.ToString());
        }

        private void OnLanguageChanged(object sender, EventArgs eventArgs)
        {
            _lang = _lm.GetCurrentLanguage();
            labelSelectLanguage.Text = _lang.GetWord("SelectLanguage");
            SetDatabaseAdapterLanguageIfNotNull();
        }

        private void SetDatabaseAdapterLanguageIfNotNull()
        {
            if (_lang == null || _databaseAdapter == null) return;
            _databaseAdapter.SetCurrentLanguage(_lang);
        }

        private void LogError(Exception ex)
        {
            _log.Error(ex);
            MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
```

## Example usage:

```csharp
var merchant = _databaseAdapter.Get<Merchant>(x => !x.Deleted && x.Id == 1);
var merchants = _databaseAdapter.Get<Merchant, bool>(x => !x.Deleted);
var merchantCount = _databaseAdapter.Count<Merchant>();
var merchantCount2 = _databaseAdapter.Count<Merchant>(x => !x.Deleted);
var merchantToInsert = new Merchant();
var inserted = _databaseAdapter.Insert(merchantToInsert);
var merchantToUpdate = new Merchant();
var update = _databaseAdapter.Update(merchantToUpdate);
var deleted = _databaseAdapter.Delete(merchantToUpdate);
var sorted = _databaseAdapter.Get<Merchant, bool>(x => !x.Deleted, x.Name);
```

## All available methods:

```csharp
public interface IDatabaseAdapter
{
    void SetCurrentLanguage(ILanguage language);

    ILanguage GetCurrentLanguage();

    string GetDatabasePath();

    string GetConnectionString();

    void CreateBoughtTable();

    void CreateCompanyEndingsTable();

    void CreateCompanyNamesTable();

    void CreateDummyCompanyTable();

    void CreateMerchantTable();

    void CreateNamesTable();

    void CreateSoldTable();

    void CreateStockTable();

    void CreateStockHistoryTable();

    void CreateStockMarketTable();

    void CreateSurnamesTable();

    void CreateTaxesTable();

    void CreateAllTables();

    List<T> Get<T>();

    T Get<T>(long id);

    ObservableCollection<T> Get<T, TValue>(Expression<Func<T, bool>> predicate = null,
        Expression<Func<T, TValue>> orderBy = null);

    T Get<T>(Expression<Func<T, bool>> predicate);

    int Insert<T>(T entity);

    int Update<T>(T entity);

    int Delete<T>(T entity);

    int Insert<T>(List<T>entities);

    int Update<T>(List<T> entities);

    int Delete<T>(List<T> entities);

    int Count<T>(Expression<Func<T, bool>> predicate = null);
}
```

For more information see [IDatabaseAdapter](https://github.com/SeppPenner/GenericDatabaseAccess/blob/master/GenericDatabaseAccess/Database/Generic/IDatabaseAdapter.cs)
or [DatabaseAdapter](https://github.com/SeppPenner/GenericDatabaseAccess/blob/master/GenericDatabaseAccess/Database/Generic/DatabaseAdapter.cs)

Change history
--------------

* **Version 1.0.0.4 (2019-05-07)** : Updated .Net version to 4.8.
* **Version 1.0.0.3 (2018-05-21)** : Added static database connection.
* **Version 1.0.0.2 (2018-03-12)** : Updated ToCollection calls.
* **Version 1.0.0.1 (2017-08-16)** : Smaller bugfixes.
* **Version 1.0.0.0 (2017-08-08)** : 1.0 release.