using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Drawing;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

/* Originally made by Yann ULRICH, Amnetik.
 * ----------- PasteCodeBot --------------
 * -------- 21 - March - 2020 ------------
 *     Feel free to use this code.
 */

namespace DiscordBotUtil
{
    static class Program
    {
        public static long timeAtWhichbotIsOn = DateTime.Now.Ticks;
        public static bool IsBotOn = true;
        
        static void Main(string[] args)
        {
            if (!Reg.TryGet(Reg.RegPath, "DefaultLanguage", out object LanguageDef))
            {
                Reg.Add(Reg.RegPath, "DefaultLanguage", "txt", Microsoft.Win32.RegistryValueKind.String);
            }
            var bot = new bot();
            bot.run_bot().GetAwaiter().GetResult();
        }
    }
    class bot
    {
        private DiscordSocketClient client;
        private CommandService command;
        private IServiceProvider service;

        public async Task run_bot()
        {
            client = new DiscordSocketClient();
            command = new CommandService();
            service = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(command)
                .BuildServiceProvider();

            var token = "";

            client.Log += log;

            await get_commands();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private Task log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task get_commands()
        {
            client.MessageReceived += run_cmd;
            await command.AddModulesAsync(Assembly.GetEntryAssembly(), service);
        }

        private async Task run_cmd(SocketMessage arg)
        {
            var message = (SocketUserMessage)arg;
            var Context = new SocketCommandContext(client, message);
            if (message.Author.IsBot)
                return;

            var arg_pos = 0;
            var result = await command.ExecuteAsync(Context, arg_pos, service);
            await FormatLanguage(Context);
            if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
        }
        public static async Task FormatLanguage(SocketCommandContext Context)
        {
            if (Program.IsBotOn)
            {
                string strCode;
                string Language;
                bool IsDoc = false;
                if (Context.Message.Attachments.Count() == 1)
                {
                    new WebClient().DownloadFile(Context.Message.Attachments.ElementAt(0).Url, "Code.txt");
                    strCode = File.ReadAllText("Code.txt");
                    Language = Context.Message.ToString().ToLower().Split(' ')[0];
                    IsDoc = true;
                }
                else
                {
                    strCode = Context.Message.ToString();
                    Language = Context.Message.ToString().ToLower().Split(' ')[0];
                }

                if (strCode.StartsWith("//")) return;
                if (strCode.StartsWith("\"\"")) return;
                if (strCode.StartsWith("'")) return;
                if (strCode.StartsWith("*")) return;

                if (!Formating.DicLanguages.Keys.Any(x => x == Language))
                {
                    Reg.TryGet("SOFTWARE\\PasteCodeBot", "DefaultLanguage", out object LanguageDef);
                    Language = LanguageDef.ToString();
                }
                else
                {
                    var Index = strCode.IndexOf(' ');
                    strCode = strCode.Remove(0, Index);
                }

                var ListMsg = Formating.PasteCode(strCode, Language);
                await Context.Channel.SendMessageAsync("From " + Context.Guild.GetUser(Context.Message.Author.Id).Nickname + " in " + Language);
                foreach (string msg in ListMsg)
                    await Context.Channel.SendMessageAsync(msg);
                if (!IsDoc)
                    await Context.Message.DeleteAsync();
            }
        }
    }
}
