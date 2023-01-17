using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Static : IEntity
    {
        public int Id { get; set; }
        public string StaticName { get; set; }
        public string StaticValue { get; set; }
    }
}
