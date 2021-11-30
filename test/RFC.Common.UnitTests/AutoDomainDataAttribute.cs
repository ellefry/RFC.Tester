using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using System.Diagnostics.CodeAnalysis;

namespace RFC.Common.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
