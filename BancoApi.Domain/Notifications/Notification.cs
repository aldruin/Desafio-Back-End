using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Notifications;
public class Notification
{
    public required string Action { get; set; }
    public required string Message { get; set; }
}
