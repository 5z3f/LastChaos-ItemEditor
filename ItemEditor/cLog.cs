using ItemEditor.Forms;

namespace ItemEditor
{
    public enum MESSAGE_TYPE
    {
        INFORMATION,
        SUCCESS,
        WARNING,
        ERROR,
        EXCEPTION,
    }

    public class cLog
    {
        System.Windows.Forms.RichTextBox rtbLog;
        public cLog(System.Windows.Forms.RichTextBox rtbLog)
        {
            this.rtbLog = rtbLog;
        }

        public void MessageLog(string message, MESSAGE_TYPE type)
		{
            rtbLog.AppendText(string.Format("[{0}] | [{1}] - {2}\n", System.DateTime.Now.ToString("hh:mm:ss"), type.ToString(), message));
		}
    }
}
