namespace War3_72bian
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            //处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, ex) => HandleException((Exception)ex.Exception);
            AppDomain.CurrentDomain.UnhandledException += (_, ex) => HandleException((Exception)ex.ExceptionObject);

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        /// <summary>
        /// 处理异常
        /// -------
        /// 注意: 子线程中的异常,
        /// 需要先在子线程中用try catch 捕获,
        /// 再用 this.BeginInvoke(() => throw e);
        /// 抛给主线程,才能在这里捕获到....
        /// </summary>
        /// <param name="error"></param>
        private static void HandleException(Exception error)
        {
            string strDateInfo = "异常：" + DateTime.Now.ToString() + "\r\n";
            var str = $"{strDateInfo}Application UnhandledException:{error.Message};\n\r堆栈信息:{error.StackTrace}";
            MessageBox.Show(str, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}