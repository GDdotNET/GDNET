﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using GDNET.Server;
using NUnit.Framework;

namespace GDNET.Tests.Server.Objects
{
    public class TestUserAccount
    {
        public void TestLogin()
        {
            UserAccount myAccount = UserAccount.Login(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Username"]);

            Assert.AreEqual(myAccount.Username, ConfigurationManager.AppSettings["Username"], "Usernames are not the same.");
            Assert.AreEqual(myAccount.Badge, ModeratorType.None, "How am I a moderator?");
        }
    }
}
