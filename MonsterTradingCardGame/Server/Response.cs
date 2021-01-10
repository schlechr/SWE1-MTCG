using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using Npgsql;
using MonsterTradingCardGame.User;
using MonsterTradingCardGame.DB;

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

        public const string internalServerErrorCode = "500 Internal Server Error";


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

            string[] verbTokens = request.Resource.Split(new char[] { '/', '?', '&' });
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
                else if (CheckTokens(verbTokens, 1, "battles"))
                    p.HandlePostBattlesMessage();
                else if (CheckTokens(verbTokens, 1, "tradings") &&
                         verbTokens.Length == 1)
                    p.HandlePostTradingsMessage();
                else if (CheckTokens(verbTokens, 1, "tradings") &&
                         verbTokens.Length == 2)
                    p.HandlePostTradingsIdMessage(verbTokens[1]);

                p.CreateFinalResponse(ref strResponseCode, ref strResponse);
            }
            else if (request.Verb == "GET")
            {
                Get g = new Get(request.Content, request.HeaderValues);

                if (CheckTokens(verbTokens, 1, "cards"))
                    g.HandleGetCardsMessage();
                else if (CheckTokens(verbTokens, 1, "deck"))
                    g.HandleGetDeckMessage(CheckTokens(verbTokens, 2, "format=plain"));
                else if (CheckTokens(verbTokens, 1, "users"))
                    g.HandleGetUsersMessage(verbTokens[1]);
                else if (CheckTokens(verbTokens, 1, "stats"))
                    g.HandleGetStatsMessage();
                else if (CheckTokens(verbTokens, 1, "score"))
                    g.HandleGetScoreMessage();
                else if (CheckTokens(verbTokens, 1, "tradings"))
                    g.HandleGetTradingsMessage();

                g.CreateFinalResponse(ref strResponseCode, ref strResponse);
                
            }
            else if(request.Verb == "PUT")
            {
                Put pu = new Put(request.Content, request.HeaderValues);

                if (CheckTokens(verbTokens, 1, "deck"))
                    pu.HandlePutDeckMessage();
                else if (CheckTokens(verbTokens, 1, "users"))
                    pu.HandlePutUsersMessage(verbTokens[1]);

                pu.CreateFinalResponse(ref strResponseCode, ref strResponse);
            }
            else if(request.Verb == "DELETE")
            {
                Delete del = new Delete(request.Content, request.HeaderValues);

                if (CheckTokens(verbTokens, 1, "tradings"))
                    del.HandleDeleteTradingsMessage(verbTokens[1]);

                del.CreateFinalResponse(ref strResponseCode, ref strResponse);
            }

            GC.Collect();
            Console.WriteLine($"Memory after request handled: {GC.GetTotalMemory(false)}");

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
