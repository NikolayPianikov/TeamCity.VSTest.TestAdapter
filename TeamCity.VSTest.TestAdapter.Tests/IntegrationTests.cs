﻿namespace TeamCity.VSTest.TestAdapter.Tests
{
    using Helpers;
    using Shouldly;
    using Xunit;

#if NETCOREAPP1_0
    public class IntegrationTests
    {
        [Theory]
        [InlineData(@"IntegrationTests\dotNetCore.XUnit.Tests\dotNetCore.XUnit.Tests.csproj", 30)]
        [InlineData(@"IntegrationTests\dotNet.XUnit.Tests\dotNet.XUnit.Tests.csproj", 10)]
        [InlineData(@"IntegrationTests\dotNetCore.MS.Tests\dotNetCore.MS.Tests.csproj", 30)]
        [InlineData(@"IntegrationTests\dotNet.MS.Tests\dotNet.MS.Tests.csproj", 10)]

        public void ShouldProduceServiceMessages(string projectName, int expectedMessageCount)
        {
            // Given
            var testCommandLine = new CommandLine(
                @"dotnet",
                "test",
                projectName,
                "-l=teamcity",
                "-a=.",
#if DEBUG
                "-c=Debug"
#else
                "-c=Release"
#endif
                );

            // When
            testCommandLine.TryExecute(out CommandLineResult result).ShouldBe(true);

            // Then
            result.ExitCode.ShouldBe(1);
            result.StdError.Trim().ShouldBe(string.Empty);
            ServiceMessages.GetNumberServiceMessage(result.StdOut).ShouldBe(expectedMessageCount);
            ServiceMessages.ResultShouldContainCorrectStructureAndSequence(result.StdOut);
        }
    }
#endif
}