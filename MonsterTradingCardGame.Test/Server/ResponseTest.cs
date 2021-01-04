using MonsterTradingCardGame.Server;
using NUnit.Framework;
using System;

namespace SWE_RestServer.Test
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
            //getMsg = "GET /messages HTTP/1.1\r\nHost: localhost: 8080\r\nUser - Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";

            //getSingleMsg = "GET /messages/1 HTTP/1.1\r\nHost: localhost: 8080\r\nUser - Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";

            postMsg = "POST /messages HTTP/1.1\r\nHost: localhost:8080\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 44\r\n" +
                "Content-Type: application/json\r\n\r\n{\"Username\":\"kienboec\", \"Password\":\"daniel\"} ";

            //putMsg = "PUT /messages/1 HTTP/1.1\r\nHost: localhost:8080\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 9\r\n" +
            //    "Content-Type: application/x-www-form-urlencoded\r\n\r\nUpdate 1";

            //deleteMsg = "DELETE /messages/1 HTTP/1.1\r\nHost: localhost: 8080\r\nUser - Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
        }

        [Test]
        public void StringToByteArrayTest()
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Assert.AreEqual(enc.GetBytes("Test"), Response.StringToByteArray("Test"));
        }

        [Test, Order(1)]
        //[TestCase("Message 1")]
        //[TestCase("Message 2")]
        public void PostRequestTest()//string test)
        {
            postCount += 1;
            Request req = Request.GetRequest(postMsg);// + postCount);
            Response resp = Response.From(req);

            //Assert.AreEqual("Message 1 ", HTTPServer.MESSAGES[0]);
            Assert.AreEqual(Response.badRequestCode, resp.status);

            postCount += 1;
            req = Request.GetRequest(postMsg);// + postCount);
            resp = Response.From(req);

            //Assert.AreEqual("Message 2 ", HTTPServer.MESSAGES[1]);
            Assert.AreEqual(Response.badRequestCode, resp.status);
        }
        /*
        [Test, Order(1)]
        public void GetRequestTest()
        {
            Request req = Request.GetRequest(getMsg);
            Response resp = Response.From(req);

            Assert.AreEqual("Message 1: Message 1 \nMessage 2: Message 2 \n", enc.GetString(resp.data));
            Assert.AreEqual(Response.okCode, resp.status);
        }

        [Test, Order(2)]
        public void GetSingleRequestTest()
        {
            Request req = Request.GetRequest(getSingleMsg);
            Response resp = Response.From(req);

            Assert.AreEqual("Message 1: Message 1 ", enc.GetString(resp.data));
            Assert.AreEqual(Response.okCode, resp.status);
        }

        [Test, Order(3)]
        public void PutRequestTest()
        {
            Request req = Request.GetRequest(putMsg);
            Response resp = Response.From(req);

            Assert.AreEqual("Message 1 updated to: Update 1 ", enc.GetString(resp.data));
            Assert.AreEqual(Response.okCode, resp.status);
        }

        [Test, Order(4)]
        public void DeleteRequestTest()
        {
            Request req = Request.GetRequest(deleteMsg);
            Response resp = Response.From(req);
            postCount -= 1;

            Assert.AreEqual("Message 1 deleted!", enc.GetString(resp.data));
            Assert.AreEqual(Response.okCode, resp.status);
        }

        [Test, Order(5)]
        public void GetSecondSingleRequestTest()
        {
            Request req = Request.GetRequest(getSingleMsg);
            Response resp = Response.From(req);

            Assert.AreEqual("Message 1: Message 2 ", enc.GetString(resp.data));
            Assert.AreEqual(Response.okCode, resp.status);
        }
        
        [Test]
        public void CheckNullRequestTest()
        {
            Response resp = Response.From(null);

            Assert.AreEqual(Response.badRequestCode, resp.status);
        }
        */
    }
}
