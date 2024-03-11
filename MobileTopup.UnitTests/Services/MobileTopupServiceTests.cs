using Microsoft.Extensions.Options;
using MobileTopup.API.Repositories;
using MobileTopup.API.Services;
using MobileTopup.API.Settings;
using MobileTopup.Contracts.Models;

namespace MobileTopup.UnitTests.Services
{
    public class MobileTopupServiceTest
    {
        private readonly MobileTopupService _topupService;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ITopupRepository> _topupRepository;
        private readonly IOptions<TopupSettings> _topupSettings = Options.Create(new TopupSettings()
        { MaxBeneficiriresAmount = 3000, TransactionFee = 1, UnverifiedMaxBeneficiryAmount = 500, VerifiedMaxBeneficiryAmount = 1000 });

        public MobileTopupServiceTest()
        {
            _userService = new Mock<IUserService>();
            _topupRepository = new Mock<ITopupRepository>();
            _topupService = new MobileTopupService(_userService.Object, _topupRepository.Object, _topupSettings);
        }

        [Fact]
        public async void GetAvailableTopupOptions_Success()
        {
            // Arrange
            var topupOptions = new List<TopupOption>
                {
                    new TopupOption("AED5", 5),
                    new TopupOption("AED10", 10),
                    new TopupOption("AED20", 20),
                    new TopupOption("AED30", 30),
                    new TopupOption("AED50", 50),
                    new TopupOption("AED75", 75),
                    new TopupOption("AED100", 100)
                };

            _topupRepository.Setup(x =>  x.GetTopupOptionsAsync()).ReturnsAsync(topupOptions);

            // Act
            var result = await _topupService.GetAvailableTopupOptions();

            // Assert
            Assert.Equal(topupOptions, result);
        }

        [Fact]
        public void TopupBeneficiary_UnverifiedUser_MaximumLimitReached()
        {
            // Arrange
            var user = new User { IsVerified = false };
            var amount = 100;
            var totalamount = 1000;

            // Act & Assert
            Assert.Throws<Exception>(() => _topupService.ValidateMaxTopupAmount(amount, user.IsVerified, totalamount));
        }

        [Fact]
        public void TopupBeneficiary_UnverifiedUser_MaximumLimitNotReached()
        {
            // Arrange
            var user = new User { IsVerified = false };
            var amount = 100;
            var totalamount = 400;

            // Act
            _topupService.ValidateMaxTopupAmount(amount, user.IsVerified, totalamount);

            // Assert
            // check if no exception is thrown
            Assert.True(true);
        }


        [Fact]
        public void TopupBeneficiary_VerifiedUser_MaximumLimitReached()
        {
            // Arrange
            var user = new User { IsVerified = true };
            var amount = 100;
            var totalamount = 1000;

            // Act & Assert
            Assert.Throws<Exception>(() => _topupService.ValidateMaxTopupAmount(amount, user.IsVerified, totalamount));
        }

        [Fact]
        public void TopupBeneficiary_VerifiedUser_MaximumLimitNotReached()
        {
            // Arrange
            var user = new User { IsVerified = true };
            var amount = 100;
            var totalamount = 400;

            // Act
            _topupService.ValidateMaxTopupAmount(amount, user.IsVerified, totalamount);

            // Assert
            // check if no exception is thrown
            Assert.True(true);
        }

        [Fact]
        public void TopupBeneficiary_AllBeneficiries_MaximumLimitNotReached()
        {
            // Arrange
            var amount = 100;
            var totalamount = 2900;

            // Act
            _topupService.ValidateMaxTopupAmountAllBeneficiaries(amount, totalamount);

            // Assert
            // check if no exception is thrown
            Assert.True(true);
        }

        [Fact]
        public void TopupBeneficiary_AllBeneficiries_MaximumLimitReached()
        {
            // Arrange
            var user = new User { IsVerified = true };
            var amount = 100;
            var totalamount = 3000;

            // Act & Assert
            Assert.Throws<Exception>(() => _topupService.ValidateMaxTopupAmountAllBeneficiaries(amount, totalamount));

        }
    }
}