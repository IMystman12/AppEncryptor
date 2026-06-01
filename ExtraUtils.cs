using System.Security.Cryptography;

public class ExtraUtils
{
    static ExtraUtils Instance = new ExtraUtils();
    public byte[] XorCrypt(byte[] bytes, byte key)
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] ^= key;
        }
        return bytes;
    }
    public byte[] ShiftEncrypt(byte[] bytes, int shift)
    {
        byte[] result = new byte[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            result[i] = (byte)(bytes[i] + shift);
        }
        return result;
    }
    public byte[] ShiftDecrypt(byte[] bytes, int shift)
    {
        byte[] result = new byte[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            result[i] = (byte)(bytes[i] - shift);
        }
        return result;
    }
    public byte[] Reverse(byte[] bytes)
    {
        byte[] result = new byte[bytes.Length];
        for (int i = 0, b = bytes.Length - 1; i < bytes.Length; i++)
        {
            result[i] = bytes[b - i];
        }
        return result;
    }
    public byte[] AESEncrypt(byte[] data, byte[] key, byte[] IV)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = IV;
            using (var val = aes.CreateEncryptor())
            {
                return AESCore(data, val);
            }
        }
    }
    public byte[] AESDecrypt(byte[] data, byte[] key, byte[] IV)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = IV;
            using (var val = aes.CreateDecryptor())
            {
                return AESCore(data, val);
            }
        }
    }
    public byte[] GenerateKey(int size = 256)
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            return aes.Key;
        }
    }
    public byte[] GenerateIV()
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateIV();
            return aes.IV;
        }
    }
    byte[] AESCore(byte[] data, ICryptoTransform cryptoTransform)
    {
        using (MemoryStream memory = new MemoryStream())
        {
            using (CryptoStream crypto = new CryptoStream(memory, cryptoTransform, CryptoStreamMode.Write))
            {
                crypto.Write(data, 0, data.Length);
                crypto.FlushFinalBlock();
                return memory.ToArray();
            }
        }
    }

    public void WriteBytes(BinaryWriter bw, byte[] bytes)
    {
        bw.Write(bytes.Length);
        bw.Write(bytes);
    }
    public byte[] ReadBytes(BinaryReader br)
    {
        byte[] bytes = new byte[br.ReadInt32()];
        bytes = br.ReadBytes(bytes.Length);
        return bytes;
    }
    public const int standardHeaderLength = 9;
}