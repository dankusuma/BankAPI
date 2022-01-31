using Bank.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Data
{
    public class Data
    {
        /// List of Users
        public static List<User> GetUserList()
        {
            List<User> userList = new()
            {
                #region Password test
                new User
                {
                    USERNAME = "userSuccess",
                    PASSWORD = "01b307acba4f54f55aafc33bb06bbbf6ca803e9a",
                    CHANGE_PASSWORD_TOKEN = "5dfbe8a246987bcc901316318e911fb2aebaac42"
                },
                new User
                {
                    USERNAME = "similarPassword",
                    PASSWORD = "01b307acba4f54f55aafc33bb06bbbf6ca803e9a",
                    CHANGE_PASSWORD_TOKEN = "5dfbe8a246987bcc901316318e911fb2aebaac42"
                },
                new User
                {
                    USERNAME = "invalidToken",
                    CHANGE_PASSWORD_TOKEN = "5dfbe8a246987bcc901316318e911fb2aebaac42"
                },
                new User
                {
                    USERNAME = "tokenExpired"
                },
                new User
                {
                    USERNAME = "forgotPasswordEmailFormat",
                    EMAIL = "validMail@mail.com"
                },
                #endregion
                #region PIN Test
                new User
                {
                    USERNAME = "pinNotExist",
                    PIN = null
                },
                new User
                {
                    USERNAME = "pinExist",
                    PIN = "09a9ed2c9b4c439667f00e5b07f7283971654f6c"
                },
                new User
                {
                    USERNAME = "pinSuccess",
                    PIN = "09a9ed2c9b4c439667f00e5b07f7283971654f6c"
                },
                new User
                {
                    USERNAME = "similarPin",
                    PIN = "1775866fc55b8179d8b3f92c432d217c27423958"
                },
                new User
                {
                    USERNAME = "pinGeneralNumber",
                    BIRTH_DATE = new DateTime(1989, 03, 08),
                },
                new User
                {
                    USERNAME = "pinDateFormat",
                    BIRTH_DATE = new DateTime(1989, 03, 08),
                },
                new User
                {
                    USERNAME = "invalidPinInput",
                },
                new User
                {
                    USERNAME = "pinLength",
                },
                new User
                {
                    USERNAME = "pinNull",
                },
                #endregion
                new User
                {
                    USERNAME = "dummyUser",
                    NAME = "DUMMY DUMMY",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy1@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user",
                    BIRTH_DATE = new DateTime(2000, 03, 13) // 2000-03-13 00:00:00.000
                },
                new User
                {
                    USERNAME = "dummyUser",
                    NAME = "DUMMY DUMMY",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy1@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user",
                    BIRTH_DATE = new DateTime(2000, 03, 13) // 2000-03-13 00:00:00.000
                },
                new User
                {
                    USERNAME = "dummyPINTrue",
                    PIN = "065314",
                    NAME = "DUMMY KEDUA KAKA",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy2@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user"
                },
                new User
                {
                    USERNAME = "dummyPINFalse",
                    NAME = "DUMMY KETIGA",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy3@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user",
                    PIN = ""
                },
                new User
                {
                    USERNAME = "dummyPINFalseNull",
                    NAME = "DUMMY KETIGA",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy3@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user",
                    PIN = null
                },
            };
            return userList;
        }

        public static List<RefMaster> GetRefMasterList()
        {
            List<RefMaster> refMasterList = new()
            {
                #region Password Test
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_FROM", //
                    VALUE = "toshiro89@gmail.com", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_FROM_PASSWORD", //
                    VALUE = "uudflhwdhwvvkhfv", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_SUBJECT_RESET", //
                    VALUE = "Reset Password", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_BODY_TEMPLATE_PATH", //
                    VALUE = "/Views/Templates/ChangePassword.cshtml", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_LINK", //
                    VALUE = "https://159.223.84.230/ChangePassword?mode=create&username={0}&token={1}&reff={2}", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_SIGNATURE", //
                    VALUE = "Bank App", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_SERVER", //
                    VALUE = "smtp.gmail.com", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_PORT", //
                    VALUE = "465", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_SSL", //
                    MASTER_CODE_DESCRIPTION = "", //
                    VALUE = "TRUE", //
                    IS_ACTIVE = true, //
                },
                new RefMaster
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_DEFAULT_CREDENTIALS", //
                    VALUE = "FALSE", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                #endregion
                #region PIN Test
                new RefMaster
                {
                    ID = 34,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "VALIDATION",
                    MASTER_CODE_DESCRIPTION = "Invalid pin",
                    VALUE = null,
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 35,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "VALIDATION",
                    MASTER_CODE_DESCRIPTION = "Invalid input. Only accept 6 digit numbers",
                    VALUE = "",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 36,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please do not use your Date of Birth (DOB)",
                    VALUE = "yyMMdd",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 37,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please do not use your Date of Birth (DOB)",
                    VALUE = "ddMMyy",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 38,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please do not use your Date of Birth (DOB)",
                    VALUE = "MMyyyy",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 39,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please do not use your Date of Birth (DOB)",
                    VALUE = "yyyyMM",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 40,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please do not use your Date of Birth (DOB)",
                    VALUE = "yyyydd",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 41,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please do not use your Date of Birth (DOB)",
                    VALUE = "ddyyyy",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 42,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin",
                    VALUE = "123456",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 43,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin",
                    VALUE = "654321",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 44,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "LENGTH",
                    MASTER_CODE_DESCRIPTION = "PIN Length",
                    VALUE = "6",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 45,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "000000",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 46,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "111111",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 47,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "222222",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 48,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "333333",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 49,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "444444",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 50,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "555555",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 51,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "666666",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 52,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "777777",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 53,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "888888",
                    IS_ACTIVE = true,
                },
                new RefMaster
                {
                    ID = 54,
                    MASTER_GROUP = "PIN",
                    MASTER_CODE = "FORMAT",
                    MASTER_CODE_DESCRIPTION = "Please DO NOT use other general number as your pin",
                    VALUE = "999999",
                    IS_ACTIVE = true,
                }
                #endregion
            };

            return refMasterList;
        }
    }
}
