namespace Repo_Manager_Deluxe
{
    using System;
    using System.Diagnostics;

    internal class Extract
    {
        public static void deb(string file, string repo)
        {
            Process process = new Process();
            ProcessStartInfo info1 = new ProcessStartInfo();
            info1.WindowStyle = ProcessWindowStyle.Normal;
            info1.WorkingDirectory = repo;
            info1.FileName = "cmd.exe";
            info1.RedirectStandardInput = true;
            info1.CreateNoWindow = true;
            info1.UseShellExecute = false;
            process.StartInfo = info1;
            process.Start();
            process.StandardInput.WriteLine("cd temp");
            process.StandardInput.WriteLine("tar zxvf " + file);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
        }

        public static void create_packages_bz2(string file, string repo)
        {
            Process process = new Process();
            ProcessStartInfo info1 = new ProcessStartInfo();
            info1.WindowStyle = ProcessWindowStyle.Normal;
            info1.WorkingDirectory = repo;
            info1.FileName = "cmd.exe";
            info1.RedirectStandardInput = true;
            info1.CreateNoWindow = true;
            info1.UseShellExecute = false;
            process.StartInfo = info1;
            process.Start();
            process.StandardInput.WriteLine("cd temp");
            process.StandardInput.WriteLine("tar zxvf " + file);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
        }
    }
}

