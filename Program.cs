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
    ExtraUtils utils = new ExtraUtils();
    var key = utils.GenerateKey();
    var iv = utils.GenerateIV();
    var bytes = fileData.ToArray();
    fileData.Dispose();
    GC.Collect();
    fileData = new MemoryStream();
    //compressing
    using (var fs = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Result.execution"), FileMode.Create))
    {
        using (BinaryWriter bw = new BinaryWriter(fs))
        {
            utils.WriteBytes(bw, utils.AESEncrypt(bytes, key, iv));
            utils.WriteBytes(bw, iv);
            bw.Write(executePath);
            utils.WriteBytes(bw, key);
            bw.Flush();
        }
    }
}
