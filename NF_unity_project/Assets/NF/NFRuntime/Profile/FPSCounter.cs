namespace NFRuntime.Profile
{
    public class FPSCounter
    {
        public int FPS_Avg { get; private set; }
        public int FPS_High { get; private set; }
        public int FPS_Low { get; private set; }

        int[] mFpsBuffer;
        int mFpsBufferIndex = 0;

        public FPSCounter()
        {
            Init(60);
        }

        public void Tick(float unscaledDeltaTime)
        {
            UpdateBuffer(unscaledDeltaTime);
            CalculateFPS();
        }

        void Init(int frameRange)
        {
            if (frameRange <= 0)
            {
                frameRange = 1;
            }
            mFpsBuffer = new int[frameRange];
            mFpsBufferIndex = 0;
        }

        void UpdateBuffer(float unscaledDeltaTime)
        {
            // Time.unscaledDeltaTime

            mFpsBuffer[mFpsBufferIndex++] = (int)(1f / unscaledDeltaTime);
            if (mFpsBufferIndex >= mFpsBuffer.Length)
            {
                mFpsBufferIndex = 0;
            }
        }

        void CalculateFPS()
        {
            int sum = 0;
            int highest = 0;
            int lowest = int.MaxValue;
            for (int i = 0; i < mFpsBuffer.Length; i++)
            {
                int fps = mFpsBuffer[i];
                sum += fps;
                if (fps > highest)
                {
                    highest = fps;
                }
                if (fps < lowest)
                {
                    lowest = fps;
                }
            }
            FPS_Avg = (int)((float)sum / mFpsBuffer.Length);
            FPS_High = highest;
            FPS_Low = lowest;
        }


        static string[] mCachedStringsFrom00To99 = {
        "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
        "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
        "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
        "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
        "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
        "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
        "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
        "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
    };

        public (int, string) GetFPS_Avg()
        {
            var clampedFPS = Clamp(FPS_Avg, 0, 99);
            return (clampedFPS, mCachedStringsFrom00To99[clampedFPS]);
        }

        public (int, string) GetFPS_High()
        {
            var clampedFPS = Clamp(FPS_High, 0, 99);
            return (clampedFPS, mCachedStringsFrom00To99[clampedFPS]);
        }

        public (int fps, string fpsStr) GetFPS_Low()
        {
            var clampedFPS = Clamp(FPS_Low, 0, 99);
            return (clampedFPS, mCachedStringsFrom00To99[clampedFPS]);
        }

        public static int Clamp(int val, int min, int max)
        {
            if (val < min)
            {
                return min;
            }

            if (val > max)
            {
                return max;
            }
            return val;
        }
    }
}