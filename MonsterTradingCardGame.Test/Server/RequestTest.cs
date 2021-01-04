using System.Collections.Generic;
using MonsterTradingCardGame.Server;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Linq;

namespace SWE_RestServer.Test
{
    public class RequestTest
    {
        public string getMsg;
        public string getSingleMsg;
        public string postMsg;
        public string putMsg;
        public string deleteMsg;

        [SetUp]
        public void Setup()
        {
            getMsg = "GET /messages HTTP/1.1\r\nHost: localhost: 8080\r\nUser - Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";

            getSingleMsg = "GET /messages/1 HTTP/1.1\r\nHost: localhost: 8080\r\nUser - Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";

            postMsg = "POST /messages HTTP/1.1\r\nHost: localhost:10001\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 9\r\n" +
                "Content-Type: application/json\r\n\r\n{\"Username\":\"kienboec\", \"Password\":\"daniel\"} ";

            putMsg = "PUT /messages/1 HTTP/1.1\r\nHost: localhost:8080\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 9\r\n" +
                "Content-Type: application/x-www-form-urlencoded\r\n\r\nUpdate 1";

            deleteMsg = "DELETE /messages/1 HTTP/1.1\r\nHost: localhost: 8080\r\nUser - Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
        }

        [Test] // CreateValuePairTest() needs to be working
        public void ReadHeaderValuesTest()
        {
            string content = "";

            string request = Regex.Replace(postMsg, "\n", " ");
            string[] tokens = request.Split(" ");

            Dictionary<string, string> hv = Request.ReadHeaderValues(tokens, ref content);

            Assert.AreEqual("User-Agent", hv.FirstOrDefault(x => x.Value == "curl/7.55.1\r").Key);
            Assert.AreEqual("application/json\r", hv["Content-Type"]);
        }

        [Test] // ReadHeaderValuesTest() needs to be working
        public void CreateRequestTest()
        {    
            Request req = Request.GetRequest(getMsg);
            Assert.AreEqual("GET", req.Verb);
            Assert.AreEqual("/messages", req.Resource);
            Assert.AreEqual("HTTP/1.1\r", req.Version);

            Assert.AreEqual("Accept", req.HeaderValues.FirstOrDefault(x => x.Value == "*/*\r").Key);

            Assert.AreEqual(" ", req.Content);
        }

        [Test] // ReadHeaderValuesTest() needs to be working
        public void CreateRequestTest2()
        {
            Request req = Request.GetRequest(putMsg);
            Assert.AreEqual("PUT", req.Verb);
            Assert.AreEqual("/messages/1", req.Resource);
            Assert.AreEqual("HTTP/1.1\r", req.Version);

            Assert.AreEqual("Accept", req.HeaderValues.FirstOrDefault(x => x.Value == "*/*\r").Key);

            Assert.AreEqual("Update 1 ", req.Content);
        }
    }
}
