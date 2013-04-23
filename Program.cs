using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace CryptoDemo
{
	class Program
	{
		/// <summary>
		/// Demonstration program that shows how to use the System.Security.Cryptography APIs
		/// to perform symmetrical encryption and decryption using the 3DES algorithm.
		/// 
		/// Revision History:
		/// 23-Apr-03 DaveC
		///		Initial version to play with GitHub archiving.
		/// </summary>
		static void Main()
		{
			try
			{
				string _secret = "3599A59E-8F74-40FF-8A01-7BA4DE5B539E";
				Guid binKey = new Guid(_secret);
				byte[] myKey = binKey.ToByteArray();

				// Create a new TripleDESCryptoServiceProvider object
				// to generate a key and initialization vector (IV).
				TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();

				// Create a string to encrypt.
				string sData = "Here is some data to encrypt.";

				// Encrypt the string to an in-memory buffer.
				//byte[] Data = EncryptTextToMemory(sData, tDESalg.Key, tDESalg.IV);
				byte[] Data = EncryptTextToMemory(sData, myKey, tDESalg.IV);

				// Decrypt the buffer back to a string.
				//string Final = DecryptTextFromMemory(Data, tDESalg.Key, tDESalg.IV);
				string Final = DecryptTextFromMemory(Data, myKey, tDESalg.IV);

				// Display the decrypted string to the console.
				Console.WriteLine(Final);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}

		/// <summary>
		/// Encrypts a text string into a byte array using 3DES encryption.
		/// </summary>
		/// <param name="Data">Text to be encrypted</param>
		/// <param name="Key">Symmetrical encryption key</param>
		/// <param name="IV">Initialization vector</param>
		/// <returns>Byte array of containing encrypted data.</returns>
		public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
		{
			try
			{
				// Create a MemoryStream.
				MemoryStream mStream = new MemoryStream();

				// Create a CryptoStream using the MemoryStream 
				// and the passed key and initialization vector (IV).
				CryptoStream cStream = new CryptoStream(mStream,
					new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV),
					CryptoStreamMode.Write);

				// Convert the passed string to a byte array.
				byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

				// Write the byte array to the crypto stream and flush it.
				cStream.Write(toEncrypt, 0, toEncrypt.Length);
				cStream.FlushFinalBlock();

				// Get an array of bytes from the 
				// MemoryStream that holds the 
				// encrypted data.
				byte[] ret = mStream.ToArray();

				// Close the streams.
				cStream.Close();
				mStream.Close();

				// Return the encrypted buffer.
				return ret;
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				return null;
			}

		}

		/// <summary>
		/// Decrypts a byte array of data into a plain-text string using 3DES.
		/// </summary>
		/// <param name="Data">Data to be decrypted.</param>
		/// <param name="Key">Key that was used to encrypt the data.</param>
		/// <param name="IV">Initialization vector.</param>
		/// <returns></returns>
		public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
		{
			try
			{
				// Create a new MemoryStream using the passed 
				// array of encrypted data.
				MemoryStream msDecrypt = new MemoryStream(Data);

				// Create a CryptoStream using the MemoryStream 
				// and the passed key and initialization vector (IV).
				CryptoStream csDecrypt = new CryptoStream(msDecrypt,
					new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV),
					CryptoStreamMode.Read);

				// Create buffer to hold the decrypted data.
				byte[] fromEncrypt = new byte[Data.Length];

				// Read the decrypted data out of the crypto stream
				// and place it into the temporary buffer.
				csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

				//Convert the buffer into a string and return it.
				return new ASCIIEncoding().GetString(fromEncrypt);
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				return null;
			}
		}
	}
}
