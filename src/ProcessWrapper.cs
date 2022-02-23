using System;
using System.Diagnostics;

namespace Decoherence
{
    public class ProcessWrapper
    {
        private readonly Process mProcess;

        public int ExitCode => mProcess.ExitCode;

        public string Stderr => mProcess.StandardError.ReadToEnd();
        
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="printInCurrentProcess">子进程的stdout、stderr是否在本进程输出（unix平台的默认行为，win平台不会）</param>
        /// <returns></returns>
        public ProcessWrapper(
            string fileName,
            string arguments,
            bool printInCurrentProcess)
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
        }

        /// <summary>
        /// 阻塞执行
        /// </summary>
        /// <returns>进程返回值</returns>
        public int Run()
        {
            mProcess.Start();
            mProcess.WaitForExit();
            return mProcess.ExitCode;
        }
    }
}