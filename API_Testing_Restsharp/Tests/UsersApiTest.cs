using API_Application;
using API_Helper;
using API_Verification.Tests.RequestPayload;
using API_Verification.Tests.ResponsePayload;
using API_Verification.Tests.ResponsePayload.GetUserById;
using API_Verification.Tests.ResponsePayload.GetUserByPage;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net;
using System.Text.Json;

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
            var restApiClient = new RestApiClient(baseUrl);

            // Act
            var usersPageResponse = restApiClient
                                   .AddQueryParameter(queryParams.Item1, queryParams.Item2)
                                   .Get<GetUserByPage>(resource);

            // Assert
            Assert.That(usersPageResponse.IsSuccessful, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(usersPageResponse.Data!.page, Is.EqualTo(1));
                Assert.That(usersPageResponse.Data.total, Is.EqualTo(12));
                Assert.That(usersPageResponse.Data.total_pages, Is.EqualTo(2));
                Assert.That(usersPageResponse.Data.per_page, Is.EqualTo(6));
            });
            restApiClient.DisposeClient();
        }

        [Test, Category("Users_API_Test"), Order(2)]
        public void VerifyGetUserApiUsingPage2()
        {
            // Arrange
            var resource = "api/users";
            var queryParams = ("page", "2");
            var restApiClient = new RestApiClient(baseUrl);

            // Act
            var usersPageResponse = restApiClient
                                   .AddQueryParameter(queryParams.Item1, queryParams.Item2)
                                   .Get<GetUserByPage>(resource);

            // Assert
            Assert.That(usersPageResponse.IsSuccessful, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(usersPageResponse.Data!.page, Is.EqualTo(2));
                Assert.That(usersPageResponse.Data.total, Is.EqualTo(12));
                Assert.That(usersPageResponse.Data.total_pages, Is.EqualTo(2));
                Assert.That(usersPageResponse.Data.per_page, Is.EqualTo(6));
            });
            restApiClient.DisposeClient();
        }

        [Test, Category("Users_API_Test"), Order(3)]
        public void VerifyGetUserByIdApi()
        {
            // Arrange
            var resource = "api/users/{id}";
            var urlSegments = ("id", "2");
            var restApiClient = new RestApiClient(baseUrl);

            // Act
            var apiResponse = restApiClient
                             .AddUrlSegment(urlSegments.Item1, urlSegments.Item2)
                             .Get<GetUserById>(resource);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(apiResponse.IsSuccessful, Is.True);
                Assert.That(apiResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(apiResponse.Data!.data.id, Is.EqualTo(2));
                Assert.That(apiResponse.Data.data.email, Is.EqualTo(apiTestData["Id2UserDetails"]!["EmailId"]!.ToString()));
                Assert.That(apiResponse.Data.data.avatar, Is.EqualTo(apiTestData["Id2UserDetails"]!["AvatarUrl"]!.ToString()));
                Assert.That(apiResponse.Data.data.first_name, Is.EqualTo(apiTestData["Id2UserDetails"]!["FirstName"]!.ToString()));
                Assert.That(apiResponse.Data.data.last_name, Is.EqualTo(apiTestData["Id2UserDetails"]!["LastName"]!.ToString()));
            });
            restApiClient.DisposeClient();
        }

        [Test, Category("Users_API_Test"), Order(4)]
        public void VerifyInvalidUserById()
        {
            // Arrange
            var resource = "api/users/{id}";
            var urlSegments = ("id", "23");
            var restApiClient = new RestApiClient(baseUrl);

            // Act
            var apiResponse = restApiClient
                             .AddUrlSegment(urlSegments.Item1, urlSegments.Item2)
                             .Get<GetUserById>(resource);

            // Assert
            Assert.That(apiResponse.IsSuccessful, Is.False);
            Assert.Multiple(() =>
            {
                Assert.That(apiResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(JObject.Parse(apiResponse.Content!.ToString()).HasValues, Is.False);
            });
            restApiClient.DisposeClient();
        }

        [Test, Category("Users_API_Test"), Order(5)]
        public void VerifyCreateUserApi()
        {
            // Arrange
            var resource = "api/users";
            var userDetails = new CreatUser()
            {
                name = apiTestData["CreateUserApiDetails"]!["name"]!.ToString(),
                job = apiTestData["CreateUserApiDetails"]!["job"]!.ToString(),
            };
            var jsonPayload = JsonSerializer.Serialize(userDetails);
            var restApiClient = new RestApiClient(baseUrl);

            // Act
            var apiResponse = restApiClient
                             .Post<UserCreated>(resource, jsonPayload);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(apiResponse.IsSuccessful, Is.True);
                Assert.That(apiResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(apiResponse.Data!.job, Is.EqualTo(apiTestData["CreateUserApiDetails"]!["job"]!.ToString()));
                Assert.That(apiResponse.Data!.name, Is.EqualTo(apiTestData["CreateUserApiDetails"]!["name"]!.ToString()));
            });
            restApiClient.DisposeClient();
        }
    }
}