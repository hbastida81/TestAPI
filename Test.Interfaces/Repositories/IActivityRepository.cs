using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test.Interfaces.Repositories
{
    public interface IActivityRepository:IBaseRepository<Entities.Activity>
    {
        Task<Entities.Activity> Reschedule(Entities.Activity entity);


    }
}
