using System;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceProcess;

class MyService : ServiceBase
{
    protected override void OnStart(string[] args)
    {
        // Buraya servis başladığında yapılacak işleri yazabilirsiniz
        EventLog.WriteEntry("MyService started.");
    }

    protected override void OnStop()
    {
        EventLog.WriteEntry("MyService stopped.");
    }
}

class Program
{
    static void Main()
    {
        if (Environment.UserInteractive)
        {
            // yönetici kontrolü ve servis kurulum logic'i
            if (!IsAdministrator())
            {
                var psi = new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process.Start(psi);
                return;
            }

            if (!ServiceExists("test"))
                InstallService("test", Process.GetCurrentProcess().MainModule.FileName);

            Console.WriteLine("Servis kuruldu veya zaten vardı.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        else
        {
            ServiceBase.Run(new MyService());
        }
    }

    static bool IsAdministrator()
    {
        var id = WindowsIdentity.GetCurrent();
        var p = new WindowsPrincipal(id);
        return p.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static bool ServiceExists(string name)
    {
        try
        {
            using (var sc = new ServiceController(name)) { }
            return true;
        }
        catch
        {
            return false;
        }
    }

    static void InstallService(string name, string binPath)
    {
        var psi = new ProcessStartInfo("sc.exe",
            $"create {name} binPath= \"{binPath}\" start= auto")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        var p = Process.Start(psi);
        p.WaitForExit();
        Console.WriteLine(p.StandardOutput.ReadToEnd());
        Console.WriteLine(p.StandardError.ReadToEnd());
    }
}
