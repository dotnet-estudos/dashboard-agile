using System;
using System.Security.Cryptography;
using System.Text;

namespace DashBoardAgil.DashBoardApi.Util
{
	public class CriptografiaAES
	{
		private const string HashKey = "ChangeThisKey0000000000000000000";
		private const string IV = "ChangeThisIV0000";
		private AesCryptoServiceProvider aesCryptoServiceProvider = null;

		private CriptografiaAES()
		{
			aesCryptoServiceProvider = new AesCryptoServiceProvider();
			aesCryptoServiceProvider.BlockSize = 128;
			aesCryptoServiceProvider.KeySize = 256;
			aesCryptoServiceProvider.Mode = CipherMode.CBC;
			aesCryptoServiceProvider.Padding = PaddingMode.PKCS7;

			aesCryptoServiceProvider.IV = ASCIIEncoding.ASCII.GetBytes(IV);
			aesCryptoServiceProvider.Key = ASCIIEncoding.ASCII.GetBytes(HashKey);
		}

		private static CriptografiaAES _instance = null;
		public static CriptografiaAES Instance
		{
			get
			{
				return _instance ?? (_instance = new CriptografiaAES());
			}
		}

		public string Criptografar(string senha)
		{
			AesCryptoServiceProvider cryptoServiceProvider = aesCryptoServiceProvider;

			ICryptoTransform transform = cryptoServiceProvider.CreateEncryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);
			byte[] senhaCriptografada = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(senha), 0, senha.Length);

			return Convert.ToBase64String(senhaCriptografada);
		}

		public string Descriptografar(string senhaCriptografada)
		{
			AesCryptoServiceProvider cryptoServiceProvider = aesCryptoServiceProvider;
			ICryptoTransform transform = cryptoServiceProvider.CreateDecryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);

			byte[] enc_bytes = Convert.FromBase64String(senhaCriptografada);
			byte[] decrypted_bytes = transform.TransformFinalBlock(enc_bytes, 0, enc_bytes.Length);

			return ASCIIEncoding.ASCII.GetString(decrypted_bytes);
		}
	}
}