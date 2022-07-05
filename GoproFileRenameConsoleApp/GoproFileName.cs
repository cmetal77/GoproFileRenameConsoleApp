using System;
using System.Collections.Generic;
using System.IO;

namespace GoproFileRenameConsoleApp
{
    internal class GoproFileName
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

        public override string ToString()
        {
            return Path.GetFileName(NewFileName);
        }

        public string Preview()
        {
            return string.Format("{0} -> {1}", Path.GetFileName(OrgFileName), Path.GetFileName(NewFileName));
        }
    }

    internal static class GoproFileNameStatic
    {
        public static void ForEach(this IEnumerable<GoproFileName> gfn, Action<GoproFileName> action)
        {
            foreach (var g in gfn)
            {
                action(g);
            }
        }
    }
}
