using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace LayoutManager.Helpers
{
    public class SecurityManager
    {
        private static byte[] Key = new byte[8];//
        private static byte[] IV = new byte[8];

        public static string Encrypt(string key, string data)
        {
            string encryptedData = "";
            try
            {
                encryptedData = EncryptData(key, data);
                encryptedData = Ascii2HexString(encryptedData);
            }
            catch (Exception)
            {
                encryptedData = "-1";
            }
            return encryptedData;
        }

        /*----------------------------------------------------------------------------------*/

        private static string Ascii2HexString(string asciiString)
        {
            string hex = "";

            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        private static string HexString2Ascii(string hexString)
        {
            StringBuilder stringBuilder = new();

            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                stringBuilder.Append(Convert.ToString(Convert.ToChar(int.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber))));
            }
            return stringBuilder.ToString();
        }

        private static bool InitializeKey(string key)
        {
            try
            {
                byte[] numArray = new byte[(int)checked((uint)key.Length)];
                new ASCIIEncoding().GetBytes(key, 0, key.Length, numArray, 0);
                byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(numArray);
                for (int index = 0; index < 8; ++index)
                    Key[index] = hash[index];
                for (int index = 8; index < 16; ++index)
                    IV[index - 8] = hash[index];
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsValid(string key, string dataToDecrypt, bool isHexString)
        {
            dataToDecrypt = isHexString ? HexString2Ascii(dataToDecrypt) : dataToDecrypt;
            string decryptedData = DecryptData(key, dataToDecrypt);

            return decryptedData.IndexOf("Error.") == -1;
        }

        private static string EncryptData(string key, string data)
        {
            if (data.Length > 92160)
                return "Error. Data String too large. Keep within 90Kb.";
            if (!InitializeKey(key))
                return "Error. Fail to generate key for encryption";
            data = string.Format("{0,5:00000}" + data, (object)data.Length);
            byte[] numArray = new byte[(int)checked((uint)data.Length)];
            new ASCIIEncoding().GetBytes(data, 0, data.Length, numArray, 0);
            ICryptoTransform encryptor = new DESCryptoServiceProvider().CreateEncryptor(Key, IV);
            CryptoStream cryptoStream = new CryptoStream((Stream)new MemoryStream(numArray), encryptor, CryptoStreamMode.Read);
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[1024];
            int count;
            do
            {
                count = cryptoStream.Read(buffer, 0, 1024);
                if (count != 0)
                    memoryStream.Write(buffer, 0, count);
            }
            while (count > 0);
            return memoryStream.Length != 0L ? Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length) : "";
        }

        public static string DecryptData(string key, string dataToDecrypt, bool isHexString)
        {
            dataToDecrypt = isHexString ? HexString2Ascii(dataToDecrypt) : dataToDecrypt;
            return DecryptData(key, dataToDecrypt);
        }

        private static string DecryptData(string key, string data)
        {
            if (!InitializeKey(key))
                return "Error. Fail to generate key for decryption";
            if (string.IsNullOrEmpty(data))
            {
                return "Error. Fail to decrypt value is null or empty";
            }
            int num1 = 0;
            DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider
            {
                Padding = PaddingMode.None
            };
            ICryptoTransform decryptor = cryptoServiceProvider.CreateDecryptor(Key, IV);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Write);
            byte[] numArray = new byte[(int)checked((uint)data.Length)];
            byte[] buffer;
            try
            {
                buffer = Convert.FromBase64CharArray(data.ToCharArray(), 0, data.Length);
            }
            catch (Exception ex)
            {
                return "Error. Input Data is not base64 encoded.";
            }
            long num2 = 0L;
            long num3 = (long)data.Length;
            try
            {
                for (; num3 >= num2; num2 = memoryStream.Length + (long)Convert.ToUInt32(buffer.Length / cryptoServiceProvider.BlockSize * cryptoServiceProvider.BlockSize))
                    cryptoStream.Write(buffer, 0, buffer.Length);
                string @string = new ASCIIEncoding().GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                int length = Convert.ToInt32(@string.Substring(0, 5));
                string str = @string.Substring(5, length);
                num1 = (int)memoryStream.Length;
                return str;
            }
            catch (Exception ex)
            {
                return "Error. Decryption Failed. Possibly due to incorrect Key or corrputed data";
            }
        }




    }
}
