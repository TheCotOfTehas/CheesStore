﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface INotificationService
    {
        void SendConfirmationCode(string cellPhone, int code);
        public void Post();
        public void CellPhone();
    }
}
