using FluentAssertions;

using NetArchTest.Rules;
using RichillCapital.SharedKernel;

namespace RichillCapital.ArchitectureTests;

public sealed class ConventionTests : ArchitectureTest
{
    [Fact]
    public void DomainEvents_Should_HaveDomainEventSuffix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
