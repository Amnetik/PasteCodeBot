using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace DiscordBotUtil
{
    public static class Formating
    {
        //To add a language : 
        //on the left(KEY), add the string the user will have to type.
        //on the right(VALUE), add the string that corresponds to a DISCORD supported language (or shortcut admited by Discord).
        public static Dictionary<string, string> DicLanguages = new Dictionary<string, string>
        {
            {"apache"      , "apache"   },
            {"arm"         , "arm"      },
            {"assembly"    , "arm"      },
            {"bash"        , "bash"     },
            {"c"           , "c"        },
            {"coffee"      , "coffee"   },
            {"coffeescript", "coffee"   },
            {"cpp"         , "c++"      },
            {"cs"          , "cs"       },
            {"csharp"      , "cs"       },
            {"c#"          , "cs"       },
            {"css"         , "css"      },
            {"d"           , "d"        },
            {"diff"        , "diff"     },
            {"fix"         , "fix"      },
            {"fs"          , "fs"       },
            {"f#"          , "fs"       },
            {"go"          , "go"       },
            {"http"        , "http"     },
            {"html"        , "html"     },
            {"htmlbars"    , "htmlbars" },
            {"ini"         , "ini"      },
            {"java"        , "java"     },
            {"js"          , "js"       },
            {"javascript"  , "js"       },
            {"json"        , "json"     },
            {"kotlin"      , "kotlib"   },
            {"kl"          , "kotlib"   },
            {"less"        , "less"     },
            {"lua"         , "lua"      },
            {"makefile"    , "makefile" },
            {"mk"          , "makefile" },
            {"markdown"    , "markdown" },
            {"md      "    , "markdown" },
            {"matlab"      , "matlab"   },
            {"nginx"       , "nginx"    },
            {"objectivec"  , "objc"     },
            {"objc"        , "objc"     },
            {"perl"        , "perl"     },
            {"php"         , "php"      },
            {"python"      , "python"   },
            {"py"          , "python"   },
            {"rb"          , "ruby"     },
            {"ruby"        , "ruby"     },
            {"rust"        , "rust"     },
            {"scss"        , "scss"     },
            {"shellsession", "shell"    },
            {"shell"       , "shell"    },
            {"sql"         , "sql"      },
            {"swift"       , "swift"    },
            {"toml"        , "toml"     },
            {"typescript"  , "ts"       },
            {"ts"          , "ts"       },
            {"xml"         , "xml"      },
            {"yaml"        , "yaml"     },
            {"plaintext"   , ""         },
            {"txt"         , ""         },
            {"text"        , ""         },
            {"default"     , ""         },
            {"vanilla"     , ""         }
        };
        public static string Format(string message, string Method) => $"```{Method}\n{message}\n```";
        public static string Ini(string message) => $"```ini\n{message}\n```";
        public static string Normal(string message) => $"```\n{message}\n```";
        public static string[] StringTooLong(string str, int chunkSize = 2000)
        {
            var strs = new List<string>();
            while (str.Length > chunkSize)
            {
                var index = 0;
                while (str[chunkSize - index] != '\n')
                    index++;
                strs.Add(str.Substring(0, chunkSize - index));
                str = str.Remove(0, chunkSize - index);
            }
            strs.Add(str);
            return strs.ToArray();
        }
        public static string[] PasteCode(string strCode, string Language)
        {
            string strMethod = DicLanguages[Language];
            string[] strCodeSplit = StringTooLong(strCode, 1900);
            for(int i = 0; i < strCodeSplit.Length; i++)
                strCodeSplit[i] = Format(strCodeSplit[i], strMethod);
            return strCodeSplit;
        }
    }
    class Reg
    {
        public static string RegPath = "SOFTWARE\\PasteCodeBot";
        public static void Add(string path, string name, object value, RegistryValueKind type)
        {
            var key = Registry.CurrentUser.CreateSubKey(path);
            key.SetValue(name, value, type);
        }

        public static bool TryGet(string path, string name, out object value)
        {
            value = 0;

            var key = Registry.CurrentUser.CreateSubKey(path, true);

            if (key == null)
                return false;

            var tmp_value = key.GetValue(name);

            if (tmp_value == null || (string)tmp_value == string.Empty)
                return false;

            value = tmp_value;

            return true;
        }
    }
}
