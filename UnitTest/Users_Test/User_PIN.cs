using Bank.Core.Entity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Users_Test
{
    class User_PIN
    {
        private User user;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            user.PIN = "205080";
            user.HashPin();
        }

        [TestCase("205080")]
        public void VerifyPIN_EqualTo(string pin)
        {
            Assert.IsTrue(user.VerifyPin(pin));
        }

        [TestCase("205080", "e6d55c87875b84110e3a1b848c07f0b40f797606")]
        public void HashPassword_EqualTo(string pin, string hashedPIN)
        {
            user.PIN = pin;
            user.HashPin();

            Assert.AreEqual(hashedPIN, user.PIN);
        }

        [TestCase("")]
        [TestCase(null)]
        public void PIN_Empty(string pin)
        {
            user.PIN = pin;
            Assert.Throws<ArgumentNullException>(() => user.HashPin());
        }
    }
}
