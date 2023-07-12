﻿using Halibut.Tests.Support.TestCases;

namespace Halibut.Tests.Support.TestAttributes
{
    /// <summary>
    ///     Holds all the standard test cases for testing a latest client with a latest service.
    ///     In this case latest means the code as is, rather than some previously built version of halibut.
    /// </summary>
    public class LatestClientAndLatestServicePerfectNetworkTestCases : LatestClientAndLatestServiceTestCases
    {
        protected override NetworkConditionTestCase[] NetworkConditionTestCases
        {
            get
            {
                return new[]
                {
                    NetworkConditionTestCase.NetworkConditionPerfect
                };
            }
        }
    }
}