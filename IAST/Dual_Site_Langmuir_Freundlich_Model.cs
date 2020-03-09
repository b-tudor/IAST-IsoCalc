using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAST
{
    class Dual_Site_Langmuir_Freundlich_Model:Isotherm
    {
        private double _n_m1;
        public double n_m1 {        // Saturation uptake
            get { return _n_m1;  }
            set { _n_m1 = value; }
        }

        private double _n_m2;
        public double n_m2 {
            get { return _n_m2; }
            set { _n_m2 = value; }
        }

        private double _b1;
        public double b1 {                 // Activity coefficients
            get { return _b1;  }
            set { _b1 = value; }
        }

        private double _b2;
        public double b2 {
            get { return _b2; }
            set { _b2 = value; }
        }

        private double _t1;
        double t1 {                 // Deviation from homogenous surface
            get { return _t1;  }
            set { _t1 = value; }
        }

        private double _t2;
        double t2 {        
            get { return _t2; }
            set { _t2 = value; }
        }

        ~Dual_Site_Langmuir_Freundlich_Model() {}

        //Dual_Site_Langmuir_Freundlich_Model_Isotherm::Dual_Site_Langmuir_Freundlich_Model_Isotherm( char *filename ){}
        public Dual_Site_Langmuir_Freundlich_Model( Dual_Site_Langmuir_Freundlich_Model orig) {
            this.sorbate   = orig.sorbate;
            this.adsorbant = orig.adsorbant;
            this.temp      = orig.temp;
            this.n_m1      = orig.n_m1;
            this.b1        = orig.b1;
            this.t1        = orig.t1;
            this.n_m2      = orig.n_m2;
            this.b2        = orig.b2;
            this.t2        = orig.t2;
        }

        public Dual_Site_Langmuir_Freundlich_Model(
        
                string Sorbate,
                string Adsorbant,
                double Temp,
        
                // Point1
                double N_m1, // Saturation uptake
                double B1,   // Activity coefficients
                double T1,   // Deviation from homogenous surface. 
        
                // Point 2
                double N_m2, // Ditto...
                double B2,
                double T2
        )
        {
            sorbate = Sorbate;
            adsorbant = Adsorbant;
            temp = Temp;
    
            n_m1 = N_m1;
            b1   = B1;
            t1   = T1;
    
            n_m2 = N_m2;
            b2   = B2;
            t2   = T2;
        }





        public override double n ( double P ) {
        
            if( P<=0.0 ) return 0.0;
    
            double num1   = n_m1 * b1  *  Math.Pow( P, (1.0/t1));
            double denom1 =    1 + b1  *  Math.Pow( P, (1.0/t1));
            double num2   = n_m2 * b2  *  Math.Pow( P, (1.0/t2));
            double denom2 =    1 + b2  *  Math.Pow( P, (1.0/t2));
    
            return (num1/denom1) + (num2/denom2); 
        }


        public override double P(double targetN)
        {
            double currentP = 1.0;
            double lastP = 0.0;

            double precisionLvl = 1.0;


            for (int i = 1; i < 70; i++)
            {
                double N = n(currentP);
                while (N < targetN)
                {
                    if (i > 1)
                        lastP = currentP;

                    currentP += precisionLvl;
                    
                    if (currentP == lastP)
                        break;

                    N = n(currentP);

                    if (i == 1)
                        precisionLvl *= 2;

                    // If required pressure exceeds ~1000 atm, return a value indicating the calculation went bad.
                    if (currentP > 100000.0)
                        return -1.0;

                }
                precisionLvl /= 2.0;
                currentP = lastP + precisionLvl;
            }

            return (currentP + lastP) * 0.5;
        }



        // Returns spreading pressure, using the analytic integral of n
        public override double pi( 
                double P    // pressure
        ) {     
            // Terms 1 & 2 of spreading pressures for isotherms 
            double term_1 = n_m1 * t1  *  Math.Log(b1 * Math.Pow( P, (1.0/t1)) + 1);
            double term_2 = n_m2 * t2  *  Math.Log(b2 * Math.Pow( P, (1.0/t2)) + 1);

            return term_1 + term_2;
        }

        
        public override string outputFitParams( string remarkToken, string separatorToken ) {
            StringBuilder outString = new StringBuilder(1024);

            outString.AppendFormat( "{0}Dual Site Langmuir-Freundlich Model Fit Parameters:\n", remarkToken );
            outString.AppendFormat( "{0}n_m1{1}{2:F15}\n", remarkToken, separatorToken, n_m1);
            outString.AppendFormat( "{0}b1{1}{2:F15}\n",   remarkToken, separatorToken, b1);
            outString.AppendFormat( "{0}t1{1}{2:F15}\n",   remarkToken, separatorToken, t1);
            outString.AppendFormat( "{0}n_m2{1}{2:F15}\n", remarkToken, separatorToken, n_m2);
            outString.AppendFormat( "{0}b2{1}{2:F15}\n",   remarkToken, separatorToken, b2);
            outString.AppendFormat( "{0}t2{1}{2:F15}",   remarkToken, separatorToken, t2);

            return outString.ToString();
        }

    }
}
