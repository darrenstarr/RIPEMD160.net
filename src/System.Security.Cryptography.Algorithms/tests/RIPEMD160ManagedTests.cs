// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Contributed to .NET Foundation by Darren R. Starr - Conscia Norway AS

namespace System.Security.Cryptography.Hashing.Algorithms.Tests
{
    /// <summary>
    /// RIPEMD160Managed has a copy of the same implementation as RIPEMD160
    /// </summary>
    public class RIPEMD160ManagedTests : Sha384Tests
    {
        protected override HashAlgorithm Create()
        {
            return new RIPEMD160Managed();
        }
    }
}
