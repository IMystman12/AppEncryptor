
<h1 align="center">
  📦 Custom Package Builder
</h1>

<p align="center">
  <img src="https://img.shields.io/badge/Version-1.0-blue?style=for-the-badge&logo=visualstudio">
  <img src="https://img.shields.io/badge/Platform-Windows-success?style=for-the-badge&logo=windows">
  <img src="https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=csharp">
  <img src="https://img.shields.io/badge/Custom-Packaging-FF6B6B?style=for-the-badge&logo=package">
</p>

<p align="center">
  <b>✨ Package directories with custom execution paths! Not just compression! ✨</b>
</p>

<p align="center">
  Takes any folder, compresses it, and combines it with an execution path<br>
  into a single custom format file! 🚀
</p>

<hr>

## 🌟 What Makes This Special?

| Feature | Description |
| :--- | :--- |
| **📁 Custom Packaging** | Not just a ZIP - combines compressed data WITH execution instructions |
| **🔗 Path Embedding** | Stores relative executable path directly in the binary output |
| **🎯 Custom Format** | Creates `.execution` files with your own proprietary structure |
| **⚡ Memory Efficient** | All processing done in MemoryStream - no temp files |
| **🛠️ Fully Customizable** | Easy to modify the `Encrypte` method for your own format |

## 🔧 The Code

```csharp
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

// THIS IS WHERE THE MAGIC HAPPENS - Custom packaging!
void Encrypte(MemoryStream fileData, string executePath)
{
    byte[] bytes = fileData.ToArray();
    fileData = new MemoryStream();
    using (BinaryWriter bin = new BinaryWriter(fileData))
    {
        bin.Write(executePath);  // Write execution path FIRST
        bin.Write(bytes);        // Write compressed data SECOND
    }
    File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Result.execution"), fileData.ToArray());
}
```

## 📊 What's Really Happening?

### Input → Process → Output

```
Your Folder 📁
     ↓
[Zip Compression]
     ↓
Compressed Data 📦
     ↓
[Custom Packaging] ←─── Execution Path 📍
     ↓
Result.execution 🎯
(Binary: [Path String] + [ZIP Data])
```

### The Custom Format Explained

| Position | Content | Size |
|:---|:---|:---|
| **Header** | Execution Path String | Variable |
| **Body** | Complete ZIP Data | Variable |

This is NOT a standard ZIP file! It's YOUR custom format that contains:
1. **The execution path** (where the program should run from)
2. **The compressed folder** (all your files)

## 🎯 Why This Is Different

### ❌ Not Just Compression
- Regular ZIP: Just compressed files
- This tool: **Compressed files + execution instructions**

### ❌ Not Just an Installer
- Regular installer: Complex scripts and dependencies
- This tool: **Simple binary format you control**

### ✅ Your Own Format
- You decide the structure
- You control the reading/writing
- You can extend it however you want

## 🚀 How To Use

1. **Run the program**
```
Path:
C:\MyGame\Assets
```

2. **Enter execution path**
```
Execute Path:
C:\MyGame\Assets\Launcher.exe
```

3. **Get your custom package**
```
Result.execution created!
```

## 💡 Customize It Your Way!

### Idea 1: Add Multiple Paths
```csharp
void Encrypte(MemoryStream fileData, List<string> executePaths)
{
    byte[] bytes = fileData.ToArray();
    using (BinaryWriter bin = new BinaryWriter(new MemoryStream()))
    {
        bin.Write(executePaths.Count);  // Store how many paths
        foreach(var path in executePaths)
        {
            bin.Write(path);  // Store each path
        }
        bin.Write(bytes);  // Store compressed data
        
        File.WriteAllBytes("Result.custom", ((MemoryStream)bin.BaseStream).ToArray());
    }
}
```

### Idea 2: Add Metadata
```csharp
void Encrypte(MemoryStream fileData, string executePath)
{
    byte[] bytes = fileData.ToArray();
    using (BinaryWriter bin = new BinaryWriter(new MemoryStream()))
    {
        bin.Write("CUSTOMv1.0");  // Magic number / version
        bin.Write(DateTime.Now.Ticks);  // Timestamp
        bin.Write(executePath);  // Execution path
        bin.Write(bytes.Length);  // Data size
        bin.Write(bytes);  // Actual data
        
        File.WriteAllBytes("Result.package", ((MemoryStream)bin.BaseStream).ToArray());
    }
}
```

### Idea 3: Add Real Encryption
```csharp
void Encrypte(MemoryStream fileData, string executePath)
{
    byte[] bytes = fileData.ToArray();
    byte[] encrypted = AES_Encrypt(bytes, "your-password");  // REAL encryption
    
    using (BinaryWriter bin = new BinaryWriter(new MemoryStream()))
    {
        bin.Write(executePath);
        bin.Write(encrypted);  // Store encrypted data
        
        File.WriteAllBytes("Result.secure", ((MemoryStream)bin.BaseStream).ToArray());
    }
}
```

## 🔍 Sample Output Structure

If you open `Result.execution` in a hex editor, you'd see:

```
Offset  Content
0000    "Assets\Launcher.exe"  ← Plain text path
0020    PK...                   ← ZIP file signature
0023    [Compressed Data...]    ← Your actual files
```

## 🛠️ Tech Stack

- **Language**: C#
- **Compression**: System.IO.Compression (ZIP)
- **Binary Writing**: BinaryWriter
- **Memory**: Pure MemoryStream (no disk writes until final)

## 📦 Build Your Own Version

```bash
git clone https://github.com/yourusername/CustomPackageBuilder.git
cd CustomPackageBuilder
dotnet build
dotnet run
```

## 🎨 Make It Your Own

The `Encrypte` method is YOUR sandbox:
- Change the binary format
- Add encryption
- Include multiple files
- Add version info
- Sign your packages
- Anything you want!

## 📄 License

MIT - Use it, modify it, make it your own!

---

<p align="center">
  <b>This is NOT just a compressor - it's a CUSTOM PACKAGE BUILDER!</b>
  <br>
  ⭐ Star if you like custom formats! ⭐
</p>
```
