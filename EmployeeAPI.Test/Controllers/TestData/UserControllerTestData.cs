using Bogus;
using ModelLibrary.Entities;
using System.Collections.Generic;
using ModelLibrary.Enums;

namespace EmployesAPI.Test.Data
{
    public static class UserControllerTestData
    {
        /// <summary>
        /// Generates a valid user entity with random data.
        /// </summary>
        /// <returns>A valid <see cref="User"/> object.</returns>
        public static User GenerateValidUser()
        {
            var fakeUser = new Faker<User>()
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.HashPassword, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.Role, f => f.PickRandom<Role>())
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(30, DateTime.Today.AddYears(-18))) // At least 18 years old
                .RuleFor(u => u.PhoneNumbers, f => GeneratePhoneNumbers());
            return fakeUser.Generate();
        }

        /// <summary>
        /// Generates phone numbers for a user.
        /// </summary>
        /// <returns>A list of <see cref="PhoneNumber"/> objects.</returns>
        private static List<PhoneNumber> GeneratePhoneNumbers()
        {
            return new Faker<PhoneNumber>()
                .RuleFor(p => p.Number, f => f.Phone.PhoneNumber())
                .Generate(2); // Generates two unique phone numbers.
        }
    }
}