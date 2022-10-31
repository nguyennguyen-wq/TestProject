
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebEFMVCDemo.Controllers;
using WebEFMVCDemo.Data;
using WebEFMVCDemo.Models;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data.Entity;
using System.Reflection.Metadata;

namespace TestProject
{
    public class DbContextTest
    {

        [Fact]
        public async Task TestIndex()
        {
            var options = new DbContextOptionsBuilder<WebEFMVCDemoContext>()
                .UseInMemoryDatabase(databaseName: "databaseTest").Options;
            using (var context = new WebEFMVCDemoContext(options))
            {                
                var controller = new MedlemsController(context);  
                var result = await controller.Index();
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task TestMedlemCount()
        {
            var options = new DbContextOptionsBuilder<WebEFMVCDemoContext>()
                .UseInMemoryDatabase(databaseName: "databaseTest").Options;
            using (var context = new WebEFMVCDemoContext(options))
            {
                context.Kontingents.AddRange(
                   new Kontingent() { KontintId = 1, Name = "betalt" },
                   new Kontingent() { KontintId = 2, Name = "ikke betalt" }
                );

                context.Medlems.AddRange(
                   new Medlem()
                   {
                       Medlem_Id = Guid.NewGuid(),
                       Fornavn = "Mr Demo",
                       Etternavn = "Appli",
                       Bosted = "USA",
                       MobilTlf = 222222,
                       Email = "usa@com",
                       Fodselsdato = DateTime.Parse("1959-4-15"),
                       KontintId = 2,
                   },

                   new Medlem()
                   {
                       Medlem_Id = Guid.NewGuid(),
                       Fornavn = "Mr Demo",
                       Etternavn = "App",
                       Bosted = "usa",
                       MobilTlf = 222222,
                       Email = "usa@com",
                       Fodselsdato = DateTime.Parse("1959-4-15"),
                       KontintId = 1,
                   }
                );
                context.SaveChanges();   
            }
            
            using (var context = new WebEFMVCDemoContext(options))
            {
                var controller = new MedlemsController(context);
                var result = await controller.Index();
                Assert.IsType<ViewResult>(result);
                var viewResult = Assert.IsType<ViewResult>(result);
                var medlem = Assert.IsType<List<Medlem>>(viewResult.Model);
                Assert.Equal(2, medlem.Count);
            }
        }
    }
}