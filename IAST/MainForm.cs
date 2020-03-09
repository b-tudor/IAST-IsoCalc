using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace IAST
{
    public partial class MainForm : Form
    {
     
        private static string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        

        public MainForm()
        {
            InitializeComponent();
        }

        


         // MENU ITEM CONTROLS/BEHAVIOR
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog();
        }

        private void openParameterSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sorbentID;
            double temp = 20;
            string IDA;
            double Anm1 = 1, Ab1 = 2, At1 = 3, Anm2 = 4, Ab2 = 5, At2 = 6;
            string IDB;
            double Bnm1 = 7, Bb1 = 8, Bt1 = 9, Bnm2 = 10, Bb2 = 11, Bt2 = 12;
            double YA = 0.0;

            sorbentID = "TEST";
            IDA = "TEST A";
            IDB = "TEST B";

            // Values have been read in, now populate the GUI fields
            textBox_SorbentID.Text   = sorbentID;
            textBox_SorbateA.Text    = IDA;
            textBox_SorbateB.Text    = IDB;
            textBox_Temperature.Text = temp.ToString();

            textBox_nm1A.Text = Anm1.ToString();
            textBox_b1A.Text  = Ab1.ToString();
            textBox_t1A.Text  = At1.ToString();
            textBox_nm2A.Text = Anm2.ToString();
            textBox_b2A.Text  = Ab2.ToString();
            textBox_t2A.Text  = At2.ToString();

            textBox_nm1B.Text = Bnm1.ToString();
            textBox_b1B.Text  = Bb1.ToString();
            textBox_t1B.Text  = Bt1.ToString();
            textBox_nm2B.Text = Bnm2.ToString();
            textBox_b2B.Text  = Bb2.ToString();
            textBox_t2B.Text  = Bt2.ToString();


            int YA_trackbar_setting = (int) Math.Round(YA * 100.0, MidpointRounding.AwayFromZero );
            if( YA_trackbar_setting < 1  )
                YA_trackbar_setting = 1;
            if( YA_trackbar_setting > 99 )
                YA_trackbar_setting = 99;
            trackBar_Ratio.Value = YA_trackbar_setting;
            label_YA.Text = YA_trackbar_setting.ToString();
            label_YB.Text = (100-YA_trackbar_setting).ToString();

        }


        private void saveParameterSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Parameters for Dual-Site Langmuir Freundlich model
            string sorbentID;
            double temp = 0;
            string IDA;
            double Anm1 = 0, Ab1 = 0, At1 = 0, Anm2 = 0, Ab2 = 0, At2 = 0;
            string IDB;
            double Bnm1 = 0, Bb1 = 0, Bt1 = 0, Bnm2 = 0, Bb2 = 0, Bt2 = 0;

            getTempAndSpecyIDs(ref temp, out sorbentID, out IDA, out IDB);

            // Validate the numeric fields for DS-LF Model Parameters
            // and if valid, extract values from the GUI

            if (!validate_DSLF_UIFields())
                return;
            if (!getDSLFModelParams(ref Anm1, ref Ab1, ref At1, ref Anm2, ref Ab2, ref At2, ref Bnm1, ref Bb1, ref Bt1, ref Bnm2, ref Bb2, ref Bt2))
                return;

            // RETRIEVE YA FROM UI
            double YA = 0;
            if (trackBar_Ratio.Value <= 1)
                YA = 1.0 / 100.0;
            else if (trackBar_Ratio.Value >= 99)
                YA = 99.0 / 100.0;
            else
                YA = trackBar_Ratio.Value / 100.0;


            // Get the output filename from user
            string outFilename;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = initialDirectory;
            saveFileDialog.Filter = "IAST Files (*.iast)|*.iast";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            outFilename = saveFileDialog.FileName;
            // Save the path so we can start there next time
            try { initialDirectory = System.IO.Path.GetDirectoryName(outFilename); }
            catch (Exception) { initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }

            // Build the output string and save it to a file. 

            StringBuilder output = new StringBuilder();
            using (StreamWriter outFile = new StreamWriter(outFilename))
            {
                output.AppendLine(string.Format("#{0}(A)/{1}(B) selectivity in {2}", IDA, IDB, sorbentID));
                output.AppendLine(string.Format("SID  {0}", sorbentID));
                output.AppendLine(string.Format("T    {0}", temp));
                output.AppendLine(string.Format("YA   {0}", YA));
                output.AppendLine(string.Format("#Params for sorbate A({0})", IDA));
                output.AppendLine(string.Format("AID  {0}",  IDA ));
                output.AppendLine(string.Format("ANM1 {0}", Anm1));
                output.AppendLine(string.Format("AB1  {0}",  Ab1));
                output.AppendLine(string.Format("AT1  {0}",  At1));
                output.AppendLine(string.Format("ANM2 {0}", Anm2));
                output.AppendLine(string.Format("AB2  {0}",  Ab2));
                output.AppendLine(string.Format("AT2  {0}",  At2));

                output.AppendLine(string.Format("#Params for sorbate B({0})", IDB));
                output.AppendLine(string.Format("BID  {0}",  IDB));
                output.AppendLine(string.Format("BNM1 {0}", Bnm1));
                output.AppendLine(string.Format("BB1  {0}",  Bb1));
                output.AppendLine(string.Format("BT1  {0}",  Bt1));
                output.AppendLine(string.Format("BNM2 {0}", Bnm2));
                output.AppendLine(string.Format("BB2  {0}",  Bb2));
                output.AppendLine(string.Format("BT2  {0}",  Bt2));
             
                outFile.Write(output.ToString());
            }

        }



         // BEHAVIOR FOR THE "CALCULATE & SAVE" BUTTON 
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        private void button_GO_Click(object sender, EventArgs e) {

            // Parameters for Dual-Site Langmuir Freundlich model
            string sorbentID;
            double temp = 0;
            string IDA;
            double Anm1 = 0, Ab1 = 0, At1 = 0, Anm2 = 0, Ab2 = 0, At2 = 0;
            string IDB;
            double Bnm1 = 0, Bb1 = 0, Bt1 = 0, Bnm2 = 0, Bb2 = 0, Bt2 = 0;

            getTempAndSpecyIDs( ref temp, out sorbentID, out IDA, out IDB);



            // Validate the numeric fields for DS-LF Model Parameters
            // and if valid, extract params from the GUI and instantiate the isotherm objects
            
            if (!validate_DSLF_UIFields())
                return;
            if (!getDSLFModelParams(ref Anm1, ref Ab1, ref At1, ref Anm2, ref Ab2, ref At2, ref Bnm1, ref Bb1, ref Bt1, ref Bnm2, ref Bb2, ref Bt2))
                return;
            
            // Create the isotherm model object for sorbate A
            Dual_Site_Langmuir_Freundlich_Model DSLF_A = new Dual_Site_Langmuir_Freundlich_Model(
                IDA, sorbentID, temp, Anm1, Ab1, At1, Anm2, Ab2, At2
            );
            
            // Create the isotherm model object for sorbate B
            Dual_Site_Langmuir_Freundlich_Model DSLF_B = new Dual_Site_Langmuir_Freundlich_Model(
                IDB, sorbentID, temp, Bnm1, Bb1, Bt1, Bnm2, Bb2, Bt2
            );

            // Make sure the range data entered in the UI is valid before proceeding
            if( ! validateUIRangeFields() ) 
                return;

            // Set a flag indicating whether or not to produce the molar uptake data
            // determination of this setting is taken from the user's input in the UI
            Boolean produceMoleData = checkBox_molarUptake.Checked;
            
            // EVERYTHING SEEMS TO BE VALIDATED
            
            // Calculate the Isotherm


            // RETRIEVE YA FROM UI
            double YA = 0;
            if (trackBar_Ratio.Value <= 1)
                YA = 1.0 / 100.0;
            else if (trackBar_Ratio.Value >= 99)
                YA = 99.0 / 100.0;
            else
                YA = trackBar_Ratio.Value / 100.0;


            // RETRIEVE PRESSURE RANGES FROM UI
            double prevPressure = 0.0;
            bool[] useRange = new bool[4] { checkBox_R1.Checked, checkBox_R2.Checked, checkBox_R3.Checked, checkBox_R4.Checked };
            double[] start = new double[4];
            double[] stop  = new double[4];
            double[] step  = new double[4];
            TextBox[] startTB = new TextBox[4] { textBox_R1start, textBox_R2start, textBox_R3start, textBox_R4start };
            TextBox[] stopTB  = new TextBox[4] { textBox_R1stop,  textBox_R2stop,  textBox_R3stop,  textBox_R4stop  };
            TextBox[] stepTB  = new TextBox[4] { textBox_R1step,  textBox_R2step,  textBox_R3step,  textBox_R4step  };
            // Store all the range info into arrays
            for (int i = 0; i < 4; i++)
                if (useRange[i])
                {
                    start[i] = Double.Parse(startTB[i].Text); 
                    stop[i]  = Double.Parse(stopTB[i].Text);
                    step[i]  = Double.Parse(stepTB[i].Text);
                }



            
            // Grab the output filename
            string outFilename;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            
            saveFileDialog.InitialDirectory = initialDirectory;
            saveFileDialog.Filter = "csv files - Excel (*.csv)|*.csv|data files - XM Grace (*.dat)|*.dat";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            if ( saveFileDialog.ShowDialog() != DialogResult.OK )
                return;

            outFilename = saveFileDialog.FileName;
            // Save the path so we can start there next time
            try{ initialDirectory = System.IO.Path.GetDirectoryName(outFilename); }
            catch (Exception) { initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }

            const int CSV = 1;
            const int DAT = 2;
            int outFileType = saveFileDialog.FilterIndex;



            // cycle through the pressure ranges, generating the output
            List<string> data = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                if (useRange[i])
                {
                    double pressure = start[i];
                    int k = 0;
                    while (pressure <= stop[i])
                    {
                        // prevPressure just prevents a "double data point" when the last value in
                        // a pressure range is identical to the first value in the next
                        if (pressure != prevPressure)
                        {
                            prevPressure = pressure;
                            double sel;
                            double XA, XB, YB; // molar fractions for sorbed and bulk phases
                            try
                            {
                                sel = DSLF_A.selectivity(DSLF_B, pressure, YA);
                            }
                            catch (ConstraintException ce)
                            {
                                sel = -1;
                            }
                            if (outFileType == CSV)
                            {
                                if (produceMoleData)
                                {
                                    double NA, NB;
                                    DSLF_A.getN_forEachComponent(DSLF_B, pressure, YA, out NA, out NB);

                                    DSLF_A.get_SorbedAndBulkMoleRatios_fromYA(DSLF_B, pressure, YA, out XA, out YB, out XB);
                                    double nA = DSLF_A.n(pressure * YA / XA);
                                    double nB = DSLF_B.n(pressure * YB / XB);
                                    double pA = pressure * YA / XA;
                                    double pB = pressure * YB / XB;
                                    data.Add(String.Format("{0:F3}\t{1:F3}\t{2:F3}\t{3:F3}\n", pressure, sel, NA, NB));
                                }
                                else
                                    data.Add(String.Format("{0:F3}\t{1:F3}\n", pressure, sel));
                            }
                            else
                            {
                                if (produceMoleData)
                                {
                                    double NA, NB;
                                    DSLF_A.getN_forEachComponent( DSLF_B, pressure, YA, out NA, out NB );
                                    data.Add(String.Format("{0:F3}, {1:F3}, {2:F3}, {3:F3}\n", pressure, sel, NA, NB));
                                }
                                else
                                    data.Add(String.Format("{0:F3}\t{1:F3}\n", pressure, sel));
                            }
                        }
                        k++;
                        pressure = start[i] + k * step[i];
                    }
                }
            }


            // Everything seems to be ok--build the output string and save
            // it to a file. 

            StringBuilder output = new StringBuilder();

            using (StreamWriter outFile = new StreamWriter(outFilename))
            {

                if (outFileType == CSV) {
                    output.AppendLine( string.Format( "{0}(A)/{1}(B) selectivity in {2}", IDA, IDB, sorbentID ));
                    output.AppendLine( string.Format( "T\t{0:F1}K", temp ));
                    output.AppendLine( string.Format( "yA\t{0:F3}", YA ));
                    output.AppendLine("");
                    output.AppendLine( string.Format("Fit Params for sorbate A({0})", IDA ));
                    output.AppendLine( DSLF_A.outputFitParams("", "\t"));
                    output.AppendLine("");
                    output.AppendLine( string.Format("Fit Params for sorbate B({0})", IDB));
                    output.AppendLine( DSLF_B.outputFitParams("", "\t"));
                    output.AppendLine("");
                    if( produceMoleData )
                        output.AppendLine( "Pressure (kPa)\tSelectivity\tMolar Uptake A (mol/kg)\tMolar Uptake B (mol/kg)");
                    else
                        output.AppendLine("Pressure (kPa)\tSelectivity");
                }

                if (outFileType == DAT)
                {
                    output.AppendLine(string.Format("# {0}(A)/{1}(B) selectivity in {2}", IDA, IDB, sorbentID));
                    output.AppendLine(string.Format("# T, {0:F1}K", temp));
                    output.AppendLine(string.Format("# yA, {0:F3}", YA));
                    output.AppendLine(string.Format("# Fit Params for sorbate A({0})", IDA));
                    output.AppendLine(DSLF_A.outputFitParams("# ", ", "));
                    output.AppendLine(string.Format("# Fit Params for sorbate B({0})", IDB));
                    output.AppendLine(DSLF_B.outputFitParams("# ", ", "));
                    if( produceMoleData )
                        output.AppendLine("#Pressure (kPa), Selectivity, Molar Uptake A (mol/g), Molar Uptake B (mol/g)");
                    else
                        output.AppendLine("#Pressure (kPa), Selectivity");
                }

                foreach (string s in data)
                    output.Append(s);
                
                outFile.Write(output.ToString());
            }
        }




        // Extract the DS-LF model parameters from the GUI
        // These values are checked to ensure they are actual numbers
        private Boolean getDSLFModelParams(ref double Anm1, ref double Ab1, ref double At1, ref double Anm2, ref double Ab2, ref double At2, ref double Bnm1, ref double Bb1, ref double Bt1, ref double Bnm2, ref double Bb2, ref double Bt2)
        {
            // If all were valid, get the values 
            try
            {
                Anm1 = Double.Parse(textBox_nm1A.Text);
                Ab1 = Double.Parse(textBox_b1A.Text);
                At1 = Double.Parse(textBox_t1A.Text);
                Anm2 = Double.Parse(textBox_nm2A.Text);
                Ab2 = Double.Parse(textBox_b2A.Text);
                At2 = Double.Parse(textBox_t2A.Text);

                Bnm1 = Double.Parse(textBox_nm1B.Text);
                Bb1 = Double.Parse(textBox_b1B.Text);
                Bt1 = Double.Parse(textBox_t1B.Text);
                Bnm2 = Double.Parse(textBox_nm2B.Text);
                Bb2 = Double.Parse(textBox_b2B.Text);
                Bt2 = Double.Parse(textBox_t2B.Text);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown input error. Please check all your parameters and try again.", "Numeric Input Error");
                return false;
            }
        }



        // Extract temperature and chemical specy names from the GUI.
        // This data is for record keeping, only, so there is little or no validation performed.
        // These values are for the user's convenience only and are not required.
        private void getTempAndSpecyIDs( ref double temp, out string sorbentID, out string IDA, out string IDB)
        {
            // Grab the IDs of the sorbent and sorbates, 
            // if they were entered.

            if (String.IsNullOrEmpty(textBox_SorbentID.Text))
                sorbentID = "unspecified";
            else
                sorbentID = textBox_SorbentID.Text;

            if (String.IsNullOrEmpty(textBox_SorbateA.Text))
                IDA = "unspecified";
            else
                IDA = textBox_SorbateA.Text;

            if (String.IsNullOrEmpty(textBox_SorbateB.Text))
                IDB = "unspecified";
            else
                IDB = textBox_SorbateB.Text;


            // Grab the temperature, if it is valid
            temp = 0;
            if (!String.IsNullOrEmpty(textBox_Temperature.Text))
                try { temp = Double.Parse(textBox_Temperature.Text); }
                catch (Exception)
                {
                    MessageBox.Show("Unreadable temperature parameter, output will list temperature as 0K. This is for your records only and will not alter the calculated results.", "Temperature Format Error");
                    temp = 0;
                }
            if (temp < 0)
            {
                MessageBox.Show("Temperature parameter is negative, output will list temperature as 0K. This is for your records only and will not alter the calculated results.", "Temperature Format Error");
                temp = 0;
            }
        }















        // IDIOSYNCRATIC BEHAVIOR FOR MISC GUI CONTROLS
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox src = (CheckBox)sender;

            TextBox[] startBox = new TextBox[4];
            TextBox[] stopBox = new TextBox[4];
            TextBox[] stepBox = new TextBox[4];

            startBox[0] = textBox_R1start; stopBox[0] = textBox_R1stop; stepBox[0] = textBox_R1step;
            startBox[1] = textBox_R2start; stopBox[1] = textBox_R2stop; stepBox[1] = textBox_R2step;
            startBox[2] = textBox_R3start; stopBox[2] = textBox_R3stop; stepBox[2] = textBox_R3step;
            startBox[3] = textBox_R4start; stopBox[3] = textBox_R4stop; stepBox[3] = textBox_R4step;

            int range;
            if (src == checkBox_R1) range = 0;
            else if (src == checkBox_R2) range = 1;
            else if (src == checkBox_R3) range = 2;
            else range = 3;

            if (src.Checked)
            {
                startBox[range].Enabled = true;
                stopBox[range].Enabled = true;
                stepBox[range].Enabled = true;
            }
            else
            {
                startBox[range].Enabled = false;
                stopBox[range].Enabled = false;
                stepBox[range].Enabled = false;
            }

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar_Ratio.Value == 0)
            {
                label_YA.Text = "1";
                label_YB.Text = "99";
            }
            else if (trackBar_Ratio.Value == 100)
            {
                label_YA.Text = "99";
                label_YB.Text = "1";
            }
            else
            {
                label_YA.Text = "" + trackBar_Ratio.Value;
                label_YB.Text = "" + (100 - trackBar_Ratio.Value);
            }
        }








        // FORM VALIDATION ROUTINES
        //////////////////////////////////////////////////////////////////////////////////////////////////////


        //  Validates all the Range fields in the IAST panel, displaying
        //   alert boxes when errors are discovered.
        private Boolean validateUIRangeFields()
        {  

            // At least 1 range must be selected
            bool[] useRange = new bool[4] { false, false, false, false};
            // pointers to GUI elements, which will allow us to loop through them
            TextBox[] startTB = new TextBox[4] { textBox_R1start, textBox_R2start, textBox_R3start, textBox_R4start };
            TextBox[] stopTB = new TextBox[4]  { textBox_R1stop,  textBox_R2stop,  textBox_R3stop,  textBox_R4stop  };
            TextBox[] stepTB = new TextBox[4]  { textBox_R1step,  textBox_R2step,  textBox_R3step,  textBox_R4step  };

            double[] start = new double[4];
            double[] stop = new double[4];
            double[] step = new double[4];

            useRange[0] = checkBox_R1.Checked;
            useRange[1] = checkBox_R2.Checked;
            useRange[2] = checkBox_R3.Checked;
            useRange[3] = checkBox_R4.Checked;

            if (!(useRange[0] || useRange[1] || useRange[2] || useRange[3]))
            {
                // If we get here, no ranges are checked
                MessageBox.Show("At least one range must be specified.", "Range Error");
                return false;
            }


            // Validate the fields for each range...
            // But only if we are using the range

            for (int i = 0; i < 4; i++)
            {
                if (useRange[i])
                {

                    try { start[i] = Double.Parse(startTB[i].Text); }
                    catch (Exception)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nInvalid start pressure specified.", "Range Error: Unreadable start value");
                        return false;
                    }

                    try { stop[i] = Double.Parse(stopTB[i].Text); }
                    catch (Exception)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nInvalid stop pressure specified.", "Range Error: Unreadable stop value");
                        return false;
                    }

                    try { step[i] = Double.Parse(stepTB[i].Text); }
                    catch
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nInvalid pressure step (increment) specified.", "Range Error: Unreadable step value");
                        return false;
                    }


                    // Check for non-positive pressures & steps

                    if (start[i] <= 0.0)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nStart pressure must be a positive (non-zero) value.", "Range Error: Invalid range start");
                        return false;
                    }

                    if (stop[i] <= start[i])
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nStop pressure in a range must be greater than the start pressure for that range.", "Range Error: Inverted range");
                        return false;
                    }

                    if (step[i] <= 0.001)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nPressure step must be at least 0.001", "Range Error: Invalid increment");
                        return false;
                    }
                }
            }


            // Check for range overlaps

            for (int i = 0; i < 4; i++)
            {

                // Only for ranges we are using
                if (useRange[i])
                {
                    for (int j = i + 1; j < 4; j++)
                    {

                        // and only against ranges we are using
                        if (useRange[j])
                        {
                            if (stop[i] > start[j])
                            {
                                MessageBox.Show("Ranges " + (i + 1) + " & " + (j + 1) + "\nStart of Range " + (j + 1) + " cannot be less than the end of Range " + (i + 1) + ". \nRanges must be listed in increasing order.", "Range Overlap Error");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }





        // Verified that a UI TextBox object contains valid numeric input. 
        // If the box is empty or non-numeric, an alert is displayed.

        private Boolean hasInvalidNumericInput(TextBox tb)
        {

            string source="nowhere";

            // I know this is sloppy/lazy--I'm reworking the previous IAST-only code and this 
            // project is not a priority. 

            if (tb == textBox_nm1A) source = "parameter nm1 for Sorbate A";
            else if (tb == textBox_b1A)  source = "parameter b1 for Sorbate A";
            else if (tb == textBox_t1A)  source = "parameter t1 for Sorbate A";
            else if (tb == textBox_nm2A) source = "parameter nm2 for Sorbate A";
            else if (tb == textBox_b2A)  source = "parameter b2 for Sorbate A";
            else if (tb == textBox_t2A)  source = "parameter t2 for Sorbate A";

            else if (tb == textBox_nm1B) source = "parameter nm1 for Sorbate B";
            else if (tb == textBox_b1B) source = "parameter b1 for Sorbate B";
            else if (tb == textBox_t1B) source = "parameter t1 for Sorbate B";
            else if (tb == textBox_nm2B) source = "parameter nm2 for Sorbate B";
            else if (tb == textBox_b2B) source = "parameter b2 for Sorbate B";
            else if (tb == textBox_t2B) source = "parameter t2 for Sorbate B";

            else if (tb == textBox_QST_nm1T1) source = "parameter nm1 for Sorbate T1";
            else if (tb == textBox_QST_b1T1) source = "parameter b1 for Sorbate T1";
            else if (tb == textBox_QST_t1T1) source = "parameter t1 for Sorbate T1";
            else if (tb == textBox_QST_nm2T1) source = "parameter nm2 for Sorbate T1";
            else if (tb == textBox_QST_b2T1) source = "parameter b2 for Sorbate T1";
            else if (tb == textBox_QST_t2T1) source = "parameter t2 for Sorbate T1";

            else if (tb == textBox_QST_nm1T2) source = "parameter nm1 for Sorbate T2";
            else if (tb == textBox_QST_b1T2) source = "parameter b1 for Sorbate T2";
            else if (tb == textBox_QST_t1T2) source = "parameter t1 for Sorbate T2";
            else if (tb == textBox_QST_nm2T2) source = "parameter nm2 for Sorbate T2";
            else if (tb == textBox_QST_b2T2) source = "parameter b2 for Sorbate T2";
            else if (tb == textBox_QST_t2T2) source = "parameter t2 for Sorbate T2";

            if (String.IsNullOrEmpty(tb.Text)) {
                System.Windows.Forms.MessageBox.Show("A value must be entered for " + source, "Input Error");
                return true;
            }

            try
            {
                Double.Parse(tb.Text);
            }
            catch (System.ArgumentNullException)
            {
                System.Windows.Forms.MessageBox.Show("\"NULL\" error for " + source, "Input Error: NULL Exception" );
                return true;

            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Unreadable numeric format for " + source, "Input Error: Formatting");
                return true;
            }
            catch (System.OverflowException)
            {
                System.Windows.Forms.MessageBox.Show("The number entered for " + source + " cannot be represented on this machine.", "Input Error: Overflow");
                return true;
            }

            return false;
        }





        // Validates all of the DSLF fields in the UI, displaying
        // alert boxes when errors are discoverd.

        public bool validate_DSLF_UIFields()
        {

            if (hasInvalidNumericInput(textBox_nm1A)) return false;
            if (hasInvalidNumericInput(textBox_b1A)) return false;
            if (hasInvalidNumericInput(textBox_t1A)) return false;
            if (hasInvalidNumericInput(textBox_nm2A)) return false;
            if (hasInvalidNumericInput(textBox_b2A)) return false;
            if (hasInvalidNumericInput(textBox_t2A)) return false;

            if (hasInvalidNumericInput(textBox_nm1B)) return false;
            if (hasInvalidNumericInput(textBox_b1B)) return false;
            if (hasInvalidNumericInput(textBox_t1B)) return false;
            if (hasInvalidNumericInput(textBox_nm2B)) return false;
            if (hasInvalidNumericInput(textBox_b2B)) return false;
            if (hasInvalidNumericInput(textBox_t2B)) return false;

            return true;
        }



        private void checkBox_QST_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox src = (CheckBox)sender;

            TextBox[] startBox = new TextBox[2];
            TextBox[] stopBox = new TextBox[2];
            TextBox[] stepBox = new TextBox[2];

            startBox[0] = textBox_QST_R1start; stopBox[0] = textBox_QST_R1stop; stepBox[0] = textBox_QST_R1step;
            startBox[1] = textBox_QST_R2start; stopBox[1] = textBox_QST_R2stop; stepBox[1] = textBox_QST_R2step;

            int range;
            if (src == checkBox_QST_R1) range = 0;
            else range = 1;
            

            if (src.Checked)
            {
                startBox[range].Enabled = true;
                stopBox[range].Enabled = true;
                stepBox[range].Enabled = true;
            }
            else
            {
                startBox[range].Enabled = false;
                stopBox[range].Enabled = false;
                stepBox[range].Enabled = false;
            }

        }


        private void radio_QST_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_QST_Auto.Checked)
            {
                // disable the ranges
                checkBox_QST_R1.Enabled = false;
                textBox_QST_R1start.Enabled = false;
                textBox_QST_R1stop.Enabled = false;
                textBox_QST_R1step.Enabled = false;

                checkBox_QST_R2.Enabled = false;
                textBox_QST_R2start.Enabled = false;
                textBox_QST_R2stop.Enabled = false;
                textBox_QST_R2step.Enabled = false;
            }
            else
            {
                // enable the ranges
                checkBox_QST_R1.Enabled = true;
                if (checkBox_QST_R1.Checked)
                {
                    textBox_QST_R1start.Enabled = true;
                    textBox_QST_R1stop.Enabled = true;
                    textBox_QST_R1step.Enabled = true;
                }

                checkBox_QST_R2.Enabled = true;
                if (checkBox_QST_R2.Checked)
                {
                    textBox_QST_R2start.Enabled = true;
                    textBox_QST_R2stop.Enabled = true;
                    textBox_QST_R2step.Enabled = true;
                }
            }
            
        }


        //  
        //   Validation and routines for extracting data from the QST panel
        //   A lot of existing code could have been reworked to access the Qst 
        //   AND the IAST panel, but it was easier to copy/paste and I got
        //   ish to do!
        //
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        public bool validate_QST_DSLF_UIFields()
        {

            if (hasInvalidNumericInput(textBox_QST_nm1T1)) return false;
            if (hasInvalidNumericInput(textBox_QST_b1T1)) return false;
            if (hasInvalidNumericInput(textBox_QST_t1T1)) return false;
            if (hasInvalidNumericInput(textBox_QST_nm2T1)) return false;
            if (hasInvalidNumericInput(textBox_QST_b2T1)) return false;
            if (hasInvalidNumericInput(textBox_QST_t2T1)) return false;

            if (hasInvalidNumericInput(textBox_QST_nm1T2)) return false;
            if (hasInvalidNumericInput(textBox_QST_b1T2)) return false;
            if (hasInvalidNumericInput(textBox_QST_t1T2)) return false;
            if (hasInvalidNumericInput(textBox_QST_nm2T2)) return false;
            if (hasInvalidNumericInput(textBox_QST_b2T2)) return false;
            if (hasInvalidNumericInput(textBox_QST_t2T2)) return false;

            return true;
        }

        // Extract the DS-LF model parameters from the GUI on the Qst Panel
        // These values are checked to ensure they are actual numbers
        private Boolean get_QST_DSLFModelParams(ref double Anm1, ref double Ab1, ref double At1, ref double Anm2, ref double Ab2, ref double At2, ref double Bnm1, ref double Bb1, ref double Bt1, ref double Bnm2, ref double Bb2, ref double Bt2)
        {
            // If all were valid, get the values 
            try
            {
                Anm1 = Double.Parse(textBox_QST_nm1T1.Text);
                Ab1 = Double.Parse(textBox_QST_b1T1.Text);
                At1 = Double.Parse(textBox_QST_t1T1.Text);
                Anm2 = Double.Parse(textBox_QST_nm2T1.Text);
                Ab2 = Double.Parse(textBox_QST_b2T1.Text);
                At2 = Double.Parse(textBox_QST_t2T1.Text);

                Bnm1 = Double.Parse(textBox_QST_nm1T2.Text);
                Bb1 = Double.Parse(textBox_QST_b1T2.Text);
                Bt1 = Double.Parse(textBox_QST_t1T2.Text);
                Bnm2 = Double.Parse(textBox_QST_nm2T2.Text);
                Bb2 = Double.Parse(textBox_QST_b2T2.Text);
                Bt2 = Double.Parse(textBox_QST_t2T2.Text);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown input error. Please check all your parameters and try again.", "Numeric Input Error");
                return false;
            }
        }


        

        //  Validates all the Range fields in the IAST panel, displaying
        //   alert boxes when errors are discovered.
        private Boolean validate_QST_UIRangeFields()
        {  

            // At least 1 range must be selected
            bool[] useRange = new bool[2] { false, false };
            // pointers to GUI elements, which will allow us to loop through them
            TextBox[] startTB = new TextBox[2] { textBox_QST_R1start, textBox_QST_R2start };
            TextBox[] stopTB = new TextBox[2]  { textBox_QST_R1stop,  textBox_QST_R2stop };
            TextBox[] stepTB = new TextBox[2]  { textBox_QST_R1step,  textBox_QST_R2step };

            double[] start = new double[2];
            double[] stop = new double[2];
            double[] step = new double[2];

            useRange[0] = checkBox_QST_R1.Checked;
            useRange[1] = checkBox_QST_R2.Checked;

            if (!(useRange[0] || useRange[1] ))
            {
                // If we get here, no ranges are checked
                MessageBox.Show("At least one range must be specified.", "Range Error");
                return false;
            }


            // Validate the fields for each range...
            // But only if we are using the range

            for (int i = 0; i < 2; i++)
            {
                if (useRange[i])
                {

                    try { start[i] = Double.Parse(startTB[i].Text); }
                    catch (Exception)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nInvalid start uptake specified.", "Range Error: Unreadable start value");
                        return false;
                    }

                    try { stop[i] = Double.Parse(stopTB[i].Text); }
                    catch (Exception)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nInvalid stop uptake specified.", "Range Error: Unreadable stop value");
                        return false;
                    }

                    try { step[i] = Double.Parse(stepTB[i].Text); }
                    catch
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nInvalid uptake step (increment) specified.", "Range Error: Unreadable step value");
                        return false;
                    }


                    // Check for non-positive pressures & steps

                    if (start[i] <= 0.0)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nStart uptake must be a positive (non-zero) value.", "Range Error: Invalid range start");
                        return false;
                    }

                    if (stop[i] <= start[i])
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nStop uptake in a range must be greater than the start uptake for that range.", "Range Error: Inverted range");
                        return false;
                    }

                    if (step[i] <= 0.001)
                    {
                        MessageBox.Show("Range " + (i + 1) + "\nUptake step must be at least 0.001", "Range Error: Invalid increment");
                        return false;
                    }
                }
            }


            // Check for range overlaps
            if( useRange[0] && useRange[1] ) {
                if (stop[0] > start[1])
                {
                    MessageBox.Show("Start of Range 2 cannot be less than the end of Range 1. \nRanges must occur in increasing order, without overlap", "Range Overlap Error");
                    return false;
                }
            }

            return true;
        }














        // Extract temperature and chemical specy names from the GUI.
        // This data is for record keeping, only, so there is little or no validation performed.
        // These values are for the user's convenience only and are not required.
        private Boolean get_QST_TempandSpecyIDs( out double t1, out double t2, out string sorbentID, out string sorbateID )
        {
            // Grab the IDs of the sorbent and sorbates, 
            // if they were entered.

            if (String.IsNullOrEmpty(textBox_QST_SorbentID.Text))
                sorbentID = "unspecified";
            else
                sorbentID = textBox_QST_SorbentID.Text;


            if (String.IsNullOrEmpty( textBox_QST_SorbateID.Text ))
                sorbateID = "unspecified";
            else
                sorbateID = textBox_QST_SorbateID.Text;

            // Grab and validate the temperature, for first isotherm
            t1 = 0; t2 = 0;
            if (!String.IsNullOrEmpty( textBox_QST_Temp_T1.Text))
                try { t1 = Double.Parse(textBox_QST_Temp_T1.Text); }
                catch (Exception)
                {
                    MessageBox.Show("Unreadable temperature parameter for isotherm T1.", "Temperature Format Error");
                    return false;
                }
            if (t1 <= 0)
            {
                MessageBox.Show("Temperature parameter for isotherm T1 must be greater than 0.", "Temperature Format Error");
                return false;
            }


            // Grab and validate the temperature for the second isotherm
            if (!String.IsNullOrEmpty(textBox_QST_Temp_T2.Text))
                try { t2 = Double.Parse(textBox_QST_Temp_T2.Text); }
                catch (Exception)
                {
                    MessageBox.Show("Unreadable temperature parameter for isotherm T2.", "Temperature Format Error");
                    return false;
                }
            if (t2 <= 0)
            {
                MessageBox.Show("Temperature parameter for isotherm T2 must be greater than 0.", "Temperature Format Error");
                return false;
            }

            if (t1 == t2)
            {
                MessageBox.Show("Temperature parameters for isotherms T1 and T2 must not be equal.", "Temperature Format Error");
                return false;
            }



            return true;
        }









        private void button_GO_Qst_Click(object sender, EventArgs e)
        {
            // Parameters for Dual-Site Langmuir Freundlich model
            string sorbentID;
            double T1temp = 0, T2temp = 0;
            string sorbateID;
            double T1nm1 = 0, T1b1 = 0, T1t1 = 0, T1nm2 = 0, T1b2 = 0, T1t2 = 0;
            double T2nm1 = 0, T2b1 = 0, T2t1 = 0, T2nm2 = 0, T2b2 = 0, T2t2 = 0;

            if (!get_QST_TempandSpecyIDs(out T1temp, out T2temp, out sorbentID, out sorbateID))
            {
                return;
            }


            

            // Validate the numeric fields for DS-LF Model Parameters
            // and if valid, extract params from the GUI and instantiate the isotherm objects

            if (!validate_QST_DSLF_UIFields())
                return;
            if (!get_QST_DSLFModelParams(ref T1nm1, ref T1b1, ref T1t1, ref T1nm2, ref T1b2, ref T1t2, ref T2nm1, ref T2b1, ref T2t1, ref T2nm2, ref T2b2, ref T2t2))
                return;

            // Create the isotherm model object for sorbate A
            Dual_Site_Langmuir_Freundlich_Model DSLF_T1 = new Dual_Site_Langmuir_Freundlich_Model(
                sorbateID, sorbentID, T1temp, T1nm1, T1b1, T1t1, T1nm2, T1b2, T1t2
            );

            // Create the isotherm model object for sorbate B
            Dual_Site_Langmuir_Freundlich_Model DSLF_T2 = new Dual_Site_Langmuir_Freundlich_Model(
                sorbateID, sorbentID, T2temp, T2nm1, T2b1, T2t1, T2nm2, T2b2, T2t2
            );

            
            // Make sure the range data entered in the UI is valid before proceeding
            if (radio_QST_Custom.Checked)
            {
                if (!validate_QST_UIRangeFields())
                    return;
            }
            

            // EVERYTHING SEEMS TO BE VALIDATED

            // Calculate the Isotherm

            // RETRIEVE UPTAKE RANGES FROM UI
            double prevUptake = 0.0;
            bool[] useRange = new bool[2] { checkBox_QST_R1.Checked, checkBox_QST_R2.Checked };
            double[] start = new double[2];
            double[] stop = new double[2];
            double[] step = new double[2];
            TextBox[] startTB = new TextBox[2] { textBox_QST_R1start, textBox_QST_R2start };
            TextBox[] stopTB = new TextBox[2] { textBox_QST_R1stop, textBox_QST_R2stop };
            TextBox[] stepTB = new TextBox[2] { textBox_QST_R1step, textBox_QST_R2step };

            // Store all the range info into arrays
            if( radio_QST_Auto.Checked ) {
                useRange[0] = true;
                useRange[1] = false;
                start[0] = 0.01;
                step[0] = 0.01;

                // Calculate the uptake at 1 atmosphere, for the isotherm at the lower temperature
                Dual_Site_Langmuir_Freundlich_Model dslf = (DSLF_T1.temp < DSLF_T2.temp)?DSLF_T1:DSLF_T2;
                double n = dslf.n(101.325); // 1 atm = 101.325 kPa
                n = ((int)(n * 100) + 1) / 100.0; // round up, to the nearest 1/100th
                
                // Assign a default uptake of 0.1 if the calculated uptake turns out to be less than our starting uptake
                // This is unlikely, but is here, just in case, to avoid an infinite loop.
                stop[0] = (n > start[0]) ? n : 0.1;   
                
            } else {
                for (int i = 0; i < 2; i++)
                    if (useRange[i])
                    {
                        start[i] = Double.Parse(startTB[i].Text);
                        stop[i] = Double.Parse(stopTB[i].Text);
                        step[i] = Double.Parse(stepTB[i].Text);
                    }

            }

            // Grab the output filename
            string outFilename;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = initialDirectory;
            saveFileDialog.Filter = "csv files - Excel (*.csv)|*.csv|data files - XM Grace (*.dat)|*.dat";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            outFilename = saveFileDialog.FileName;
            // Save the path so we can start there next time
            try { initialDirectory = System.IO.Path.GetDirectoryName(outFilename); }
            catch (Exception) { initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }

            const int CSV = 1;
            const int DAT = 2;
            int outFileType = saveFileDialog.FilterIndex;

            

            // cycle through the uptake ranges, generating the output
            List<string> data = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                if (useRange[i])
                {
                    double uptake = start[i];
                    int k = 0;
                    while (uptake <= stop[i])
                    {
                        // prevUptake just prevents a "double data point" when the last value in
                        // an uptake range is identical to the first value in the next
                        if (uptake != prevUptake)
                        {
                            prevUptake = uptake;
                            
                            double qst=0.0;

                            
                            try
                            {
                                qst = Isotherm.Qst(DSLF_T1, DSLF_T2, uptake );
                            }
                            catch (Exception ce)
                            {
                                qst = -1;
                            }
                            

                            if (outFileType == CSV)
                                data.Add(String.Format("{0:F3}\t{1:F3}\n", uptake, qst));
                            else
                                data.Add(String.Format("{0:F3}, {1:F3}\n", uptake, qst));
                        }
                        k++;
                        uptake = start[i] + k * step[i];
                    }
                }
            }

            

            // Everything seems to be ok--build the output string and save
            // it to a file. 
            
            StringBuilder output = new StringBuilder();

            using (StreamWriter outFile = new StreamWriter(outFilename))
            {

                if (outFileType == CSV)
                {
                    //output.AppendLine(string.Format("{0}(A)/{1}(B) selectivity in {2}", IDA, IDB, sorbentID));
                    //output.AppendLine(string.Format("T\t{0:F1}K", temp));
                    //output.AppendLine(string.Format("yA\t{0:F3}", YA));
                    //output.AppendLine("");
                    output.AppendLine(string.Format("Fit Params for T1 isotherm({0} in {1} @ {2:f2} K)", sorbateID, sorbentID, DSLF_T1.temp ));
                    output.AppendLine(DSLF_T1.outputFitParams("", "\t"));
                    output.AppendLine("");
                    output.AppendLine(string.Format("Fit Params for T2 isotherm({0}) in {1} @ {2:f2} K)", sorbateID, sorbentID, DSLF_T2.temp ));
                    output.AppendLine(DSLF_T2.outputFitParams("", "\t"));
                    output.AppendLine("");
                    output.AppendLine("Uptake (mmol/g)\tQst (kJ/mol)");
                }

                if (outFileType == DAT)
                {
                    //output.AppendLine(string.Format("# {0}(A)/{1}(B) selectivity in {2}", IDA, IDB, sorbentID));
                    //output.AppendLine(string.Format("# T, {0:F1}K", temp));
                    //output.AppendLine(string.Format("# yA, {0:F3}", YA));

                    output.AppendLine(string.Format("# Fit Params for T1 isotherm({0} in {1} @ {2:f2} K)", sorbateID, sorbentID, DSLF_T1.temp ));
                    output.AppendLine(DSLF_T1.outputFitParams("# ", ", "));
                    output.AppendLine(string.Format("# Fit Params for T2 isotherm({0}) in {1} @ {2:f2} K)", sorbateID, sorbentID, DSLF_T2.temp ));
                    output.AppendLine(DSLF_T2.outputFitParams("# ", ", "));
                    output.AppendLine("#Uptake (mmol/g), Qst (kJ/mol)");
                }

                foreach (string s in data)
                    output.Append(s);

                outFile.Write(output.ToString());
            }
            
        }

    }
}
