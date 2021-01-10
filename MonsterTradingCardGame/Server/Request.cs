using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MonsterTradingCardGame.Server
{
    public class Request
    {
        public string Verb { get; set; }
        public string Resource { get; set; }
        public string Version { get; set; }
        public Dictionary<string,string> HeaderValues { get; set; }
        public string Content { get; set; }

        private Request(string verb, string res, string version, Dictionary<string,string> headervalues, string content)
        {
            Verb = verb;
            Resource = res;
            Version = version;
            HeaderValues = headervalues;
            Content = content;
        }

        public static Request GetRequest(string request)
        {
            if (string.IsNullOrEmpty(request))
                return null;

            request = Regex.Replace(request, "\n", " ");
            
            string[] tokens = request.Split(" ");
            string verb = tokens[0];
            string res = tokens[1];
            string version = tokens[2];
            string content = "";

            Dictionary<string, string> hv = ReadHeaderValues(tokens, ref content);

            return new Request(verb, res, version, hv, content);
        }

        public static Dictionary<string, string> ReadHeaderValues(string[] tokens, ref string content)
        {
            Dictionary<string, string> rv = new Dictionary<string, string>();
            string key = "";
            string val = "";
            for (int i = 3; i < tokens.Length; i++)
            {
                if (key == "Content")
                    val += tokens[i] + " ";
                else if (tokens[i].EndsWith(":"))
                {
                    if (val != "")
                    {
                        rv.Add(key, val.Remove(val.Length - 1));
                        key = "";
                        val = "";
                    }

                    key = tokens[i].Remove(tokens[i].Length - 1);
                }
                else if (tokens[i] == "\r")
                {
                    rv.Add(key, val.Remove(val.Length - 1));
                    key = "Content";
                    val = "";
                }
                else
                {
                    val += tokens[i] + " ";
                }
            }
            content = val;

            return rv;
        }
    }
}
