using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using gv = Globals.Variables;

class Frequency_capture
{
    //public static void Frequency_wave(uint NumChannels, uint Samples, uint SampleRate, int[] left, int[] right)
    public static void Frequency_wave(RIFF_header obj) // Read only the corresponding object 
    {
        // Check the frequency of the wave signal
        // Allocate 1/resolution of the total size of the wave file
        float[,] time_channel = new float[obj.NumChannels, (obj.Samples/obj.NumChannels) / gv.resolution];
        int[,] time_index = new int[obj.NumChannels, 1];
        int pos = 0;
        int i = 0;

        if (obj.BitsPerSample != 8)
        {
            while (i != obj.NumChannels)
            {
                for (i = 0; i < obj.NumChannels; i++)
                {
                    pos = 0;
                    for (int j = 1; j < (obj.Samples / obj.NumChannels) / gv.resolution; j++) // Dismiss first position of the array to avoid reading error
                    {
                        if (obj.BitsPerSample != 8)
                        {
                            if (obj.channel_int[i, j] >= 0 && obj.channel_int[i, j - 1] < 0) // Start positive phase of the wave
                            {
                                if (obj.channel_int[i, time_index[i, 0]] == 0)
                                {
                                    time_channel[i, time_index[i, 0]] = j / (float)obj.SampleRate;
                                }
                                else
                                {
                                    time_channel[i, time_index[i, 0]] = ((((j / (float)obj.SampleRate)) + (j - 1) / (float)obj.SampleRate)) / 2;
                                }
                                time_index[i, 0] = pos++;
                            }
                            else if (obj.channel_int[i, j] <= 0 && obj.channel_int[i, j - 1] > 0) // Start negative phase of the wave
                            {
                                if (obj.channel_int[i, j] == 0)
                                {
                                    time_channel[i, time_index[i, 0]] = j / (float)obj.SampleRate;
                                }
                                else
                                {
                                    time_channel[i, time_index[i, 0]] = ((((j / (float)obj.SampleRate)) + (j - 1) / (float)obj.SampleRate)) / 2;
                                }
                                time_index[i, 0] = pos++;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            while (i != obj.NumChannels)
            {
                for (i = 0; i < obj.NumChannels; i++)
                {
                    pos = 0;
                    for (int j = 1; j < (obj.Samples / obj.NumChannels) / gv.resolution; j++) // Dismiss first position of the array to avoid reading error
                    {
                        if (obj.channel_int[i, j] >= 0x7F && obj.channel_int[i, j - 1] < 0x7F) // Start positive phase of the wave
                        {
                            if (obj.channel_int[i, time_index[i, 0]] == 0)
                            {
                                time_channel[i, time_index[i, 0]] = j / (float)obj.SampleRate;
                            }
                            else
                            {
                                time_channel[i, time_index[i, 0]] = ((((j / (float)obj.SampleRate)) + (j - 1) / (float)obj.SampleRate)) / 2;
                            }
                            time_index[i, 0] = pos++;
                        }
                        else if (obj.channel_int[i, j] <= 0x7F && obj.channel_int[i, j - 1] > 0x7F) // Start negative phase of the wave
                        {
                            if (obj.channel_int[i, j] == 0)
                            {
                                time_channel[i, time_index[i, 0]] = j / (float)obj.SampleRate;
                            }
                            else
                            {
                                time_channel[i, time_index[i, 0]] = ((((j / (float)obj.SampleRate)) + (j - 1) / (float)obj.SampleRate)) / 2;
                            }
                            time_index[i, 0] = pos++;
                        }
                    }
                }
            }
        }

        float total_time = 0.0f;
        float frequency = 0.0f;

        for (i = 0; i < obj.NumChannels; i++)
        {
            total_time = 0;
            frequency = 0;

            total_time = time_channel[i, time_index[i, 0] - 2] - time_channel[i, 0]; // -2 = -1 (decrement because of the last increment) and -1 (decrement to respect the start/end wave form) 
            frequency = (1 / total_time) * ((time_index[i, 0] - 1) / 2);

            // Uncomment the following lines to print all the information at the debug output window
            Debug.WriteLine("Channel number: " + i);
            Debug.WriteLine("Time channel: " + total_time + " s");
            Debug.WriteLine("Frequency left channel: " + frequency + " Hz");
            Debug.WriteLine("");

        }
    }
}
