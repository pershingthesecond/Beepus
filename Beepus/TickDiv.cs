﻿using System;

namespace Beepus
{
    public class TickDiv
    {
        public int microsecondsPerQuarterNote = 500;

        private bool metricalTiming;

        private ushort ppqn; // Used with metrical timing
        private float fps; // Used with timecode
        private ushort ticksPerFrame; // Used with timecode

        public TickDiv(byte topByte, byte lowByte)
        {
            byte[] data;

            if (BitConverter.IsLittleEndian)
            {
                data = new byte[] { lowByte, topByte };
            }
            else
            {
                data = new byte[] { topByte, lowByte };
            }

            ushort tickdiv = BitConverter.ToUInt16(data, 0);

            // Check timing scheme
            metricalTiming = ((tickdiv & 0x8000) == 0) ? true : false;
            Console.WriteLine((metricalTiming) ? "This file uses metrical timing" : "This file uses timecode");

            // Set values...
            if (metricalTiming)     // ... for metrical timing
            {
                ppqn = tickdiv;
                Console.WriteLine($"Pulses per quarter note: {ppqn}");
            }
            else                    // ... for timecode
            {
                throw new NotImplementedException("Timecode is currently not implemented");
            }
        }

        public int GetTickLenght()
        {
            return microsecondsPerQuarterNote / ppqn;
        }

        public int GetDuration(int ticks)
        {
            return ticks * GetTickLenght();
        }
    }
}
