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
            // Verifica se a thread atual é a thread da UI
            if (richTextBox.InvokeRequired)
            {
                // Se não for, chama Invoke para fazer a atualização na thread da UI
                richTextBox.Invoke(new Action(() => WriteLog(message, color)));
            }
            else
            {
                // Se for a thread correta, escreve diretamente no RichTextBox
                richTextBox.SelectionStart = richTextBox.TextLength;
                richTextBox.SelectionLength = 0;

                richTextBox.SelectionColor = color;
                richTextBox.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
                richTextBox.SelectionColor = richTextBox.ForeColor; // Volta para a cor padrão
            }
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
