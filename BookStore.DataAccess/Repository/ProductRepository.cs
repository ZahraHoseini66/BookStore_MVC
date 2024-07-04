﻿using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
       ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
