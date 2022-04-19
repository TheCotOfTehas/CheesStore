using Xunit;

namespace Store.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void IsÑategories_WithNull_ReturnFalse()
        {
            bool result = Product.IsÑategories(null);
            Assert.False(result);
        }

        [Fact]
        public void IsÑategories_VoidString_ReturnFalse()
        {
            bool result = Product.IsÑategories("  ");
            Assert.False(result);
        }
    }
}