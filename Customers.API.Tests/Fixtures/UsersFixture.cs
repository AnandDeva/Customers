using Customers.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.UnitTests.Fixtures
{
    public static class UserFixture
    {
        public static List<User> GetTestUsers() => new() {
                new User {
                    Name = "Anand",
                    Email = "anand@test.com",
                    Address = new Address() {
                        City = "Chennai",
                        Street = "Test",
                        ZipCode = "900922"
                    }
                },
                new User {
                    Name = "Naveen",
                    Email = "naveen@test.com",
                    Address = new Address() {
                        City = "Gujarat",
                        Street = "Sandiwa",
                        ZipCode = "900921"
                    }
                },
                new User {
                    Name = "Deepak",
                    Email = "deepak@test.com",
                    Address = new Address() {
                        City = "Hyderabad",
                        Street = "Fidaozo",
                        ZipCode = "900920"
                    }
                }
            };
    };
}
