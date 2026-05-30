using System.IO.Compression;
using System.Security.Cryptography;

Console.WriteLine("Path:");
string path = Console.ReadLine();
while (!Directory.Exists(path))
{
    Console.WriteLine("Invaild! Try again!");
    path = Console.ReadLine();
}

MemoryStream stream = new MemoryStream();
ZipFile.CreateFromDirectory(path, stream, CompressionLevel.Fastest, true);

Console.WriteLine("Execute Path:");
string pathExecute = Console.ReadLine();
while (!File.Exists(pathExecute))
{
    Console.WriteLine("Invaild! Try again!");
    pathExecute = Console.ReadLine();
}

Encrypte(stream, Path.GetRelativePath(Directory.GetParent(path).FullName, pathExecute));

//Or create your own encryptor
void Encrypte(MemoryStream fileData, string executePath)
{
    int seed = Random.Shared.Next();
    Random rng = new Random(seed);
    ExtraUtils eu = new ExtraUtils();
    byte[] keys = eu.Reverse(eu.ShiftDecrypt(eu.XorCrypt(eu.GenerateKey(), (byte)rng.Next()), rng.Next()))
      , IV = eu.Reverse(eu.ShiftDecrypt(eu.XorCrypt(eu.GenerateIV(), (byte)rng.Next()), rng.Next()));

    byte[] bytes = eu.AESEncrypt(fileData.ToArray(), keys, IV);
    fileData = new MemoryStream();
    int length = 99;
    byte[] aHeader = new byte[length];
    for (int i = 0; i < length; i++)
    {
        aHeader[i] = (byte)new Random().Next();
    }

    using (BinaryWriter bin = new BinaryWriter(fileData))
    {
        for (int i = 0; i < length; i++)
        {
            aHeader[i] = (byte)new Random().Next();
        }
        bin.Write(aHeader);

        bin.Write(executePath);
        bin.Write(seed);

        for (int i = 0; i < length; i++)
        {
            aHeader[i] = (byte)new Random().Next();
        }
        bin.Write(aHeader);

        bin.Write(bytes.Length);
        bin.Write(bytes);

        for (int i = 0; i < length; i++)
        {
            aHeader[i] = (byte)new Random().Next();
        }
        bin.Write(aHeader);

        bin.Write(keys.Length);
        bin.Write(keys);

        for (int i = 0; i < length; i++)
        {
            aHeader[i] = (byte)new Random().Next();
        }
        bin.Write(aHeader);

        bin.Write(IV.Length);
        bin.Write(IV);

        for (int i = 0; i < length; i++)
        {
            aHeader[i] = (byte)new Random().Next();
        }
        bin.Write(aHeader);
    }

    File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Result.execution"), eu.Reverse(eu.ShiftDecrypt(eu.XorCrypt(fileData.ToArray(), (byte)rng.Next()), rng.Next())));
}