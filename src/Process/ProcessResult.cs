using System;
using System.Diagnostics;

namespace Decoherence
{
    public class ProcessResult
    {
        public int ExitCode => mProcess.ExitCode;
        
        public string Stdout
        {
            get
            {
                if (mStdout == null)
                {
                    mStdout = mStdoutGetter();
                }

                return mStdout;
            }
        }

        public string Stderr
        {
            get
            {
                if (mStderr == null)
                {
                    mStderr = mStderrGetter();
                }

                return mStderr;
            }
        }

        private readonly Process mProcess;
        private readonly Func<string> mStdoutGetter;
        private readonly Func<string> mStderrGetter;
        private string? mStdout;
        private string? mStderr;

        public ProcessResult(Process process, Func<string> stdoutGetter, Func<string> stderrGetter)
        {
            mProcess = process;
            mStdoutGetter = stdoutGetter;
            mStderrGetter = stderrGetter;
        }
    }
}