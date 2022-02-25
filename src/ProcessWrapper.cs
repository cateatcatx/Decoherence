using System;
using System.Diagnostics;
using Decoherence.Logging;

namespace Decoherence
{
    public class ProcessWrapper
    {
        private readonly Process mProcess;

        public int ExitCode => mProcess.ExitCode;

        public string Stderr => mProcess.StandardError.ReadToEnd();

        private readonly ILogger? mLogger;

        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="printInCurrentProcess">子进程的stdout、stderr是否在本进程输出（unix平台的默认行为，win平台不会）</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public ProcessWrapper(
            string fileName,
            string arguments,
            bool printInCurrentProcess,
            ILogger? logger = null)
        {
            ThrowUtil.ThrowIfArgumentNullOrWhiteSpace(fileName);
            ThrowUtil.ThrowIfArgumentNullOrWhiteSpace(arguments);
            
            mProcess = new Process();
            mProcess.StartInfo.FileName = fileName;
            mProcess.StartInfo.Arguments = arguments;
            mProcess.StartInfo.CreateNoWindow = true;
            mProcess.StartInfo.UseShellExecute = false;

            if (printInCurrentProcess)
            {
                throw new NotImplementedException();
            }
            else
            {
                // 平台统一（unix平台下stdout、stderr会直接在当前进程输出，这里强制所有平台都不输出）
                mProcess.StartInfo.RedirectStandardOutput = true;
                mProcess.StartInfo.RedirectStandardError = true;
            }

            mLogger = logger;
        }

        /// <summary>
        /// 阻塞执行
        /// </summary>
        /// <returns>进程返回值</returns>
        public int Run()
        {
            mLogger?.Verbose($"[ProcessWrapper][Run]{mProcess.StartInfo.FileName} {mProcess.StartInfo.Arguments}");
            
            mProcess.Start();
            mProcess.WaitForExit();
            return mProcess.ExitCode;
        }
    }
}