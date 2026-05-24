//using Asp.Versioning;
//using Moq;
//using System.ComponentModel.Design;

//namespace AssetNex.API.Tests;

//public class UnitTest1
//{
//    [Fact]
//    public void Test1()
//    {

//    }

//    [Fact]
//    public async Task GetAllAssets_ShouldReturnAssets()
//    {
//        // Arrange
//        var mockRepo = new Mock<IAssetRepository>();

//        mockRepo.Setup(r => r.GetAllAssetsAsync())
//                .ReturnsAsync(new List<Asset> { new Asset { Id = 1 } });//when getall is called on the mock, return this fake list instead of hitting the db, 


//        var service = new AssetService(mockRepo.Object); //creates the real asset service, but injects the fake repo , .object converts the mock wrapper into the 
//        //actual interface object 

//        // Act
//        var result = await service.GetAllAssetsAsync();//calls the method we're testing 

//        // Assert
//        Assert.Single(result);//result has exactly one item 

//        mockRepo.Verify(r => r.GetAllAssetsAsync(), Times.Once); //confirms getall was called exactly once during the test. catches bugs where service 
//        //forgets to call the repo.


//    }

//    [Fact]

//    public async Task Method_ShouldDoSomething()
//    {
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(x => x.Method()).ReturnsAsync(fakeData);

//        var service = new RealService(mockRepo.Object);

//        var result = await service.Method();

//        Assert.Equal(expected, result);

//        mockRepo.Verify(x => x.Method(), Times.Once);
//    }


//    [Fact]

//    public async Task Method_ShouldDoSomething()
//    {
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(x => x.Method()).ReturnsAsync(fakeData);
//        var service = new RealService(mockRepo.Object);
//        var result = await service.Method();
//        Assert.Equal(expected, result);
//        mockRepo.Verify(x => x.Method(), Times.Once);

//    }

//    [Fact]

//    public async Task Method_ShouldDoSomething()
//    {
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(x => x.Method()).ReturnsAsync(fakeData);
//    }
//    [Fact]
//    public async Task Method_ShouldDoSomething()
//    {
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(r => r.GetAllAssetsAsync()).ReturnsAsync(fakeData);
//        var service = new RealService(mockRepo.Object);
//        var result = await service.Method();
//        Assert.Equal(expected, result);
//        mockRepo.Verify(x => x.Method(), Times.Once);
//    }

//    [Fact]
//    public async Task Method_ShouldDoSomething()
//    {
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(r => r.GetAllAssetsAsync()).ReturnAsync(fakeData);
//        var service = new RealService(mockRepo.Object);
//        var result = await service.Method();
//        Assert.Equal(expected, result);
//        mockRepo.Verify(x => x.Method(), Times.Once);


//    }

//    [Fact]

//    public async Task Method_ShouldDoSomething()
//    {
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(x => x.Method()).ReturnsAsync(fakeData);
//        var service = new RealService(mockRepo.Object);

//        var result = await service.Method();

//        Assert.Equal(expected, result);
//        mockRepo.Verify(x => x.Method(), Times.Once);
//    }

//    [Fact]
//    public async Task Method_ShouldDoSomething()
//    {
//        // Arrange - setup mock
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(x => x.Method()).ReturnsAsync(fakeData);
//        var service = new RealService(mockRepo.Object);
//        // Act - call the real method
//        var result = await service.Method();
//        Assert.Equal(expected, result);
//        mockRepo.Verify(x => x.Method(), Times.Once);
//    }
//    [Fact]

//    public async Task GetAllAssets_ShouldReturnAssets()
//    {
//        var mockRepo = new Mock<IAssetRepository>();
//        mockRepo.Setup(r => r.GetAllAssetsAsync())
//            .ReturnAsync(new List<Asset> { new Asset { Id = 1 } });
//        var service = new AssetService(mockRepo.Object);
//        var result = await service.GetAllAssetsAsync();
//        Assert.Single(result);
//        mockRepo.Verify(r => GetAllAssetsAsync(), Times.Once);
//    }

//    [Fact]

//    public async Task GetAllAssets_ShouldReturnAssets()
//    {
//        var mockRepo = new Mock<IAssetsAssignmentRep>();
//        mockRepo.Setup(r => r.GetAllAssetsAsync()).ReturnsAsync(new List<Asset> { new Asset { Id = 1 } });
//        var service = new AssetService(mockRepo.Object);
//        var result = await service.GetAllAssetsAsync();
//        Assert.Single(result);
//        mockRepo.Verify(r => r.GetAllAssetsAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task MethodShouldDoSomething()
//    {
//        var mockRepo = new Mock<Interface>();
//        mockRepo.Setup(x => x.Method().ReturnsAsync(fakeData));
//        var service = new AssetService(mockRepo.Object);
//        var result = await service.Method();
//        Assert.Single(result);
//        mockRepo.Verify(r => r.GetAllAssetsAsync(), Times.Once);

//    }

//}