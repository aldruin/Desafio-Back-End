using BancoApi.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Notifications;
public interface INotificationHandler
{
    List<Notification> GetNotifications();
    bool HasNotification();
    bool AddNotification(Notification notificacao);
    bool AddNotification(string acao, string mensagem);
}
