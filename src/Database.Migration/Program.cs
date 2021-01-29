using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Database.Migration
{
    class Program
    {
        private static readonly string rootDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).Location);

        static void Main()
        {
            var connectionString = ConnectionStringHelper.GetConnectionString("TestDB");
            Console.WriteLine("Your ConnectionString is: {0}", connectionString);
            var scriptsDir = Path.Combine(rootDirectory, "Scripts");

            var processInfo = new ProcessStartInfo(GetRoundHouseFileName())
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "/f=" + scriptsDir + " /vf=BuildInfo /c=" + connectionString + " --ni",
                WorkingDirectory = Path.GetDirectoryName(GetRoundHouseFileName())
            };

            var exitCode = 0;

            using (var process = Process.Start(processInfo))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                var input = process.StandardInput;
                var output = process.StandardOutput;

                Console.WriteLine(output.ReadLine());
                Console.WriteLine(output.ReadLine());
                Console.WriteLine(output.ReadLine());

#if DEBUG
                Console.WriteLine("Press enter to Continue");
                input.Write(Console.ReadLine());
                input.Close();
#endif
                Console.WriteLine(output.ReadToEnd());
#if DEBUG
                Console.ReadLine();
#endif
                process.WaitForExit();
                exitCode = process.ExitCode;
                process.Close();
            }

            Environment.ExitCode = exitCode;
        }
        private static string GetRoundHouseFileName()
        {
            return Path.Combine(rootDirectory, "RoundhousE", "rh.exe");
        }

    }
}
