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
        private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

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
                    byte[] decripedbytes = CryptographyHelper.Decrypt(memoryStream.ToArray(), "pokemon");

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
            catch
            {
                //failed to deserialise!
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
                byte[] encryptedData = CryptographyHelper.Encrypt(bytes, "pokemon");

                //Write the encripted bytes to the file
                fileStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }
    }
}
