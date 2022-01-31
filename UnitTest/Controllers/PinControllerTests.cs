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
    public class PinControllerTests
    {
        private static IConfiguration _config;
        private static IRepository _repo;

        private PinController _pinController;
        private List<User> _users;
        private List<RefMaster> _refMasters;

        [SetUp]
        public void SetUp()
        {
            _config = A.Fake<IConfiguration>();
            _repo = A.Fake<IRepository>();

            _users = Data.GetUserList();
            _refMasters = Data.GetRefMasterList();

            A.CallTo(() => _repo.List<User>(null)).Returns(_users);
            A.CallTo(() => _repo.List<RefMaster>(null)).Returns(_refMasters);

            _pinController = new PinController(_repo, _config);
        }

        [TestCase("userNotRegistered", "", "Username not registered.")]
        [TestCase("pinNull", null, "Invalid pin")]
        [TestCase("pinLength", "1989", "Pin too short. Only accept 6 digit numbers.")]
        [TestCase("pinLength", "08031989", "Pin too long. Only accept 6 digit numbers.")]
        [TestCase("invalidPinInput", "", "Invalid input. Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "a23456", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "1b3456", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "12c456", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "123d56", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "1234e6", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "12345f", "Only accept 6 digit numbers")]
        [TestCase("pinDateFormat", "890308", "Please do not use your Date of Birth (DOB)")] // yyMMddPin
        [TestCase("pinDateFormat", "080389", "Please do not use your Date of Birth (DOB)")] // ddMMyyPin
        [TestCase("pinDateFormat", "031989", "Please do not use your Date of Birth (DOB)")] // MMyyyyPin
        [TestCase("pinDateFormat", "198903", "Please do not use your Date of Birth (DOB)")] // yyyyMMPin
        [TestCase("pinDateFormat", "198908", "Please do not use your Date of Birth (DOB)")] // yyyyddPin
        [TestCase("pinDateFormat", "081989", "Please do not use your Date of Birth (DOB)")] // ddyyyyPin
        [TestCase("pinGeneralNumber", "000000", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "111111", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "222222", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "333333", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "444444", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "555555", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "666666", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "777777", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "888888", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "999999", "Please DO NOT use other general number as your pin")]
        public void CreatePIN_Validation_Failed(string username, string pin, string expected)
        {
            // Arrange
            Pin data = new() { USERNAME = username, PIN = pin };

            // Act
            var act = _pinController.CreatePIN(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }

        [TestCase("pinSuccess", "019283", "PIN created successfully. Please re-login")]
        public void CreatePIN_Success(string username, string pin, string expected)
        {
            // Arrange
            Pin data = new() { USERNAME = username, PIN = pin };

            // Act
            var act = _pinController.CreatePIN(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }

        [TestCase("userNotRegistered", "", "Username not registered.")]
        [TestCase("pinNull", null, "Invalid pin")]
        [TestCase("pinLength", "1989", "Pin too short. Only accept 6 digit numbers.")]
        [TestCase("pinLength", "08031989", "Pin too long. Only accept 6 digit numbers.")]
        [TestCase("invalidPinInput", "", "Invalid input. Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "a23456", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "1b3456", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "12c456", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "123d56", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "1234e6", "Only accept 6 digit numbers")]
        [TestCase("invalidPinInput", "12345f", "Only accept 6 digit numbers")]
        [TestCase("pinDateFormat", "890308", "Please do not use your Date of Birth (DOB)")] // yyMMddPin
        [TestCase("pinDateFormat", "080389", "Please do not use your Date of Birth (DOB)")] // ddMMyyPin
        [TestCase("pinDateFormat", "031989", "Please do not use your Date of Birth (DOB)")] // MMyyyyPin
        [TestCase("pinDateFormat", "198903", "Please do not use your Date of Birth (DOB)")] // yyyyMMPin
        [TestCase("pinDateFormat", "198908", "Please do not use your Date of Birth (DOB)")] // yyyyddPin
        [TestCase("pinDateFormat", "081989", "Please do not use your Date of Birth (DOB)")] // ddyyyyPin
        [TestCase("pinGeneralNumber", "000000", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "111111", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "222222", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "333333", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "444444", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "555555", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "666666", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "777777", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "888888", "Please DO NOT use other general number as your pin")]
        [TestCase("pinGeneralNumber", "999999", "Please DO NOT use other general number as your pin")]
        [TestCase("similarPin", "102938", "New pin must be different from the old one.")]
        public void ChangePIN_Validation_Failed(string username, string pin, string expected)
        {
            // Arrange
            Pin data = new() { USERNAME = username, PIN = pin };

            // Act
            var act = _pinController.ChangePIN(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }

        [TestCase("pinSuccess", "102938", "PIN updated successfully")]
        public void ChangePIN_Success(string username, string pin, string expected)
        {
            // Arrange
            Pin data = new() { USERNAME = username, PIN = pin };

            // Act
            var act = _pinController.ChangePIN(data) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }

        [TestCase("userNotRegistered", "Username not registered.")]
        public void PINStatus_Validation_Failed(string username, string expected)
        {
            // Act
            var act = _pinController.PINStatus(username) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }

        [TestCase("pinExist", "True")]
        [TestCase("pinNotExist", "False")]
        public void PINStatus_Success(string username, string expected)
        {
            // Act
            var act = _pinController.PINStatus(username) as ObjectResult;

            string result = act.Value.ToString();

            Assert.AreEqual(result, expected);
        }
    }
}