using RichillCapital.Domain;
using RichillCapital.Infrastructure.Persistence;
using RichillCapital.UseCases;
using System.Reflection;

namespace RichillCapital.ArchitectureTests;

public abstract class ArchitectureTest
{
    protected static readonly Assembly DomainAssembly = typeof(DomainServiceExtensions).Assembly;
    protected static readonly Assembly UseCaseAssembly = typeof(ApplicationServiceExtensions).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(PersistenceServiceExtensions).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}