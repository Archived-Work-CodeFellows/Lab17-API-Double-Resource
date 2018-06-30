using System;
using Xunit;
using System.Linq;
using ToDoApi.Data;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;
using ToDoApi.Controllers;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApiTests
{
    public class ToDoListControllerTests
    {
        [Fact]
        public void CanCreateAToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoList list = new ToDoList
                {
                    Name = "test list",
                    IsDone = true
                };

                ToDoListController tdlc = new ToDoListController(context);
                var result = tdlc.Create(list).Result;
                var r = (ObjectResult)result;
                var status = r.StatusCode.Value;

                Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)status);
            }
        }
        [Fact]
        public async void CanReadAToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoList list = new ToDoList
                {
                    ID = 1,
                    Name = "test list",
                    IsDone = true
                };

                ToDoListController tdlc = new ToDoListController(context);
                await tdlc.Create(list);

                var findList = await tdlc.GetById(list.ID);
                var result = (ObjectResult)findList.Result;
                var readList = (ToDoList)result.Value;

                Assert.Equal(list.Name, readList.Name);
            }
        }
        [Fact]
        public async void CanUpdateAToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoList list = new ToDoList
                {
                    ID = 1,
                    Name = "test todo",
                    IsDone = true
                };

                ToDoListController tdlc = new ToDoListController(context);
                await tdlc.Create(list);

                ToDoList update = new ToDoList
                {
                    ID = list.ID,
                    Name = "list test",
                    IsDone = false
                };

                await tdlc.Update(list.ID, update);
                var findItem = await tdlc.GetById(update.ID);
                var result = (ObjectResult)findItem.Result;
                var readItem = (ToDoList)result.Value;


                Assert.Equal(update.Name, readItem.Name);
            }
        }
        [Fact]
        public void CanDeleteAToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoList list = new ToDoList
                {
                    ID = 1,
                    Name = "test list",
                    IsDone = true
                };

                ToDoListController tdlc = new ToDoListController(context);
                var result = tdlc.Create(list).Result;

                var results = context.ToDoLists.Where(i => i.Name == "test list");
                Assert.Equal(1, results.Count());

                var remove = tdlc.Delete(list.ID);
                Assert.True(remove.IsCompletedSuccessfully);
            }
        }

        [Fact]
        public async void CanAddToDoItemstoaToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {
                ToDoList list = new ToDoList
                {
                    ID = 1,
                    Name = "test list",
                    IsDone = true
                };
                ToDoListController tdlc = new ToDoListController(context);
                await tdlc.Create(list);

                ToDoList result = context.ToDoLists.Find(list.ID);
                Assert.Equal("test list", result.Name);

                ToDoItem item = new ToDoItem
                {
                    ID = 1,
                    Name = "test todo",
                    IsDone = true,
                    ListID = 1
                };
                ToDoItem item2 = new ToDoItem
                {
                    ID = 2,
                    Name = "to test do",
                    IsDone = false,
                    ListID = 1
                };

                ToDoController tdc = new ToDoController(context);
                await tdc.Create(item);
                await tdc.Create(item2);

                var viewItems = tdlc.GetById(list.ID).Result;
                var r = (ObjectResult)viewItems.Result;
                ToDoList todoItems = (ToDoList)r.Value;
                Assert.Equal(2, todoItems.ToDoItems.Count());
            }
        }
        [Fact]
        public async void CanRemoveToDoItemsFromAToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {
                ToDoList list = new ToDoList
                {
                    ID = 1,
                    Name = "test list",
                    IsDone = true
                };
                ToDoListController tdlc = new ToDoListController(context);
                await tdlc.Create(list);

                ToDoList result = context.ToDoLists.Find(list.ID);
                Assert.Equal("test list", result.Name);

                ToDoItem item = new ToDoItem
                {
                    ID = 1,
                    Name = "test todo",
                    IsDone = true,
                    ListID = 1
                };
                ToDoItem item2 = new ToDoItem
                {
                    ID = 2,
                    Name = "to test do",
                    IsDone = false,
                    ListID = 1
                };

                ToDoController tdc = new ToDoController(context);
                await tdc.Create(item);
                await tdc.Create(item2);

                await tdc.Delete(item.ID);
                await tdc.Delete(item2.ID);

                var viewItems = tdlc.GetById(list.ID).Result;
                var r = (ObjectResult)viewItems.Result;
                ToDoList todoItems = (ToDoList)r.Value;
                Assert.Empty(todoItems.ToDoItems);
            }
        }
    }
}
