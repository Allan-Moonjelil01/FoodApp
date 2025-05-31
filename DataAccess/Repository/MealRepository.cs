using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MealRepository : Repository<Meal>, IMealRepository
    {
        private ApplicationDbContext _db;

        public MealRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void IMealRepository.Save()
        {
            _db.SaveChanges();
        }

        void IMealRepository.Update(Meal obj)
        {
            _db.Update(obj);
        }
    }
}
