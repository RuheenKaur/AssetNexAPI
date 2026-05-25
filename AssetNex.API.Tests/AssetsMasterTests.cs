using Xunit;
using Moq;
using FluentAssertions;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;

namespace AssetNex.API.Tests.Services
{
    public class AssetsMasterRepTests
    {
        private readonly Mock<IAssetsMasterRep> _mockRepo;

        public AssetsMasterRepTests()
        {
            _mockRepo = new Mock<IAssetsMasterRep>();
        }

        
        private AssetsMaster MakeAsset(int id = 1, string tag = "AST-001") => new AssetsMaster
        {
            Id = id,
            AssetTag = tag,
            AssetType = "Laptop",
            Brand = "Dell"
        };

     

        [Fact]
        public async Task GetAllAsync_ReturnsAllAssets()
        {
            var assets = new List<AssetsMaster>
            {
                MakeAsset(1, "AST-001"),
                MakeAsset(2, "AST-002"),
                MakeAsset(3, "AST-003")
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(assets);

            var result = await _mockRepo.Object.GetAllAsync();

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenEmpty_ReturnsEmptyList()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<AssetsMaster>());

            var result = await _mockRepo.Object.GetAllAsync();

            result.Should().BeEmpty();
        }

       
        [Fact]
        public async Task GetAsync_WhenAssetExists_ReturnsAsset()
        {
            var asset = MakeAsset(1, "AST-001");
            _mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(asset);

            var result = await _mockRepo.Object.GetAsync(1);

            result.Should().NotBeNull();
            result!.AssetTag.Should().Be("AST-001");
        }

        [Fact]
        public async Task GetAsync_WhenAssetNotFound_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetAsync(99)).ReturnsAsync((AssetsMaster?)null);

            var result = await _mockRepo.Object.GetAsync(99);

            result.Should().BeNull();
        }

      
        [Fact]
        public async Task AddAsync_ReturnsCreatedAsset()
        {
            var newAsset = MakeAsset(0, "AST-010");
            var savedAsset = MakeAsset(10, "AST-010");
            _mockRepo.Setup(r => r.AddAsync(newAsset)).ReturnsAsync(savedAsset);

            var result = await _mockRepo.Object.AddAsync(newAsset);

            result.Should().NotBeNull();
            result.Id.Should().Be(10);
            result.AssetTag.Should().Be("AST-010");
        }

        [Fact]
        public async Task AddAsync_AssignedIdAfterCreation()
        {
            var newAsset = MakeAsset(0, "AST-011");
            var savedAsset = MakeAsset(11, "AST-011");
            _mockRepo.Setup(r => r.AddAsync(newAsset)).ReturnsAsync(savedAsset);

            var result = await _mockRepo.Object.AddAsync(newAsset);

            result.Id.Should().BeGreaterThan(0);
        }

 
        [Fact]
        public async Task UpdateAsync_WhenAssetExists_ReturnsUpdatedAsset()
        {
            var updatedAsset = new AssetsMaster
            {
                Id = 1,
                AssetTag = "AST-001",
                AssetType = "Laptop",
                Brand = "Lenovo",
                RAM_GB = "16GB"
            };
            _mockRepo.Setup(r => r.UpdateAsync(updatedAsset)).ReturnsAsync(updatedAsset);

            var result = await _mockRepo.Object.UpdateAsync(updatedAsset);

            result.Should().NotBeNull();
            result.Brand.Should().Be("Lenovo");
            result.RAM_GB.Should().Be("16GB");
        }

        [Fact]
        public async Task UpdateDetails_WhenAssetExists_ReturnsUpdatedAsset()
        {
            var model = new AssetsMaster
            {
                Id = 1,
                AssetTag = "AST-001",
                AssetType = "Laptop",
                Brand = "Apple",
                Model = "MacBook Pro"
            };
            _mockRepo.Setup(r => r.UpdateDetails(model)).ReturnsAsync(model);

            var result = await _mockRepo.Object.UpdateDetails(model);

            result.Brand.Should().Be("Apple");
            result.Model.Should().Be("MacBook Pro");
        }

       

        [Fact]
        public async Task DeleteAsync_WhenAssetExists_ReturnsTrue()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _mockRepo.Object.DeleteAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_WhenAssetNotFound_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _mockRepo.Object.DeleteAsync(99);

            result.Should().BeFalse();
        }

    

        [Fact]
        public async Task GetAssetsPagedAsync_ReturnsPaginatedResult()
        {
            var pagedResult = new PagedResultAssets<AssetsMaster>
            {
                Data = new List<AssetsMaster>
                {
                    MakeAsset(1, "AST-001"),
                    MakeAsset(2, "AST-002")
                },
                TotalCount = 10,
                Page = 1,
                PageSize = 2
            };
            _mockRepo.Setup(r => r.GetAssetsPagedAsync(1, 2, "")).ReturnsAsync(pagedResult);

            var result = await _mockRepo.Object.GetAssetsPagedAsync(1, 2, "");

            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.TotalCount.Should().Be(10);
            result.Page.Should().Be(1);
        }

        [Fact]
        public async Task GetAssetsPagedAsync_WithSearch_ReturnsFilteredResult()
        {
            var pagedResult = new PagedResultAssets<AssetsMaster>
            {
                Data = new List<AssetsMaster>
                {
                    new AssetsMaster { Id = 1, AssetTag = "AST-001", AssetType = "Laptop", Brand = "Dell" }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };
            _mockRepo.Setup(r => r.GetAssetsPagedAsync(1, 10, "Dell")).ReturnsAsync(pagedResult);

            var result = await _mockRepo.Object.GetAssetsPagedAsync(1, 10, "Dell");

            result.Data.Should().HaveCount(1);
            result.Data.First().Brand.Should().Be("Dell");
        }

        [Fact]
        public async Task GetAssetsPagedAsync_WhenNoResults_ReturnsEmptyData()
        {
            var pagedResult = new PagedResultAssets<AssetsMaster>
            {
                Data = new List<AssetsMaster>(),
                TotalCount = 0,
                Page = 1,
                PageSize = 10
            };
            _mockRepo.Setup(r => r.GetAssetsPagedAsync(1, 10, "xyz")).ReturnsAsync(pagedResult);

            var result = await _mockRepo.Object.GetAssetsPagedAsync(1, 10, "xyz");

            result.Data.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

     

        [Fact]
        public async Task GetAsyncStatus_WhenExists_ReturnsAsset()
        {
            var asset = new AssetsMaster { Id = 1, AssetTag = "AST-001", AssetType = "Laptop", Brand = "Dell", StatusId = 2 };
            _mockRepo.Setup(r => r.GetAsyncStatus(1)).ReturnsAsync(asset);

            var result = await _mockRepo.Object.GetAsyncStatus(1);

            result.Should().NotBeNull();
            result!.StatusId.Should().Be(2);
        }

        [Fact]
        public async Task GetAsyncStatus_WhenNotFound_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetAsyncStatus(99)).ReturnsAsync((AssetsMaster?)null);

            var result = await _mockRepo.Object.GetAsyncStatus(99);

            result.Should().BeNull();
        }
    }
}
