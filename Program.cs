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
    byte[] bytes = fileData.ToArray();
    fileData = new MemoryStream();
    using (BinaryWriter bin = new BinaryWriter(fileData))
    {
        bin.Write(executePath);
        bin.Write(bytes);
    }
    File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Result.execution"), fileData.ToArray());
}