using System;
using System.Collections.Generic;
using System.Linq;

namespace NF.Common.PIDControl
{
    // ref: https://www.gamedev.net/articles/programming/math-and-physics/pid-control-of-physics-bodies-r3885/)
    // ref: https://blog.nullbus.net/84
    // TODO(pyoung): Add RingList<T>
    public class PIDController
    {
        float mDt = 0.01f;
        int mMaxHistory = 7;
        public float Kp { get; set; }
        public float Ki { get; set; }
        public float Kd { get; set; }
        public float KPlant { get; set; }
        List<float> mErrors = new List<float>();
        List<float> mOutputs = new List<float>();

        public float GetLastError() => mErrors.LastOrDefault();
        public float GetLastOutput() => mOutputs.LastOrDefault();

        static bool IsNearZero(float v)
        {
            return (Math.Abs(v) <= float.Epsilon);
        }

        public PIDController()
        {
            mErrors.Capacity = mMaxHistory;
            mOutputs.Capacity = mMaxHistory;

            ResetConstants();
            ResetHistory();
        }

        public void AddSample(float error)
        {
            mErrors.Add(error);
            while (mErrors.Count > mMaxHistory)
            {
                mErrors.RemoveAt(0);
            }
            CalculateNextOutput();
        }

        public void ResetHistory()
        {
            mErrors.Clear();
            mOutputs.Clear();
        }

        public void ResetConstants()
        {
            Ki = 0;
            Kd = 0;
            Kp = 0;
            KPlant = 1;
        }

        float SingleStepPredictor(float x0, float y0, float x1, float y1, float dt)
        {
            /* Given y0 = m*x0 + b
             *       y1 = m*x1 + b
             *
             *       Sovle for m, b
             *
             *       => m = (y1-y0)/(x1-x0)
             *          b = y1-m*x1
             */

            // assert(!MathUtilities::IsNearZero(x1 - x0));
            float m = (y1 - y0) / (x1 - x0);
            float b = y1 - m * x1;
            float result = m * (x1 + dt) + b;
            return result;
        }
        void CalculateNextOutput()
        {
            const int MIN_SAMPLES = 3;

            if (mErrors.Count < MIN_SAMPLES)
            {
                mOutputs.Add(0);
            }
            else
            {
                int errorSize = mErrors.Count;

                // [P]roportional
                float prop = Kp * mErrors[errorSize - 1];

                // [I]ntegral - Use Extended Simpson's Rule
                float integral = 0;
                for (int i = 1; i < errorSize - 1; i += 2)
                {
                    integral += 4 * mErrors[i];
                }
                for (int i = 2; i < errorSize - 1; i += 2)
                {
                    integral += 2 * mErrors[i];
                }
                integral += mErrors[0];
                integral += mErrors[errorSize - 1];
                integral /= (3 * mDt);
                integral *= Ki;

                // [D]erivative
                float deriv = Kd * (mErrors[errorSize - 1] - mErrors[errorSize - 2]) / mDt;

                // Total P+I+D
                float result = KPlant * (prop + integral + deriv);

                mOutputs.Add(result);
            }
        }
    }
}
