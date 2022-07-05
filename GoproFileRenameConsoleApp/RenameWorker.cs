using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GoproFileRenameConsoleApp
{
    internal class RenameWorker
    {
        public enum RenameOptions
        {
            Overwrite,
            Ignore,
            Stop
        }

        public RenameOptions RenameOption { get; set; } = RenameOptions.Ignore;

        public void Rename(IEnumerable<GoproFileName> goproFileNames, Action<string> msgOutCallback)
        {
            foreach(var g in goproFileNames)
            {
                Rename(g, msgOutCallback);
            }
        }

        public void Rename(GoproFileName goproFileName, Action<string> msgOutCallback)
        {
            var orgFileName = goproFileName.OrgFileName;
            var newFileName = goproFileName.NewFileName;

            if (!File.Exists(orgFileName))
            {
                throw new FileNotFoundException(string.Format("{0} 파일이 존재하지 않습니다.", orgFileName));
            }

            if (File.Exists(newFileName))
            {
                switch(RenameOption)
                {
                    case RenameOptions.Overwrite:
                        File.Move(orgFileName, newFileName, true);
                        msgOutCallback?.Invoke(string.Format("변경 완료(덮어씀) : {0} -> {1}", Path.GetFileName(orgFileName), Path.GetFileName(newFileName)));
                        break;
                    case RenameOptions.Ignore:
                        msgOutCallback?.Invoke(string.Format("변경 무시 : {0} -> {1}", Path.GetFileName(orgFileName), Path.GetFileName(newFileName)));
                        break;
                    case RenameOptions.Stop:
                        throw new Exception(String.Format("변경 하려는 파일명 {0}이 이미 존재 하여 작업을 중지 합니다.", newFileName));
                }
            }
            else
            {
                File.Move(orgFileName, newFileName);
                msgOutCallback?.Invoke(string.Format("변경 완료 : {0} -> {1}", Path.GetFileName(orgFileName), Path.GetFileName(newFileName)));
            }
        }

        public void SetRenameOption(string renameOption)
        {
            switch(renameOption.ToLower())
            {
                case "-w":
                    RenameOption = RenameOptions.Overwrite;
                    break;
                case "-i":
                    RenameOption = RenameOptions.Overwrite;
                    break;
                case "-s":
                    RenameOption = RenameOptions.Overwrite;
                    break;
            }
        }

        public static bool IsIn(string renameOption)
        {
            var options = new[] { "-w", "-i", "-s" };

            return options.Any(o => string.Compare(renameOption, o, true) == 0);
        }

        public static IEnumerable<GoproFileName>? ListUp(string path)
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
    }
}
