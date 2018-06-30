using System;
using Xunit;
using System.Linq;
using ToDoApi;
using ToDoApi.Data;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApiTests
{
    public class APItests
    {
        [Fact]
        public async void CanCreateAToDoItem()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoItem item = new ToDoItem();
                item.Name = "test todo";
                item.IsDone = true;

                await context.ToDoItems.AddAsync(item);
                await context.SaveChangesAsync();

                var results = context.ToDoItems.Where(i => i.Name == "test todo");

                Assert.Equal(1, results.Count());
            }
        }
        [Fact]
        public async void CanReadAToDoItem()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoItem item = new ToDoItem();
                item.Name = "test todo";
                item.IsDone = true;

                await context.ToDoItems.AddAsync(item);
                await context.SaveChangesAsync();

                ToDoItem results = context.ToDoItems.Find(item.ID);

                Assert.True(results.IsDone);
            }
        }
        [Fact]
        public async void CanUpdateAToDoItem()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoItem item = new ToDoItem();
                item.Name = "test todo";
                item.IsDone = true;

                await context.ToDoItems.AddAsync(item);
                await context.SaveChangesAsync();

                ToDoItem todo = await context.ToDoItems.FindAsync(item.ID);
                ToDoItem update = new ToDoItem();
                update.ID = todo.ID;
                update.Name = "todo test";
                update.IsDone = false;
                update.ListID = todo.ListID;

                context.Entry(todo).State = EntityState.Detached;
                todo = update;
                context.ToDoItems.Update(todo);
                await context.SaveChangesAsync();

                ToDoItem results = await context.ToDoItems.FindAsync(todo.ID);

                Assert.Equal(update.Name, results.Name);
            }
        }
        [Fact]
        public async void CanDeleteAToDoItem()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoItem item = new ToDoItem();
                item.Name = "test todo";
                item.IsDone = true;

                await context.ToDoItems.AddAsync(item);
                await context.SaveChangesAsync();

                var results = context.ToDoItems.Where(i => i.Name == "test todo");
                Assert.Equal(1, results.Count());

                ToDoItem removed = context.ToDoItems.Find(item.ID);
                context.ToDoItems.Remove(removed);
                await context.SaveChangesAsync();

                var check = context.ToDoItems.Where(i => i.Name == "test todo");
                Assert.Equal(0, check.Count());

            }
        }

        [Fact]
        public async void CanCreateAToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoList list = new ToDoList();
                list.Name = "test list";
                list.IsDone = true;

                await context.ToDoLists.AddAsync(list);
                await context.SaveChangesAsync();

                var results = context.ToDoLists.Where(i => i.Name == "test list");

                Assert.Equal(1, results.Count());
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

                ToDoList list = new ToDoList();
                list.Name = "test list";
                list.IsDone = true;

                await context.ToDoLists.AddAsync(list);
                await context.SaveChangesAsync();

                ToDoList results = context.ToDoLists.Find(list.ID);

                Assert.True(results.IsDone);
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

                ToDoList list = new ToDoList();
                list.Name = "test todo";
                list.IsDone = true;

                await context.ToDoLists.AddAsync(list);
                await context.SaveChangesAsync();

                ToDoList todoList = await context.ToDoLists.FindAsync(list.ID);
                ToDoList update = new ToDoList();
                update.ID = todoList.ID;
                update.Name = "list test";
                update.IsDone = false;

                context.Entry(todoList).State = EntityState.Detached;
                todoList = update;
                context.ToDoLists.Update(todoList);
                await context.SaveChangesAsync();

                ToDoList results = await context.ToDoLists.FindAsync(todoList.ID);

                Assert.Equal(update.Name, results.Name);
            }
        }
        [Fact]
        public async void CanDeleteAToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {

                ToDoList list = new ToDoList();
                list.Name = "test list";
                list.IsDone = true;

                await context.ToDoLists.AddAsync(list);
                await context.SaveChangesAsync();

                var results = context.ToDoLists.Where(i => i.Name == "test list");
                Assert.Equal(1, results.Count());

                ToDoList removed = context.ToDoLists.Find(list.ID);
                context.ToDoLists.Remove(removed);
                await context.SaveChangesAsync();

                var check = context.ToDoLists.Where(i => i.Name == "test todo");
                Assert.Equal(0, check.Count());

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
                ToDoList list = new ToDoList();
                list.Name = "test list";
                list.IsDone = true;
                await context.ToDoLists.AddAsync(list);
                await context.SaveChangesAsync();

                ToDoList result = context.ToDoLists.Find(list.ID);
                Assert.Equal("test list", result.Name);

                await context.ToDoItems.AddRangeAsync(
                new ToDoItem
                {
                    Name = "item1",
                    IsDone = true,
                    ListID = result.ID,
                },
                new ToDoItem
                {
                    Name = "item2",
                    IsDone = true,
                    ListID = result.ID,
                });
                await context.SaveChangesAsync();

                ToDoList filledList = context.ToDoLists.Find(list.ID);
                filledList.ToDoItems = context.ToDoItems.Where(i => i.ListID == filledList.ID)
                                              .ToList();
                Assert.Equal(2, filledList.ToDoItems.Count());
            }
        }
        [Fact]
        public async void CanRemoveToDoItemstoaToDoList()
        {
            DbContextOptions<ToDoDbContext> options =
                new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (ToDoDbContext context = new ToDoDbContext(options))
            {
                ToDoList list = new ToDoList();
                list.Name = "test list";
                list.IsDone = true;
                await context.ToDoLists.AddAsync(list);
                await context.SaveChangesAsync();

                ToDoList result = context.ToDoLists.Find(list.ID);
                Assert.Equal("test list", result.Name);

                await context.ToDoItems.AddRangeAsync(
                new ToDoItem
                {
                    Name = "item1",
                    IsDone = true,
                    ListID = result.ID,
                },
                new ToDoItem
                {
                    Name = "item2",
                    IsDone = true,
                    ListID = result.ID,
                });
                await context.SaveChangesAsync();

                ToDoList filledList = context.ToDoLists.Find(list.ID);
                filledList.ToDoItems = context.ToDoItems.Where(i => i.ListID == filledList.ID)
                                              .ToList();
                Assert.Equal(2, filledList.ToDoItems.Count());

                var removeToDos = context.ToDoItems.Select(i => i).ToList();
                foreach(ToDoItem item in removeToDos)
                {
                    item.ListID = 0;
                }
                context.ToDoItems.UpdateRange(removeToDos);
                await context.SaveChangesAsync();
                filledList.ToDoItems = context.ToDoItems.Where(i => i.ListID == filledList.ID)
                                              .ToList();
                Assert.Empty(filledList.ToDoItems);
            }
        }
    }
}
