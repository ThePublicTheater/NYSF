using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Wbd.Utilities
{
	public static class Utility
	{
		private static readonly string[] namePrefixes = {"mc"};

		private static MD5CryptoServiceProvider encryptionClient;

		static Utility()
		{
			encryptionClient = new MD5CryptoServiceProvider();
		}

		public static string EncryptPassword(string password, string saltString)
		{
			string saltedPass = saltString + password;
			byte[] originalBytes = ASCIIEncoding.Default.GetBytes(saltedPass);
			byte[] encodedBytes = encryptionClient.ComputeHash(originalBytes);
			return BitConverter.ToString(encodedBytes).Replace("-", "");
		}

		public static Dictionary<string, List<Dictionary<string, object>>> ConvertToDictionary(
				DataSet dataSet)
		{
			Dictionary<string, List<Dictionary<string, object>>> tablesList =
				new Dictionary<string, List<Dictionary<string, object>>>();
			foreach (DataTable table in dataSet.Tables)
			{
				List<Dictionary<string, object>> rowsList = new List<Dictionary<string, object>>();
				foreach (DataRow row in table.Rows)
				{
					Dictionary<string, object> columnData = new Dictionary<string, object>();
					foreach (DataColumn col in table.Columns)
					{
						columnData.Add(col.ColumnName, row[col].ToString());
					}
					rowsList.Add(columnData);
				}
				tablesList.Add(table.TableName, rowsList);
			}
			return tablesList;
		}

/*		public static string PrepName(string value, bool requireNonWhiteSpaceChars)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				if (requireNonWhiteSpaceChars)
				{
					throw new ApplicationException("Name requires non-whitespace characters.");
				}
				return null;
			}
			List<string> namePieces = new List<string>();
			foreach (Match m in Regex.Matches(value, @"(\w+|\W+)"))
			{
				namePieces.Add(m.ToString());
			}
			StringBuilder nameBuilder = new StringBuilder();
			bool lastWasWord = false;
			foreach (string piece in namePieces)
			{
				if (Regex.IsMatch(piece, @"\w+"))
				{
					if (lastWasWord)
					{
						nameBuilder.Append(" ");
					}
					int lengthOfPrefix = 0;
					string loweredPiece = piece.ToLower();
					foreach (string prefix in namePrefixes)
					{
						if (loweredPiece.StartsWith(prefix))
						{
							lengthOfPrefix = prefix.Length;
							break;
						}
					}
					char[] chars = loweredPiece.ToCharArray();
					chars[0] = Char.ToUpper(chars[0]);
					if (lengthOfPrefix > 0 && chars.Length >= lengthOfPrefix)
					{
						chars[lengthOfPrefix] = Char.ToUpper(chars[lengthOfPrefix]);
					}
					string capsCorrected = new String(chars);
					nameBuilder.Append(capsCorrected);
					lastWasWord = true;
				}
				else
				{
					nameBuilder.Append(piece);
					lastWasWord = false;
				}
			}
			return nameBuilder.ToString();
		}

		public static string PrepName(string value)
		{
			return PrepName(value, true);
		}*/
	}
}
