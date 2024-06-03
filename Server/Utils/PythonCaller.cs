using System.Diagnostics;

namespace Server.Utils
{
    public static class PythonCaller
    {
        public static string CallPythonCLI(string command, params string[] args)
        {
            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var scriptPath = Path.Combine(parentDir, "cli.py");
            var pythonPath = Path.Combine(parentDir, "env/Scripts/python");

            var startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"{scriptPath} {command} {string.Join(" ", args)}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
