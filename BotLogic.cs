﻿using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BotBlum
{
    public class BotLogic
    {
        private string token = "Bearer token";
        private HttpClient httpClient = new HttpClient();
        public Logger logger = null;
        public Boolean isRunning = false;
        public Boolean stop = false;
        private int minPoint = 40;
        private int maxPoint = 150;

        public BotLogic(string bearerToken, Logger lg, int minP, int maxP)
        {
            setToken(bearerToken);


            this.logger = lg;
            this.minPoint = minP;
            this.maxPoint = maxP;

            // set timeout of 6 seconds
            httpClient.Timeout = TimeSpan.FromSeconds(3);

            // set common headers
            httpClient.DefaultRequestHeaders.Add("Origin", "https://telegram.blum.codes");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Microsoft Edge\";v=\"126\", \"Microsoft Edge WebView2\";v=\"126\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            setToken(token);
        }
        public void setToken(string bearerToken)
        {
            if (String.IsNullOrEmpty(bearerToken))
            {
                throw new Exception("O Bearer Token não pode ser nulo ou vázio");
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
                logger.Warn("O token atual não é válido");
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
            logger.Info("Tentando iniciar um novo game");
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
            
            logger.Info($"GameId '{gameIdRes}' iniciado");

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

            logger.Info($"GameId {gameId} recompensa de {points} pontos capturada!");
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
                isRunning = true;
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
                logger.Info("Aguardando 33 segundos para capturar recompensa");
                await Sleep(33000); // wait 33 seconds before claiming
                var points = GeneratePoints();
                await ClaimGameReward(gameId, points);
                await Sleep(500);

                var newUserBalance = await GetUserBalance();
                var newAvailableBalance = newUserBalance.GetProperty("availableBalance").GetString();

                isRunning = false;

                Double claimedInt = (Double.Parse(newAvailableBalance) - Double.Parse(availableBalance));
                string claimed = claimedInt.ToString();

                logger.Info($"Game reward claimed - {gameId} - free tickets: {playPasses} - balance: {newAvailableBalance} - claimed -> {claimed} - tried -> {points}");

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

        public async Task<Boolean> StartMainLoop()
        {
            logger.Info("Bot started");
            while (!stop)
            {
                await ExecuteBot();
            }

            stop = false;
            logger.Info("Bot stopped.");
            return true;
        }
    }
}
