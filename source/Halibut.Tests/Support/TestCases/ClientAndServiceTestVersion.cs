﻿using System;
using Halibut.Tests.Support.BackwardsCompatibility;

namespace Halibut.Tests.Support.TestCases
{
    public class ClientAndServiceTestVersion
    {
        public string? ClientVersion;
        public string? ServiceVersion;

        ClientAndServiceTestVersion(string? clientVersion, string? serviceVersion)
        {
            ClientVersion = clientVersion;
            ServiceVersion = serviceVersion;
        }

        public static ClientAndServiceTestVersion Latest()
        {
            return new ClientAndServiceTestVersion(null, null);
        }

        public static ClientAndServiceTestVersion ClientOfVersion(string clientVersion)
        {
            return new ClientAndServiceTestVersion(clientVersion, null);
        }

        public static ClientAndServiceTestVersion ServiceOfVersion(string serviceVersion)
        {
            return new ClientAndServiceTestVersion(null, serviceVersion);
        }

        public bool IsLatest()
        {
            return ClientVersion == null && ServiceVersion == null;
        }

        public bool IsPreviousClient()
        {
            return ClientVersion != null;
        }

        public bool IsPreviousService()
        {
            return ServiceVersion != null;
        }

        public override string ToString()
        {
            if (IsLatest())
            {
                return "v:Latest";
            }

            if (IsPreviousClient())
            {
                return $"vClient:{ClientVersion}";
            }

            if (IsPreviousService())
            {
                return $"vService:{ServiceVersion}";
            }

            return "shrug";
        }
    }

    public class ClientAndServiceBuilderFactory
    {
        public static Func<ServiceConnectionType, IClientAndServiceBuilder> ForVersion(ClientAndServiceTestVersion version)
        {
            if (version.IsLatest())
            {
                return LatestClientAndLatestServiceBuilder.ForServiceConnectionType;
            }

            if (version.IsPreviousClient())
            {
                return sct => PreviousClientVersionAndLatestServiceBuilder.ForServiceConnectionType(sct).WithClientVersion(version.ClientVersion!);
            }

            if (version.IsPreviousService())
            {
                return sct => LatestClientAndPreviousServiceVersionBuilder.ForServiceConnectionType(sct).WithServiceVersion(version.ServiceVersion!);
            }

            throw new Exception($"We don't know what kind of thing to build here: version={version}");
        }
    }
}