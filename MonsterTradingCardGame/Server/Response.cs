using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using Npgsql;
using MonsterTradingCardGame.User;

namespace MonsterTradingCardGame.Server
{
    public class Response
    {
        public string status { get; }
        public string version { get; }
        public string mime { get; }
        public byte[] data { get; }

        public const string okCode          = "200 OK";
        public const string createCode      = "201 Message created";
        public const string noContentCode   = "204 No Content";

        public const string badRequestCode  = "400 Bad Request";
        public const string unathorizedCode = "401 Unauthorized";
        public const string forbiddenCode   = "403 Forbidden";
        public const string notFoundCode    = "404 Message not found";
        public const string conflictCode    = "409 Conflict";


        private Response(string status, string version, string mime, byte[] data)
        {
            this.status = status;
            this.version = version;
            this.mime = mime;
            this.data = data;
        }

        public static Response From(Request request)
        {
            if (request == null)
                return MakeBadRequest();

            string[] verbTokens = request.Resource.Split("/");
            string strResponse = "ERROR: CRUD Resource not correct";
            string strResponseCode = notFoundCode;

            if (verbTokens.Length < 1 || (verbTokens.Length == 1 && verbTokens[0] == ""))
                return MakeBadRequest();
            else if (verbTokens[0] == "")
                verbTokens = verbTokens.Where(val => val != "").ToArray();


            if (request.Verb == "POST")
            {
                Post p = new Post(request.Content, request.HeaderValues);

                if (CheckTokens(verbTokens, 1, "users")) //verbTokens.Length == 1 && verbTokens[0] == "users"
                    p.HandlePostUsersMessage();
                else if (CheckTokens(verbTokens, 1, "sessions"))
                    p.HandlePostSessionsMessage();
                else if (CheckTokens(verbTokens, 1, "packages"))
                    p.HandlePostPackagesMessage();
                else if (CheckTokens(verbTokens, 1, "transactions") &&
                         CheckTokens(verbTokens, 2, "packages"))
                    p.HandlePostTransactionsPackagesMessage();

                if( p.RespCode != "")
                {
                    strResponseCode = p.RespCode;
                    strResponse = p.Resp;
                }
            }
            else if (request.Verb == "GET")
            {
                Get g = new Get(request.Content, request.HeaderValues);

                if (CheckTokens(verbTokens, 1, "cards"))
                    g.HandleGetCardsMessage();
            }

            return new Response(strResponseCode, request.Version, "text/plain", StringToByteArray(strResponse));
        }

        public static bool CheckTokens(string[] tokens, int len, string key)
        {
            if (tokens.Length >= len && tokens[len-1] == key)
                return true;
            return false;
        }

        private static Response MakeBadRequest()
        {
            return new Response("400 Bad Request", "", "text/plain", new byte[0]);
        }

        public static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str); // curl request shows "(52) Empty reply from server" // only when string has < 6 chars
        }

        public void Post(NetworkStream stream)
        {
            /*StreamWriter writer = new StreamWriter(stream);
            
            writer.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n",
                version, status, HTTPServer.NAME, mime, data.Length));*/
            stream.Write(data, 0, data.Length);
        }

    }
}
