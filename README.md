GenericDatabaseAccess
====================================

GenericDatabaseAccess is an assembly on how to access databases in a generic way and using C#'s LINQ.
The assembly was written and tested in .Net 4.6.2.

[![Build status](https://ci.appveyor.com/api/projects/status/xwfx8q48taebdx2e?svg=true)](https://ci.appveyor.com/project/SeppPenner/genericdatabaseaccess)

## Example usage:

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

Change history
--------------

* **Version 1.0.0.0 (2017-08-08)** : 1.0 release.