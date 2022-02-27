using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Interfaces.Services;

namespace Test.Interfaces.Services
{
    public interface IActivityServices: IBaseService<Entities.Activity>
    {
        Task<Entities.Activity> Reschedule(Entities.Activity entity);
         
    }
}
