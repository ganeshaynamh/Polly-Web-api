using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Models
{
    public interface IDemoModel<DemoModel>
    {
        IEnumerable<DemoModel> getAll();

        DemoModel Get(int id);
        void Add(DemoModel demoModel);
        
    }
}
