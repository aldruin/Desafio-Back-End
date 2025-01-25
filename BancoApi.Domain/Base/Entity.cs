using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Base;
public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();

    }
    public Guid Id { get; protected set; }
}
