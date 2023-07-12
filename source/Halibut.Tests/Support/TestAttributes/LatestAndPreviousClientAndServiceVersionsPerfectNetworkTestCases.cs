﻿using Halibut.Tests.Support.BackwardsCompatibility;
using Halibut.Tests.Support.TestCases;

namespace Halibut.Tests.Support.TestAttributes
{
    public class LatestAndPreviousClientAndServiceVersionsPerfectNetworkTestCases : ClientAndServiceTestCases
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

        public LatestAndPreviousClientAndServiceVersionsPerfectNetworkTestCases()
            : base(ClientAndServiceTestVersion.Latest(),
                ClientAndServiceTestVersion.ClientOfVersion(PreviousVersions.v5_0_236_Used_In_Tentacle_6_3_417),
                ClientAndServiceTestVersion.ServiceOfVersion(PreviousVersions.v5_0_236_Used_In_Tentacle_6_3_417))
        {
        }
    }
}