using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Discord.Commands;
using System.Net;
using System.IO;


namespace DiscordBotUtil
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("*default", true)]
        public async Task ChangeDefaultLanguage()
        {
            var msg = Context.Message.ToString();
            string arg;
            if (msg.Split(' ').Length > 1)
            {
                arg = string.Concat(msg.Split(' ')[1]);
                Reg.Add(Reg.RegPath, "DefaultLanguage", arg, Microsoft.Win32.RegistryValueKind.String);
                Reg.TryGet(Reg.RegPath, "DefaultLanguage", out object LanguageDef);
                await ReplyAsync(Formating.Ini("Default language is now [" + LanguageDef + "]."));
            }
            else
            {
                Reg.TryGet(Reg.RegPath, "DefaultLanguage", out object LanguageDef);
                await ReplyAsync(Formating.Ini("Current default language is : [" + LanguageDef + "]."));
            }
                
                
        }
        
        [Command("*alllanguages", true)]
        public async Task AllLanguages() => await All();
        
        [Command("*all", true)]
        public async Task All()
        {
            List<string> ListStr = new List<string>();
            string ListMsgPlainTxt = string.Empty;
            string[] Languages = Formating.DicLanguages.Keys.ToArray();
            
            for(int i = 0; i < Languages.Length; i += 3)
            {
                if(i + 2 >= Languages.Length - 5)
                {
                    if ( i + 1 >= Languages.Length - 5)
                    {
                        if (i >= Languages.Length - 5)
                        {
                        }
                        else
                            ListStr.Add("| [" + Languages[i] + "]" + new string(' ', 15 - Languages[i].Length) + " |");
                    }
                    else
                        ListStr.Add("| [" + Languages[i] + "]" + new string(' ', 15 - Languages[i].Length) + " | [" + Languages[i + 1] + "]" + new string(' ', 15 - Languages[i + 1].Length) + " |");

                }
                else
                    ListStr.Add("| [" + Languages[i] + "]" + new string(' ', 15 - Languages[i].Length) + " | [" + Languages[i+1] + "]" + new string(' ', 15 - Languages[i+1].Length) + " | [" + Languages[i+2] + "]" + new string(' ', 15 - Languages[i+2].Length) + " |");
            }

            await ReplyAsync(Formating.Normal("All languages shortcuts supported by the bot :"));
            await ReplyAsync(Formating.Ini(string.Join("\n", ListStr)));
            await ReplyAsync(Formating.Normal("Following are plaintext with no color (they are all equivalent)"));
            await ReplyAsync(Formating.Ini("| [" + Languages[Languages.Length - 5] + "]" + new string(' ', 15 - Languages[Languages.Length - 5].Length) + " | [" + Languages[Languages.Length - 4] + "]" + new string(' ', 15 - Languages[Languages.Length - 4].Length) + " | [" + Languages[Languages.Length - 3] + "]" + new string(' ', 15 - Languages[Languages.Length - 3].Length) + " |"
                            + "| [" + Languages[Languages.Length - 2] + "]" + new string(' ', 15 - Languages[Languages.Length - 2].Length) + " | [" + Languages[Languages.Length - 1] + "]" + new string(' ', 15 - Languages[Languages.Length - 1].Length) + " |"));
        }
        

        [Command("*on", true)]
        public async Task On()
        {
            //RAJOUTER EN ARGUMENT LE TEMPS 
            Program.IsBotOn= true;
            await ReplyAsync(Formating.Normal("Bot is working !"));
        }

        [Command("*off", true)]
        public async Task Off()
        {
            //RAJOUTER EN ARGUMENT LE TEMPS 
            Program.IsBotOn = false;
            await ReplyAsync(Formating.Normal("Bot is at rest !"));
        }

        [Command("*toggle", true)]
        public async Task Toggle()
        {
            if (Program.IsBotOn) await Off();
            else await On();
        }

        [Command("*help", true)]
        public async Task Help()
        {
            await ReplyAsync(Formating.Normal("Help Menu :"));
            string[] ListStr =
            {
                "[*howto]         | Teaches you how to use this simple bot.",
                "[*default]       | Retrieves the value of the current DEFAULT language",
                "[*default] [arg] | Changes the value of the default language.",
                "[*all]           | Retrieves all the languages supported by the bot.",
                "[*alllanguages]  | Retrieves all the languages supported by the bot.",
                "[*on]            | Turns on the bot.",
                "[*off]           | Turns off the bot.",
                "[*toggle]        | Toggles the state of the bot."

            };
            await ReplyAsync(Formating.Ini(string.Join("\n", ListStr)));
        }
        [Command("*howto", true)]
        public async Task HowTo()
        {
            await ReplyAsync(Formating.Normal("How to use:"));
            string[] ListStr =
            {
                "The bot automaticaly copies your message, deletes it, and pastes it again with better formating.",
                "To chose the formating of your choice, the first word of your message should be an argument :",
                "It can only be one of the languages supported (use command [*all] to see the complete list)",
                "",
                "There's a default language that lets you bypass this argument maneuver.",
                "If you don't enter a language at the beginning, it will take the default language.",
                "By default, it should be plain text, but you can change it with [*default] [arg]",
                "I recommend you setup the default language to the one you use the most",
                "I also recommend to give the bot the rights to read only a SINGLE text channel dedicated to paste code.",
                "",
                "Note that no prefix is required for the paste code to run.",
                "Use [//] or ['] or [\"\"] to comment and the bot will leave your message alone.",
                "The bot also works with 2000+ characters, just paste it as a .txt (discord will do it for you!)",
                "",
                "Please refer to [*help] menu to use commands."

            };
            await ReplyAsync(Formating.Ini(string.Join("\n", ListStr)));
        }
    }
}
