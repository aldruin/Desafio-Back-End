using BancoApi.Application.Notifications;
using BancoApi.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Handlers;
public class NotificationHandler : INotificationHandler
{
    private readonly List<Notification> _notificationList;

    public NotificationHandler(List<Notification> notificationList)
    {
        _notificationList = new List<Notification>();
    }

    public bool AddNotification(Notification notification)
    {
        _notificationList.Add(notification);
        return false;
    }

    public bool AddNotification(string action, string message)
    {
        var notification = new Notification()
        {
            Action = action,
            Message = message
        };
        _notificationList.Add(notification);
        return false;
    }

    public List<Notification> GetNotifications()
    {
        return _notificationList;
    }

    public bool HasNotification()
    {
        return _notificationList.Any();
    }
}