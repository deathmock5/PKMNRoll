using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll
{
    class CryptographyHelper
    {
        //Default Key
        public static string _key = "pokemon";
        //Default initial vector
        private byte[] _ivByte = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };
        private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

        /// <summary> 
        /// Hash enum value 
        /// </summary> 
        public enum HashName
        {
            SHA1 = 1,
            MD5 = 2,
            SHA256 = 4,
            SHA384 = 8,
            SHA512 = 16
        }

        public static byte[] Encrypt(byte[] plain, string password)
        {
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(plain, 0, plain.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public static byte[] Decrypt(byte[] cipher, string password)
        {
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(cipher, 0, cipher.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public static Object DeserialiseAndDecript(string filename)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(filename))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Read file to memory
                    byte[] buffer = new byte[16 * 1024];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                    }

                    //Decript
                    byte[] decripedbytes = CryptographyHelper.Decrypt(memoryStream.ToArray(), _key);

                    // This resets the memory stream position for the following read operation
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    //write the decripte bytes to the stream
                    memoryStream.Write(decripedbytes, 0, decripedbytes.Length);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    //Deserialise the class
                    BinaryFormatter bformatter = new BinaryFormatter();
                    return bformatter.Deserialize(memoryStream);
                }
            }
            catch(Exception e)
            {
                //failed to deserialise!
                Game.log(e.Message);
            }
            return null;
        }

        public static void SerialiseObjectAndEncript(Object myobject,string filename)
        {
            using (FileStream fileStream = File.OpenWrite(filename))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Serialize to memory instead of to file
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, myobject);

                // This resets the memory stream position for the following read operation
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Get the bytes
                byte[] bytes = new byte[memoryStream.Length];
                memoryStream.Read(bytes, 0, (int)memoryStream.Length);

                // Encrypt your bytes with your chosen passkey
                byte[] encryptedData = CryptographyHelper.Encrypt(bytes, _key);

                //Write the encripted bytes to the file
                fileStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }

        public static string ComputeHash(string plainText)
        {
            return ComputeHash(plainText, SALT, HashName.SHA256);
        }

        /// <summary> 
        /// Compute Hash 
        /// </summary> 
        /// <param name="plainText">plain text</param> 
        /// <param name="salt">salt string</param> 
        /// <returns>string</returns> 
        public static string ComputeHash(string plainText, string salt)
        {
            byte[] saltBytes;
            if (!string.IsNullOrEmpty(salt))
            {
                // Convert salt text into a byte array. 
                saltBytes = ASCIIEncoding.ASCII.GetBytes(salt);
            }
            else
            {
                // Define min and max salt sizes. 
                int minSaltSize = 4;
                int maxSaltSize = 8;
                // Generate a random number for the size of the salt. 
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);
                // Allocate a byte array, which will hold the salt. 
                saltBytes = new byte[saltSize];
                // Initialize a random number generator. 
                RNGCryptoServiceProvider rngCryptoServiceProvider =
                            new RNGCryptoServiceProvider();
                // Fill the salt with cryptographically strong byte values. 
                rngCryptoServiceProvider.GetNonZeroBytes(saltBytes);
            }
            return ComputeHash(plainText, saltBytes, HashName.MD5);
        }

        /// <summary> 
        /// Compute Hash 
        /// </summary> 
        /// <param name="plainText">plain text</param> 
        /// <param name="salt">salt string</param> 
        /// <param name="hashName">Hash Name</param> 
        /// <returns>string</returns> 
        public static string ComputeHash(string plainText, byte[] saltBytes, HashName hashName)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                // Convert plain text into a byte array. 
                byte[] plainTextBytes = ASCIIEncoding.ASCII.GetBytes(plainText);
                // Allocate array, which will hold plain text and salt. 
                byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

                //Copy the salt bytes to array
                for (int i = 0; i < saltBytes.Length;i++ )
                {
                    plainTextWithSaltBytes[i + plainTextBytes.Length] = saltBytes[i];
                }

                // Copy plain text bytes into resulting array. 
                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }

                // Append salt bytes to the resulting array. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] =
                                        saltBytes[i];
                }
                HashAlgorithm hash = null;
                switch (hashName)
                {
                    case HashName.SHA1:
                        hash = new SHA1Managed();
                        break;
                    case HashName.SHA256:
                        hash = new SHA256Managed();
                        break;
                    case HashName.SHA384:
                        hash = new SHA384Managed();
                        break;
                    case HashName.SHA512:
                        hash = new SHA512Managed();
                        break;
                    case HashName.MD5:
                        hash = new MD5CryptoServiceProvider();
                        break;
                }
                // Compute hash value of our plain text with appended salt. 
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                // Create array which will hold hash and original salt bytes. 
                byte[] hashWithSaltBytes =
                    new byte[hashBytes.Length + saltBytes.Length];
                // Copy hash bytes into resulting array. 
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }
                // Append salt bytes to the result. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
                }
                // Convert result into a base64-encoded string. 
                string hashValue = Convert.ToBase64String(hashWithSaltBytes);
                // Return the result. 
                return hashValue;
            }
            return string.Empty;
        } 
    }
}
