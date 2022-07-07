if (args.Length == 1)
    DoWork();
else
    ShowHelp();

void DoWork()
{
    var mp4Files = Directory.GetFiles(args[0])
                            .Where(f => Path.GetFileName(f).ToLower().StartsWith("g") &&
                                        Path.GetFileNameWithoutExtension(f).Length == 8 &&
                                        string.Compare(Path.GetExtension(f).ToLower(), ".mp4") == 0)
                            .Select(f => new GoproFileName(f))
                            .OrderBy(f => Path.GetFileName(f.NewFileName));

    if (mp4Files != null && mp4Files.Any())
    {
        foreach (var g in mp4Files)
        {
            File.Move(g.OrgFileName, g.NewFileName, true);
            Console.WriteLine(string.Format("변경 완료 : {0} -> {1}", Path.GetFileName(g.OrgFileName), Path.GetFileName(g.NewFileName)));
        }
    }
    else
    {
        Console.WriteLine("이름 변경 대상 MP4 파일이 없습니다.");
    }
}

void ShowHelp()
{
    Console.WriteLine("GoproFileRenameConsoleApp [MP4 파일이 있는 폴더 경로]");
}

class GoproFileName
{
    public readonly string OrgFileName;
    public readonly string NewFileName;

    public GoproFileName(string orgFilePathName)
    {
        OrgFileName = orgFilePathName;

        var orgFileNameWithoutExtention = Path.GetFileNameWithoutExtension(OrgFileName);
        var fileNumber = orgFileNameWithoutExtention.Substring(4);
        var chapterNumber = orgFileNameWithoutExtention.Substring(2, 2);
        NewFileName = Path.Combine(Path.GetDirectoryName(orgFilePathName), String.Format("G{0}{1}_{2}", fileNumber, chapterNumber, Path.GetFileName(OrgFileName)));
    }
}

