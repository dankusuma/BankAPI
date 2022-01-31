using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using UnitTest.Data;

namespace Bank.Api.Controllers.Tests
{
    [TestFixture()]
    public class PasswordControllerTests
    {
        private static IConfiguration _config;
        private static IRepository _repo;

        private PasswordController _passwordController;
        private List<User> _users;
        private List<RefMaster> _mailSetting;

        [SetUp]
        public void SetUp()
        {
            _config = A.Fake<IConfiguration>();
            _repo = A.Fake<IRepository>();

            _users = Data.GetUserList();
            _mailSetting = Data.GetRefMasterList();

            A.CallTo(() => _repo.List<User>(null)).Returns(_users);
            A.CallTo(() => _repo.List<RefMaster>(null)).Returns(_mailSetting);

            _passwordController = new PasswordController(_repo, _config);
        }

        [TestCase("mail@mail.com", "Email not registered.")]
        [TestCase("validMail@mailcom", "Incorrect email format.")]
        public void ForgotPassword_Validation_Failed(string email, string expected)
        {
            // Arrange
            ForgotPassword data = new() { EMAIL = email };

            // Act
            var act = _passwordController.ForgotPassword(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }

        [TestCase("validMail@mail.com", "Email sent to {0} successfully.")]
        public void ForgotPassword_Success(string email, string expected)
        {
            // Arrange
            ForgotPassword data = new() { EMAIL = email };

            // Act
            var act = _passwordController.ForgotPassword(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, string.Format(expected, email));
        }

        [TestCase("change", "userNotRegistered", "", "", "202201290137", "Username not registered.")]
        [TestCase("change", "similarPassword", "1234567890", "5dfbe8a246987bcc901316318e911fb2aebaac42", "202301290137", "New password must be different from the old one.")]
        public void ChangePassword_Validation_Failed(string mode, string username, string password, string token, string reff, string expected)
        {
            // Arrange
            ChangePassword data = new ChangePassword(mode, username, password, token, reff);

            // Act
            var act = _passwordController.ChangePassword(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }

        [TestCase("forgot", "userSuccess", "0987654321", "5dfbe8a246987bcc901316318e911fb2aebaac42", "202301290137", "Password updated successfully.")]
        [TestCase("change", "userSuccess", "0987654321", "", "", "Password updated successfully.")]
        public void ChangePassword_Success(string mode, string username, string password, string token, string reff, string expected)
        {
            // Arrange
            ChangePassword data = new ChangePassword(mode, username, password, token, reff);

            // Act
            var act = _passwordController.ChangePassword(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }
    }
}