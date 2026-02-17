using System.IO.Compression;

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
path = Console.ReadLine();
while (!File.Exists(path))
{
    Console.WriteLine("Invaild! Try again!");
    path = Console.ReadLine();
}
Encrypte(stream, path);

//Or create your own encryptor
void Encrypte(MemoryStream fileData, string executePath)
{
    byte[] bytes = fileData.ToArray();
    fileData = new MemoryStream();
    using (BinaryWriter bin = new BinaryWriter(fileData))
    {
        bin.Write(executePath);
        bin.Write(bytes);
    }
    bytes = fileData.ToArray();

    for (int j = 0; j < bytes.Length; j++)
    {
        bytes[j] ^= 0x55;
    }

    fileData = new MemoryStream();
    new MemoryStream(bytes).CopyTo(new GZipStream(fileData, CompressionLevel.Fastest));
    File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Result.execution"), fileData.ToArray());
}