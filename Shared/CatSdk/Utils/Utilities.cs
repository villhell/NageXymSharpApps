using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NageXymSharpApps.Shared.CatSdk.Utils
{
    public static class Utilities
    {
        public class CharacterMapBuilder
        {
            public Dictionary<char, byte?> Map = new Dictionary<char, byte?>();

            public void AddRange(char start, char end, byte baseValue)
            {
                var startCode = (byte)start;
                var endCode = (byte)end;

                for (byte code = startCode; code <= endCode; ++code)
                {
                    Map[(char)code] = (byte)(code - startCode + baseValue);
                }
            }
        }

        public static class CharMappingUtils
        {
            public static CharacterMapBuilder CreateBuilder()
            {
                return new CharacterMapBuilder();
            }
        }

        private static Dictionary<char, byte?> CharToNibbleMap()
        {
            var builder = CharMappingUtils.CreateBuilder();
            builder.AddRange('0', '9', 0);
            builder.AddRange('a', 'f', 10);
            builder.AddRange('A', 'F', 10);
            return builder.Map;
        }

        public static Dictionary<char, byte?> CharToDigitMap()
        {
            var builder = CharMappingUtils.CreateBuilder();
            builder.AddRange('0', '9', 0);
            return builder.Map;
        }

        public static readonly char[] NibbleToCharMap = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        public const int DecodedBlockSize = 5;
        public const int EncodedBlockSize = 8;

        public static byte? TryParseByte(char char1, char char2)
        {
            var charMap = CharToNibbleMap();
            var nibble1 = charMap[char1];
            var nibble2 = charMap[char2];
            if (nibble1 is null || nibble2 is null)
            {
                return null;
            }

            return (byte)((nibble1.Value << 4) | nibble2.Value);
        }

        /**
         * Tries to parse a string representing an unsigned integer.
         * @param {string} str The string to parse.
         * @returns { number | undefined} The number represented by the input or undefined.
         */
        public static int? TryParseUint(string str)
        {
            if ("0" == str)
            {
                return 0;
            }
            byte? value = 0;
            foreach (char charVal in str)
            {
                Dictionary<char, byte?> charMap = CharToDigitMap();
                byte? digit;
                if (!charMap.TryGetValue(charVal, out digit) || (0 == value && 0 == digit))
                {
                    return null;
                }

                value *= 10;
                value += digit;

                if (value > Int64.MaxValue)
                {
                    return null;
                }
            }
            return value;
        }

        public static class IdGeneratorConst
        {
            public static int[] NamespaceBaseId { get; } = new int[] { 0, 0 };
            public static Regex NamePattern { get; } = new Regex("^[a-z0-9][a-z0-9-_]*$");
        }

        public static void ThrowInvalidFqn(dynamic reason, dynamic name)
        {
            throw new Exception($"fully qualified id is invalid due to {reason} ({name})");
        }

        public static string ExtractPartName(string name, int start, int size)
        {
            if (0 == size)
            {
                ThrowInvalidFqn("empty part", name);
            }
            string partName = name.Substring(start, size);
            if (!Regex.IsMatch(partName, "^[a-z0-9][a-z0-9-_]*$"))
            {
                ThrowInvalidFqn($"invalid part name [{partName}]", name);
            }
            return partName;
        }

        public static void Append<T>(List<T> path, T id)
        {
            path.Add(id);
        }

        public static int Split(string name, Action<int, int> processor)
        {
            int start = 0;
            for (int index = 0; index < name.Length; ++index)
            {
                if (name[index] == '.')
                {
                    processor(start, index - start);
                    start = index + 1;
                }
            }
            return start;
        }

        public static uint[] GenerateNamespaceId(uint[] parentId, string name)
        {
            var sha3 = new Sha3Digest(256);
            byte[] buffer = new byte[8];
            Buffer.BlockCopy(parentId, 0, buffer, 0, 8);
            sha3.BlockUpdate(buffer, 0, buffer.Length);
            sha3.BlockUpdate(Encoding.UTF8.GetBytes(name), 0, Encoding.UTF8.GetByteCount(name));
            byte[] result = new byte[sha3.GetDigestSize()];
            sha3.DoFinal(result, 0);
            uint first = BitConverter.ToUInt32(result, 0);
            uint second = BitConverter.ToUInt32(result, 4);
            return new uint[] { first, (second | 0x80000000u) };
        }

        public static void EncodeBlock(byte[] input, int inputOffset, char[] output, int outputOffset)
        {
            output[outputOffset + 0] = Alphabet[input[inputOffset + 0] >> 3];
            output[outputOffset + 1] = Alphabet[((input[inputOffset + 0] & 0x07) << 2) | (input[inputOffset + 1] >> 6)];
            output[outputOffset + 2] = Alphabet[(input[inputOffset + 1] & 0x3e) >> 1];
            output[outputOffset + 3] = Alphabet[((input[inputOffset + 1] & 0x01) << 4) | (input[inputOffset + 2] >> 4)];
            output[outputOffset + 4] = Alphabet[((input[inputOffset + 2] & 0x0f) << 1) | (input[inputOffset + 3] >> 7)];
            output[outputOffset + 5] = Alphabet[(input[inputOffset + 3] & 0x7f) >> 2];
            output[outputOffset + 6] = Alphabet[((input[inputOffset + 3] & 0x03) << 3) | (input[inputOffset + 4] >> 5)];
            output[outputOffset + 7] = Alphabet[input[inputOffset + 4] & 0x1f];
        }

        public static Dictionary<char, byte?> CharToDecodedCharMap()
        {
            var builder = new CharacterMapBuilder();
            builder.AddRange('A', 'Z', 0);
            builder.AddRange('2', '7', 26);
            return builder.Map;
        }

        public static byte? DecodeChar(char c)
        {
            var charMap = CharToDecodedCharMap();
            if (charMap.TryGetValue(c, out byte? decodedChar))
            {
                return decodedChar;
            }
            throw new Exception($"illegal base32 character {c}");
        }

        public static byte[] DecodeBlock(string input, int inputOffset)
        {
            byte?[] bytes = new byte?[EncodedBlockSize];
            for (int i = 0; i < EncodedBlockSize; ++i)
            {
                bytes[i] = DecodeChar(input[inputOffset + i]);
            }

            byte[] output = new byte[DecodedBlockSize];
            output[0] = (byte)((bytes[0] << 3) | (bytes[1] >> 2));
            output[1] = (byte)(((bytes[1] & 0x03) << 6) | (bytes[2] << 1) | (bytes[3] >> 4));
            output[2] = (byte)(((bytes[3] & 0x0f) << 4) | (bytes[4] >> 1));
            output[3] = (byte)(((bytes[4] & 0x01) << 7) | (bytes[5] << 2) | (bytes[6] >> 3));
            output[4] = (byte)(((bytes[6] & 0x07) << 5) | bytes[7]);

            return output;
        }

        public static void ParseObjectProperties(object obj, Action<string, object> parse)
        {
            if (obj is IDictionary<string, object> dictionary)
            {
                foreach (var pair in dictionary)
                {
                    if (pair.Value is IDictionary<string, object> subDict)
                    {
                        ParseObjectProperties(subDict, parse);
                    }
                    else
                    {
                        parse(pair.Key, pair.Value);
                    }
                }
            }
            else if (obj is IEnumerable<object> enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item is IDictionary<string, object> subDict)
                    {
                        ParseObjectProperties(subDict, parse);
                    }
                }
            }
        }
    }
}
