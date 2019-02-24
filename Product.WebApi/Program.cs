﻿using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Product.WebApi.DataAccess;
using Product.WebApi.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Product.WebApi
{
    public class Program
    {
        private static void DBPopulateTestData()
        {
            try
            {
                using (var db = new ProductsContext())
                {
                    db.ProductOwners.Add(new ProductOwner { OwnerId = 1, OwnerName = "owner 1", Address = "owner address 1", Email = "owner1@owners.net", Phone = "213421412341" });
                    db.ProductOwners.Add(new ProductOwner { OwnerId = 2, OwnerName = "owner 2", Address = "owner address 2", Email = "owner2@owners.net", Phone = "213421412341" });
                    db.ProductOwners.Add(new ProductOwner { OwnerId = 3, OwnerName = "owner 3", Address = "owner address 3", Email = "owner3@owners.net", Phone = "213421412341" });

                    db.Producers.Add(new Manufacturer() { ManufacturerId = 1, ManufactureName = "Manufacturer 1", Address = "Manufacturer address 1" });

                    db.Users.Add(new User() { UserId = 1, Name = "sysusr", Email = "sysusr@net.ua", FirstName = "System", LastName = "System", Password = "syspasswd"});

                    db.Categories.Add(new Category() { CategoryId = 1, Name = "Category 1", Description = "Category Description 1", Parent = null, ParentId = null, Children = null });
                    db.Categories.Add(new Category() { CategoryId = 2, Name = "Category 2", Description = "Category Description 2", Parent = null, ParentId = 1, Children = null });
                    db.Categories.Add(new Category() { CategoryId = 3, Name = "Category 3", Description = "Category Description 3", Parent = null, ParentId = null, Children = null });

                    var count = db.SaveChanges();

                    db.Products.Add(new Models.Product() { ProductId = 1, ProductName = "product name 1", Description = "product 1 description", Price = 23321.34m, Owner = db.ProductOwners.First(), Producer = db.Producers.FirstOrDefault(), Category = db.Categories.First() });
                    db.Products.Add(new Models.Product() { ProductId = 2, ProductName = "product name 2", Description = "product 2 description", Price = 221.34m, Owner = db.ProductOwners.Last(), Producer = db.Producers.Last(), Category = db.Categories.First() });
                    db.Products.Add(new Models.Product() { ProductId = 3, ProductName = "product name 3", Description = "product 3 description", Price = 231.34m, Owner = db.ProductOwners.First(), Producer = db.Producers.FirstOrDefault(), Category = db.Categories.First() });
                    db.Products.Add(new Models.Product() { ProductId = 0, ProductName = "product name 3", Description = "product 3 description", Price = 231.34m, Owner = db.ProductOwners.First(), Producer = db.Producers.FirstOrDefault(), Category = db.Categories.Last() });
                    count = db.SaveChanges();
                    Console.WriteLine("{0} records saved to database", count);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void Main(string[] args)
        {
            //DBPopulateTestData();

            try
            {
                //Build Config
                var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseSerilog(dispose: true) // Set serilog as the loggin provider.
                .UseSerilog((context, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration); 
                })
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
                .UseIIS();

    }
}
