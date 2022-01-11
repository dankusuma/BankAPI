using Bank.Core.Entity;
using NUnit.Framework;
using System;

namespace UnitTest.Users_Test
{
    [TestFixture]
    public class User_HashPassword
    {
        private User user;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            user.PASSWORD = "admin";
            
        }

        [Test]
        public void HashPassword_ReturnTrue()
        {
            //test Linter
            user.HashPassword();
            Assert.AreEqual(user.PASSWORD, "d033e22ae348aeb5660fc2140aec35850c4da997");
        }

        [TestCase("admin", "d033e22ae348aeb5660fc2140aec35850c4da997")]
        public void HashPassword_EqualTo(string password, string hashedPassword)
        {
            user.PASSWORD = password;
            user.HashPassword();

            Assert.AreEqual(hashedPassword, user.PASSWORD);
        }
    }
}
