using Bank.Core.Entity;
using NUnit.Framework;
using System;

namespace UnitTest.Users_Test
{
    [TestFixture]
    public class User_VerifyPassword
    {
        private User user;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            user.PASSWORD = "d033e22ae348aeb5660fc2140aec35850c4da997";
        }

        [Test]
        public void VerifyPassword_ReturnTrue()
        {
            var result = user.VerifyPassword("admin");
            Assert.IsTrue(result);
        }

        [TestCase("admin")]
        public void VerifyPassword_EqualTo(string password)
        {
            user.PASSWORD = password;
            user.HashPassword();

            Assert.IsTrue(user.VerifyPassword(password));
        }

        [TestCase("")]
        [TestCase(null)]
        public void Password_Empty(string password)
        {
            user.PASSWORD = password;
            Assert.Throws<ArgumentNullException>(() => user.HashPassword());
        }
    }
}
