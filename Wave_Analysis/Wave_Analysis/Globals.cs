using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Globals
{
    public class Variables
    {
        /// Resolution
        /// Is used to read the initial part of the wave file 
        /// Example: 4 -> Reads 1/4 of the file to estimate the wave frequency
        /// It works on the Frequency_capture.Frequency_wave function
        public const uint resolution = 4;

        //public const string filename = "C:\\Users\\Miguel Dias\\Documents\\Visual Studio 2013\\Projects\\WAVE_Analysis\\test_sin.wav";
        // or
        //public const string filename = @"C:\Users\Miguel Dias\Documents\Visual Studio 2013\Projects\WAVE_Analysis\sinwave_24bit.wav";
        //public const string filename = @"C:\Users\Miguel Dias\Documents\Visual Studio 2013\Projects\WAVE_Analysis\sinwave_24bit_2.wav";
        public const string filename = @"C:\Users\Miguel Dias\Documents\Visual Studio 2013\Projects\WAVE_Analysis\sinwave_24bit_3.wav";
        //public const string filename = @"C:\Users\Miguel Dias\Documents\Visual Studio 2013\Projects\WAVE_Analysis\sinwave_8bit.wav";
        //public const string filename = @"C:\Users\Miguel Dias\Documents\Visual Studio 2013\Projects\WAVE_Analysis\test_sin.wav";
        //public const string filename2 = @"C:\Users\Miguel Dias\Documents\Visual Studio 2013\Projects\WAVE_Analysis\audiocheck.net_sin_60Hz_0dBFS_10s.wav";
        public const string filename3 = @"C:\Users\Miguel Dias\Documents\Visual Studio 2013\Projects\WAVE_Analysis\audiocheck.net_sin_250Hz_0dBFS_10s.wav";
    }
}
