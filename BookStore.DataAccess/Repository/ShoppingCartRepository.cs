using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
       ApplicationDbContext _db { get; set; }
        public ShoppingCartRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(ShoppingCart obj)
        {
            _db.Update(obj);
        }
    }
}
