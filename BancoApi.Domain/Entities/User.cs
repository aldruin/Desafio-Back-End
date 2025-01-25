using BancoApi.Domain.Base;
using BancoApi.Domain.Rules;
using BancoApi.Domain.ValueObject;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Entities;
public class User : Entity
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public  Email Email { get; set; }
    public  Password Password{ get; set; }
    public virtual Wallet Wallet { get; set; }

    public User() { }

    public void SetPassword()
    {
        this.Password.Value = SecurityUtils.HashSHA256(this.Password.Value);
    }

    public void Validate() => new UserValidator().ValidateAndThrow(this);

    
}
