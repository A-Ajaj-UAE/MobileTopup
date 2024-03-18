using FluentValidation;
using Microsoft.Extensions.Logging;
using MobileTopup.API.Repositories;
using MobileTopup.API.Services;
using MobileTopup.Contracts.Exceptions;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Validatiors;
using MobileTopup.Contracts.Requests;
using AutoMapper;

namespace MobileTopup.UnitTests.Services
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<HttpClient> httpClient;   
        private readonly Mock<ILogger<UserService>> logger;   
        private readonly Mock<IMapper> mapper;   

        public UserServiceTest()
        {
            var userValidator = new UserValidator();
            var BenficiaryValidator = new BeneficiaryValidator();
            _userRepository = new Mock<IUserRepository>();
            httpClient = new Mock<HttpClient>();
            logger = new Mock<ILogger<UserService>>();
            mapper = new Mock<IMapper>();
            _userService = new UserService(_userRepository.Object,BenficiaryValidator, userValidator, httpClient.Object, logger.Object, mapper.Object);
        }


        [Fact]
        public void AddTopUpBeneficiary_Success()
        {
            // Arrange
            var user = new User { IsVerified = true };
            user.Beneficiaries = new List<Beneficiary>
            {
                new Beneficiary("Beneficiary 1", true),
                new Beneficiary("Beneficiary 2", true),
                new Beneficiary("Beneficiary 3", true),
                new Beneficiary("Beneficiary 4", true),
            };

            var beneficiaryRequst = new AddBeneficiaryRequest { NickName = "Beneficiary 5", IsActive = true };

            // Act
            _userService.AddBeneficiary(user, beneficiaryRequst);

            // Assert
            Assert.Contains(beneficiaryRequst.NickName, user.Beneficiaries.Select(b => b.NickName));
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

            var beneficiaryRequst = new AddBeneficiaryRequest { NickName = "Beneficiary 6", IsActive = true };

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => _userService.AddBeneficiary(user, beneficiaryRequst));
            
            Assert.IsType<ValidationException>(exception);

            //validate error is BeneficiaryExceptions.MaxActiveBenfenciryExceeded
            Assert.Contains(BeneficiaryExceptions.MaxActiveBenfenciryExceeded, exception.Errors.Select(e=> e.ErrorMessage));
        }

        [Fact]
        public void AddTopUpBeneficiary_NicknameMaxLength()
        {
            // Arrange
            var user = new User { IsVerified = true };
            user.Beneficiaries = new List<Beneficiary>();

            var beneficiaryRequst = new AddBeneficiaryRequest { NickName = "Beneficiary more than 20 chars", IsActive = true };

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => _userService.AddBeneficiary(user, beneficiaryRequst));
            
            Assert.IsType<ValidationException>(exception);

            Assert.Contains(BeneficiaryExceptions.BenefenciryNickNameLenghtExceeded, exception.Errors.Select(e=> e.ErrorMessage));
        }

        [Fact]
        public void User_ViewAvailableTopupBeneficiaries()
        {
            // Arrange
            var user = new User { IsVerified = false };
            user.Beneficiaries = new List<Beneficiary>();

            var beneficiary = new AddBeneficiaryRequest{ NickName = "Beneficiary 1", IsActive = true };

            // Act
            _userService.AddBeneficiary(user, beneficiary);

            // Assert
            Assert.Contains(beneficiary.NickName, (user.Beneficiaries.Select(b => b.NickName)));
            
        }

        


        
    }
}