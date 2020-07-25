using System;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using Newtonsoft.Json;

//model
using hello.restaurant.api;
using hello.restaurant.api.Controllers;
using hello.restaurant.api.Models;
using hello.restaurant.api.Services;
using hello.restaurant.api.test.Utilities;
using hello.restaurant.api.Repositories;

namespace hello.restaurant.api.test.Controllers
{
    public class BlogControllerITest : BaseHttpTest
    {

        protected Mock<IBlogRepository> mockRepository { get; set; }

        public BlogControllerITest()
        {       
            mockRepository = new Mock<IBlogRepository>();
            
            //serilog
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        }

        public class ListAsync : BlogControllerITest
        {
            private IEnumerable<Blog> Blogs => new Blog[] {
                new Blog { Id= "1", Name = "http://blogs.msdn.com/dotnet", Title = "Dotnet" },
                new Blog { Id= "2", Name = "http://blogs.msdn.com/webdev", Title = "Webdev" },
                new Blog { Id= "3", Name = "http://blogs.msdn.com/visualstudio", Title = "Vscode" }
            };

            protected override void ConfigureServices(IServiceCollection services)
            {
                //IoC
                 services
                .AddSingleton(x => mockRepository.Object);
            }

            [Fact]
            public async Task Should_return_the_list_blogs()
            {
                // Arrange

                var expectedNumberOfBlogs = Blogs.Count();
                mockRepository
                    .Setup(x => x.ListAsync())
                        .ReturnsAsync(Blogs);

                // Act
                var httpResponse = await Client.GetAsync("api/blog/list");

                // Assert
                httpResponse.EnsureSuccessStatusCode();
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();
                var blogs = JsonConvert.DeserializeObject<Blog[]>(stringResponse);
    
                //Log.Information(stringResponse);

                Assert.NotNull(blogs);
                Assert.Equal(expectedNumberOfBlogs, blogs.Length);
                Assert.Collection(blogs,
                    clan => Assert.Equal(Blogs.ElementAt(0).Title, blogs[0].Title),
                    clan => Assert.Equal(Blogs.ElementAt(1).Title, blogs[1].Title),
                    clan => Assert.Equal(Blogs.ElementAt(2).Title, blogs[2].Title)
                );
            }
        }
        public class CreateAsync : BlogControllerITest
        {
            [Fact]
            public async Task Should_create_the_blogs_return_OkObjectResult_with_blogs()
            {
                // Arrange
                var blogToCreate =   new Blog() { 
                                Id = "b1357cd1-2901-3e8c-9852-1e659bceae98",
                                Title = "Fantastic Beasts: The Crimes of Grindelwald",
                                Name = "JK. Rowling",
                                Description = "Fantastic Beasts: The Crimes of Grindelwald is a 2018 fantasy film directed by David Yates and written by J. K. Rowling. A joint British and American production, it is the sequel to Fantastic Beasts and Where to Find Them (2016). It is the second instalment in the Fantastic Beasts film series, and the tenth overall in the Wizarding World franchise, which began with the Harry Potter film series. The film features an ensemble cast that includes Eddie Redmayne, Katherine Waterston, Dan Fogler, Alison Sudol, Ezra Miller, Zoë Kravitz, Callum Turner, Claudia Kim, William Nadylam, Kevin Guthrie, Jude Law, and Johnny Depp. The plot follows Newt Scamander and Albus Dumbledore as they attempt to take down the dark wizard Gellert Grindelwald, while facing new threats in a more divided wizarding world.",
                                Level = 9, 
                                Created = DateTime.Now
                            };

                var blogBody = JsonConvert.SerializeObject(blogToCreate); //blogToCreate.ToJsonHttpContent();
                var httpContent = new StringContent(blogBody, Encoding.UTF8, "application/json");
                //var mapper = new Mappers.BlogEntityToBlogMapper();
                //BlogEntity createdEntity = null;
                
                mockRepository.Setup(x => x.CreateAsync(blogToCreate))
                    .ReturnsAsync(blogToCreate);

                // Act
                var result = await Client.PostAsync("api/blog/post", httpContent);

                // Assert
                result.EnsureSuccessStatusCode();
                var stringResponse = await result.Content.ReadAsStringAsync();
                var blog = JsonConvert.DeserializeObject<Blog>(stringResponse);

                //Log.Information(stringResponse);

                Assert.NotNull(blog);
               // Assert.NotNull(createdEntity);

                //try to get
                /* result = await Client.GetAsync("api/blog/get/hello");

                result.EnsureSuccessStatusCode();
                stringResponse = await result.Content.ReadAsStringAsync();
                Log.Information(stringResponse); */
            }
}
    }
}