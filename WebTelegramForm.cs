using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotBlum.Properties;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace BotBlum
{
    internal class WebTelegramForm: Form
    {
        Logger logger;
        private WebView2 webView21;
        WebView2 webView;
        public WebTelegramForm(Logger logger)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebTelegramForm));
            this.logger = logger;
            this.Text = "Web.Telegram";
            this.Size = new System.Drawing.Size(800, 600);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.StartPosition = FormStartPosition.WindowsDefaultBounds;

            InitializeComponent();
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebTelegramForm));
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.SuspendLayout();
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView.Location = new System.Drawing.Point(0, 0);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(284, 261);
            this.webView.TabIndex = 0;
            this.webView.ZoomFactor = 1D;
            // 
            // WebTelegramForm
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.webView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WebTelegramForm";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Web.Telegram";
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.ResumeLayout(false);

        }

        public async Task<string> GetQueryId()
        {
            try
            {
                // Navega para o Telegram Web
                webView.Source = new Uri("https://web.telegram.org");
                while (true)
                {
                    await Task.Delay(1000); // 1 segundos, ajustável
                    // Injeção de script para verificar se o usuário está logado
                    string loginStatus = await webView.ExecuteScriptAsync("document.querySelector(\"body\").textContent.includes('Log in');");

                    // Se estiver logado, navegue até o bot
                    if (loginStatus == "false")
                    {
                        logger.Info("User is logged in");
                        // Navega até o bot especificado
                        webView.Source = new Uri("https://web.telegram.org/k/#@BlumCryptoBot");
                        // Espera um pouco para a página carregar
                        logger.Info("Bot page loaded");


                        logger.Info("Waiting for the bot command button to appear");
                        // Verifica se o botão de comando do bot apareceu
                        while (true)
                        {
                            // espera o botão de comando do bot aparecer
                            await Task.Delay(2000);
                            string isCommandButtonVisible = await webView.ExecuteScriptAsync("document.querySelector('div.new-message-bot-commands.is-view') != null;");
                            if (isCommandButtonVisible == "true")
                            {
                                break;
                            }
                        }

                        logger.Info("Clicking on the bot command button");
                        // Clica no comando do bot automaticamente
                        await webView.ExecuteScriptAsync(@"document.querySelector('div.new-message-bot-commands.is-view').click();");


                        logger.Info("Waiting blum app to load");
                        // Espera um pouco para carregar o iframe do WebApp
                        while (true)
                        {
                            await Task.Delay(1000);
                            // Captura a URL do WebApp dentro do iframe
                            string isIframeInPage = await webView.ExecuteScriptAsync("document.querySelectorAll('iframe').length > 0;");

                            if (isIframeInPage == "true") { break; }
                        }

                        // Captura a URL do WebApp
                        string iframeUrl = await webView.ExecuteScriptAsync("document.querySelectorAll('iframe')[0].src;");
                        logger.Info($"WebApp URL: {iframeUrl.Substring(0, 10)}");
                        if (!iframeUrl.Contains("https://telegram.blum.codes"))
                        {
                            MessageBox.Show("Error while getting WebApp URL", "Web Telegram Automation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }

                        iframeUrl = iframeUrl.Replace("\"", "");
                        iframeUrl = iframeUrl.Replace(" ", "");
                        // Navega até o WebApp
                        webView.Source = new Uri(iframeUrl); ;

                        // Espera o WebApp carregar
                        await Task.Delay(2000);  // Ajuste conforme necessário

                        while (true)
                        {
                            // Espera o WebApp carregar
                            await Task.Delay(1000);
                            // Verifica se o WebApp carregou
                            string isWebAppLoaded = await webView.ExecuteScriptAsync("!!this?.Telegram?.WebApp?.initData;");
                            if (isWebAppLoaded == "true") { break; }
                        }
                         // Executa o script para capturar o query_id
                        string queryId = await webView.ExecuteScriptAsync("this?.Telegram?.WebApp?.initData;");
                        // Mostra o query_id
                        MessageBox.Show($"Query ID: {queryId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return queryId;
                    }
                }

                return null;
            }
            catch (InvalidOperationException ex)
            {
                logger.Warn("O WebApp foi fechado inesperadamente: "+ex.Message);
                return null;
            }
            catch (Exception ex) { 
                logger.Error(ex.Message);
                return null;
            }
            
        }
    }
}
