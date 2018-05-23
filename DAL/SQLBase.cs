﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    interface SQLBase<T> where T: class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T entity);
        void update(T entity);
        void remove(int id);
    }
}
