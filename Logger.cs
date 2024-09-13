using System;
using System.Drawing;
using System.Windows.Forms;

namespace BotBlum
{
    public class Logger
    {
        private RichTextBox richTextBox;

        public Logger(RichTextBox rtb)
        {
            this.richTextBox = rtb;
        }

        // Função auxiliar para escrever texto com a cor especificada
        private void WriteLog(string message, Color color)
        {
            richTextBox.Invoke(new Action(() =>
            {
                richTextBox.SelectionStart = richTextBox.TextLength;
                richTextBox.SelectionLength = 0;

                richTextBox.SelectionColor = color;
                richTextBox.AppendText($"{DateTime.Now}: {message}\n");
                richTextBox.SelectionColor = richTextBox.ForeColor;
                richTextBox.ScrollToCaret();
            }));
        }

        public void Info(string message)
        {
            WriteLog(message, Color.Green);  // Verde para info
        }

        public void Error(string message)
        {
            WriteLog(message, Color.Red);  // Vermelho para error
        }

        public void Warn(string message)
        {
            WriteLog(message, Color.Orange);  // Amarelo para warn
        }

        public void Log(string message)
        {
            WriteLog(message, Color.Black);  // Normal (preto) para log
        }
    }
}
