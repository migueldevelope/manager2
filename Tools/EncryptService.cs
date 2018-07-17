using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
  public static class EncryptServices
  {

    public static string GetMD5Hash(string input)
    {
      System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
      byte[] hash = md5.ComputeHash(inputBytes);
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      for (int i = 0; i < hash.Length; i++)
      {
        sb.Append(hash[i].ToString("X2"));
      }
      return sb.ToString();
    }


    public static string EncryptOff(string password)
    {
      var senha = string.Empty;
      try
      {
        if (password != string.Empty)
        {
          for (int i = 0; i < password.Length - 1; i = i + 6)
          {
            senha = String.Concat(senha, (char)(Int32.Parse(password.Substring(i, 3)) - Int32.Parse(password.Substring(i + 3, 3))));
          }
        }
        return senha;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string EncryptOff(string password, string key)
    {
      try
      {
        Byte[] bylKey = Encoding.ASCII.GetBytes(key);
        Byte[] bylIV = { 0, 0, 0, 0, 0, 0, 0, 0 };
        CryptoStream oCryptoStream;
        string decryptedData;
        Byte[] bylData = Convert.FromBase64String(password);
        var oMemoryStream = new MemoryStream();
        oMemoryStream.SetLength(0);
        var oTripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider();
        oTripleDesCryptoServiceProvider.Padding = PaddingMode.Zeros;
        oCryptoStream = new CryptoStream(oMemoryStream, oTripleDesCryptoServiceProvider.CreateDecryptor(bylKey, bylIV), CryptoStreamMode.Write);
        oCryptoStream.Write(bylData, 0, bylData.Length);
        oCryptoStream.FlushFinalBlock();
        oMemoryStream.Seek(0, SeekOrigin.Begin);
        decryptedData = Encoding.Unicode.GetString(oMemoryStream.ToArray(), 0, Int32.Parse(oMemoryStream.Length.ToString()));

        return RemoveNullStrings(decryptedData.ToString());
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string EncryptOn(String password)
    {
      int[] inlCod;
      int[] inlSoma;
      int inlNum = 10;
      string senha = string.Empty;
      try
      {
        if (password != string.Empty)
        {
          inlCod = new int[password.Length];
          inlSoma = new int[password.Length];

          for (int iIx1 = 0; iIx1 < (password.Length); iIx1++)
          {
            inlCod[iIx1] = password.Substring(iIx1, 1).ToCharArray()[0];
            inlSoma[iIx1] = inlNum;
            inlNum += 10;
            if (inlNum == 60)
            {
              inlNum = 10;
            }
          }
          for (int iIx1 = 0; iIx1 < (password.Length); iIx1++)
          {
            for (int iIx2 = 0; iIx2 < (password.Length); iIx2++)
            {
              if (inlCod[iIx1] == inlCod[iIx2])
              {
                inlSoma[iIx1] += 1;
              }
            }
            inlCod[iIx1] += inlSoma[iIx1];
            senha = string.Concat(senha, String.Format("{0:000}{1:000}", inlCod[iIx1], inlSoma[iIx1]));
          }
          return senha;
        }
        else
        {
          return string.Empty;
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string EncryptOn(string password, string key)
    {
      var oMemoryStream = new MemoryStream();
      var oTripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider();
      Byte[] bylKey = Encoding.ASCII.GetBytes(key);
      Byte[] bylIV = { 0, 0, 0, 0, 0, 0, 0, 0 };
      CryptoStream oCryptoStream;
      Byte[] bylData;
      try
      {
        oMemoryStream.SetLength(0);
        oTripleDesCryptoServiceProvider.Padding = PaddingMode.Zeros;
        oCryptoStream = new CryptoStream(oMemoryStream, oTripleDesCryptoServiceProvider.CreateEncryptor(bylKey, bylIV), CryptoStreamMode.Write);
        bylData = Encoding.Unicode.GetBytes(password);
        oCryptoStream.Write(bylData, 0, bylData.Length);
        oCryptoStream.FlushFinalBlock();
        oMemoryStream.Seek(0, SeekOrigin.Begin);
        bylData = oMemoryStream.ToArray();
        oCryptoStream.Close();
        return RemoveNullStrings(Convert.ToBase64String(bylData));
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string Key()
    {
      return EncryptOff("086011059021152031115041160051089012122021147031090041151052075011121021079031118041150051125011");
    }

    private static string RemoveNullStrings(string palavraComNulos)
    {
      StringBuilder palavraSemNulos = new StringBuilder();
      foreach (Char caracter in palavraComNulos)
      {
        if (caracter != '\0')
        {
          palavraSemNulos.Append(caracter);
        }
      }
      return palavraSemNulos.ToString();
    }

  }
}
