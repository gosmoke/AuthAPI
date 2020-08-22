using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ContextLifeManager
{
    public interface IContextLifeManager
    {
        IDbContextScope Create(DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting);
        IDbContextReadOnlyScope CreateReadOnly(DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting);
        IDbContextScope CreateWithTransaction(IsolationLevel isolationLevel);
        IDbContextReadOnlyScope CreateReadOnlyWithTransaction(IsolationLevel isolationLevel);
        IDisposable SuppressAmbientContext();
    }
}
