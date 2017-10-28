using System;
using System.Linq;
using System.Text;

namespace RIPEMD160
{
    public class RIPEMD160
    {
        /********************************************************************\
        *
        *      FILE:     rmd160.h
        *
        *      CONTENTS: Header file for a sample C-implementation of the
        *                RIPEMD-160 hash-function. 
        *      TARGET:   any computer with an ANSI C compiler
        *
        *      AUTHOR:   Antoon Bosselaers, ESAT-COSIC
        *      DATE:     1 March 1996
        *      VERSION:  1.0
        *
        *      Copyright (c) Katholieke Universiteit Leuven
        *      1996, All Rights Reserved
        *
        \********************************************************************/

        /********************************************************************/

        /* if this line causes a compiler error, 
           adapt the defintion of dword above */
        //typedef int the_correct_size_was_chosen [sizeof (dword) == 4? 1: -1];

        /********************************************************************/
        /* macro definitions */

        /* collect four bytes into one word: */
        //#define BYTES_TO_DWORD(strptr)                    \
        //            (((dword) *((strptr)+3) << 24) | \
        //            ((dword) *((strptr)+2) << 16) | \
        //            ((dword) *((strptr)+1) <<  8) | \
        //            ((dword) *(strptr)))

        static public UInt32 ReadUInt32(byte[] buffer, long offset)
        {
            return
                (Convert.ToUInt32(buffer[3 + offset]) << 24) |
                (Convert.ToUInt32(buffer[2 + offset]) << 16) |
                (Convert.ToUInt32(buffer[1 + offset]) << 8) |
                (Convert.ToUInt32(buffer[0 + offset]));
        }


        /* ROL(x, n) cyclically rotates x over n bits to the left */
        /* x must be of an unsigned 32 bits type and 0 <= n < 32. */
        //#define ROL(x, n)        (((x) << (n)) | ((x) >> (32-(n))))
        static UInt32 RotateLeft(UInt32 value, int bits)
        {
            return (value << bits) | (value >> (32 - bits));
        }

        /* the five basic functions F(), G() and H() */
        //#define F(x, y, z)        ((x) ^ (y) ^ (z))
        static UInt32 F(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ y ^ z;
        }
        //#define G(x, y, z)        (((x) & (y)) | (~(x) & (z))) 
        static UInt32 G(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y) | (~x & z);
        }
        //#define H(x, y, z)        (((x) | ~(y)) ^ (z))
        static UInt32 H(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x | ~y) ^ z;
        }
        //#define I(x, y, z)        (((x) & (z)) | ((y) & ~(z)))
        static UInt32 I(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & z) | (y & ~z);
        }
        //#define J(x, y, z)        ((x) ^ ((y) | ~(z)))
        static UInt32 J(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ (y | ~z);
        }

        /* the ten basic operations FF() through III() */
        //#define FF(a, b, c, d, e, x, s)        {\
        //    (a) += F((b), (c), (d)) + (x);\
        //    (a) = ROL((a), (s)) + (e);\
        //    (c) = ROL((c), 10);\
        //}
        static void FF(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += F(b, c, d) + x;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        //#define GG(a, b, c, d, e, x, s)        {\
        //    (a) += G((b), (c), (d)) + (x) + 0x5a827999UL;\
        //    (a) = ROL((a), (s)) + (e);\
        //    (c) = ROL((c), 10);\
        //}
        static void GG(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += G(b, c, d) + x + (UInt32)0x5a827999;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        //#define HH(a, b, c, d, e, x, s)        {\
        //    (a) += H((b), (c), (d)) + (x) + 0x6ed9eba1UL;\
        //    (a) = ROL((a), (s)) + (e);\
        //    (c) = ROL((c), 10);\
        //}
        static void HH(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += H(b, c, d) + x + (UInt32)0x6ed9eba1;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }
        //#define II(a, b, c, d, e, x, s)        {\
        //    (a) += I((b), (c), (d)) + (x) + 0x8f1bbcdcUL;\
        //    (a) = ROL((a), (s)) + (e);\
        //    (c) = ROL((c), 10);\
        //}
        static void II(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += I(b, c, d) + x + (UInt32)0x8f1bbcdc;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }
        //#define JJ(a, b, c, d, e, x, s)        {\
        //    (a) += J((b), (c), (d)) + (x) + 0xa953fd4eUL;\
        //    (a) = ROL((a), (s)) + (e);\
        //    (c) = ROL((c), 10);\
        //}
        static void JJ(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += J(b, c, d) + x + (UInt32)0xa953fd4e;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }
        //#define FFF(a, b, c, d, e, x, s)        {\
        //    (a) += F((b), (c), (d)) + (x);\
        //    (a) = ROL((a), (s)) + (e);\
        //    (c) = ROL((c), 10);\
        //}
        static void FFF(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += F(b, c, d) + x;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }
        // #define GGG(a, b, c, d, e, x, s)        {\
        //     (a) += G((b), (c), (d)) + (x) + 0x7a6d76e9UL;\
        //     (a) = ROL((a), (s)) + (e);\
        //     (c) = ROL((c), 10);\
        // }
        static void GGG(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += G(b, c, d) + x + (UInt32)0x7a6d76e9;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }
        // #define HHH(a, b, c, d, e, x, s)        {\
        //     (a) += H((b), (c), (d)) + (x) + 0x6d703ef3UL;\
        //     (a) = ROL((a), (s)) + (e);\
        //     (c) = ROL((c), 10);\
        // }
        static void HHH(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += H(b, c, d) + x + (UInt32)0x6d703ef3;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }
        // #define III(a, b, c, d, e, x, s)        {\
        //     (a) += I((b), (c), (d)) + (x) + 0x5c4dd124UL;\
        //     (a) = ROL((a), (s)) + (e);\
        //     (c) = ROL((c), 10);\
        // }
        static void III(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += I(b, c, d) + x + (UInt32)0x5c4dd124;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }
        // #define JJJ(a, b, c, d, e, x, s)        {\
        //     (a) += J((b), (c), (d)) + (x) + 0x50a28be6UL;\
        //     (a) = ROL((a), (s)) + (e);\
        //     (c) = ROL((c), 10);\
        // }
        static void JJJ(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += J(b, c, d) + x + (UInt32)0x50a28be6;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        /*
         *  initializes MDbuffer to "magic constants"
         */
        static public void MDinit(ref UInt32[] MDbuf)
        {
            MDbuf[0] = (UInt32)0x67452301;
            MDbuf[1] = (UInt32)0xefcdab89;
            MDbuf[2] = (UInt32)0x98badcfe;
            MDbuf[3] = (UInt32)0x10325476;
            MDbuf[4] = (UInt32)0xc3d2e1f0;
        }

        /*
         *  the compression function.
         *  transforms MDbuf using message bytes X[0] through X[15]
         */
        static public void compress(ref UInt32[] MDbuf, UInt32[] X)
        {
            UInt32 aa = MDbuf[0];
            UInt32 bb = MDbuf[1];
            UInt32 cc = MDbuf[2];
            UInt32 dd = MDbuf[3];
            UInt32 ee = MDbuf[4];
            UInt32 aaa = MDbuf[0];
            UInt32 bbb = MDbuf[1];
            UInt32 ccc = MDbuf[2];
            UInt32 ddd = MDbuf[3];
            UInt32 eee = MDbuf[4];

            /* round 1 */
            FF(ref aa, bb, ref cc, dd, ee, X[0], 11);
            FF(ref ee, aa, ref bb, cc, dd, X[1], 14);
            FF(ref dd, ee, ref aa, bb, cc, X[2], 15);
            FF(ref cc, dd, ref ee, aa, bb, X[3], 12);
            FF(ref bb, cc, ref dd, ee, aa, X[4], 5);
            FF(ref aa, bb, ref cc, dd, ee, X[5], 8);
            FF(ref ee, aa, ref bb, cc, dd, X[6], 7);
            FF(ref dd, ee, ref aa, bb, cc, X[7], 9);
            FF(ref cc, dd, ref ee, aa, bb, X[8], 11);
            FF(ref bb, cc, ref dd, ee, aa, X[9], 13);
            FF(ref aa, bb, ref cc, dd, ee, X[10], 14);
            FF(ref ee, aa, ref bb, cc, dd, X[11], 15);
            FF(ref dd, ee, ref aa, bb, cc, X[12], 6);
            FF(ref cc, dd, ref ee, aa, bb, X[13], 7);
            FF(ref bb, cc, ref dd, ee, aa, X[14], 9);
            FF(ref aa, bb, ref cc, dd, ee, X[15], 8);

            /* round 2 */
            GG(ref ee, aa, ref bb, cc, dd, X[7], 7);
            GG(ref dd, ee, ref aa, bb, cc, X[4], 6);
            GG(ref cc, dd, ref ee, aa, bb, X[13], 8);
            GG(ref bb, cc, ref dd, ee, aa, X[1], 13);
            GG(ref aa, bb, ref cc, dd, ee, X[10], 11);
            GG(ref ee, aa, ref bb, cc, dd, X[6], 9);
            GG(ref dd, ee, ref aa, bb, cc, X[15], 7);
            GG(ref cc, dd, ref ee, aa, bb, X[3], 15);
            GG(ref bb, cc, ref dd, ee, aa, X[12], 7);
            GG(ref aa, bb, ref cc, dd, ee, X[0], 12);
            GG(ref ee, aa, ref bb, cc, dd, X[9], 15);
            GG(ref dd, ee, ref aa, bb, cc, X[5], 9);
            GG(ref cc, dd, ref ee, aa, bb, X[2], 11);
            GG(ref bb, cc, ref dd, ee, aa, X[14], 7);
            GG(ref aa, bb, ref cc, dd, ee, X[11], 13);
            GG(ref ee, aa, ref bb, cc, dd, X[8], 12);

            /* round 3 */
            HH(ref dd, ee, ref aa, bb, cc, X[3], 11);
            HH(ref cc, dd, ref ee, aa, bb, X[10], 13);
            HH(ref bb, cc, ref dd, ee, aa, X[14], 6);
            HH(ref aa, bb, ref cc, dd, ee, X[4], 7);
            HH(ref ee, aa, ref bb, cc, dd, X[9], 14);
            HH(ref dd, ee, ref aa, bb, cc, X[15], 9);
            HH(ref cc, dd, ref ee, aa, bb, X[8], 13);
            HH(ref bb, cc, ref dd, ee, aa, X[1], 15);
            HH(ref aa, bb, ref cc, dd, ee, X[2], 14);
            HH(ref ee, aa, ref bb, cc, dd, X[7], 8);
            HH(ref dd, ee, ref aa, bb, cc, X[0], 13);
            HH(ref cc, dd, ref ee, aa, bb, X[6], 6);
            HH(ref bb, cc, ref dd, ee, aa, X[13], 5);
            HH(ref aa, bb, ref cc, dd, ee, X[11], 12);
            HH(ref ee, aa, ref bb, cc, dd, X[5], 7);
            HH(ref dd, ee, ref aa, bb, cc, X[12], 5);

            /* round 4 */
            II(ref cc, dd, ref ee, aa, bb, X[1], 11);
            II(ref bb, cc, ref dd, ee, aa, X[9], 12);
            II(ref aa, bb, ref cc, dd, ee, X[11], 14);
            II(ref ee, aa, ref bb, cc, dd, X[10], 15);
            II(ref dd, ee, ref aa, bb, cc, X[0], 14);
            II(ref cc, dd, ref ee, aa, bb, X[8], 15);
            II(ref bb, cc, ref dd, ee, aa, X[12], 9);
            II(ref aa, bb, ref cc, dd, ee, X[4], 8);
            II(ref ee, aa, ref bb, cc, dd, X[13], 9);
            II(ref dd, ee, ref aa, bb, cc, X[3], 14);
            II(ref cc, dd, ref ee, aa, bb, X[7], 5);
            II(ref bb, cc, ref dd, ee, aa, X[15], 6);
            II(ref aa, bb, ref cc, dd, ee, X[14], 8);
            II(ref ee, aa, ref bb, cc, dd, X[5], 6);
            II(ref dd, ee, ref aa, bb, cc, X[6], 5);
            II(ref cc, dd, ref ee, aa, bb, X[2], 12);

            /* round 5 */
            JJ(ref bb, cc, ref dd, ee, aa, X[4], 9);
            JJ(ref aa, bb, ref cc, dd, ee, X[0], 15);
            JJ(ref ee, aa, ref bb, cc, dd, X[5], 5);
            JJ(ref dd, ee, ref aa, bb, cc, X[9], 11);
            JJ(ref cc, dd, ref ee, aa, bb, X[7], 6);
            JJ(ref bb, cc, ref dd, ee, aa, X[12], 8);
            JJ(ref aa, bb, ref cc, dd, ee, X[2], 13);
            JJ(ref ee, aa, ref bb, cc, dd, X[10], 12);
            JJ(ref dd, ee, ref aa, bb, cc, X[14], 5);
            JJ(ref cc, dd, ref ee, aa, bb, X[1], 12);
            JJ(ref bb, cc, ref dd, ee, aa, X[3], 13);
            JJ(ref aa, bb, ref cc, dd, ee, X[8], 14);
            JJ(ref ee, aa, ref bb, cc, dd, X[11], 11);
            JJ(ref dd, ee, ref aa, bb, cc, X[6], 8);
            JJ(ref cc, dd, ref ee, aa, bb, X[15], 5);
            JJ(ref bb, cc, ref dd, ee, aa, X[13], 6);

            /* parallel round 1 */
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[5], 8);
            JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[14], 9);
            JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[7], 9);
            JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[0], 11);
            JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[9], 13);
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[2], 15);
            JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[11], 15);
            JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[4], 5);
            JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[13], 7);
            JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[6], 7);
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[15], 8);
            JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[8], 11);
            JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[1], 14);
            JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[10], 14);
            JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[3], 12);
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[12], 6);

            /* parallel round 2 */
            III(ref eee, aaa, ref bbb, ccc, ddd, X[6], 9);
            III(ref ddd, eee, ref aaa, bbb, ccc, X[11], 13);
            III(ref ccc, ddd, ref eee, aaa, bbb, X[3], 15);
            III(ref bbb, ccc, ref ddd, eee, aaa, X[7], 7);
            III(ref aaa, bbb, ref ccc, ddd, eee, X[0], 12);
            III(ref eee, aaa, ref bbb, ccc, ddd, X[13], 8);
            III(ref ddd, eee, ref aaa, bbb, ccc, X[5], 9);
            III(ref ccc, ddd, ref eee, aaa, bbb, X[10], 11);
            III(ref bbb, ccc, ref ddd, eee, aaa, X[14], 7);
            III(ref aaa, bbb, ref ccc, ddd, eee, X[15], 7);
            III(ref eee, aaa, ref bbb, ccc, ddd, X[8], 12);
            III(ref ddd, eee, ref aaa, bbb, ccc, X[12], 7);
            III(ref ccc, ddd, ref eee, aaa, bbb, X[4], 6);
            III(ref bbb, ccc, ref ddd, eee, aaa, X[9], 15);
            III(ref aaa, bbb, ref ccc, ddd, eee, X[1], 13);
            III(ref eee, aaa, ref bbb, ccc, ddd, X[2], 11);

            /* parallel round 3 */
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[15], 9);
            HHH(ref ccc, ddd, ref eee, aaa, bbb, X[5], 7);
            HHH(ref bbb, ccc, ref ddd, eee, aaa, X[1], 15);
            HHH(ref aaa, bbb, ref ccc, ddd, eee, X[3], 11);
            HHH(ref eee, aaa, ref bbb, ccc, ddd, X[7], 8);
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[14], 6);
            HHH(ref ccc, ddd, ref eee, aaa, bbb, X[6], 6);
            HHH(ref bbb, ccc, ref ddd, eee, aaa, X[9], 14);
            HHH(ref aaa, bbb, ref ccc, ddd, eee, X[11], 12);
            HHH(ref eee, aaa, ref bbb, ccc, ddd, X[8], 13);
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[12], 5);
            HHH(ref ccc, ddd, ref eee, aaa, bbb, X[2], 14);
            HHH(ref bbb, ccc, ref ddd, eee, aaa, X[10], 13);
            HHH(ref aaa, bbb, ref ccc, ddd, eee, X[0], 13);
            HHH(ref eee, aaa, ref bbb, ccc, ddd, X[4], 7);
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[13], 5);

            /* parallel round 4 */
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[8], 15);
            GGG(ref bbb, ccc, ref ddd, eee, aaa, X[6], 5);
            GGG(ref aaa, bbb, ref ccc, ddd, eee, X[4], 8);
            GGG(ref eee, aaa, ref bbb, ccc, ddd, X[1], 11);
            GGG(ref ddd, eee, ref aaa, bbb, ccc, X[3], 14);
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[11], 14);
            GGG(ref bbb, ccc, ref ddd, eee, aaa, X[15], 6);
            GGG(ref aaa, bbb, ref ccc, ddd, eee, X[0], 14);
            GGG(ref eee, aaa, ref bbb, ccc, ddd, X[5], 6);
            GGG(ref ddd, eee, ref aaa, bbb, ccc, X[12], 9);
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[2], 12);
            GGG(ref bbb, ccc, ref ddd, eee, aaa, X[13], 9);
            GGG(ref aaa, bbb, ref ccc, ddd, eee, X[9], 12);
            GGG(ref eee, aaa, ref bbb, ccc, ddd, X[7], 5);
            GGG(ref ddd, eee, ref aaa, bbb, ccc, X[10], 15);
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[14], 8);

            /* parallel round 5 */
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[12], 8);
            FFF(ref aaa, bbb, ref ccc, ddd, eee, X[15], 5);
            FFF(ref eee, aaa, ref bbb, ccc, ddd, X[10], 12);
            FFF(ref ddd, eee, ref aaa, bbb, ccc, X[4], 9);
            FFF(ref ccc, ddd, ref eee, aaa, bbb, X[1], 12);
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[5], 5);
            FFF(ref aaa, bbb, ref ccc, ddd, eee, X[8], 14);
            FFF(ref eee, aaa, ref bbb, ccc, ddd, X[7], 6);
            FFF(ref ddd, eee, ref aaa, bbb, ccc, X[6], 8);
            FFF(ref ccc, ddd, ref eee, aaa, bbb, X[2], 13);
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[13], 6);
            FFF(ref aaa, bbb, ref ccc, ddd, eee, X[14], 5);
            FFF(ref eee, aaa, ref bbb, ccc, ddd, X[0], 15);
            FFF(ref ddd, eee, ref aaa, bbb, ccc, X[3], 13);
            FFF(ref ccc, ddd, ref eee, aaa, bbb, X[9], 11);
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[11], 11);

            /* combine results */
            ddd += cc + MDbuf[1];               /* final result for MDbuf[0] */
            MDbuf[1] = MDbuf[2] + dd + eee;
            MDbuf[2] = MDbuf[3] + ee + aaa;
            MDbuf[3] = MDbuf[4] + aa + bbb;
            MDbuf[4] = MDbuf[0] + bb + ccc;
            MDbuf[0] = ddd;
        }

        /*
         *  puts bytes from strptr into X and pad out; appends length 
         *  and finally, compresses the last block(s)
         *  note: length in bits == 8 * (lswlen + 2^32 mswlen).
         *  note: there are (lswlen mod 64) bytes left in strptr.
         */
        static public void MDfinish(ref UInt32[] MDbuf, byte[] strptr, long index, UInt32 lswlen, UInt32 mswlen)
        {
            //UInt32 i;                                 /* counter       */
            var X = Enumerable.Repeat((UInt32)0, 16).ToArray();                             /* message words */


            /* put bytes from strptr into X */
            for (var i = 0; i < (lswlen & 63); i++)
            {
                /* byte i goes into word X[i div 4] at pos.  8*(i mod 4)  */
                X[i >> 2] ^= Convert.ToUInt32(strptr[i + index]) << (8 * (i & 3));
            }

            /* append the bit m_n == 1 */
            X[(lswlen >> 2) & 15] ^= (UInt32)1 << Convert.ToInt32(8 * (lswlen & 3) + 7);

            if ((lswlen & 63) > 55)
            {
                /* length goes to next block */
                compress(ref MDbuf, X);
                X = Enumerable.Repeat((UInt32)0, 16).ToArray();
            }

            /* append length in bits*/
            X[14] = lswlen << 3;
            X[15] = (lswlen >> 29) | (mswlen << 3);
            compress(ref MDbuf, X);
        }
    }

    class TestRMD
    {
        static int RMDsize = 160;

        /*
        * returns RMD(message)
        * message should be a string terminated by '\0'
        */
        public static byte[] RMD(byte[] message)
        {
            var MDbuf = new UInt32[RMDsize / 32];   /* contains (A, B, C, D(, E))   */
            var hashcode = new byte[RMDsize / 8]; /* for final hash-value         */
            var X = new UInt32[16];               /* current 16-word chunk        */
            UInt32 i;                   /* counter                      */
            UInt32 length;              /* length in bytes of message   */
            UInt32 nbytes;              /* # of bytes not yet processed */

            /* initialize */
            RIPEMD160.MDinit(ref MDbuf);
            length = Convert.ToUInt32(message.Length);

            var index = 0;
            /* process message in 16-word chunks */
            for (nbytes = length; nbytes > 63; nbytes -= 64)
            {
                for (i = 0; i < 16; i++)
                {
                    X[i] = RIPEMD160.ReadUInt32(message, index);
                    index += 4;
                }
                RIPEMD160.compress(ref MDbuf, X);
            }                                    /* length mod 64 bytes left */

            /* finish: */
            RIPEMD160.MDfinish(ref MDbuf, message, index, length, 0);

            for (i = 0; i < RMDsize / 8; i += 4)
            {
                hashcode[i] = Convert.ToByte(MDbuf[i >> 2] & 0xFF);         /* implicit cast to byte  */
                hashcode[i + 1] = Convert.ToByte((MDbuf[i >> 2] >> 8) & 0xFF);  /*  extracts the 8 least  */
                hashcode[i + 2] = Convert.ToByte((MDbuf[i >> 2] >> 16) & 0xFF);  /*  significant bits.     */
                hashcode[i + 3] = Convert.ToByte((MDbuf[i >> 2] >> 24) & 0xFF);
            }

            return hashcode;
        }

        public static void RMDstring(string message, string print, string expectedValue)
        {
            var hashcode = RMD(Encoding.ASCII.GetBytes(message));
            var hashText = string.Join("", hashcode.Select(x => x.ToString("X2")).ToList());
            Console.WriteLine("* message: " + message);
            Console.WriteLine("  hashcode: " + print + " - " + hashText + " - " + (hashText.ToLower() == expectedValue.ToLower() ? "match" : "fail"));
        }

        /*
         *   standard test suite
         */
        public static void testsuite()
        {
            Console.WriteLine("RIPEMD-" + RMDsize.ToString() + " test suite results (ASCII):");

            RMDstring("", "\"\" (empty string)", "9c1185a5c5e9fc54612808977ee8f548b2258d31");
            RMDstring("a", "\"a\"", "0bdc9d2d256b3ee9daae347be6f4dc835a467ffe");
            RMDstring("abc", "\"abc\"", "8eb208f7e05d987a9b044a8e98c6b087f15a0bfc");
            RMDstring("message digest", "\"message digest\"", "5d0689ef49d2fae572b881b123a85ffa21595f36");
            RMDstring("abcdefghijklmnopqrstuvwxyz", "\"abcdefghijklmnopqrstuvwxyz\"", "f71c27109c692c1b56bbdceb5b9d2865b3708dbc");
            RMDstring("abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq",
                      "\"abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq\"", "12a053384a9c0c88e405a06c27dcf49ada62eb2b");
            RMDstring("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789",
                      "\"A...Za...z0...9\"", "b0e20b6e3116640286ed3a87a5713079b21f5189");
            RMDstring("12345678901234567890123456789012345678901234567890123456789012345678901234567890",
                      "8 times \"1234567890\"", "9b752e45573d4b39f4dbd3323cab82bf63326bfb");
            RMDstring("The quick brown fox jumped over the lazy dog.", "A lazy dog", "ec457d0a974c48d5685a7efa03d137dc8bbde7e3");
            RMDstring("The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.The quick brown fox jumped over the lazy dog.", "lots of lazy dogs", "c6c679e161a8b51a83329c74dc6f36680fccca1a");

            var millionTimesResult = RMD(Enumerable.Repeat((byte)97, 1000000).ToArray());
            var hashText = string.Join("", millionTimesResult.Select(x => x.ToString("X2")).ToList());
            Console.WriteLine("* message: " + "lots of 'a's");
            Console.WriteLine("  hashcode: " + "a million times 'a'" + " - " + hashText + " - " + (hashText.ToLower() == "52783243c1697bdbe16d37f97f68f08325dc1528" ? "match" : "fail"));
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            TestRMD.testsuite();
        }
    }
}
