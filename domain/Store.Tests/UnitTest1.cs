using Xunit;

namespace Store.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Is�ategories_WithNull_ReturnFalse()
        {
            bool result = Product.Is�ategories(null);
            Assert.False(result);
        }

        [Fact]
        public void Is�ategories_VoidString_ReturnFalse()
        {
            bool result = Product.Is�ategories("  ");
            Assert.False(result);
        }
    }
}