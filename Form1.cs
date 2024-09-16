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
        public ConfigManager configManager;
        public Form1()
        {
            InitializeComponent();

            logger = new Logger(DebugTextBox);
            Uri uri = new Uri("https://telegram.blum.codes/#tgWebAppData=query_id%3DAAEL5EQNAAAAAAvkRA3j65J-%26user%3D%257B%2522id%2522%253A222618635%252C%2522first_name%2522%253A%2522Gabriel%2522%252C%2522last_name%2522%253A%2522Gollo%2522%252C%2522username%2522%253A%2522GabrielGollo%2522%252C%2522language_code%2522%253A%2522en%2522%252C%2522allows_write_to_pm%2522%253Atrue%257D%26auth_date%3D1726510879%26hash%3Daaca452d41c564321a1a8973c53cda42856b4f985f96780381c9ca03de2cfeb6&tgWebAppVersion=7.8&tgWebAppPlatform=web&tgWebAppThemeParams=%7B%22bg_color%22%3A%22%23212121%22%2C%22button_color%22%3A%22%238774e1%22%2C%22button_text_color%22%3A%22%23ffffff%22%2C%22hint_color%22%3A%22%23aaaaaa%22%2C%22link_color%22%3A%22%238774e1%22%2C%22secondary_bg_color%22%3A%22%23181818%22%2C%22text_color%22%3A%22%23ffffff%22%2C%22header_bg_color%22%3A%22%23212121%22%2C%22accent_text_color%22%3A%22%238774e1%22%2C%22section_bg_color%22%3A%22%23212121%22%2C%22section_header_text_color%22%3A%22%238774e1%22%2C%22subtitle_text_color%22%3A%22%23aaaaaa%22%2C%22destructive_text_color%22%3A%22%23ff595a%22%7D");
        }

        private async void buttonStartBot_Click(object sender, EventArgs e)
        {
            try
            {
               string queryId = queryIdTextBox.Text;
                if (string.IsNullOrEmpty(queryId)) {
                    throw new Exception("The field  Query ID cannot be null or empty.");
                }

                buttonStartBot.Enabled = false;
                int min = (int)minPointNumeric.Value;
                int max = (int)maxPointNumeric.Value;
                string token = bearerTokenTextBox.Text;

                if (botLogic == null)
                {
                    botLogic = new BotLogic(logger, min, max);
                }

                Boolean isValid = false;
                if (!String.IsNullOrEmpty(token))
                {
                    botLogic.setToken(token);
                    isValid = await botLogic.CheckToken();
                }


                if (!isValid) {
                    logger.Warn("Trying to get another token with queryId...");
                    try
                    {
                        string newToken = await botLogic.LoginUsingQueryId(queryId);
                        bearerTokenTextBox.Text = newToken;
                    } catch (Exception ex) {
                        logger.Error("Failed to get token with queryId: " + ex.Message);
                        logger.Warn("Trying to get another token by refreshing token...");
                        try
                        {
                            string newToken2 = await botLogic.RefreshToken();
                            bearerTokenTextBox.Text = newToken2;

                        }
                        catch (Exception ex2)
                        {
                            throw new Exception("Failed to get token, you'll need to get the query_id string in telegram web." + ex2.Message);
                        }
                    }
                }

                buttonStopBot.Enabled = true;
                
                await Task.Run(() => botLogic.StartMainLoop());
                buttonStartBot.Enabled = true;
            }
            catch (Exception ex) {
                logger.Error(ex.Message);
                MessageBox.Show(ex.Message, "Bot Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonStartBot.Enabled = true;
            }
        }

        private void buttonStopBot_Click(object sender, EventArgs e)
        {
            buttonStopBot.Enabled = false;

            botLogic.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            configManager = new ConfigManager();
            logger.Info("Loading configs...");

            queryIdTextBox.Text = configManager.BotConfig.QueryId;
            bearerTokenTextBox.Text = configManager.BotConfig.BearerToken;
            minPointNumeric.Value = configManager.BotConfig.MinPoint;
            maxPointNumeric.Value = configManager.BotConfig.MaxPoint;

            logger.Info("Configs loaded.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (botLogic != null && botLogic.isRunning)
            {
                // Exibir uma mensagem e cancelar o fechamento
                MessageBox.Show("The bot is running. Stop the bot and wait it finish before closing.", "The bot is running", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Cancelar o fechamento
                e.Cancel = true;
                return;
            }

            if (configManager == null)
            {
                logger.Error("Failed to save configs, configManager was not instantiated."); return;
            }

            logger.Info("Saving Configs...");

            configManager.BotConfig.QueryId = queryIdTextBox.Text;
            configManager.BotConfig.BearerToken = bearerTokenTextBox.Text;
            configManager.BotConfig.MinPoint = (int)minPointNumeric.Value;
            configManager.BotConfig.MaxPoint = (int)maxPointNumeric.Value;

            configManager.SaveConfig();

            logger.Info("Configs saved.");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            queryIdTextBox.UseSystemPasswordChar = !checkBox2.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bearerTokenTextBox.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            WebTelegramForm webTelegramForm = new WebTelegramForm(logger);
            webTelegramForm.Show();

            string queryId = await webTelegramForm.GetQueryId();
            if(!string.IsNullOrEmpty(queryId))
            {
                queryIdTextBox.Text = queryId.Trim().Trim('"');
            }

            webTelegramForm.Close();
        }
    }
}
