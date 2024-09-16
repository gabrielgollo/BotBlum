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
        }

        private async void buttonStartBot_Click(object sender, EventArgs e)
        {
            try
            {
               string queryId = queryIdTextBox.Text;
                if (string.IsNullOrEmpty(queryId)) {
                    throw new Exception("O campo Query ID não pode ser vazio.");
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
                    logger.Warn("Tentando gerar outro token com queryId...");
                    try
                    {
                        string newToken = await botLogic.LoginUsingQueryId(queryId);
                        bearerTokenTextBox.Text = newToken;
                    } catch (Exception ex) {
                        logger.Error("Falha ao gerar token com queryId: " + ex.Message);
                        logger.Warn("Tentando gerar outro token com renovação de token...");
                        try
                        {
                            string newToken2 = await botLogic.RefreshToken();
                            bearerTokenTextBox.Text = newToken2;

                        }
                        catch (Exception ex2)
                        {
                            throw new Exception("Falha ao gerar outro token, pegue manualmente." + ex2.Message);
                        }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            configManager = new ConfigManager();
            logger.Info("Carregando configurações...");

            queryIdTextBox.Text = configManager.BotConfig.QueryId;
            bearerTokenTextBox.Text = configManager.BotConfig.BearerToken;
            minPointNumeric.Value = configManager.BotConfig.MinPoint;
            maxPointNumeric.Value = configManager.BotConfig.MaxPoint;

            logger.Info("Configurações carregadas.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (botLogic != null && botLogic.isRunning)
            {
                // Exibir uma mensagem e cancelar o fechamento
                MessageBox.Show("O bot está rodando. Pare o bot antes de fechar.", "Bot em execução", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Cancelar o fechamento
                e.Cancel = true;
                return;
            }

            if (configManager == null)
            {
                logger.Error("Falha ao salvar configurações, configManager não foi inicializado."); return;
            }

            logger.Info("Salvando configurações...");

            configManager.BotConfig.QueryId = queryIdTextBox.Text;
            configManager.BotConfig.BearerToken = bearerTokenTextBox.Text;
            configManager.BotConfig.MinPoint = (int)minPointNumeric.Value;
            configManager.BotConfig.MaxPoint = (int)maxPointNumeric.Value;

            configManager.SaveConfig();

            logger.Info("Configurações salvas.");
        }
    }
}
