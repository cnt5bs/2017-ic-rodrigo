﻿using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class UserNotAdmVM : Base
    {
        public List<User> users { get; set; }
    }
}