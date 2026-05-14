using Core.Abstract.IRepositories;
using Core.Concrete.Entities;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PiggyBankRepository : Repository<PiggyBank>, IPiggyBankRepository
    {
        public PiggyBankRepository(ApplicationDbContext context) : base(context) { }

        public List<PiggyBank> GetByUserId(string userId) =>
            _context.PiggyBanks.Where(x => x.UserId == userId && !x.IsDeleted).ToList();
    }
}
