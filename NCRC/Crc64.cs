using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Moonpyk.Crc {
    public class Crc64 : HashAlgorithm {
        public const UInt64 DefaultPolynomial = 0XC96C5795D7870F42L;
        public const UInt64 DefaultSeed       = 0XFFFFFFFFFFFFFFFF;

        private static UInt64[] _defaultTable;
        private readonly UInt64 _seed;
        private readonly UInt64[] _table;
        private UInt64 _hash;

        public Crc64() {
            _table = InitializeTable(DefaultPolynomial);
            _seed = DefaultSeed;
            Initialize();
        }

        public override int HashSize {
            get {
                return 64;
            }
        }

        private static UInt64[] InitializeTable(UInt64 polynomial) {
            if (polynomial == DefaultPolynomial && _defaultTable != null) {
                return _defaultTable;
            }

            var createTable = new UInt64[256];

            for (var i = 0 ; i < 256 ; i++) {
                var entry = (UInt64)i;

                for (var j = 0 ; j < 8 ; j++)
                    if ((entry & 1) == 1) {
                        entry = (entry >> 1) ^ polynomial;
                    } else {
                        entry = entry >> 1;
                    }

                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial) {
                _defaultTable = createTable;
            }

            return createTable;
        }

        public override sealed void Initialize() {
            throw new NotImplementedException();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize) {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal() {
            throw new NotImplementedException();
        }

        public static UInt64 CalculateHash(UInt64[] table, UInt64 seed, IList<byte> buffer, int start, int size) {
            var crc = seed;

            for (var i = start ; i < size ; i++) {
                unchecked {
                    crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                }
            }

            return crc;
        }

        public static byte[] UInt64ToBigEndianBytes(UInt64 x) {
            return new[] {
                (byte)((x >> 56) & 0xff),
                (byte)((x >> 48) & 0xff),
                (byte)((x >> 40) & 0xff),
                (byte)((x >> 32) & 0xff),
                (byte)((x >> 24) & 0xff),
                (byte)((x >> 16) & 0xff),
                (byte)((x >> 8) & 0xff),
                (byte)(x & 0xff)
            };
        }
    }
}
