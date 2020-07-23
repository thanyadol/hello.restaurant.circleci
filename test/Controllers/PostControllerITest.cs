using System;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Moq;
using Serilog;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

//model
using hello.restaurant.api;
using hello.restaurant.api.Controllers;
using hello.restaurant.api.Models;
using hello.restaurant.api.Services;
using hello.restaurant.api.test.Utilities;
using hello.restaurant.api.Repositories;

namespace hello.restaurant.api.test.Controllers
{
    public class PostControllerITest : BaseHttpTest
    {
        protected Mock<IPostRepository> mockRepository { get; set; }
        protected IEnumerable<Post> Posts => new Post[] {
            new Post { 
                        Id = "1",
                        Author = "Carl-Hugo Marcotte",
                        Title = "Design Patterns",
                        Content = "In this article series, I’d like to go back a little (from my previous Microservices Aggregation article which was more advanced) and        introduce some more basic design patterns. Those patterns help decouple the application flow and extract its responsibilities into separate classes.",
                        Url =  "https://www.forevolve.com/en/articles/2017/08/11/design-patterns-web-api-service-and-repository-part-1/",
                        
                        Published = DateTime.Now.AddDays(1)
                        },
            new Post { 
                        Id = "2",
                        Author = "Clermont-Fd Area, France",
                        Title = "From STUPID to SOLID Code!",
                        Content = "In the following, I will introduce both STUPID and SOLID principles. Keep in mind that these are principles, not laws. However, considering them as laws would be good for those who want to improve themselves.",
                        Url =  "https://williamdurand.fr/2013/07/30/from-stupid-to-solid-code/",
                        
                        Published = DateTime.Now.AddDays(1)
                        },
            new Post { 
                        Id = "3",
                        Author = "Clermont-Fd Area, France",
                        Title = "Dependency injection in ASP.NET Core",
                        Content = "ASP.NET Core supports the dependency injection (DI) software design pattern, which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.For more information specific to dependency injection within MVC controllers, see Dependency injection ",
                        Url =  "https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?ranMID=24542&ranEAID=je6NUbpObpQ&ranSiteID=je6NUbpObpQ-jZSnHSmXYcmtlVZJJ83qow&epi=je6NUbpObpQ-jZSnHSmXYcmtlVZJJ83qow&irgwc=1&OCID=AID681541_aff_7593_1243925&tduid=(ir__niyobvxxk0kfr3rexmlij6lydu2xmxpx9eejh2ca00)(7593)(1243925)(je6NUbpObpQ-jZSnHSmXYcmtlVZJJ83qow)()&irclickid=_niyobvxxk0kfr3rexmlij6lydu2xmxpx9eejh2ca00&view=aspnetcore-2.2",
                        
                        Published = DateTime.Now.AddDays(1)
                        }
        };

        public PostControllerITest()
        {
            mockRepository = new Mock<IPostRepository>();

            //serilog
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        
        }

        public class ListAsync : PostControllerITest
        {
            protected override void ConfigureServices(IServiceCollection services)
            {
                //fixure instance
                //services.TryAddSingleton<IEnumerable<Post>>(Posts);

                    //IoC
                 services
                .AddSingleton(x => mockRepository.Object);
            }

            [Fact]
            public async Task Should_return_the_list_of_posts()
            {
                
                // Arrange
                var expectedNumberOfPosts = Posts.Count();

                mockRepository
                    .Setup(x => x.ListAsync())
                        .ReturnsAsync(Posts);

                // Act
                var result = await Client.GetAsync("api/post/list");

                // Assert
                result.EnsureSuccessStatusCode();
                //var posts = await result.Content.ReadAsJsonObjectAsync<Post[]>();

                var stringResponse = await result.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<Post[]>(stringResponse);

                //Log.Information(stringResponse);

                Assert.NotNull(posts);
                Assert.Equal(expectedNumberOfPosts, posts.Length);
                Assert.Collection(posts,
                    clan => Assert.Equal(Posts.ElementAt(0).Id, posts[0].Id),
                    clan => Assert.Equal(Posts.ElementAt(1).Title, posts[1].Title),
                    clan => Assert.Equal(Posts.ElementAt(2).Url, posts[2].Url)
                );
            }
        }
    }
}