using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Decoherence.Logging;

namespace Decoherence
{
    public class ProcessExecutor
    {
        private readonly Action<string>? mOnStdout;
        private readonly Action<string>? mOnStderr;
        private readonly ILogger? mLogger;
        
        
        /// <param name="onStdout">用于接收stdout，null代表进程执行过程中不会输出，可以在进程执行完毕后通过Process.StandardOutput获取</param>
        /// <param name="onStderr">用于接收stderr，null代表进程执行过程中不会输出，可以在进程执行完毕后通过Process.StandardError获取</param>
        /// <param name="logger"></param>
        public ProcessExecutor(
            Action<string>? onStdout = null,
            Action<string>? onStderr = null,
            ILogger? logger = null)
        {
            mOnStdout = onStdout;
            mOnStderr = onStderr;
            mLogger = logger;
        }

        /// <summary>
        /// 同步执行
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ProcessResult Run(string filename, string arguments)
        {
            var process = new Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            
            // 平台统一（unix平台下stdout、stderr会直接在当前进程输出，这里强制所有平台都不输出）
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            Func<string> stdoutGetter;
            if (mOnStdout != null)
            {
                StringBuilder sb = new StringBuilder();
                stdoutGetter = () => sb.ToString();
                
                process.OutputDataReceived += (_, e) =>
                {
                    var data = e.Data;
                    sb.Append(data);
                    mOnStdout(data);
                };
            }
            else
            {
                stdoutGetter = () => process.StandardOutput.ReadToEnd();
            }
            
            Func<string> stderrGetter;
            if (mOnStderr != null)
            {
                StringBuilder sb = new StringBuilder();
                stderrGetter = () => sb.ToString();
                
                process.ErrorDataReceived += (_, e) =>
                {
                    var data = e.Data;
                    sb.Append(data);
                    mOnStderr(data);
                };
            }
            else
            {
                stderrGetter = () => process.StandardError.ReadToEnd();
            }

            mLogger?.Verbose($"Run process: {filename} {arguments}");
            process.Start();
            
            if (mOnStdout != null)
            {
                process.BeginOutputReadLine();
            }
            if (mOnStderr != null)
            {
                process.BeginErrorReadLine();
            }

            process.WaitForExit();

            return new ProcessResult(process, stdoutGetter, stderrGetter);
        }
    }
}