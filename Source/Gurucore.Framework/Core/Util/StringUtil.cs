using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Gurucore.Framework.Core.Util
{
	public static class StringUtil
	{
		public static string CamelToLower(this string p_sCamel)
		{
			StringBuilder sLower = new StringBuilder();
			for (int i = 0; i < p_sCamel.Length; i++)
			{
				char c = p_sCamel[i];
				if ((c >= 'A') && (c <= 'Z'))
				{
					if (i > 0)
					{
						if ((p_sCamel[i - 1] >= 'A') && (p_sCamel[i - 1] <= 'Z'))
						{
							sLower.Append((char)(c + 32));
						}
						else
						{
							sLower.Append(new char[] { '_', (char)(c + 32) });
						}
					}
					else
					{
						sLower.Append((char)(c + 32));
					}
				}
				else
				{
					sLower.Append(c);
				}
			}
			return sLower.ToString();
		}

		public static bool NullOrEmpty(this string p_sInput)
		{
			return (p_sInput == null) || (p_sInput == string.Empty);
		}

		// TODO: FRAMEWORK: thay đổi key này để tránh bị hack
		private static byte[] m_arrCryptoKey = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		private static byte[] m_arrCryptoIV = { 0, 0, 0, 0, 0, 0, 0, 0 };

		public static string Encrypt(this string p_sData)
		{
			RC2CryptoServiceProvider oRC2Service = new RC2CryptoServiceProvider();
			ICryptoTransform oTransform = oRC2Service.CreateEncryptor(m_arrCryptoKey, m_arrCryptoIV);
			MemoryStream oMemStream = new MemoryStream();
			CryptoStream oCryptoStream = new CryptoStream(oMemStream, oTransform, CryptoStreamMode.Write);
			byte[] arrData = Encoding.ASCII.GetBytes(p_sData);
			oCryptoStream.Write(arrData, 0, arrData.Length);
			oCryptoStream.FlushFinalBlock();
			byte[] arrCypher = oMemStream.ToArray();
			return HexEncoding.ToString(arrCypher);
		}

		public static string Decrypt(this string p_sCypher)
		{
			RC2CryptoServiceProvider oRC2Service = new RC2CryptoServiceProvider();
			ICryptoTransform oTransform = oRC2Service.CreateDecryptor(m_arrCryptoKey, m_arrCryptoIV);
			byte[] arrCypher = HexEncoding.GetBytes(p_sCypher);
			MemoryStream oMemStream = new MemoryStream(arrCypher);
			CryptoStream oCryptoStream = new CryptoStream(oMemStream, oTransform, CryptoStreamMode.Read);
			byte[] arrData = new byte[arrCypher.Length];
			oCryptoStream.Read(arrData, 0, arrData.Length);
			return Encoding.ASCII.GetString(arrData).Replace("\0", "");
		}

		public static string MD5Hash(this string p_sData)
		{
			MD5CryptoServiceProvider oMD5Service = new MD5CryptoServiceProvider();
			byte[] arrData = Encoding.ASCII.GetBytes(p_sData);
			byte[] arrHash = oMD5Service.ComputeHash(arrData);
			return HexEncoding.ToString(arrHash);
		}

		private class HexEncoding
		{
			public HexEncoding()
			{
			}

			public static int GetByteCount(string p_sHexString)
			{
				int nHexChars = 0;
				char cChar;
				// remove all none A-F, 0-9, characters
				for (int nIdx = 0; nIdx < p_sHexString.Length; nIdx++)
				{
					cChar = p_sHexString[nIdx];
					if (IsHexDigit(cChar))
						nHexChars++;
				}
				if (nHexChars % 2 != 0)
				{
					nHexChars--;
				}
				return nHexChars / 2; // 2 characters per byte
			}

			public static byte[] GetBytes(string p_sHexString)
			{
				int nDiscarded = 0;
				string sNewString = "";
				char c;
				// remove all none A-F, 0-9, characters
				for (int i = 0; i < p_sHexString.Length; i++)
				{
					c = p_sHexString[i];
					if (IsHexDigit(c))
						sNewString += c;
					else
						nDiscarded++;
				}
				// if odd number of characters, discard last character
				if (sNewString.Length % 2 != 0)
				{
					nDiscarded++;
					sNewString = sNewString.Substring(0, sNewString.Length - 1);
				}

				int nByteLength = sNewString.Length / 2;
				byte[] arrBytes = new byte[nByteLength];
				string hex;
				int j = 0;
				for (int i = 0; i < arrBytes.Length; i++)
				{
					hex = new String(new Char[] { sNewString[j], sNewString[j + 1] });
					arrBytes[i] = HexToByte(hex);
					j = j + 2;
				}
				return arrBytes;
			}

			public static string ToString(byte[] p_arrBytes)
			{
				string p_sHexString = "";
				for (int i = 0; i < p_arrBytes.Length; i++)
				{
					p_sHexString += p_arrBytes[i].ToString("X2");
				}
				return p_sHexString;
			}

			public static bool InHexFormat(string p_sHexString)
			{
				bool bHexFormat = true;

				foreach (char cDigit in p_sHexString)
				{
					if (!IsHexDigit(cDigit))
					{
						bHexFormat = false;
						break;
					}
				}
				return bHexFormat;
			}

			public static bool IsHexDigit(Char p_c)
			{
				int nNumChar;
				int nNumA = Convert.ToInt32('A');
				int nNum1 = Convert.ToInt32('0');
				p_c = Char.ToUpper(p_c);
				nNumChar = Convert.ToInt32(p_c);
				if (nNumChar >= nNumA && nNumChar < (nNumA + 6))
					return true;
				if (nNumChar >= nNum1 && nNumChar < (nNum1 + 10))
					return true;
				return false;
			}

			private static byte HexToByte(string p_sHex)
			{
				if (p_sHex.Length > 2 || p_sHex.Length <= 0)
					throw new ArgumentException("hex must be 1 or 2 characters in length");
				byte newByte = byte.Parse(p_sHex, System.Globalization.NumberStyles.HexNumber);
				return newByte;
			}

		}
	}
}
