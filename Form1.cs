using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotBlum
{
    public partial class Form1 : Form
    {
        public BotLogic botLogic;
        public Logger logger;
        public Form1()
        {
            InitializeComponent();

            logger = new Logger(DebugTextBox);
            //logger.Info("Teste");
            //logger.Error("Error Teste");
            //logger.Warn("Warn Teste");
        }

        private async void buttonStartBot_Click(object sender, EventArgs e)
        {
            try
            {
                buttonStartBot.Enabled = false;
                int min = (int)minPointNumeric.Value;
                int max = (int)maxPointNumeric.Value;
                string token = bearerTokenTextBox.Text;

                if (botLogic == null)
                {
                    botLogic = new BotLogic(token, logger, min, max);
                }
                botLogic.setToken(token);
                Boolean isValid = await botLogic.CheckToken();

                if (!isValid) {
                    logger.Warn("Tentando gerar outro token...");
                    try
                    {
                        string newToken = await botLogic.RefreshToken();
                        bearerTokenTextBox.Text = newToken;

                    }
                    catch (Exception ex) {
                        throw new Exception("Falha ao gerar outro token, pegue manualmente."+ex.Message);
                    }
                    
                }

                buttonStopBot.Enabled = true;
                await Task.Run(() => botLogic.StartMainLoop());
                buttonStartBot.Enabled = true;
            }
            catch (Exception ex) {
                logger.Error(ex.Message);
                MessageBox.Show(ex.Message);
                buttonStartBot.Enabled = true;
            }
        }

        private void buttonStopBot_Click(object sender, EventArgs e)
        {
            buttonStopBot.Enabled = false;

            botLogic.Stop();
        }
    }
}
