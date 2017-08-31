using EquiposTecnicosSN.Web.DataContexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EquiposTecnicosSN.Web.Services
{
    public class BaseService
    {
        protected EquiposDbContext db = new EquiposDbContext();

        /*public void Update (object entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }*/
    }
}