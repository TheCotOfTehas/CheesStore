﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface INotificationService
    {
        Task SendConfirmationCodeAsync(string cellPhone, int code);

        void SendConfirmationCode(string cellPhone, int code);

        void StartProcess(Order order);

        Task StartProcessAsync(Order order);
    }
}
