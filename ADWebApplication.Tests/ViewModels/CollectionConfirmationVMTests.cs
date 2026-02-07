using ADWebApplication.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ADWebApplication.Tests.ViewModels
{
    public class CollectionConfirmationVMTests
    {
        [Fact]
        public void CollectionConfirmationVM_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var viewModel = new CollectionConfirmationVM();

            // Assert
            Assert.Equal(0, viewModel.StopId);
            Assert.Null(viewModel.PointId);
            Assert.Equal(string.Empty, viewModel.LocationName);
            Assert.Equal(string.Empty, viewModel.Address);
            Assert.Null(viewModel.BinId);
            Assert.Equal(string.Empty, viewModel.Zone);
            Assert.Equal(0, viewModel.BinFillLevel);
            Assert.Equal("Good", viewModel.BinCondition);
            Assert.False(viewModel.CollectedElectronics);
            Assert.False(viewModel.CollectedBatteries);
            Assert.False(viewModel.CollectedCables);
            Assert.False(viewModel.CollectedAccessories);
            Assert.Null(viewModel.Remarks);
            Assert.Null(viewModel.NextPointId);
            Assert.Null(viewModel.NextLocationName);
            Assert.Null(viewModel.NextAddress);
            Assert.Null(viewModel.NextPlannedTime);
            Assert.Null(viewModel.NextFillLevel);
        }

        [Fact]
        public void CollectionConfirmationVM_AllProperties_CanBeSet()
        {
            // Arrange
            var viewModel = new CollectionConfirmationVM();
            var collectionTime = new DateTime(2026, 2, 7, 10, 30, 0);
            var nextPlannedTime = new DateTime(2026, 2, 7, 11, 0, 0);

            // Act
            viewModel.StopId = 123;
            viewModel.PointId = "P001";
            viewModel.LocationName = "Central Mall";
            viewModel.Address = "123 Main Street";
            viewModel.BinId = "BIN-001";
            viewModel.Zone = "Zone A";
            viewModel.BinFillLevel = 75;
            viewModel.CollectionTime = collectionTime;
            viewModel.BinCondition = "Damaged";
            viewModel.CollectedElectronics = true;
            viewModel.CollectedBatteries = true;
            viewModel.CollectedCables = true;
            viewModel.CollectedAccessories = true;
            viewModel.Remarks = "Heavy load collected";
            viewModel.NextPointId = "P002";
            viewModel.NextLocationName = "West Plaza";
            viewModel.NextAddress = "456 West Ave";
            viewModel.NextPlannedTime = nextPlannedTime;
            viewModel.NextFillLevel = 60;

            // Assert
            Assert.Equal(123, viewModel.StopId);
            Assert.Equal("P001", viewModel.PointId);
            Assert.Equal("Central Mall", viewModel.LocationName);
            Assert.Equal("123 Main Street", viewModel.Address);
            Assert.Equal("BIN-001", viewModel.BinId);
            Assert.Equal("Zone A", viewModel.Zone);
            Assert.Equal(75, viewModel.BinFillLevel);
            Assert.Equal(collectionTime, viewModel.CollectionTime);
            Assert.Equal("Damaged", viewModel.BinCondition);
            Assert.True(viewModel.CollectedElectronics);
            Assert.True(viewModel.CollectedBatteries);
            Assert.True(viewModel.CollectedCables);
            Assert.True(viewModel.CollectedAccessories);
            Assert.Equal("Heavy load collected", viewModel.Remarks);
            Assert.Equal("P002", viewModel.NextPointId);
            Assert.Equal("West Plaza", viewModel.NextLocationName);
            Assert.Equal("456 West Ave", viewModel.NextAddress);
            Assert.Equal(nextPlannedTime, viewModel.NextPlannedTime);
            Assert.Equal(60, viewModel.NextFillLevel);
        }

        [Fact]
        public void BinFillLevel_ValidatesRange_WithinBounds()
        {
            // Arrange
            var viewModel = new CollectionConfirmationVM
            {
                BinFillLevel = 50
            };
            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(viewModel, context, results, true);

            // Assert - No error for BinFillLevel
            Assert.DoesNotContain(results, r => r.MemberNames.Contains("BinFillLevel"));
        }

        [Fact]
        public void BinFillLevel_ValidatesRange_TooHigh()
        {
            // Arrange
            var viewModel = new CollectionConfirmationVM
            {
                BinFillLevel = 101
            };
            var context = new ValidationContext(viewModel) { MemberName = "BinFillLevel" };
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateProperty(viewModel.BinFillLevel, context, results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Fill level must be between 0 and 100");
        }

        [Fact]
        public void BinFillLevel_ValidatesRange_Negative()
        {
            // Arrange
            var viewModel = new CollectionConfirmationVM
            {
                BinFillLevel = -1
            };
            var context = new ValidationContext(viewModel) { MemberName = "BinFillLevel" };
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateProperty(viewModel.BinFillLevel, context, results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Fill level must be between 0 and 100");
        }

        [Fact]
        public void BinFillLevel_AcceptsZero()
        {
            // Arrange
            var viewModel = new CollectionConfirmationVM
            {
                BinFillLevel = 0
            };
            var context = new ValidationContext(viewModel) { MemberName = "BinFillLevel" };
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateProperty(viewModel.BinFillLevel, context, results);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void BinFillLevel_Accepts100()
        {
            // Arrange
            var viewModel = new CollectionConfirmationVM
            {
                BinFillLevel = 100
            };
            var context = new ValidationContext(viewModel) { MemberName = "BinFillLevel" };
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateProperty(viewModel.BinFillLevel, context, results);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void CollectionTime_DefaultsToNow()
        {
            // Arrange
            var before = DateTime.Now.AddSeconds(-1);
            
            // Act
            var viewModel = new CollectionConfirmationVM();
            var after = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.InRange(viewModel.CollectionTime, before, after);
        }

        [Fact]
        public void CategoryCheckboxes_CanBeToggled()
        {
            // Arrange
            var viewModel = new CollectionConfirmationVM();

            // Act & Assert - Initially false
            Assert.False(viewModel.CollectedElectronics);
            Assert.False(viewModel.CollectedBatteries);
            Assert.False(viewModel.CollectedCables);
            Assert.False(viewModel.CollectedAccessories);

            // Toggle to true
            viewModel.CollectedElectronics = true;
            viewModel.CollectedBatteries = true;
            viewModel.CollectedCables = true;
            viewModel.CollectedAccessories = true;

            Assert.True(viewModel.CollectedElectronics);
            Assert.True(viewModel.CollectedBatteries);
            Assert.True(viewModel.CollectedCables);
            Assert.True(viewModel.CollectedAccessories);
        }

        [Fact]
        public void NextStopProperties_CanAllBeNull()
        {
            // Arrange & Act
            var viewModel = new CollectionConfirmationVM
            {
                NextPointId = null,
                NextLocationName = null,
                NextAddress = null,
                NextPlannedTime = null,
                NextFillLevel = null
            };

            // Assert
            Assert.Null(viewModel.NextPointId);
            Assert.Null(viewModel.NextLocationName);
            Assert.Null(viewModel.NextAddress);
            Assert.Null(viewModel.NextPlannedTime);
            Assert.Null(viewModel.NextFillLevel);
        }

        [Fact]
        public void OptionalStrings_CanBeNull()
        {
            // Arrange & Act
            var viewModel = new CollectionConfirmationVM
            {
                PointId = null,
                BinId = null,
                Remarks = null
            };

            // Assert
            Assert.Null(viewModel.PointId);
            Assert.Null(viewModel.BinId);
            Assert.Null(viewModel.Remarks);
        }
    }
}
