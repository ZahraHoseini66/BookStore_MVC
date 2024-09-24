using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db):base(db)
        {

            _db = db;

        }
        public void Update(OrderHeader obj)
        {
           _db.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus)
        {
          OrderHeader order =  _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (order != null)
            {
                order.OrderStatus=orderStatus;
            }
          
        }
        public void UpdateStatus(int id, string orderStatus,string paymentStatus)
        {
            OrderHeader order = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (order != null)
            {
                order.OrderStatus = orderStatus;
                order.PaymentStatus = paymentStatus;
            }

        }
    }
}
