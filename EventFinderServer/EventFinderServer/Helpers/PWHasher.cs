using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Helpers
{
    public static class PWHasher
    {
        private class HashDelegates 
        { 
            public delegate string HashFunction(string password, object[] paramlist);
            public delegate bool VerifyFunction(string plain, string hashed, object[] paramlist);

            public readonly HashFunction Hash;
            public readonly VerifyFunction Verify;
            public readonly object[] Params;

            public HashDelegates(HashFunction h, VerifyFunction v, object[] p)
            {
                Hash = h;
                Verify = v;
                Params = p;
            }
        }

        #region Salt & Hash Constants: 16 & 20.

        /// <summary>
        /// Size of salt.
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Size of hash.
        /// </summary>
        private const int HashSize = 20;

        #endregion
        #region Static informations

        private const string Name = "HASH";

        /// <summary>
        /// Hashing version code.
        /// </summary>
        public static string Version = "V1";

        #region V1 additional info

        public static object[] V1Params = { 10000 };

        #endregion

        #endregion

        private static Dictionary<string, HashDelegates> VersionDictionary = new Dictionary<string, HashDelegates>()
        {
            { "V1", new HashDelegates(HashV1, VerifyV1, V1Params) }
        };

        #region Hashing algorithms

        #region AlgV1
        /// <summary>
        /// Creates a Rfc2898 hash from a password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns>The hash.</returns>
        private static string HashV1(string password, object[] paramlist)
        {
            var iterations = (int)paramlist[0];

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt;
                rng.GetBytes(salt = new byte[SaltSize]);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    var hash = pbkdf2.GetBytes(HashSize);
                    var hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                    var base64Hash = Convert.ToBase64String(hashBytes);

                    return $"{Name}|{Version}${iterations}${base64Hash}";
                }
            }
        }

        /// <summary>
        /// Verifies a password against a Rfc2898 hash.
        /// </summary>
        /// <param name="plainPass">The password.</param>
        /// <param name="hashedPass">The hash.</param>
        /// <param name="iters">Number of iterations.</param>
        /// <returns>Could be verified?</returns>
        private static bool VerifyV1(string plainPass, string hashedPass, object[] paramlist)
        {
            var iterations = (int)paramlist[0];
            
            var hashBytes = Convert.FromBase64String(hashedPass);
            
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(plainPass, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                for (var i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }
                return true;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Creates a hash from a password with the selected hash function.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password)
        {
            HashDelegates HashMethods;
            if (VersionDictionary.TryGetValue(Version, out HashMethods))
                return HashMethods.Hash(password, HashMethods.Params);
            return String.Empty;
        }

        /// <summary>
        /// Checks hash version.
        /// </summary>
        /// <param name="hashString">The hash.</param>
        /// <returns>Is supported?</returns>
        public static string GetVersion(string hashString)
        {
            if (!hashString.StartsWith("HASH|"))
                return String.Empty;

            return hashString.Split('$')[0].Substring(Name.Length + 1);
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hashedPassword">The hash.</param>
        /// <returns>Could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            var version = GetVersion(hashedPassword);

            if (String.IsNullOrEmpty(version))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            var splitted = hashedPassword.Split('$');
            
            int paramsLength = splitted.Length - 2;

            if (paramsLength < 0)
                throw new ArgumentException("Wrong hashed password format.");

            object[] paramlist = new object[paramsLength];
            Array.Copy(splitted, 1, paramlist, 0, paramsLength);
            string Hash = splitted[splitted.Length - 1];

            HashDelegates HashMethods;
            if (VersionDictionary.TryGetValue(version, out HashMethods))
                return HashMethods.Verify(password, Hash, HashMethods.Params);
            else
                throw new MissingMethodException("The method does not exists in the dictionary");
        }
    }
}
