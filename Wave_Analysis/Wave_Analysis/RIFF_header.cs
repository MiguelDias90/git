using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

// Links to understand the RIFF header
// http://soundfile.sapp.org/doc/WaveFormat/
// http://unusedino.de/ec64/technical/formats/wav.html

class RIFF_header
{
    // RIFF variables of the Wave file
    public string ChunkID { get; set; }
    public uint ChunkSize { get; set; }
    public string Format { get; set; }
    public string Subchunk1ID { get; set; }
    public uint Subchunk1Size { get; set; }
    public uint AudioFormat { get; set; }
    public uint NumChannels { get; set; }
    public uint SampleRate { get; set; }
    public uint Byterate { get; set; }
    public uint BlockAlign { get; set; }
    public uint BitsPerSample { get; set; }
    public string Subchunk2ID { get; set; }
    public uint Subchunk2Size { get; set; }

    // Channels of the Wave file
    //public byte[,] channel_byte { get; set; } // 8-bit audio Wave file
    public int[,] channel_int { get; set; }

    // Number of samples of the Wave file
    public uint Samples { get; set; }


    /*public RIFF_header()
    {
        NumChannels = this.NumChannels;
        SampleRate = this.SampleRate;
        Samples = this.Samples;
        left = this.left;
        right = this.right;
    }*/

    public void RIFF_header_reader(string filename)
    {
        // Input Wave file
        byte[] wav =            File.ReadAllBytes(filename);
        int i =                 0;
    
        //READ RIFF HEADER
        /*for (i = 0; i < 44; i++)
        {
            Debug.WriteLine("{0} 0x{1:X}", i, wav[i]);
        }*/

        /// ChunkID
        /// Chunk ID  "RIFF"  in  ASCII
        /// ENDIANNESS: Big-endian
        for(i = 0; i < 4; i++)
        { ChunkID += (char)wav[i]; }

        /// ChunkSize
        /// Total size of the Chunk in bytes, minus 8 bytes (these first 8 bytes are not included in the overall size)
        /// ENDIANNESS: Little-endian
        ChunkSize = (uint)(wav[4] + (wav[5] << 8) + (wav[6] << 16) + (wav[7] << 24));
        //Debug.WriteLine("{0:X}", ChunkSize);

        /// Format
        /// Format of the file "WAVE".
        /// ENDIANNESS: Big-endian
        for (i = 8; i < 12; i++)
        { Format += (char)wav[i]; }

        /// Subchunk1ID
        /// Contains the letters "fmt ".
        /// ENDIANNESS: Big-endian
        for (i = 12; i < 16; i++)
        { Subchunk1ID += (char)wav[i]; }

        /// Subchunk1Size
        /// Size of the rest of the Subchunk which follows this number in bytes.
        /// ENDIANNESS: Little-endian
        Subchunk1Size = (uint)wav[16] + ((uint)wav[17] << 8) + ((uint)wav[18] << 16) + ((uint)wav[19] << 24);

        /// AudioFormat
        /// Indicates the audio format.
        AudioFormat = (uint)wav[20];

        /// NumChannels
        /// Number of channels.
        /// ENDIANNESS: Big-endian
        NumChannels = (uint)(wav[22] + wav[23]);

        /// SampleRate
        /// Sample rate per second.
        /// ENDIANNESS: Little-endian
        SampleRate = (uint)(wav[24] + (wav[25] << 8) + (wav[26] << 16) + (wav[27] << 24));

        /// Byterate
        /// Byte rate per second.
        /// == SampleRate * NumChannels * BitsPerSample/8
        /// ENDIANNESS: Little-endian
        Byterate = (uint)(wav[28] + (wav[29] << 8) + (wav[30] << 16) + (wav[31] << 24));

        /// BlockAlign
        /// The number of bytes for one sample including all channels.
        /// == NumChannels * BitsPerSample/8
        /// ENDIANNESS: Little-endian
        BlockAlign = (uint)(wav[32] + (wav[33] << 8));

        /// BitsPerSample
        /// Bits per sample (8=8 bits, 16=16 bits).
        /// == NumChannels * BitsPerSample/8
        /// ENDIANNESS: Little-endian            
        BitsPerSample = (uint)wav[34];

        /// Subchunk2ID
        /// Contains the letters "data".
        /// ENDIANNESS: Big-endian  
        for (i = 36; i < 40; i++)
        { Subchunk2ID += (char)wav[i]; }

        /// Subchunk2Size
        /// Number of bytes in the data.
        /// == NumSamples * NumChannels * BitsPerSample/8
        /// ENDIANNESS: Little-endian  
        Subchunk2Size = (uint)(wav.Length - 44);
        // or
        /*Subchunk2Size = (uint)(wav[40] + (wav[41] << 8) + (wav[42] << 16) + (wav[43] << 24));*/

        // Bit rate
        float Bitrate = SampleRate * BitsPerSample * NumChannels;

        // Number of samples
        Samples = Subchunk2Size / BlockAlign;
                
        // Pos is now positioned to start of actual sound data.
        //i = 0;
        int pos = 44;
        int channel_pos = 0;

        // Allocate memory for the channel
        //channel_byte = new byte[NumChannels, (Samples / NumChannels)]; // 8-bit audio Wave file
        channel_int = new int[NumChannels, (Samples / NumChannels)];

        if (BitsPerSample == 8)
        {
            while (pos != (Subchunk2Size / NumChannels) + 44)
            {
                for (i = 0; i < NumChannels; i++)
                {
                    channel_int[i, channel_pos] = wav[pos];
                    pos = pos + (int)(BlockAlign / NumChannels); // pos++ or pos + 1
                }
                channel_pos++;
            }
        }
        else if (BitsPerSample == 16)
        {
            while (pos != (Samples / NumChannels) * NumChannels * (BlockAlign / NumChannels) + 44)
            {
                for (i = 0; i < NumChannels; i++)
                {
                    channel_int[i, channel_pos] = wav[pos] | (wav[pos + 1] << 8);

                    if (channel_int[i, channel_pos] > 0x7FFF)
                    {
                        // 2's-complement signed integer
                        // Applys bitwise NOT and adds 1
                        channel_int[i, channel_pos] = -((~channel_int[i, channel_pos] + 1) + 0xFFFF); // 2^16
                    }
                    //Debug.WriteLine("{0:X}", channel_int[i, channel_pos]);
                    pos = pos + (int)(BlockAlign / NumChannels); // pos + 2
                }
                channel_pos++;
            }
        }
        else if (BitsPerSample == 24)
        {
            while (pos != (Samples / NumChannels) * NumChannels * (BlockAlign / NumChannels) + 44)//(Subchunk2Size / NumChannels) + 41)//(Subchunk2Size / NumChannels) + 44)// 1440045)//Subchunk2Size + 44)
            {
                for (i = 0; i < NumChannels; i++)
                {
                    channel_int[i, channel_pos] = wav[pos] | (wav[pos + 1] << 8) | (wav[pos + 2] << 16);

                    if (channel_int[i, channel_pos] > 0x7FFFFF)
                    {
                        // 2's-complement signed integer
                        // Applys bitwise NOT and adds 1
                        channel_int[i, channel_pos] = -((~channel_int[i, channel_pos] + 1) + 0xFFFFFF); // 2^24
                    }
                    //Debug.WriteLine("{0:X}", channel_int[i, channel_pos]);
                    pos = pos + (int)(BlockAlign / NumChannels); // pos + 3
                }
                channel_pos++;
            }
        }

        // Uncomment the following lines to print all the information at the debug output window
        Debug.WriteLine("Input Wave file: " + filename);
        Debug.WriteLine("ChunkID: " + ChunkID);
        Debug.WriteLine("ChunkSize: " + ChunkSize + " bytes");
        Debug.WriteLine("Format: " + Format);
        Debug.WriteLine("Subchunk1ID: " + Subchunk1ID);
        Debug.WriteLine("Subchunk1Size: " + Subchunk1Size + " bytes");
        switch (AudioFormat)
        {
            case 1:
                Debug.WriteLine("AudioFormat: " + AudioFormat + " (PCM)");
                break;
            case 2:
                Debug.WriteLine("AudioFormat: " + AudioFormat + " (IBM mu-law (custom))");
                break;
            case 3:
                Debug.WriteLine("AudioFormat: " + AudioFormat + " (IBM a-law (custom))");
                break;
            case 4:
                Debug.WriteLine("AudioFormat: " + AudioFormat + " (IBM AVC ADPCM (custom))");
                break;
        }
        switch (NumChannels)
        {
            case 1:
                Debug.WriteLine("NumChannels: " + NumChannels + " (Mono)");
                break;
            case 2:
                Debug.WriteLine("NumChannels: " + NumChannels + " (Stereo)");
                break;
            default:
                Debug.WriteLine("NumChannels: " + NumChannels + " (Multi-channel)");
                break;
        }
        Debug.WriteLine("SampleRate: " + SampleRate + " Hz");
        Debug.WriteLine("Byterate: " + Byterate + " Bps");
        Debug.WriteLine("BlockAlign: " + BlockAlign);
        Debug.WriteLine("BitsPerSample: " + BitsPerSample);
        Debug.WriteLine("SubChunk2ID: " + Subchunk2ID);
        Debug.WriteLine("SubChunk2Size: " + Subchunk2Size + " bytes");
        Debug.WriteLine("Bitrate: " + Bitrate + " bps");
        Debug.WriteLine("Samples: " + Samples);

        Debug.WriteLine("");
    }

    public void RIFF_header_write(uint NumChannels, uint BitsPerSample, byte[] buffer_fpga)
    {
        var output = File.Create("ANC.wav");

        string path = @"ANC.wav";

        // Calculates the BlockAlign, i.e. the total size of the wav_buffer
        uint BlockAlign = NumChannels * (BitsPerSample / 8);

        byte[] wav_buffer = new byte[BlockAlign];
        int temp_byte = 0;
        
        //int pos_buffer_fpga = 0;

        // Write 0x00 from 0 to 43 position

        if (BitsPerSample == 8)
        {
            for (int i = 0; i < NumChannels; i++) //OU escrever direto do buffer_fpga
            {
                wav_buffer[i] = buffer_fpga[i];
            }

            // Write to the file (Not append, test)
            output.Write(wav_buffer, 0, (int)BlockAlign);

            // APPEND (?)
            /*using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(wav_buffer);
            }*/
        }
        if (BitsPerSample == 16)
        {
            for (int i = 0; i < NumChannels; i = i + 2)
            {
                temp_byte = (buffer_fpga[i] << 8) | buffer_fpga[i + 1];
                if (temp_byte > 0x7FFF)
                {
                    temp_byte = -((~temp_byte + 1) - 0xFFFF); // CONFIRMAR
                    wav_buffer[i] = (byte)temp_byte;
                    wav_buffer[i + 1] = (byte)(temp_byte >> 8);
                }
                else
                {
                    wav_buffer[i] = buffer_fpga[i + 1];
                    wav_buffer[i + 1] = buffer_fpga[i];
                }
            }
            //ESCREVER FICHEIRO
        }
        if (BitsPerSample == 24)
        {
            for (int i = 0; i < NumChannels; i = i + 3)
            {
                temp_byte = (buffer_fpga[i + 2] << 16) | (buffer_fpga[i + 1] << 8) | buffer_fpga[i + 2];
                if (temp_byte > 0x7FFFFF)
                {
                    temp_byte = -((~temp_byte + 1) - 0xFFFFFF); // CONFIRMAR
                    wav_buffer[i] = (byte)temp_byte;
                    wav_buffer[i + 1] = (byte)(temp_byte >> 8);
                    wav_buffer[i + 2] = (byte)(temp_byte >> 16);
                }
                else
                {
                    wav_buffer[i] = buffer_fpga[i + 2];
                    wav_buffer[i + 1] = buffer_fpga[i + 1];
                    wav_buffer[i + 2] = buffer_fpga[i];
                }
            }
            //ESCREVER FICHEIRO
        }
    }
}
