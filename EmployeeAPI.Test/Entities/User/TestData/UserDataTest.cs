using Bogus;
using ModelLibrary.Entities;
using System.Collections.Generic;

namespace EmployesAPI.Test.Entities.User.TestData
{
    /// <summary>
    /// Provides methods for generating test data for User entities using Bogus.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class UserTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid User entities.
        /// The generated users will have valid:
        /// - UserName
        /// - First and last name
        /// - Email
        /// - Password and confirmation password
        /// - Document number
        /// - Phone numbers (as PhoneNumber objects)
        /// - Date of birth ensuring 18+ years
        /// </summary>
        private static readonly Faker<ModelLibrary.Entities.User> UserFaker = new Faker<ModelLibrary.Entities.User>()
            .CustomInstantiator(f => new ModelLibrary.Entities.User())
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.Password, f => "StrongPass1!")
            .RuleFor(u => u.RePassword, f => "StrongPass1!") 
            .RuleFor(u => u.DocumentNumber, f => f.Random.String2(10, "0123456789"))
            .RuleFor(u => u.PhoneNumbers, f => GeneratePhoneNumbers())
            .RuleFor(u => u.DateOfBirth, f => f.Date.Past(30, DateTime.Today.AddYears(-18)));

        /// <summary>
        /// Generates a list of unique phone numbers for a user.
        /// </summary>
        /// <returns>A list of PhoneNumber objects with unique numbers.</returns>
        private static List<PhoneNumber> GeneratePhoneNumbers()
        {
            return new Faker<PhoneNumber>()
                .CustomInstantiator(f => new PhoneNumber())
                .RuleFor(p => p.Number, f => f.Phone.PhoneNumberFormat())
                .Generate(3); 
        }

        /// <summary>
        /// Generates a valid User entity with randomized data.
        /// The generated user will have all properties populated with valid values
        /// that meet the system's validation requirements.
        /// </summary>
        /// <returns>A valid User entity with randomly generated data.</returns>
        public static ModelLibrary.Entities.User GenerateValidUser()
        {
            var user = UserFaker.Generate();
            return user;
        }
    }
}