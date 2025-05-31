using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IMealRepository:IRepository<Meal>
    {
        void Update(Meal obj);
        void Save();
    }
}
