// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Contributed to .NET Foundation by Darren R. Starr - Conscia Norway AS

using Xunit;

namespace System.Security.Cryptography.Hashing.Algorithms.Tests
{
    public class RIPEMD160Tests : HashAlgorithmTest
    {
        protected override HashAlgorithm Create()
        {
            return RIPEMD160.Create();
        }

        [Fact]
        public void RIPEMD160_Empty()
        {
            Verify(
                Array.Empty<byte>(),
                "FD3C5C6D27C473E759CEB70247D2194F4F761F27");
        }

        [Fact]
        public void RIPEMD160_NistShaAll_1()
        {
            Verify(
                "abc",
                "8EB208F7E05D987A9B044A8E98C6B087F15A0BFC");
        }

        [Fact]
        public void Sha256_Fips180_MultiBlock()
        {
            VerifyMultiBlock(
                "a",
                "bc",
                "CB00753F45A35E8BB5A03D699AC65007272C32AB0EDED1631A8B605A43FF5BED8086072BA1E7CC2358BAECA134C825A7",
                "38B060A751AC96384CD9327EB1B1E36A21FDB71114BE07434C0CC7BF63F6E1DA274EDEBFE76F65FBD51AD2F14898B95B");
        }

        [Fact]
        public void Sha384_NistShaAll_2()
        {
            Verify(
                "abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu",
                "6f3fa39b6b503c384f919a49a7aa5c2c08bdfb45");
        }
    }
}