using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Net;
using System.Web;
using RestSharpDemo.Model;

namespace RestSharpDemo
{
    [TestFixture]
    public class UnitTest1
    {
        //Test 1, write the Json output in console
        [Test]
        public void GetFirstPost()
        {
            var client = new RestClient("http://localhost:3000/");
            var request = new RestRequest("posts/{postid}", Method.GET);
            request.AddUrlSegment("postid",1);
            if (client.Execute(request).StatusCode.ToString()=="OK")
            {
                Console.Write(client.Execute(request).Content);
            }
        }
        //Test 2, use serialization for data to get in good format like key value
        [Test]
        public void GetFirstPostInKeyValue()
        {
            var client = new RestClient("http://localhost:3000/");
            var request = new RestRequest("posts/{postid}", Method.GET);
            request.AddUrlSegment("postid", 1);
            var response = client.Execute(request);
            var deserialize = new JsonDeserializer();
            var outPut = deserialize.Deserialize<Dictionary<string, string>>(response);
            var authorName = outPut["author"];
            Assert.That(authorName, Is.EqualTo("Ashutosh1"), "Author  name is not correct");
        }
        //Use of JObject to parse the Json data and validate certain values
        [Test]
        public void UseNewtonSoftToGetData()
        {
            var client = new RestClient("http://localhost:3000/");
            var request = new RestRequest("posts/{postid}", Method.GET);
            request.AddUrlSegment("postid", 1);
            var response = client.Execute(request);            
            JObject obs = JObject.Parse(response.Content);
            Console.Write(obs);//Just see what is printed
            Assert.That(obs["author"].ToString(), Is.EqualTo("Ashutosh1"), "Author  name is not correct");
        }
        //Post method by ananymous body class
        [Test]
        public void PostingDataByAnonymousBody()
        {
            var client = new RestClient("http://localhost:3000/");
            var request = new RestRequest("posts/{postid}/profile", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { name = "Ravi"});
            request.AddUrlSegment("postid", 1);
            var response = client.Execute(request);
            JObject obs = JObject.Parse(response.Content);
            Assert.That(obs["name"].ToString(), Is.EqualTo("Ravi"), "Name is not correct");
        }
        //Post method by Type body class
        [Test]
        public void PostingDataByTypeBody()
        {
            var client = new RestClient("http://localhost:3000/");
            var request = new RestRequest("posts", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new Posts() { id =4, title="NewTitle4", author="Ashutosh4"});
            var response = client.Execute(request);
            JObject obs = JObject.Parse(response.Content);
            Assert.That(obs["author"].ToString(), Is.EqualTo("Ashutosh4"),"Name is not correct");
        }
        [Test]
        public void UsingGenericDeserializer()
        {
            var client = new RestClient("http://localhost:3000/");
            var request = new RestRequest("posts", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new Posts() { id = 5, title = "NewTitle5", author = "Ashutosh5" });
            var response = client.Execute<Posts>(request);
            Assert.That(response.Data.author, Is.EqualTo("Ashutosh5"), "Name is not correct");
        }

    }

}
