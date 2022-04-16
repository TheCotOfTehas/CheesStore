using Xunit;

namespace Store.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Is—ategories_WithNull_ReturnFalse()
        {
            bool result = Products.Is—ategories(null);
            Assert.False(result);
        }

        [Fact]
        public void Is—ategories_VoidString_ReturnFalse()
        {
            bool result = Products.Is—ategories("  ");
            Assert.False(result);
        }

        [Fact]
        public void Is—ategories_WithInvalidCategories_ReturnFalse()
        {
            bool result = Products.Is—ategories("ÕÂÚÛ Ú‡ÍÓÈ");
            Assert.False(result);
        }
    }
}