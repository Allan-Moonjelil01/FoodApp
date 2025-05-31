using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
   public  class MealTypeRepository:Repository<MealType>,IMealTypeRepository
    {
        private ApplicationDbContext _db;

        public MealTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        MealType IMealTypeRepository.Get(Expression<Func<MealType, bool>> filter, string? includeProperties)
        {
            IQueryable<MealType> query = _db.MealType;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault(filter);
        }

        IEnumerable<MealType> IMealTypeRepository.GetAllMeals()
        {
            return _db.MealType.Include(m => m.Meals).ToList();
        }



        void IMealTypeRepository.Save()
        {
            _db.SaveChanges();
        }

        void IMealTypeRepository.Update(MealType obj)
        {
            _db.Update(obj);
        }


    }
}
