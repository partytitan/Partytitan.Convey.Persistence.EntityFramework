using System;
using System.Collections.Generic;
using System.Text;
using Partytitan.Convey.Persistence.EntityFramework.Configuration.Enums;

namespace Partytitan.Convey.Persistence.EntityFramework.Configuration
{
    public class EntityFrameworkOptions
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType { get; set; }
    }
}
