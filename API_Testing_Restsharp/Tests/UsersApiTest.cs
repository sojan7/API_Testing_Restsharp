using API_Application;
using API_Helper;
using API_Verification.Tests.ResponsePayload;
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
            // Arrange
            var resource = "api/users";
            var queryParams = ("page", "1");
            using var restApiClient = new RestApiClient(baseUrl);

            // Act
            var usersPageResponse = restApiClient
                                   .WithQueryParameter(queryParams.Item1, queryParams.Item2)
                                   .Get<GetUserByPage>(resource, HttpStatusCode.OK);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(usersPageResponse!.page, Is.EqualTo(1));
                Assert.That(usersPageResponse.total, Is.EqualTo(12));
                Assert.That(usersPageResponse.total_pages, Is.EqualTo(2));
                Assert.That(usersPageResponse.per_page, Is.EqualTo(6));
            });
        }

        [Test, Category("Users_API_Test"), Order(2)]
        public void VerifyGetUserApiUsingPage2()
        {
            // Arrange
            var resource = "api/users";
            var queryParams = ("page", "2");
            using var restApiClient = new RestApiClient(baseUrl);

            // Act
            var usersPageResponse = restApiClient
                                   .WithQueryParameter(queryParams.Item1, queryParams.Item2)
                                   .Get<GetUserByPage>(resource, HttpStatusCode.OK);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(usersPageResponse!.page, Is.EqualTo(2));
                Assert.That(usersPageResponse.total, Is.EqualTo(12));
                Assert.That(usersPageResponse.total_pages, Is.EqualTo(2));
                Assert.That(usersPageResponse.per_page, Is.EqualTo(6));
            });
        }

        [Test, Category("Users_API_Test"), Order(3)]
        public void VerifyGetUserByIdApi()
        {
            // Arrange
            var resource = "api/users/{id}";
            var urlSegments = ("id", "2");
            using var restApiClient = new RestApiClient(baseUrl);

            // Act
            var apiResponse = restApiClient
                             .WithUrlSegment(urlSegments.Item1, urlSegments.Item2)
                             .Get<GetUserById>(resource, HttpStatusCode.OK);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(apiResponse!.data.id, Is.EqualTo(2));
                Assert.That(apiResponse.data.email, Is.EqualTo(apiTestData["Id2UserDetails"]!["EmailId"]!.ToString()));
                Assert.That(apiResponse.data.avatar, Is.EqualTo(apiTestData["Id2UserDetails"]!["AvatarUrl"]!.ToString()));
                Assert.That(apiResponse.data.first_name, Is.EqualTo(apiTestData["Id2UserDetails"]!["FirstName"]!.ToString()));
                Assert.That(apiResponse.data.last_name, Is.EqualTo(apiTestData["Id2UserDetails"]!["LastName"]!.ToString()));
            });
        }

        [Test, Category("Users_API_Test"), Order(4)]
        public void VerifyInvalidUserById()
        {
            // Arrange
            var resource = "api/users/{id}";
            var urlSegments = ("id", "23");
            using var restApiClient = new RestApiClient(baseUrl);

            // Act
            var apiResponse = restApiClient
                             .WithUrlSegment(urlSegments.Item1, urlSegments.Item2)
                             .Get<GetUserById>(resource, HttpStatusCode.NotFound);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(apiResponse.data, Is.EqualTo(null));
                Assert.That(apiResponse.support, Is.EqualTo(null));
            });
        }

        [Test, Category("Users_API_Test"), Order(5)]
        public void VerifyCreateUserApi()
        {
            // Arrange
            var resource = "api/users";
            var jsonPayload = ConstructPayload.ConstructCreateUserPayload(apiTestData);
            using var restApiClient = new RestApiClient(baseUrl);

            // Act
            var apiResponse = restApiClient
                             .Post<UserCreated>(resource, HttpStatusCode.Created, jsonPayload);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(apiResponse!.job, Is.EqualTo(apiTestData["CreateUserApiDetails"]!["job"]!.ToString()));
                Assert.That(apiResponse!.name, Is.EqualTo(apiTestData["CreateUserApiDetails"]!["name"]!.ToString()));
            });
        }
    }
}