using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class StaticManager : IStaticService
    {
        IStaticDal _staticDal;
        public StaticManager(IStaticDal staticDal)
        {
            _staticDal = staticDal;
        }
        public List<Static> GetAll()
        {
            return _staticDal.GetAll();
        }

        public Static GetStaticByName(string staticName)
        {
            return _staticDal.Get(s => s.StaticName == staticName);
        }
    }
}
