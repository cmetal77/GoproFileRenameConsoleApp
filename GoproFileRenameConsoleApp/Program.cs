/*
 * 1. 매개변수로 입력 된 경로 내 mp4 파일 목록 가져오기
 * 2. mp4 목록을 파일 생성 시간 기준으로 정렬 하기
 * 3. 정렬 된 목록에 따라 파일명 앞에 순번 붙이기
 * 4. 파일명 변경 하기
 * 
 * 5. 예외상황 처리
 * 
 * 
 * 실행 매개변수
 * 첫번째 : mp4 파일이 있는 폴더 경로
 * 두번째 : 변경 될 파일이 있는 경우 덮어쓰기(-w), 무시하고 다음 파일로 넘어가기(-i), 중지(-s)
 *          (기본값 = -i)
 * 실행 매개변수가 없이 실행 되는 경우 안내문 표시
 */

using GoproFileRenameConsoleApp;

var renameWorker = new RenameWorker();

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
    case 2:
        if (RenameWorker.IsIn(args[1]))
        {
            renameWorker.SetRenameOption(args[1]);
            DoWork();
            break;
        }
        ShowHelp();
        return;
    default:
        ShowHelp();
        return;
}

void DoWork()
{
    var mp4Files = RenameWorker.ListUp(args[0]);

    if (mp4Files != null && mp4Files.Any())
    {
        mp4Files.ForEach(m => Console.WriteLine(m.Preview()));

        Console.WriteLine("위와 같이 파일명을 변경 하시겠습니까? (y/n)");
        var answer = Console.ReadKey();

        if (answer.Key == ConsoleKey.Y)
        {
            Console.WriteLine(string.Empty);
            renameWorker.Rename(mp4Files, m => Console.WriteLine(m));
        }
    }
}

void ShowHelp()
{
    Console.WriteLine("GoproFileRenameConsoleApp [파일명] [옵션]");
    Console.WriteLine("[옵션]");
    Console.WriteLine("-i : 변경 할 파일이 존재 하는 경우 변경 하지 않고 다음 대상 파일로 넘어 갑니다.");
    Console.WriteLine("-w : 변경 할 파일이 존재 하는 경우 덮어 씁니다.");
    Console.WriteLine("-s : 변경 할 파일이 존재 하는 경우 작업을 종료 합니다.");
}


