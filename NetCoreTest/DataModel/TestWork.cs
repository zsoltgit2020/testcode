using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreTestProject.DataModel
{
    [Table("TestWork")]
    public class TestWork : IDataModel
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Email { get; set; }
        public string Attributes { get; set; }
        [Column("cr_date")]
        public DateTime CrDate { get; set; }
    }
}
