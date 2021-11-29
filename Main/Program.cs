using System;
using System.Windows.Forms;

namespace Main
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {           
                //处理未捕获的异常
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常
                Application.ThreadException += (_, ex) => HandleException((Exception) ex.Exception);
                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += (_, ex) => HandleException((Exception) ex.ExceptionObject);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="error"></param>
        static void HandleException(Exception error)
        {
            string strDateInfo = "异常：" + DateTime.Now.ToString() + "\r\n";
            var str = $"{strDateInfo}Application UnhandledException:{error.Message};\n\r堆栈信息:{error.StackTrace}";
            MessageBox.Show(str, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}