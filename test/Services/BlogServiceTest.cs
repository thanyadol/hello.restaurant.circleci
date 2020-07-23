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
using hello.restaurant.api.Repositories;
using System.Collections.ObjectModel;
using hello.restaurant.api.Exceptions;

namespace hello.restaurant.api.test.Services
{
    public class BlogServiceTest
    {
        protected Mock<IBlogRepository> mockRepo { get; set; }

        protected BlogService ServiceUnderTest { get; }

        public BlogServiceTest()
        {
            mockRepo = new Mock<IBlogRepository>();
            ServiceUnderTest = new BlogService(mockRepo.Object);
        }

        public class ListAsync : BlogServiceTest
        {
            [Fact]
            public async Task Should_return_all_blogs()
            {
                // Arrange
                var expectedBlogs = new ReadOnlyCollection<Blog>(new List<Blog>
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
                });


                mockRepo
                    .Setup(x => x.ListAsync())
                    .ReturnsAsync(expectedBlogs);

                // Act
                var result = await ServiceUnderTest.ListAsync();

                // Assert
                Assert.Same(expectedBlogs, result);
            }
        }

        public class GetAsync : BlogServiceTest
        {
            [Fact]
            public async Task Should_return_the_expected_blog()
            {
                // Arrange
                var blogKey = "a1357cd1-2901-3e8c-9852-1e659bceae51";
                var expectedBlog = new Blog {  
                                Id = blogKey,
                                Title = "Design Patterns: Asp.Net Core Web API",
                                Name = "Microsoft",
                                Description = "In this article series, I’d like to go back a little (from my previous Microservices Aggregation article which was more advanced) and introduce some more basic design patterns. Those patterns help decouple the application flow and extract its responsibilities into separate classes.",
                                Created = DateTime.Now,
                                Level = 5
                            };
                mockRepo
                    .Setup(x => x.GetAsync(blogKey))
                    .ReturnsAsync(expectedBlog);

                // Act
                var result = await ServiceUnderTest.GetAsync(blogKey);

                // Assert
                Assert.Same(expectedBlog, result);
            }

            [Fact]
            public async Task Should_return_null_if_the_blog_does_not_exist()
            {
                // Arrange
                var blogKey = "mockKey";
                mockRepo
                    .Setup(x => x.GetAsync(blogKey))
                    .ReturnsAsync(default(Blog));

                // Act
                Blog result =  null;
                try
                {
                    result = await ServiceUnderTest.GetAsync(blogKey);
                }
                catch (BlogNotFoundException) { }

                // Assert
                Assert.Null(result);
            }
        }

        /*  
        public class IsBlogExistsAsync : BlogServiceTest
        {
            [Fact]
            public async Task Should_return_true_if_the_blog_exist()
            {
                // Arrange
                var blogName = "Your Blog";
                mockRepo
                    .Setup(x => x.ReadOneAsync(blogName))
                    .ReturnsAsync(new Blog());

                // Act
                var result = await ServiceUnderTest.IsBlogExistsAsync(blogName);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public async Task Should_return_false_if_the_blog_does_not_exist()
            {
                // Arrange
                var blogName = "Unexisting Blog";
                mockRepo
                    .Setup(x => x.ReadOneAsync(blogName))
                    .ReturnsAsync(default(Blog));

                // Act
                var result = await ServiceUnderTest.IsBlogExistsAsync(blogName);

                // Assert
                Assert.False(result);
            }
        }

        public class CreateAsync : BlogServiceTest
        {
            [Fact]
            public async Task Should_throw_a_NotSupportedException()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.CreateAsync(null));
            }
        }

        public class UpdateAsync : BlogServiceTest
        {
            [Fact]
            public async Task Should_throw_a_NotSupportedException()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.UpdateAsync(null));
            }
        }

        public class DeleteAsync : BlogServiceTest
        {
            [Fact]
            public async Task Should_throw_a_NotSupportedException()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.DeleteAsync(null));
            }
        } */
        
    }
}