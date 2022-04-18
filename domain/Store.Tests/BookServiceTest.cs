using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests
{
    public class BookServiceTest
    {
        [Fact]
        public void GetAllByQueryWithCategoriCallsGetAllByQuery()
        {
            //Тут я поплыл
            var productRepositoryStub = new Mock<IProductRepository>();

            productRepositoryStub.Setup(x => x
                .GetAllByСategories(It.IsAny<string>()))
                .Returns(new[] {new Products(1, "", "", "")});

            productRepositoryStub.Setup(x => x
               .GetAllByTitleOrManufacture(It.IsAny<string>()))
               .Returns(new[] { new Products(2, "", "", "") });

            var productService = new ProductService(productRepositoryStub.Object);

            var actual = productService.GetAllByQuery("Chees");

            Assert.Collection(actual, product => Assert.Equal(1, product.Id));
        }
    }
}
