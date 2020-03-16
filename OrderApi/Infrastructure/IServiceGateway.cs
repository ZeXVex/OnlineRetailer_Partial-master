using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure
{
    interface IServiceGateway<T>
    {
        T Get(int ID);
    }
}
