using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentInfoSystem
{
    class StudentInfoContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        

        public StudentInfoContext() : base(Properties.Settings.Default.DbConnect)
        { 
        }
    }
}
