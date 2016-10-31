using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace ManageCaseFour.Models
{

    //crypt code from support.microsoft.com/en-us/kb/307010
    public class DESCryption
    {
        string secretKey;
        string fileForEncryption;
        string fileForDecryption;

        public DESCryption()
        {

        }
        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]

        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        public byte[] ConvertImageToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }
        //convert bytearray to image
        public void ConvertByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                Image img1 = Image.FromStream(mStream);
                img1.Save("C:/Users/Renuka/Dropbox/WORK/devcodecamp/CAPSTONE/pages/06192013_20_debyted.tif", ImageFormat.Tiff);
            }
        }

        // Function to Generate a 64 bits Key.
        public static string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        public static void EncryptFile(string sInputFilename,
            string sOutputFilename,
            string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename,
                FileMode.Open,
                FileAccess.Read);

            FileStream fsEncrypted = new FileStream(sOutputFilename,
                FileMode.Create,
                FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted,
                desencrypt,
                CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }

        public static void DecryptFile(string sInputFilename,
            string sOutputFilename,
            string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename,
                FileMode.Open,
                FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                desdecrypt,
                CryptoStreamMode.Read);
            //Print the contents of the decrypted file.
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
        }
    }
}