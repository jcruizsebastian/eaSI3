using eaSI3Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace eaSI3Web.Controllers.UsageStatistics
{
    public class BdStatistics
    {
        private readonly StatisticsContext _context;

        public BdStatistics(StatisticsContext context)
        {
            _context = context;
        }

        public void AddUser(string usernameJira, string passwordJira, string usernameSi3, string passwrodSi3)
        {
            var users = from u in _context.Users where u.SI3UserName.Equals(usernameSi3) select u;

            if (!users.Any())
            {
                _context.Add(new User() { JiraUserName = usernameJira, SI3UserName = usernameSi3});
                _context.SaveChanges();
                Encrypt( usernameJira,  passwordJira,  usernameSi3,  passwrodSi3);
                _context.SaveChanges();
            }
            else
            {
                var user = users.First();
                Encrypt(usernameJira, passwordJira, usernameSi3, passwrodSi3);
                //no se si es necesario hacer Update ahora
                //_context.Update(user);
                //_context.SaveChanges();
            }
        }

        public int GetUserId(string usernameSi3)
        {
            var users = from u in _context.Users where u.SI3UserName.Equals(usernameSi3) select u;
            return users.First().UserId;
        }
        public User GetUser(int id)
        {
            var users = from u in _context.Users where u.UserId == id select u;
            return users.First();
        }
        public void AddLogin(string username)
        {
            var users = from u in _context.Users where u.SI3UserName.CompareTo(username) == 0 select u;

            _context.Add(new Login() { User = (User)users.First(), ConnectionDate = DateTime.Now });
            _context.SaveChanges();
        }

        public void AddIssueCreation(string username, string jiraKey, string si3Key, int error, string message)
        {
            var users = from u in _context.Users where u.JiraUserName.CompareTo(username) == 0 select u;
            IssueCreation issueCreation = new IssueCreation()
            {
                CreationDate = DateTime.Now,
                JiraKey = jiraKey,
                SI3Key = si3Key,
                CreationResultAddtionalInfo = message,
                User = users.First(),
                CreationResult = (CreationResult)error
            };

            _context.Add(issueCreation);
            _context.SaveChanges();
        }

        public void AddWorkTracking(string username, int week, int totalHours, int error, string message)
        {
            var users = from u in _context.Users where u.SI3UserName.CompareTo(username) == 0 select u;
            WorkTracking workTracking = new WorkTracking()
            {
                TotalHours = totalHours,
                Week = week,
                User = users.First(),
                Year = DateTime.Now.Year,
                TrackingDate = DateTime.Now,
                TrackResultAddtionalInfo = message,
                TrackResult = (TrackResult)error
            };

            _context.Add(workTracking);
            _context.SaveChanges();
        }

        public void Encrypt(string jiraUserName,string jiraPassword,string Si3UserName, string Si3Password)
        {
            var users = from u in _context.Users where u.JiraUserName.Equals(jiraUserName) select u;
            var user = users.First();

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                Byte[] myArray = new byte[32];
                Byte[] myArray2 = new byte[16];
                var x = Encoding.ASCII.GetBytes("akjdbaspidbq9p83hgde9'1386546546+521398yue91kjb7t621q21yg62gp8'0");
                var ms = new MemoryStream(myArray);
                var ms2 = new MemoryStream(myArray2);
                ms.Write(x, 0, 32);
                ms.Flush();
                ms2.Write(x, 0, 16);
                ms.Flush();

                myRijndael.Key = ms.ToArray();
                myRijndael.IV = ms2.ToArray();

                byte[] JiraPassEncrypted = EncryptStringToBytes(jiraPassword, myRijndael.Key, myRijndael.IV);
                byte[] Si3PassEncrypted = EncryptStringToBytes(Si3Password, myRijndael.Key, myRijndael.IV);

                user.Password_Encrypted = JiraPassEncrypted;
                user.PasswordSi3_Encrypted = Si3PassEncrypted;

                _context.Update(user);
                _context.SaveChanges();
                
            }
        }

        public string DecryptJiraPassword(string JiraUserName)
        {
            var password = string.Empty;
            var users = from u in _context.Users where u.JiraUserName.Equals(JiraUserName) select u;
            var user = users.First();
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                Byte[] myArray = new byte[32];
                Byte[] myArray2 = new byte[16];
                var x = Encoding.ASCII.GetBytes("akjdbaspidbq9p83hgde9'1386546546+521398yue91kjb7t621q21yg62gp8'0");
                var ms = new MemoryStream(myArray);
                var ms2 = new MemoryStream(myArray2);
                ms.Write(x, 0, 32);
                ms.Flush();
                ms2.Write(x, 0, 16);
                ms.Flush();

                myRijndael.Key = ms.ToArray();
                myRijndael.IV = ms2.ToArray();

                password = DecryptStringFromBytes(user.Password_Encrypted, myRijndael.Key, myRijndael.IV);
            }
                return password;
        }
        public string DecryptSi3Password(string Si3UserName)
        {
            var password = string.Empty;
            var users = from u in _context.Users where u.SI3UserName.Equals(Si3UserName) select u;
            var user = users.First();
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                Byte[] myArray = new byte[32];
                Byte[] myArray2 = new byte[16];
                var x = Encoding.ASCII.GetBytes("akjdbaspidbq9p83hgde9'1386546546+521398yue91kjb7t621q21yg62gp8'0");
                var ms = new MemoryStream(myArray);
                var ms2 = new MemoryStream(myArray2);
                ms.Write(x, 0, 32);
                ms.Flush();
                ms2.Write(x, 0, 16);
                ms.Flush();

                myRijndael.Key = ms.ToArray();
                myRijndael.IV = ms2.ToArray();

                password = DecryptStringFromBytes(user.PasswordSi3_Encrypted, myRijndael.Key, myRijndael.IV);
            }
            return password;
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}
