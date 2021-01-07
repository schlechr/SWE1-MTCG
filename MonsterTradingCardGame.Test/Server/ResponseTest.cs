using MonsterTradingCardGame.Server;
using NUnit.Framework;
using System;

namespace MonsterTradingCardGame.Server.Test
{
    public class ResponseTest
    {
        public string getMsg;
        public string getSingleMsg;
        public string postMsg;
        public string putMsg;
        public string deleteMsg;
        public int postCount = 0;
        public System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

        [SetUp]
        public void Setup()
        {
            postMsg = "POST /messages HTTP/1.1\r\nHost: localhost:8080\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 44\r\n" +
                "Content-Type: application/json\r\n\r\n{\"Username\":\"kienboec\", \"Password\":\"daniel\"} ";

        }

        [Test]
        public void StringToByteArrayTest()
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Assert.AreEqual(enc.GetBytes("Test"), Response.StringToByteArray("Test"));
        }

        [Test, Order(1)]
        public void PostRequestTest()//string test)
        {
            postCount += 1;
            Request req = Request.GetRequest(postMsg);// + postCount);
            Response resp = Response.From(req);

            //Assert.AreEqual("Message 1 ", HTTPServer.MESSAGES[0]);
            Assert.AreEqual(Response.notFoundCode, resp.status);

            postCount += 1;
            req = Request.GetRequest(postMsg);// + postCount);
            resp = Response.From(req);

            //Assert.AreEqual("Message 2 ", HTTPServer.MESSAGES[1]);
            Assert.AreEqual(Response.notFoundCode, resp.status);
        }

        [Test]
        public void testCheckTokens_usersAsToken_true()
        {
            string[] tmp_tokens = { "users" };

            bool res = Response.CheckTokens(tmp_tokens, 1, "users");
            Assert.AreEqual(res, true);
        }

        [Test]
        public void testCheckTokens_twoTokens_true()
        {
            string[] tmp_tokens = { "transactions", "packages" };

            bool res1 = Response.CheckTokens(tmp_tokens, 1, "transactions");
            bool res2 = Response.CheckTokens(tmp_tokens, 2, "packages");

            Assert.AreEqual(res1, true);
            Assert.AreEqual(res2, true);
        }
    }
}
