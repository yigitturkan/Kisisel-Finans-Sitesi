using Core.Abstract.Repositories;
using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstract.IRepositories
{
    public interface IIncomeRepository : IRepository<Income>
    {
        List<Income> GetByUserId(string userId);
        decimal GetTotalByUserId(string userId);
    }
}
