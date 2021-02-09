using AutoMapper;

namespace AppExample.Tests
{
    using static Testing;

    class AutoMapperConfigurationTests
    {
        public void ShouldBeValid()
        {
            Scoped<IMapper>(mapper =>
            {
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
            });
        }
    }
}
