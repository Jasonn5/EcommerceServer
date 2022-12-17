using DataAccess.Interfaces;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class SaleRepository : IRepository<Sale>
    {
        private readonly IdentityDbContext dataAccess;

        public SaleRepository(IdentityDbContext dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IQueryable<Sale> List
        {
            get
            {
                return dataAccess.Set<Sale>().Include(s => s.SaleDetails);
            }
        }

        public Sale Add(Sale entity)
        {
            if (entity.SaleDetails != null)
            {
                entity.SaleDetails = entity.SaleDetails.Select(sd => {
                    sd.Id = 0;
                    return sd;
                }).ToList();
            }

            dataAccess.Set<Sale>().Add(entity);
            dataAccess.SaveChanges();

            return entity;
        }

        public void Delete(Sale entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Sale> FilterList(Expression<Func<Sale, bool>> filterFunc)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Sale> FilterListWithInclude<TInclude>(Expression<Func<Sale, bool>> filterFunc, Expression<Func<Sale, TInclude>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<Sale> FilterListWithIncludeArray<TInclude>(Expression<Func<Sale, bool>> filterFunc, Expression<Func<Sale, ICollection<TInclude>>> includeFunc) where TInclude : Entity
        {
            return dataAccess.Set<Sale>().Include(includeFunc).Where(filterFunc);
        }

        public Sale FindById(int id)
        {
            return dataAccess.Set<Sale>()
                .Include(s => s.SaleDetails)
                .SingleOrDefault(s => s.Id == id);
        }

        public Sale FindByIdWithInclude<TInclude>(int id, Expression<Func<Sale, TInclude>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public Sale FindByIdWithIncludeArray<TInclude>(int id, Expression<Func<Sale, ICollection<TInclude>>> includeFunc) where TInclude : Entity
        {
            return dataAccess.Set<Sale>()
                .Include(includeFunc)
                .FirstOrDefault(s => s.Id == id);
        }

        public IQueryable<Sale> ListWithInclude<TInclude>(Expression<Func<Sale, TInclude>> includeFunc) where TInclude : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<Sale> ListWithInclude<TInclude1, TInclude2>(Expression<Func<Sale, TInclude1>> includeFunc1, Expression<Func<Sale, TInclude2>> includeFunc2)
            where TInclude1 : Entity
            where TInclude2 : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<Sale> ListWithIncludeArray<TInclude>(Expression<Func<Sale, ICollection<TInclude>>> includeFunc) where TInclude : Entity
        {
            return dataAccess.Set<Sale>().Include(includeFunc);
        }

        public void Update(Sale entity)
        {
            var sale = dataAccess.Set<Sale>()
                .Include(s => s.SaleDetails)
                .SingleOrDefault(s => s.Id == entity.Id);

            if (entity.StatusId == (int)SaleStatusEnum.Active)
            {
                var saleDetailDeleteList = new List<SaleDetail>();

                foreach (var sd in sale.SaleDetails)
                {
                    if (!entity.SaleDetails.Any(esd => esd.Id == sd.Id))
                    {
                        saleDetailDeleteList.Add(sd);
                    }
                }

                if (saleDetailDeleteList.Count() > 0)
                {
                    RemoveSaleDetails(saleDetailDeleteList);
                }

                foreach (var sd in entity.SaleDetails)
                {
                    if (sd.Id > 0)
                    {
                        var saleDetail = dataAccess.Set<SaleDetail>().Find(sd.Id);
                        dataAccess.Entry(saleDetail).CurrentValues.SetValues(sd);
                    }
                    else
                    {
                        sd.Id = 0;
                        sale.SaleDetails.Add(sd);
                    }
                }
            }

            sale.Date = entity.Date;
            sale.StatusId = entity.StatusId;
            dataAccess.SaveChanges();
        }

        private void RemoveSaleDetails(ICollection<SaleDetail> list)
        {
            foreach (var sd in list)
            {
                dataAccess.Set<SaleDetail>().Remove(sd);
            }

            try
            {
                dataAccess.SaveChanges();
            }
            catch
            {
                throw new ApplicationException("No se puede eliminar el detalle de venta.");
            }
        }
    }
}
