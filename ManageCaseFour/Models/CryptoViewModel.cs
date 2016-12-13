using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ManageCaseFour.Models
{
    public class CryptoViewModel
    {
        Rijndael myRin;
        byte[] Key;
        byte[] IV;
        ICryptoTransform encryptor;
        ICryptoTransform decryptor;
        private static CryptoViewModel crypto;
        public string filename;
        public string byteArrayFilename;
        public string resultFilename;
        public bool result;

        public CryptoViewModel()
        {
        }

        public byte[] ConvertImageToByteArray(string filename)
        {
            Image img = Image.FromFile(filename);

            using (MemoryStream memStream = new MemoryStream())
            {
                img.Save(memStream, img.RawFormat);
                return memStream.ToArray();
            }
        }


        public Image ConvertByteArrayToImage(byte[] byteArrayIn, string resultFilename)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
            ms.Seek(0, SeekOrigin.Begin);
            ms.Write(byteArrayIn, 0, byteArrayIn.Length);
            Image returnImage = Image.FromStream(ms, false);
            returnImage.Save(resultFilename, ImageFormat.Tiff);
            return returnImage;


        }


        public byte[] EncryptArrayToBytes(byte[] original, ICryptoTransform encryptor, byte[] key, byte[] IV)
        {
            if (original == null || original.Length <= 0)
                throw new ArgumentNullException("original");
            if (key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encryptedOriginal = null;


            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, this.encryptor, CryptoStreamMode.Write))
                {

                    csEncrypt.Write(original, 0, original.Length);
                    csEncrypt.FlushFinalBlock();
                    encryptedOriginal = msEncrypt.ToArray();
                }
            }
            return encryptedOriginal;
        }


        public byte[] DecryptBytesToArray(byte[] encryptedOriginal, ICryptoTransform decryptor, byte[] key, byte[] IV)
        {
            if (encryptedOriginal == null || encryptedOriginal.Length <= 0)
                throw new ArgumentNullException("encryptedOriginal");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] decryptedOriginal = null;

            using (MemoryStream msDecrypt = new MemoryStream())
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                {
                    csDecrypt.Write(encryptedOriginal, 0, encryptedOriginal.Length);
                    csDecrypt.FlushFinalBlock();
                    decryptedOriginal = msDecrypt.ToArray();
                }
            }
            return decryptedOriginal;
        }

        public bool SaveByteArray(byte[] original, string byteArrayFilename)
        {
            BinaryWriter Writer = null;
            try
            {
                Writer = new BinaryWriter(File.OpenWrite(byteArrayFilename));
                Writer.Write(original);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}