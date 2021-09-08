using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AES
{
    public static string EncryptKey = "SherlockHolmes";

    private static string AESHead = "AESEncrypt";
    //凯撒加密偏移量
    private static int CaesarcipherOffset = 5;

    /// <summary>
    /// 默认密钥向量
    /// </summary>
    private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    //加密前字节1长度
    private static int EncryptBeforeLength1 = 100;
    //加密后字节1长度
    private static int EncryptAfterLength1 = 112;
    //加密第二组字节的偏移量
    private static int byteOffset = 200;
    //加密前字节2长度
    private static int EncryptBeforeLength2 = 400;
    //加密后字节2长度
    private static int EncryptAfterLength2 = 416;

    /// <summary>
    /// 文件加密，传入文件路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="EncrptyKey"></param>
    public static void AESFileEncrypt(string path, string EncrptyKey)
    {
        if (!File.Exists(path))
            return;

        try
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (fs != null)
                {
                    //读取字节头，判断是否已经加密过了
                    byte[] headBuff = new byte[10];
                    fs.Read(headBuff, 0, headBuff.Length);
                    string headTag = Encoding.UTF8.GetString(headBuff);
                    if (headTag == AESHead)
                    {
#if UNITY_EDITOR
                        Debug.Log(path + "已经加密过了！");
#endif
                        return;
                    }
                    //加密并且写入字节头
                    fs.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.SetLength(0);
                    byte[] headBuffer = Encoding.UTF8.GetBytes(AESHead);
                    fs.Write(headBuffer, 0, headBuffer.Length);
                    //凯撒加密
                    // buffer = CaesarcipherEncrypt(buffer, CaesarcipherOffset);
                    //取部分字节加密
                    byte[] partBt1 = SubByte(buffer, 0, EncryptBeforeLength1);
                    byte[] EncBuffer1 = AESEncrypt(partBt1, EncrptyKey);
                    EncryptAfterLength1 = EncBuffer1.Length;
                    //Debug.Log("EncBuffer1:" + EncBuffer1.Length);
                    //foreach (var item in EncBuffer1)
                    //{
                    //    Debug.Log("EncBuffer1:"+item);
                    //}
                    byte[] partBt2 = SubByte(buffer, byteOffset, EncryptBeforeLength2);
                    byte[] EncBuffer2 = AESEncrypt(partBt2, EncrptyKey);
                    EncryptAfterLength2 = EncBuffer2.Length;
                    //foreach (var item in EncBuffer2)
                    //{
                    //    Debug.Log("EncBuffer2:" + item);
                    //}


                    //Debug.Log("EncBuffer2:" + EncBuffer2.Length);
                    byte[] temp = SubByte(buffer, byteOffset - EncryptBeforeLength1, byteOffset - EncryptBeforeLength1);
                    //foreach (var item in temp)
                    //{
                    //    Debug.Log("temp:" + item);
                    //}
                    //Debug.Log("temp:" + temp.Length);
                    byte[] temp1 = SubByte(buffer, partBt1.Length + temp.Length + partBt2.Length, buffer.Length - (partBt1.Length + temp.Length + partBt2.Length));
                    //Debug.Log("temp1:" + temp1.Length);
                    byte[] all = new byte[EncBuffer1.Length + EncBuffer2.Length + temp.Length + temp1.Length];
                    Debug.Log("All:" + all.Length);

                    EncBuffer1.CopyTo(all, 0);
                    temp.CopyTo(all, EncBuffer1.Length);
                    EncBuffer2.CopyTo(all, EncBuffer1.Length + temp.Length);
                    temp1.CopyTo(all, EncBuffer1.Length + temp.Length + EncBuffer2.Length);

                    fs.Write(all, 0, all.Length);


                    // byte[] EncBuffer = AESEncrypt(buffer, EncrptyKey);
                    //Debug.Log("EncBuffer:" + EncBuffer.Length);
                    //fs.Write(EncBuffer, 0, EncBuffer.Length);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }


    /// <summary>
    /// 文件解密，传入文件路径（会改动加密文件，不适合运行时）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="EncrptyKey"></param>
    public static void AESFileDecrypt(string path, string EncrptyKey)
    {
        if (!File.Exists(path))
        {
            return;
        }
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (fs != null)
                {
                    byte[] headBuff = new byte[10];
                    fs.Read(headBuff, 0, headBuff.Length);
                    string headTag = Encoding.UTF8.GetString(headBuff);
                    if (headTag == AESHead)
                    {
                        byte[] buffer = new byte[fs.Length - headBuff.Length];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length - headBuff.Length));

                        fs.Seek(0, SeekOrigin.Begin);
                        fs.SetLength(0);

                        //取部分字节解密


                        byte[] partBt1 = SubByte(buffer, 0, EncryptAfterLength1);
                        byte[] EncBuffer1 = AESDecrypt(partBt1, EncrptyKey);
                        //foreach (var item in partBt1)
                        //{
                        //    Debug.LogError("partBt1:"+item);
                        //}

                        // Debug.Log("EncBuffer1:" + EncBuffer1.Length);
                        byte[] temp = SubByte(buffer, EncryptAfterLength1, byteOffset - EncryptBeforeLength1);
                        //foreach (var item in temp)
                        //{
                        //    Debug.LogError("temp:" + item);
                        //}
                        byte[] partBt2 = SubByte(buffer, partBt1.Length + temp.Length, EncryptAfterLength2);
                        byte[] EncBuffer2 = AESDecrypt(partBt2, EncrptyKey);
                        //Debug.Log("EncBuffer2:" + EncBuffer2.Length);
                        //foreach (var item in partBt2)
                        //{
                        //    Debug.LogError("partBt2:" + item);
                        //}
                        //Debug.Log("temp:" + temp.Length);
                        byte[] temp1 = SubByte(buffer, partBt1.Length + temp.Length + partBt2.Length, buffer.Length - (partBt1.Length + temp.Length + partBt2.Length));
                        //  Debug.Log("temp1:" + temp1.Length);
                        byte[] all = new byte[EncBuffer1.Length + temp.Length + EncBuffer2.Length + temp1.Length];
                        Debug.Log("all:" + all.Length);

                        EncBuffer1.CopyTo(all, 0);
                        temp.CopyTo(all, EncBuffer1.Length);
                        EncBuffer2.CopyTo(all, EncBuffer1.Length + temp.Length);
                        temp1.CopyTo(all, EncBuffer1.Length + temp.Length + EncBuffer2.Length);

                        fs.Write(all, 0, all.Length);
                        // byte[] DecBuffer = AESDecrypt(buffer, EncrptyKey);
                        ////凯撒解密
                        //// DecBuffer = CaesarcipherDecrypt(DecBuffer, CaesarcipherOffset);
                        //Debug.Log("DecBuffer:" + DecBuffer.Length);
                        //fs.Write(DecBuffer, 0, DecBuffer.Length);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 文件界面，传入文件路径，返回字节
    /// </summary>
    /// <returns></returns>
    public static byte[] AESFileByteDecrypt(string path, string EncrptyKey)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        byte[] DecBuffer = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs != null)
                {
                    byte[] headBuff = new byte[10];
                    fs.Read(headBuff, 0, headBuff.Length);
                    string headTag = Encoding.UTF8.GetString(headBuff);

                    if (headTag == AESHead)
                    {
                        byte[] buffer = new byte[fs.Length - headBuff.Length];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length - headBuff.Length));

                        //fs.Seek(0, SeekOrigin.Begin);
                        //fs.SetLength(0);

                        //取部分字节解密
                        byte[] partBt1 = SubByte(buffer, 0, EncryptAfterLength1);
                        byte[] EncBuffer1 = AESDecrypt(partBt1, EncrptyKey);

                        byte[] temp = SubByte(buffer, EncryptAfterLength1, byteOffset - EncryptBeforeLength1);

                        byte[] partBt2 = SubByte(buffer, partBt1.Length + temp.Length, EncryptAfterLength2);
                        byte[] EncBuffer2 = AESDecrypt(partBt2, EncrptyKey);

                        byte[] temp1 = SubByte(buffer, partBt1.Length + temp.Length + partBt2.Length, buffer.Length - (partBt1.Length + temp.Length + partBt2.Length));

                        DecBuffer = new byte[EncBuffer1.Length + temp.Length + EncBuffer2.Length + temp1.Length];


                        EncBuffer1.CopyTo(DecBuffer, 0);
                        temp.CopyTo(DecBuffer, EncBuffer1.Length);
                        EncBuffer2.CopyTo(DecBuffer, EncBuffer1.Length + temp.Length);
                        temp1.CopyTo(DecBuffer, EncBuffer1.Length + temp.Length + EncBuffer2.Length);
                    }
                    else
                    {
                        byte[] zipdata = new byte[fs.Length];
                        fs.Read(zipdata, 0, zipdata.Length);
                        DecBuffer = zipdata;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return DecBuffer;
    }

    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    public static string AESEncrypt(string EncryptString, string EncryptKey)
    {
        return Convert.ToBase64String(AESEncrypt(Encoding.Default.GetBytes(EncryptString), EncryptKey));
    }

    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    public static byte[] AESEncrypt(byte[] EncryptByte, string EncryptKey)
    {
        if (EncryptByte.Length == 0) { throw (new Exception("明文不得为空")); }
        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }
        byte[] m_strEncrypt;
        byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
        byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            MemoryStream m_stream = new MemoryStream();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(EncryptKey, m_salt);
            ICryptoTransform transform = m_AESProvider.CreateEncryptor(pdb.GetBytes(32), m_btIV);
            CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);


            Debug.Log(EncryptByte.Length);
            m_csstream.Write(EncryptByte, 0, EncryptByte.Length);

            m_csstream.FlushFinalBlock();
            m_strEncrypt = m_stream.ToArray();



            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return m_strEncrypt;
    }


    /// <summary>
    /// AES加密算法
    /// </summary>
    /// <param name="inputByteArray">明文</param>
    /// <param name="strKey">密钥</param>
    /// <returns>返回加密后的密文字节数组</returns>
    //public static byte[] AESEncrypt(byte[] inputByteArray, string strKey)
    //{
    //    //分组加密算法
    //    SymmetricAlgorithm des = Rijndael.Create();
    //    des.Padding = PaddingMode.None;
    //    int covering = (inputByteArray.Length + 1) % 16;
    //    int coveringLength = 0;
    //    if (covering != 0)//手动补位
    //    {
    //        coveringLength = 16 - covering;
    //    }
    //    int dataLength = (inputByteArray.Length + 1) + coveringLength;
    //    byte[] dataArray = new byte[dataLength];
    //    Buffer.BlockCopy(inputByteArray, 0, dataArray, 0, inputByteArray.Length);
    //    dataArray[dataArray.Length - 1] = Convert.ToByte(coveringLength);
    //    //设置密钥及密钥向量
    //    des.Key = Encoding.UTF8.GetBytes(strKey);
    //    des.IV = _key1;
    //    MemoryStream ms = new MemoryStream();
    //    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
    //    cs.Write(dataArray, 0, dataArray.Length);
    //    cs.FlushFinalBlock();
    //    byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组
    //    cs.Close();
    //    ms.Close();
    //    return cipherBytes;
    //}


    /// <summary>
    /// 凯撒密码加密
    /// </summary>
    /// <param name="EncryptByte"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static byte[] CaesarcipherEncrypt(byte[] EncryptByte, int offset)
    {
        for (int i = 0; i < EncryptByte.Length; i++)//恺撒加密
        {
            EncryptByte[i] += (byte)offset;
        }
        return EncryptByte;
    }
    /// <summary>
    /// 凯撒解密
    /// </summary>
    /// <param name="DecryptByte"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static byte[] CaesarcipherDecrypt(byte[] DecryptByte, int offset)
    {
        for (int i = 0; i < DecryptByte.Length; i++)//恺撒解密
        {
            DecryptByte[i] += (byte)offset;
        }
        return DecryptByte;
    }

    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    public static string AESDecrypt(string DecryptString, string DecryptKey)
    {
        return Convert.ToBase64String(AESDecrypt(Encoding.Default.GetBytes(DecryptString), DecryptKey));
    }

    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    public static byte[] AESDecrypt(byte[] DecryptByte, string DecryptKey)
    {
        if (DecryptByte.Length == 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
        byte[] m_strDecrypt;
        byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
        byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            MemoryStream m_stream = new MemoryStream();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(DecryptKey, m_salt);
            ICryptoTransform transform = m_AESProvider.CreateDecryptor(pdb.GetBytes(32), m_btIV);
            CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);


            m_csstream.Write(DecryptByte, 0, DecryptByte.Length);

            m_csstream.FlushFinalBlock();
            m_strDecrypt = m_stream.ToArray();
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return m_strDecrypt;
    }


    /// <summary>
    /// 截取字节数组
    /// </summary>
    /// <param name="srcBytes">要截取的字节数组</param>
    /// <param name="startIndex">开始截取位置的索引</param>
    /// <param name="length">要截取的字节长度</param>
    /// <returns>截取后的字节数组</returns>
    public static byte[] SubByte(byte[] srcBytes, int startIndex, int length)
    {
        MemoryStream bufferStream = new MemoryStream();
        byte[] returnByte = new byte[] { };
        if (srcBytes == null) { return returnByte; }
        if (startIndex < 0) { startIndex = 0; }
        if (startIndex < srcBytes.Length)
        {
            if (length < 1 || length > srcBytes.Length - startIndex) { length = srcBytes.Length - startIndex; }
            bufferStream.Write(srcBytes, startIndex, length);
            returnByte = bufferStream.ToArray();
            bufferStream.SetLength(0);
            bufferStream.Position = 0;
        }
        bufferStream.Close();
        bufferStream.Dispose();
        return returnByte;
    }


}

