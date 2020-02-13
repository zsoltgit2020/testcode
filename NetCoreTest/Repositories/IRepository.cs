using NetCoreTestProject.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreTestProject.Repositories
{
    public interface IRepository<T> where T : IDataModel
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
