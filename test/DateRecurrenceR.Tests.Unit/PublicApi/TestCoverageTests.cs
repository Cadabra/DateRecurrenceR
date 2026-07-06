using System.Reflection;
using System.Runtime.CompilerServices;
using DateRecurrenceR.Internals;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.PublicApi;

/// <summary>
/// Enforces the coverage convention: every class and struct in the library has a dedicated
/// test class marked with <see cref="TestSubjectAttribute"/>. Adding a new type without tests
/// fails this test. Interfaces and the explicitly exempted implementation details are excluded.
/// </summary>
public sealed class TestCoverageTests
{
    /// <summary>Types that deliberately have no dedicated test class.</summary>
    private static readonly Type[] Exemptions =
    [
        typeof(PatternKind), // parsing detail of PatternSerializer, covered by PatternSerializerTests
        typeof(PatternComponents) // parsing detail of PatternSerializer, covered by PatternSerializerTests
    ];

    [Fact]
    public void Every_class_and_struct_has_a_test_class()
    {
        var sourceAssembly = typeof(Recurrence).Assembly;
        var testAssembly = typeof(TestCoverageTests).Assembly;

        var subjects = testAssembly.GetTypes()
            .SelectMany(type => type.GetCustomAttributes<TestSubjectAttribute>())
            .Select(attribute => attribute.Subject)
            .ToHashSet();

        var uncovered = sourceAssembly.GetTypes()
            .Where(type => !type.IsNested)
            .Where(type => !type.IsInterface)
            .Where(type => !type.IsSubclassOf(typeof(Delegate)))
            .Where(type => type.GetCustomAttribute<CompilerGeneratedAttribute>() is null)
            .Where(type => !type.Name.StartsWith('<'))
            .Where(type => !Exemptions.Contains(type))
            .Where(type => !subjects.Contains(type))
            .Select(type => type.FullName)
            .Order()
            .ToList();

        uncovered.Should().BeEmpty(
            "every class and struct must have a test class marked with [TestSubject]");
    }
}
