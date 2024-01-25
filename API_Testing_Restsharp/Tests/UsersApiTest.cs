﻿using API_Application;
using API_Helper;
using API_Verification.Tests.ResponsePayload.GetUserById;
using API_Verification.Tests.ResponsePayload.GetUserByPage;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net;

namespace API_Verification.Tests
{
    [TestFixture]
    public class UsersApiTest
    {
        private readonly string baseUrl;
        private readonly JObject apiTestData;

        public UsersApiTest()
        {
            var apiTestDataFile = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName!, "Tests\\TestData\\UserDetails.json");
            baseUrl = ReqresApplication.BaseUrl;
            apiTestData = JsonHelper.GetTestData(apiTestDataFile);
        }

        [Test, Category("Users_API_Test"), Order(1)]
        public void VerifyGetUserApiUsingPage1()
        {
            var resource = "api/users";
            var queryParams = ("page", "1");

            var usersPageResponse = ApiSettings
                                   .Get(resource, baseUrl)
                                   .AddQueryParameter(queryParams.Item1, queryParams.Item2)
                                   .Execute<GetUserByPage>();

            Assert.That(usersPageResponse.IsSuccessful, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(usersPageResponse.Data!.page, Is.EqualTo(1));
                Assert.That(usersPageResponse.Data.total, Is.EqualTo(12));
                Assert.That(usersPageResponse.Data.total_pages, Is.EqualTo(2));
                Assert.That(usersPageResponse.Data.per_page, Is.EqualTo(6));
            });
        }

        [Test, Category("Users_API_Test"), Order(2)]
        public void VerifyGetUserApiUsingPage2()
        {
            var resource = "api/users";
            var queryParams = ("page", "2");

            var getUserByPageResponse = ApiSettings
                                   .Get(resource, baseUrl)
                                   .AddQueryParameter(queryParams.Item1, queryParams.Item2)
                                   .Execute<GetUserByPage>();

            Assert.That(getUserByPageResponse.IsSuccessful, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(getUserByPageResponse.Data!.page, Is.EqualTo(2));
                Assert.That(getUserByPageResponse.Data.total, Is.EqualTo(12));
                Assert.That(getUserByPageResponse.Data.total_pages, Is.EqualTo(2));
                Assert.That(getUserByPageResponse.Data.per_page, Is.EqualTo(6));
            });
        }

        [Test, Category("Users_API_Test"), Order(3)]
        public void VerifyGetUserByIdApi()
        {
            var resource = "api/users/{id}";
            var urlSegments = ("id", "2");

            var getUserByIdResponse = ApiSettings
                                   .Get(resource, baseUrl)
                                   .AddUrlSegment(urlSegments.Item1, urlSegments.Item2)
                                   .Execute<GetUserById>();

            Assert.That(getUserByIdResponse.IsSuccessful, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(getUserByIdResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(getUserByIdResponse.Data!.data.id, Is.EqualTo(2));
                Assert.That(getUserByIdResponse.Data.data.email, Is.EqualTo(apiTestData["Id2UserDetails"]!["EmailId"]!.ToString()));
                Assert.That(getUserByIdResponse.Data.data.avatar, Is.EqualTo(apiTestData["Id2UserDetails"]!["AvatarUrl"]!.ToString()));
                Assert.That(getUserByIdResponse.Data.data.first_name, Is.EqualTo(apiTestData["Id2UserDetails"]!["FirstName"]!.ToString()));
                Assert.That(getUserByIdResponse.Data.data.last_name, Is.EqualTo(apiTestData["Id2UserDetails"]!["LastName"]!.ToString()));
            });
        }

        [Test, Category("Users_API_Test"), Order(4)]
        public void VerifyInvalidUserById()
        {
            var resource = "api/users/{id}";
            var urlSegments = ("id", "23");

            var apiResponse = ApiSettings
                                   .Get(resource, baseUrl)
                                   .AddUrlSegment(urlSegments.Item1, urlSegments.Item2)
                                   .Execute();

            Assert.That(apiResponse.IsSuccessful, Is.False);

            Assert.Multiple(() =>
            {
                Assert.That(apiResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(JObject.Parse(apiResponse.Content!.ToString()).HasValues, Is.False);
            });
        }
    }
}