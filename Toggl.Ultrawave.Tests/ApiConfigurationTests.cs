﻿using System;
using FluentAssertions;
using Toggl.Ultrawave.Network;
using Xunit;
using static Toggl.Ultrawave.ApiEnvironment;
using static Toggl.Ultrawave.Network.Credentials;

namespace Toggl.Ultrawave.Tests
{
    public class ApiConfigurationTests
    {
        public class TheConstructor
        {
            private static readonly UserAgent correctUserAgent = new UserAgent("Test", "1.0");

            [Fact]
            public void ThrowsWhenTheCredentialsParameterIsNull()
            {
                Action constructingWithWrongParameteres =
                    () => new ApiConfiguration(Staging, null, correctUserAgent);

                constructingWithWrongParameteres
                    .ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void ThrowsWhenTheUserAgentIsNull()
            {
                Action constructingWithWrongParameteres =
                    () => new ApiConfiguration(Staging, None, null);

                constructingWithWrongParameteres
                    .ShouldThrow<ArgumentException>();
            }
        }
    }
}
