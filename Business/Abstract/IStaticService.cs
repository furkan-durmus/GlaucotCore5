using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IStaticService
    {
        List<Static> GetAll();
        Static GetStaticByName(string staticName);
    }
}
