using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SkwurlBotFix.Bots.Commands;
using DSharpPlus.Entities;
using System.Linq;

namespace SkwurlBotFix.Bots
{
    public class Bot
    {
        public readonly EventId BotEventId = new EventId(42, "Skwurl-Bot");
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        private static DiscordChannel _adminChannel;
        private static int _beats;

        public async Task RunAsync()
        {
            var activity = new DiscordActivity(";;help");

            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);


            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.All
            };

            Client = new DiscordClient(config);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(5)
            });

            Client.Ready += OnClientReady;
            Client.GuildAvailable += Client_GuildAvailable;
            Client.Heartbeated += OnHeartbeat;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                //Services = services
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<UserCommands>();
            Commands.RegisterCommands<UtilCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            sender.Logger.LogInformation(BotEventId, "Client is ready to process events.");

            sender.Logger.LogInformation(BotEventId, "Client Status Updated.");

            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
        {
            _adminChannel = e.Guild.Channels.Values.First(x => x.Name == "admin-chat");

            sender.Logger.LogInformation(BotEventId, $"Guild available: {e.Guild.Name}");

            return Task.CompletedTask;
        }

        private async Task OnHeartbeat(DiscordClient sender, HeartbeatEventArgs e)
        {
            _beats++;
            var x = new DiscordActivity($";help | <Vector Utils>");
            await Client.UpdateStatusAsync(x, UserStatus.Online, DateTimeOffset.MaxValue);
        }

        public struct ConfigJson
        {
            [JsonProperty("token")]
            public string Token { get; private set; }
            [JsonProperty("prefix")]
            public string Prefix { get; private set; }
        }
    }
}
