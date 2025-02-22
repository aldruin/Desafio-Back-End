﻿using BancoApi.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Users.Dtos;
public record UserDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
