using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
//using System.Threading; // Thread.Sleep(x) function

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

using gv = Globals.Variables;

namespace WAVE_Analysis
{
    public partial class Form1 : Form
    {
        //static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        RIFF_header Audio1;
        RIFF_header Audio2;
        RIFF_header Audio3;

        //RIFF_header Write1;

        public Form1()
        {
            InitializeComponent();

            Audio1 = new RIFF_header();
            //Audio2 = new RIFF_header();
            //Audio3 = new RIFF_header();

            Audio1.RIFF_header_reader(gv.filename);
            //Audio2.RIFF_header_reader(gv.filename2);
            //Audio3.RIFF_header_reader(gv.filename3);

            //RIFF_header.RIFF_header_reader(Globals.Variables.filename);
            /*Frequency_capture.Frequency_wave(Audio1.NumChannels, Audio1.Samples, Audio1.SampleRate, Audio1.left, Audio1.right);
            Frequency_capture.Frequency_wave(Audio2.NumChannels, Audio2.Samples, Audio2.SampleRate, Audio2.left, Audio2.right);
            Frequency_capture.Frequency_wave(Audio3.NumChannels, Audio3.Samples, Audio3.SampleRate, Audio3.left, Audio3.right);*/

            Frequency_capture.Frequency_wave(Audio1);
            //Frequency_capture.Frequency_wave(Audio2);
            //Frequency_capture.Frequency_wave(Audio3);

            /*
            //TESTS:
            Debug.WriteLine(Audio1.NumChannels);
            Debug.WriteLine(Audio2.NumChannels);
            Debug.WriteLine(Audio3.NumChannels);
            Debug.WriteLine(Audio1.NumChannels);
            Debug.WriteLine(Audio1.Subchunk2Size);
            Debug.WriteLine(Audio2.Subchunk2Size);
            Debug.WriteLine(Audio3.Subchunk2Size);*/

            //Write1 = new RIFF_header();
            //Write1.RIFF_header_write(7);

            //RIFF_header.RIFF_header_write(1, 24, Audio1.left);
        }



        //public PlotModel Model { get; private set; }

        // Buffer to preview 67 ms of the Wave file
        uint SampleBuffer = 0;

        uint count = 0;
        uint dif = 0;

        // Timer event
        // WAIT THAT TIME
        // see https://stackoverflow.com/questions/1142828/add-timer-to-a-windows-forms-application
        public void Plot_Timer(object sender, EventArgs e)
        {

            PlotModel model1 = new PlotModel
            {
                Background = OxyColor.FromRgb(255,255,255),
                LegendTitle = "Channel 1",
                LegendTitleColor = OxyColors.Gray,
                LegendTitleFont = "Calibri",
                PlotType = PlotType.XY, // Unnecessary
                Title = "Channel 1",
                Subtitle = "Channel 1",
                PlotAreaBackground = OxyColors.LightGray,
                PlotAreaBorderColor = OxyColors.Silver
                //LegendBackground = OxyColors.IndianRed,
                //LegendBorder = OxyColors.IndianRed
                //LegendSymbolLength = 24
            };
            LineSeries s1 = new LineSeries
            {
                MarkerFill = OxyColors.Silver, // ?
                MarkerSize = 3, // ?
                BrokenLineColor = OxyColors.White,
                StrokeThickness = 2,
                Color = OxyColors.IndianRed,
                //MarkerType = MarkerType.Circle,
                //MarkerSize = 6,
                //MarkerStroke = OxyColors.White,
                //MarkerFill = OxyColors.SkyBlue,
                //MarkerStrokeThickness = 1.5
                
            };

            OxyPlot.Axes.LinearAxis LAY;
            LAY = new OxyPlot.Axes.LinearAxis()
            {
                Key = "YAxis",
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                AbsoluteMaximum = 1,
                AbsoluteMinimum = -1,
                MinorStep = 5,
            };

            PlotModel model2 = new PlotModel
            {
                //LegendSymbolLength = 24
            };
            LineSeries s2 = new LineSeries
            {
                Color = OxyColors.SkyBlue,
                //MarkerType = MarkerType.Circle,
                //MarkerSize = 6,
                //MarkerStroke = OxyColors.White,
                //MarkerFill = OxyColors.GreenYellow,
                //MarkerStrokeThickness = 1.5
            };

            SampleBuffer = (uint)((Audio1.SampleRate * 67) / (float)1000);

            for (int i = 0; i < SampleBuffer; i++)
            {
                if (count >= (Audio1.Samples / Audio1.NumChannels)) // Create last frame with the size of the buffer
                {
                    dif = (Audio1.Samples / Audio1.NumChannels) - SampleBuffer;
                    for (i = (int)dif; i < SampleBuffer; i++)
                    {
                        s1.Points.Add(new DataPoint(i, (Audio1.channel_int[0, i] / (float)0xFFFFFF)));
                        s2.Points.Add(new DataPoint(i, (Audio1.channel_int[1, i] / (float)0xFFFFFF)));
                        count = (uint)SampleBuffer;
                    }
                    count = 0;
                }
                else // Create frames with all the wave points
                {
                    s1.Points.Add(new DataPoint(count, (Audio1.channel_int[0, count] / (float)0xFFFFFF)));
                    s2.Points.Add(new DataPoint(count, (Audio1.channel_int[1, count] / (float)0xFFFFFF)));

                    count = count + (Audio1.BlockAlign / Audio1.NumChannels); // +1 (8-bit) | +2 (16-bit) | +3 (24-bit)
                }
            }

            model1.Series.Add(s1);
            plotView1.Model = model1;

            model2.Series.Add(s2);
            plotView2.Model = model2;

            /*MessageBox.Show("The form will now be closed.", "Time Elapsed");
            this.Close();*/
        }

        private void Play_Click(object sender, EventArgs e)
        {
            // Timer of 67 ms to refresh the window frame
            timer1.Interval = 67; // 67 ms
            timer1.Tick += new EventHandler(Plot_Timer);
            timer1.Start();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            // Stop timer
            timer1.Stop();
        }
    }
}
