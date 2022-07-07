using GoproFileRenameConsoleApp;
using System.Globalization;

switch (args.Length)
{
    case 1:
        if (!Directory.Exists(args[0]))
        {
            ShowHelp();
            return;
        }

        DoWork();
        break;
    default:
        ShowHelp();
        return;
}

void DoWork()
{
    var mp4Files = ListUp(args[0]);

    if (mp4Files != null && mp4Files.Any())
    {
        mp4Files.ForEach(m => Console.WriteLine(m.Preview()));

        Console.WriteLine("위와 같이 파일명을 변경 하시겠습니까? (y/n)");
        var answer = Console.ReadKey();

        if (answer.Key == ConsoleKey.Y)
        {
            Console.WriteLine(string.Empty);
            Rename(mp4Files, m => Console.WriteLine(m));
        }
    }
}

void ShowHelp()
{
    Console.WriteLine("GoproFileRenameConsoleApp [MP4 파일이 있는 폴더 경로]");
}

void Rename(IEnumerable<GoproFileName> goproFileNames, Action<string> msgOutCallback)
{
    foreach (var g in goproFileNames)
    {
        if (!File.Exists(g.OrgFileName))
        {
            throw new FileNotFoundException(string.Format("{0} 파일이 존재하지 않습니다.", g.OrgFileName));
        }

        File.Move(g.OrgFileName, g.NewFileName, true);
        msgOutCallback?.Invoke(string.Format("변경 완료 : {0} -> {1}", Path.GetFileName(g.OrgFileName), Path.GetFileName(g.NewFileName)));
    }
}

IEnumerable<GoproFileName>? ListUp(string path)
{
    if (Directory.Exists(path))
    {
        return Directory.GetFiles(path)
                            .Where(f => Path.GetFileName(f).StartsWith("G", true, CultureInfo.CurrentCulture) &&
                                        Path.GetFileNameWithoutExtension(f).Length == 8 &&
                                        string.Compare(Path.GetExtension(f).ToLower(), ".mp4") == 0)
                            .Select(f => new GoproFileName(f))
                            .OrderBy(f => f.ToString());
    }

    return null;
}


