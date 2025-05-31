using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IMealTypeRepository:IRepository<MealType>
    {
        void Update(MealType obj);
        void Save();

        IEnumerable<MealType> GetAllMeals();

        MealType Get(Expression<Func<MealType, bool>> filter, string? includeProperties = null);

    }
}
