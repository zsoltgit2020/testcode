using Microsoft.EntityFrameworkCore;
using NetCoreTestProject.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreTestProject.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly TestWorkDBContext _context;

        public IUnitOfWork UnitOfWork { get { return _context; } }

        public TestRepository(TestWorkDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TestWork Add(TestWork testModel)
        {
            return _context.TestWorkModel.Add(testModel).Entity;
        }

        public void Update(TestWork testModel)
        {
            _context.Entry(testModel).State = EntityState.Modified;
        }
    
        public void Remove(TestWork testModel)
        {
            _context.TestWorkModel.Remove(testModel);
        }

        /// <summary>
        /// generated csv file content
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetCSVByDate(DateTime date)
        {
            var resultList = _context.TestWorkModel.Where(w => 
                w.CrDate.Year == date.Year
                && w.CrDate.Month == date.Month
                && w.CrDate.Day == date.Day
                ).ToList();
            StringBuilder strb = new StringBuilder();
            if (resultList != null && resultList.Count() > 0)
            {
                strb.AppendLine("Key,Email,Attributes");
                resultList.AsEnumerable().Select(s => strb.AppendLine(
                    $"{s.Key},{s.Email},{s.Attributes}")
                 ).ToList();
                return strb.ToString();
            }

            return string.Empty;
        }

        public bool ValidInsertAttributeByEmailAddress(string email, string attribute)
        {
            return !(_context.TestWorkModel.Count(c => c.Email == email && c.Attributes == attribute) > 0);
        }

        /// <summary>
        /// send email if the attributes count greater 10
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string[] Get10AttributeByEmailAddress(string email)
        {
            return _context.TestWorkModel.Where(c => c.Email == email).Select(s => s.Attributes).ToArray();
        }

    }
}
