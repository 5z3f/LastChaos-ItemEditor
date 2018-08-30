using System;

namespace ItemEditor.Structure.Model
{
    internal class cDecoder
    {
        private static byte[] XorCode;

        /*
        public static uint DecodeMesh(uint value)
        {
            ulong ul = 0;
            byte ub = 17;

            ub += 13;
            ul |= Convert.ToUInt64(ub << 24);
            ub += 23;
            ul |= Convert.ToUInt64(ub << 16);
            ub += 19;
            ul |= Convert.ToUInt64(ub << 8);
            ub += 29;
            ul |= Convert.ToUInt64(ub << 0);
            ub += 5;

          //  return ???
        }
        */

        /*
        public static ulong DecodeTex(ulong value)
        {
            ulong ulChecker = 0;
            int ubChecker = 4;

            ubChecker += 17;
            ulChecker |= (ulong)ubChecker << 24;
            ubChecker += (byte)02;
            ulChecker |= (ulong)ubChecker << 16;
            ubChecker += (byte)41;
            ulChecker |= (ulong)ubChecker << 8;
            ubChecker += (byte)01;
            ulChecker |= (ulong)ubChecker << 0;
            ubChecker += (byte)6;

            return value ^ ulChecker;
        }
        */

		public static uint DecodeMesh(uint Code)
		{
			byte[] array = new byte[4];
			array = BitConverter.GetBytes(Code);

			for (int i = 0; i < 4; i++)
			{
				array[i] = (byte)(array[i] ^ XorCode[i]);
				XorCode[i] = (byte)(XorCode[i] + 89);
			}

			return BitConverter.ToUInt32(array, 0);
		}

		public static void ResetKey()
		{
			XorCode = new byte[4] { 101, 72, 53, 30 };
		}
	}
}
