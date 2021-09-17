using System;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;

// random number generator library

namespace RNGGen
{
    public class RNGSys
    {
        public static int seedCount = 0;

        // randm generator engines
        public static readonly ThreadLocal<Random> random_gen = new ThreadLocal<Random>(() => new Random(GenerateSeed()));  
        private static RNGCryptoServiceProvider crypt_random_gen = new RNGCryptoServiceProvider();


        // seed maker for the normal random one
        private static int GenerateSeed(){
            // note the usage of Interlocked, remember that in a shared context we can't just "seedCount++"
            int seedend = (int) ((DateTime.Now.Ticks << 4) + (Interlocked.Increment(ref seedCount)));

            int middleseed = (int)DateTime.Now.Ticks + (int)DateTime.Now.Millisecond + (int)DateTime.Now.Day + (int)Environment.TickCount;

            int baseseed = Guid.NewGuid().GetHashCode() + new DateTime().Millisecond;

            int readyseed = baseseed + middleseed + seedend;
            return readyseed;
        }


        // public generator
        public static int RandomNumber(int min, int max)  
        {  
            if(max < min)
            {
                return 0;
            }

            int res = random_gen.Value.Next(min, max); 
            return res;
        }  


        // public cryptography generator
        public static int CryptRandomNumber(int min, int max)
        {
            if(max < min)
            {
                return 0;
            }

            int res = RandomIntFromCryptRNG(min, max);
            return res;
        }

        /* public random random generator, more random because a random number generator randomizes which path to go, 
        cryptography or normal, not good for cryptography because most likely it will land on the normal one */
        public static int RandomRandomNumber(int min, int max)
        {
            int go = RandomNumber(2, 5);
            int res = 0;

            // 1 and 2 for cryptography, 3 and 4 for normal
            if(go == 2 || go == 1)
            {
                res = CryptRandomNumber(min, max);
            }
            else{
                res = RandomNumber(min, max);
            }

            return res;
        }

        // public next generator
        public static int NextRandomNumber(int max)
        {
            if(max < 0)
            {
                return 0;
            }
            int res = random_gen.Value.Next(max);
            return res;
        }

        public static int NextRandomRandomNumber(int max)
        {
            return RandomRandomNumber(0, max);
        }

        // cryptography generator
        static int RandomIntFromCryptRNG(int min, int max)
        {
            // Generate four random bytes
            byte[] four_bytes = new byte[4];
            crypt_random_gen.GetBytes(four_bytes);

            // Convert the bytes to a UInt32
            UInt32 scale = BitConverter.ToUInt32(four_bytes, 0);

            // And use that to pick a random number >= min and < max
            return (int)(min + (max - min) * (scale / (uint.MaxValue + 1.0)));
        }

    }
}