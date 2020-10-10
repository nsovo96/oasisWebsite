using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
  public  interface Irepository<T, Tkey> 
    {
        string Insert(T Item, Tkey key);

    }
}