﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    public class UserNotification
    {
        [Key]
        [Column(Order = 1)]
        public int NotificationId { get; private set; }

        [Key]
        [Column(Order = 2)]
        public string UserId { get; private set; }

        public bool IsRead { get; private set; }

        public Notification Notification { get; private set; }

        public ApplicationUser User { get; private set; }

        protected UserNotification()
        {

        }

        public UserNotification(ApplicationUser user, Notification notification)
        {
            User = user ?? throw new ArgumentNullException("user");
            Notification = notification ?? throw new ArgumentNullException("notification");
            UserId = user.Id;
            NotificationId = notification.Id;
        }

        public void Read()
        {
            IsRead = true;
        }
    }
}