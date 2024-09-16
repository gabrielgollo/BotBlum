using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BotBlum
{
    public class BotLogic
    {
        private string token = "";
        private HttpClient httpClient = new HttpClient();
        public Logger logger = null;
        public Boolean isRunning = false;
        public Boolean stop = false;
        private int minPoint = 40;
        private int maxPoint = 150;

        public BotLogic(Logger lg, int minP, int maxP)
        {
            this.logger = lg;
            this.minPoint = minP;
            this.maxPoint = maxP;

            // set timeout of 6 seconds
            httpClient.Timeout = TimeSpan.FromSeconds(3);

            // set common headers
            httpClient.DefaultRequestHeaders.Add("Origin", "https://telegram.blum.codes");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Microsoft Edge\";v=\"126\", \"Microsoft Edge WebView2\";v=\"126\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
        }
        public void setToken(string bearerToken)
        {
            if (String.IsNullOrEmpty(bearerToken))
            {
                throw new Exception("Bearer Token cannot be null or empty");
            }

            bearerToken = bearerToken.Trim();
            if (!bearerToken.Contains("earer"))
            {
                bearerToken = "Bearer " + bearerToken;
            }
            this.token = bearerToken;
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", bearerToken);
        }

        public async Task<string> LoginUsingQueryId(string queryId)
        {
            try
            {
                var body = new
                {
                    query = queryId
                };
                var contentBody = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://user-domain.blum.codes/api/v1/auth/provider/PROVIDER_TELEGRAM_MINI_APP", contentBody);
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(content);

                // will access property token.access
                if (data.TryGetProperty("token", out var tokenElement) &&
                    tokenElement.TryGetProperty("access", out var accessTokenElement))
                {
                    string accessToken = accessTokenElement.GetString();
                    
                    setToken(accessToken);

                    return accessToken;
                }

                throw new Exception("Failed to get access token using queryId");
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken == CancellationToken.None)
            {
                // Isso confirma que foi um timeout e não um cancelamento manual
                logger.Error("Request was canceled due to timeout.");
                throw new Exception("The request timed out.");
            }
            catch (Exception ex) {
                logger.Error("Failed to get access token using queryId");
                throw new Exception("Failed to get access token using queryId");
            }
        }

        public async Task<Boolean> CheckToken()
        {
            try
            {
                var response = await httpClient.GetAsync("https://user-domain.blum.codes/api/v1/user/me");

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(content);

                // should check if has a propertie message with "Invalid jwt token"

                data.TryGetProperty("message", out var message_out);
                string message = message_out.GetString();
                if(!String.IsNullOrEmpty(message) && String.Equals(message, "Invalid jwt token"))
                {
                    return false;
                }
                return true;
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken == CancellationToken.None)
            {
                // Isso confirma que foi um timeout e não um cancelamento manual
                logger.Error("Request was canceled due to timeout.");
                throw new Exception("The request timed out.");
            }

            catch (Exception ex) {
                logger.Warn("The current token is invalid.");
                return false;
            }
        }

        public async Task<string> RefreshToken()
        {
            try
            {
                var body = new
                {
                    refresh = this.token.Replace("Bearer ", "")
                };
                var contentBody = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Remove("Authorization");
                var response = await httpClient.PostAsync("https://user-domain.blum.codes/api/v1/auth/refresh", contentBody);
                var contentResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Unauthorized) {
                    throw new Exception($"Failed to request with status: '{response.StatusCode.ToString()}'; and response: {contentResponse} - while refreshing token");      
                }

                var data = JsonSerializer.Deserialize<JsonElement>(contentResponse);

                data.TryGetProperty("access", out var responseToken);

                string newToken = responseToken.GetString();
                
                setToken(newToken);

                return newToken;
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken == CancellationToken.None)
            {
                // Isso confirma que foi um timeout e não um cancelamento manual
                logger.Error("Request was canceled due to timeout.");
                throw new Exception("The request timed out.");
            }
        }

        public async Task<string> InitGameSession()
        {
            logger.Info("Trying to init a new game...");
            var response = await httpClient.PostAsync("https://game-domain.blum.codes/api/v1/game/play", null);
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Error starting game session: GAME_SESSION_ERROR");
            }

            var data = JsonSerializer.Deserialize<JsonElement>(content);
            if (!data.TryGetProperty("gameId", out var gameId))
            {
                throw new Exception("Returned no gameId: GAME_SESSION_ERROR");
            }
            var gameIdRes = gameId.GetString();
            
            logger.Info($"GameId '{gameIdRes}' started");

            return gameIdRes;
        }

        public async Task ClaimGameReward(string gameId, int points)
        {
            var body = new
            {
                gameId = gameId,
                points = points
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://game-domain.blum.codes/api/v1/game/claim", content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Error starting game session: GAME_SESSION_ERROR");
            }

            logger.Info($"GameId {gameId} reward of {points} points claimed!");
        }

        public async Task<JsonElement> GetUserBalance()
        {
            var response = await httpClient.GetAsync("https://game-domain.blum.codes/api/v1/user/balance");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(content);
        }

        public Task Sleep(int ms)
        {
            return Task.Delay(ms);
        }

        public int GeneratePoints()
        {
            var random = new Random();
            return random.Next(this.minPoint, this.maxPoint);
        }

        public async Task ExecuteBot()
        {
            try
            {
                logger.Info("Executing bot in 3s");
                await Sleep(3000);
                if(stop){ return; }
                var userBalance = await GetUserBalance();
                int playPasses = userBalance.GetProperty("playPasses").GetInt32();
                var availableBalance = userBalance.GetProperty("availableBalance").GetString();

                logger.Info($"User balance - free tickets: {playPasses} - balance: {availableBalance}");

                if (playPasses <= 0)
                {
                    logger.Error("Insufficient balance");
                    return;
                }

                var gameId = await InitGameSession();
                logger.Info("Waiting 33s to claim reward");
                await Sleep(33000); // wait 33 seconds before claiming
                var points = GeneratePoints();
                await ClaimGameReward(gameId, points);
                await Sleep(500);

                var newUserBalance = await GetUserBalance();
                var newAvailableBalance = newUserBalance.GetProperty("availableBalance").GetString();

                

                Double claimedNumber = (Double.Parse(newAvailableBalance, CultureInfo.InvariantCulture) - Double.Parse(availableBalance, CultureInfo.InvariantCulture));
                string claimed = claimedNumber.ToString();

                logger.Info($"Game reward claimed for {gameId} - free tickets: {playPasses} - balance: {newAvailableBalance} - claimed -> {claimed} - triedToClaim -> {points}");

                logger.Warn("Waiting 5s");
                await Sleep(5000);
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken == CancellationToken.None)
            {
                // Isso confirma que foi um timeout e não um cancelamento manual
                logger.Error("Request was canceled due to timeout.");
                await Sleep(3000);
            }

            catch (Exception ex)
            {
                logger.Error($"Error: {ex.Message}");
                await Sleep(3000);
            }
        }
        public Boolean Stop()
        {
            logger.Warn("Stopping bot...");
            stop = true;

            return true;
        }

        public async Task<Boolean> AsyncStop()
        {
            logger.Warn("Stopping bot...");
            stop = true;

            while (isRunning)
            {
                await Task.Delay(100);
            }

            return true;
        }

        public async Task<Boolean> StartMainLoop()
        {
            logger.Info("Bot started");

            while (!stop)
            {
                isRunning = true;
                await ExecuteBot();
            }

            isRunning = false;
            stop = false;
            logger.Info("Bot stopped.");
            return true;
        }
    }
}
