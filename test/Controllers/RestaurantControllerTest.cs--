using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using System;
using Xunit;
using Moq;

//model
using hello.restaurant.api;
using hello.restaurant.api.Controllers;
using hello.restaurant.api.Models;
using hello.restaurant.api.Services;

namespace hello.restaurant.api.test.Controllers
{
    public class BlogControllerTest
    {
        protected BlogController ControllerUnderTest { get; set; }

        protected Mock<IBlogService> mockService { get; set; }

        public BlogControllerTest()
        {
            mockService = new Mock<IBlogService>();
            ControllerUnderTest = new BlogController(mockService.Object);

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        }

        private IEnumerable<Blog> GetBlogTestSessions()
        {
            throw new NotImplementedException();
        }

        public class ListAsync : BlogControllerTest
        {
            
            [Fact]
            public async void Should_return_OkObjectResult_with_blogs()
            {
                // Arrange
                //should serialize from JSON
                var expectedBlog = new Blog[]
                {
                    new Blog() { 
                                Id = "b1357cd1-2901-3e8c-9852-1e659bceae98",
                                Title = "Fantastic Beasts: The Crimes of Grindelwald",
                                Name = "JK. Rowling",
                                Description = "Fantastic Beasts: The Crimes of Grindelwald is a 2018 fantasy film directed by David Yates and written by J. K. Rowling. A joint British and American production, it is the sequel to Fantastic Beasts and Where to Find Them (2016). It is the second instalment in the Fantastic Beasts film series, and the tenth overall in the Wizarding World franchise, which began with the Harry Potter film series. The film features an ensemble cast that includes Eddie Redmayne, Katherine Waterston, Dan Fogler, Alison Sudol, Ezra Miller, Zoë Kravitz, Callum Turner, Claudia Kim, William Nadylam, Kevin Guthrie, Jude Law, and Johnny Depp. The plot follows Newt Scamander and Albus Dumbledore as they attempt to take down the dark wizard Gellert Grindelwald, while facing new threats in a more divided wizarding world.",
                                Level = 9, 
                                Created = DateTime.Now
                            },

                    new Blog() {  
                                Id = "a1357cd1-2901-3e8c-9852-1e659bceae51",
                                Title = "Design Patterns: Asp.Net Core Web API",
                                Name = "Microsoft",
                                Description = "In this article series, I’d like to go back a little (from my previous Microservices Aggregation article which was more advanced) and introduce some more basic design patterns. Those patterns help decouple the application flow and extract its responsibilities into separate classes.",
                                Created = DateTime.Now,
                                Level = 5
                            }
                };

                //setup DI
                mockService.Setup(repo => repo.ListAsync()).ReturnsAsync(expectedBlog);

                // Act
                var result = await ControllerUnderTest.ListAsync();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var model = Assert.IsType<Blog[]>(okResult.Value);

                //Log.Information(okResult.Value.GetType().ToString());
                
                Assert.Same(expectedBlog, model);
            }
        }

        public class GetAsync : BlogControllerTest
        {
            [Fact]
            public async Task Should_return_the_OkObjectResult_blog()
            {
                // Arrange
                var blogId = "b1357cd1-2901-3e8c-9852-1e659bceae98";
                var expectedBlog = new Blog() { 
                                Id = "b1357cd1-2901-3e8c-9852-1e659bceae98",
                                Title = "Fantastic Beasts: The Crimes of Grindelwald",
                                Name = "JK. Rowling",
                                Description = "Fantastic Beasts: The Crimes of Grindelwald is a 2018 fantasy film directed by David Yates and written by J. K. Rowling. A joint British and American production, it is the sequel to Fantastic Beasts and Where to Find Them (2016). It is the second instalment in the Fantastic Beasts film series, and the tenth overall in the Wizarding World franchise, which began with the Harry Potter film series. The film features an ensemble cast that includes Eddie Redmayne, Katherine Waterston, Dan Fogler, Alison Sudol, Ezra Miller, Zoë Kravitz, Callum Turner, Claudia Kim, William Nadylam, Kevin Guthrie, Jude Law, and Johnny Depp. The plot follows Newt Scamander and Albus Dumbledore as they attempt to take down the dark wizard Gellert Grindelwald, while facing new threats in a more divided wizarding world.",
                                Level = 9, 
                                Created = DateTime.Now
                            };
                
                //setup DI
                mockService.Setup(repo => repo.GetAsync(blogId)).ReturnsAsync(expectedBlog);

                // Act
                var result = await ControllerUnderTest.GetAsync(blogId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var blog = Assert.IsType<Blog>(okResult.Value);

                Assert.Same(expectedBlog, blog);
            }

            [Fact]
            public async Task Should_return_NotFoundObjectResult_if_the_blog_does_not_exist()
            {
                // Arrange
                var blogId = "b1357cd1-2901-3e8c-9852-1e659bceae98";

                //setup DI
                //mockService.Setup(repo => repo.GetAsync(blogId)).ReturnsAsync(new Blog());

                // Act
                var result = await ControllerUnderTest.GetAsync(blogId);

                // Assert
                var notfoundResult = Assert.IsType<NotFoundResult>(result);
                // ar blog = Assert.IsType<null>(notfoundResult.Value);

                //Log.Information(notfoundResult.Value);
                //Assert.Null(notfoundResult.Value);
                //mockService.Verify();
            }
        }
    }
}