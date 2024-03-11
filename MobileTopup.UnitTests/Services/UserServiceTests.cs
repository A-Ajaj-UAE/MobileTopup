using FluentValidation;
using Microsoft.Extensions.Logging;
using MobileTopup.API.Repositories;
using MobileTopup.API.Services;
using MobileTopup.Contracts.Exceptions;
using MobileTopup.Contracts.Models;
using MobileTopup.Contracts.Validatiors;

namespace MobileTopup.UnitTests.Services
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly Mock<UserRepository> _userRepository;
        private readonly Mock<HttpClient> httpClient;   
        private readonly Mock<ILogger<UserService>> logger;   

        public UserServiceTest()
        {
            var userValidator = new UserValidator();
            var BenficiaryValidator = new BeneficiaryValidator();
            _userRepository = new Mock<UserRepository>();
            httpClient = new Mock<HttpClient>();
            logger = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepository.Object,BenficiaryValidator, userValidator, httpClient.Object, logger.Object);
        }


        [Fact]
        public void AddTopUpBeneficiary_Success()
        {
            // Arrange
            var user = new User { IsVerified = true };
            user.Beneficiaries = new List<Beneficiary>
            {
                new Beneficiary("Beneficiary 1",true),
                new Beneficiary("Beneficiary 2",true),
                new Beneficiary("Beneficiary 3",true),
                new Beneficiary("Beneficiary 4",true),
            };

            var beneficiary = new Beneficiary("Beneficiary 5", true);

            // Act
            _userService.AddBeneficiary(user, beneficiary);
       
            // Assert
            Assert.Contains(beneficiary, user.Beneficiaries);
        }

        [Fact]
        public void AddTopUpBeneficiary_MaximumLimitReached()
        {
           // Arrange
           var user = new User { IsVerified = true};
            user.Beneficiaries = new List<Beneficiary>
            {
                new Beneficiary("Beneficiary 1",true),
                new Beneficiary("Beneficiary 2",true),
                new Beneficiary("Beneficiary 3",true),
                new Beneficiary("Beneficiary 4",true),
                new Beneficiary("Beneficiary 5",true) 
            };

            var beneficiary = new Beneficiary ("Beneficiary 6",true);

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => _userService.AddBeneficiary(user, beneficiary));
            Assert.IsType(typeof(ValidationException), exception);
            Assert.Contains(BeneficiaryExceptions.MaxActiveBenfenciryExceeded, exception.Errors.Select(e=> e.ErrorMessage));
        }

        [Fact]
        public void AddTopUpBeneficiary_NicknameMaxLength()
        {
            // Arrange
            var user = new User { IsVerified = true };
            user.Beneficiaries = new List<Beneficiary>();

            var beneficiary = new Beneficiary("Beneficiary more than 20 chars", true);

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => _userService.AddBeneficiary(user, beneficiary));
            Assert.IsType(typeof(ValidationException), exception);
            Assert.Contains(BeneficiaryExceptions.BenefenciryNickNameLenghtExceeded, exception.Errors.Select(e=> e.ErrorMessage));
        }

        [Fact]
        public void User_ViewAvailableTopupBeneficiaries()
        {
            // Arrange
            var user = new User { IsVerified = false };
            user.Beneficiaries = new List<Beneficiary>();

            var beneficiary = new Beneficiary("Beneficiary 1", true);

            // Act
            _userService.AddBeneficiary(user, beneficiary);

            // Assert
            Assert.Single(user.Beneficiaries, beneficiary);
        }

        


        
    }
}