using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using GDNET.Extensions.Serialization;
using GDNET.Extensions.Attributes;
using GDNET.Server.IO.Net;
using GDNET.Server;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GDNET.Tests.Server.Objects
{
    public class TestAccount
    {
        [Test]
        public void TestRyuuhouAccount()
        {
            Account acc = Account.Get(7361923);

            Assert.AreEqual(acc.Username, "Ryuuhou", "Usernames are not the same.");
        }
        
        [Test]
        public void TestShaggy23Account()
        {
            Account acc = Account.Get(2888);

            Assert.AreEqual(acc.Username, "shaggy23", "Usernames are not the same.");
            Assert.AreEqual(acc.Badge, ModeratorType.Elder, "uh oh brothers.");
        }
    }
}
