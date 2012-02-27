using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace AngularExample
{
    public class DataContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
    }

    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CombinedName { get { return string.Format("{0} {1}", FirstName, LastName); } }
    }
}