using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.ValueObject;
public class Password
{
    public string Value { get; set; }
    public Password() { }

    public Password (string value)
    {
        this.Value = value ?? throw new ArgumentNullException(nameof(Password));
    }
}
