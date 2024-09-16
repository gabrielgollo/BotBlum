using System;
using System.IO;
using System.Text.Json;

namespace BotBlum
{
    public class ConfigManager
    {
        private string configFilePath;

        public BotConfig BotConfig { get; private set; }

        public ConfigManager()
        {
            // Definir o caminho do arquivo de configuração na raiz do .exe
            configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "botconfig.json");

            // Carregar a configuração se o arquivo existir, senão usar valores padrão
            LoadConfig();
        }

        private void LoadConfig()
        {
            if (File.Exists(configFilePath))
            {
                try
                {
                    string json = File.ReadAllText(configFilePath);
                    BotConfig = JsonSerializer.Deserialize<BotConfig>(json);
                }
                catch
                {
                    // Caso de erro ao carregar, definir valores padrão
                    BotConfig = new BotConfig();
                }
            }
            else
            {
                // Definir valores padrão se o arquivo não existir
                BotConfig = new BotConfig();
            }
        }

        public void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(BotConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFilePath, json);
            }
            catch (Exception ex)
            {
                // Log ou tratamento de erro
                Console.WriteLine($"Erro ao salvar a configuração: {ex.Message}");
            }
        }
    }

    // Classe que contém as configurações do bot
    public class BotConfig
    {
        public string QueryId { get; set; } = "";
        public string BearerToken { get; set; } = "";
        public int MinPoint { get; set; } = 40;
        public int MaxPoint { get; set; } = 150;
    }
}
