using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.ValueObject;
public class Email
{
    public string Value { get; set; }
    public Email() { }

    public Email(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(Email));
    }
}
