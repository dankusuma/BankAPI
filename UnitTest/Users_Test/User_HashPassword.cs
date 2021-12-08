using Bank.Core.Entity;
using NUnit.Framework;

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
            user.password = "admin";
            
        }
        [Test]
        public void HashPassword_ReturnTrue()
        {
            //test Linter
            user.HashPassword();
            Assert.AreEqual(user.password, "d033e22ae348aeb5660fc2140aec35850c4da997");
        }
    }
}
