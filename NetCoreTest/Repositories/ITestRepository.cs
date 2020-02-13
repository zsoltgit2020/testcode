using NetCoreTestProject.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreTestProject.Repositories
{
    public interface ITestRepository : IRepository<TestWork>
    {
        TestWork Add(TestWork testWork);

        void Update(TestWork testWork);

        void Remove(TestWork testWork);

        string GetCSVByDate(DateTime date);

        bool ValidInsertAttributeByEmailAddress(string email, string attribute);

        string[] Get10AttributeByEmailAddress(string email);
    }
}
