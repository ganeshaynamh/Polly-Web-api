using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Models
{
    public class DemoRepository : IDemoModel<DemoModel>
    {
        private readonly DemoDbContext demoDbContext;

        public DemoRepository(DemoDbContext demoDbContext)
        {
            this.demoDbContext = demoDbContext;
        }
        public void Add(DemoModel demoModel)
        {
            demoDbContext.ModelItem.Add(demoModel);
            demoDbContext.SaveChanges();
        }

        public DemoModel Get(int id)
        {
            return demoDbContext.ModelItem.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<DemoModel> getAll()
        {
            return demoDbContext.ModelItem.ToList();
        }
    }
}
