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
    public class CartRepository: Repository<Cart>,ICartRepository
    {
        private ApplicationDbContext _db;

        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void ICartRepository.Save()
        {
            _db.SaveChanges();
        }

        void ICartRepository.Update(Cart obj)
        {
            _db.Update(obj);
        }
    }
}
