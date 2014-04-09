// Guids.cs
// MUST match guids.h
using System;

namespace ChristianRHelle.VSPackage
{
    static class GuidList
    {
        public const string guidVSPackagePkgString = "e035f4eb-5ebf-4537-b84e-5be04b888ec1";
        public const string guidVSPackageCmdSetString = "933646f1-d609-41f8-85dd-24b2e3340ef9";

        public static readonly Guid guidVSPackageCmdSet = new Guid(guidVSPackageCmdSetString);
    };
}