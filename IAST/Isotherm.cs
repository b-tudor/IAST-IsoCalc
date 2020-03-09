using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAST
{
    abstract class Isotherm
    {
        private string _sorbate;
        public string sorbate {
            get { return _sorbate; }
            set { _sorbate = value; }
        }

        private string _adsorbant;
        public string adsorbant {
            get { return _adsorbant; }
            set { _adsorbant = value; }
        }

        private double _temp;
        public double temp {
            get { return _temp; }
            set { _temp = value; }
        }

        public Isotherm() {}
        ~Isotherm() {}

        // n(P) returns the number of moles sorbed at the given pressure
        public abstract double n(double P);
        // P(n) returns the pressure at which the specified number of moles, n, would be sorbed
        public abstract double P(double n);

        // Qst( A_T1, A_T2, n ) returns the heat of adsorption associated with a molar uptake, n, given two isotherms
        // for a single species, recorded at temperatures T1 and T2
        public static double Qst(Isotherm Isotherm_T1, Isotherm Isotherm_T2, double n)
        {
            const double R = .008314462175;  // kJ/(mol*K)

            double P1 = Isotherm_T1.P(n);
            double T1 = Isotherm_T1.temp;

            double P2 = Isotherm_T2.P(n);
            double T2 = Isotherm_T2.temp;

            double numerator = -R * Math.Log(P2 / P1);
            double denominator = (T1 - T2) / (T1 * T2);

            return numerator / denominator;
        }

        public abstract string outputFitParams( string remarkToken, string separatorToken );



        // The default spreading pressure calculator...
        // For the sake of performance, this virtual function SHOULD be overwritten
        // in the event that an analytic integral form is available for any particular
        // model that is descending from this class. In almost all cases, an analytic
        // integral will be faster than this all-purpose technique. 
        public virtual double pi( double P ) {
            return numeric_pi(P);
        }


        
        // Given the pressure, P, and the mole fraction of THIS sorbate in the 
        // gas phase, YA, returns the selectivity of THIS sorbate vs sorbate B.
        public double selectivity( Isotherm B, double P, double YA ) {
            double XA = x( B, P, YA );
            double XB = 1-XA;
            double YB = 1-YA;
    
            return ((XA)/(YA)) / ((XB)/(YB));
        }

        public void get_SorbedAndBulkMoleRatios_fromYA(Isotherm B, double P, double YA, out double XA, out double YB, out double XB)
        {
            XA = x(B, P, YA);
            XB = 1 - XA;
            YB = 1 - YA;
        }

        public void getN_forEachComponent(Isotherm B, double P, double YA, out double NA, out double NB )
        {
            double XA, XB, YB;
            get_SorbedAndBulkMoleRatios_fromYA( B, P, YA, out XA, out YB, out XB);
            double NA_stp = n(P * YA / XA);
            double NB_stp = B.n(P * YB / XB);
            double Ntot_stp = ( NA_stp * NB_stp ) / (NB_stp * XA + NA_stp * XB );
            NA = XA * Ntot_stp;
            NB = XB * Ntot_stp;

        }


        // Given a pressure, P, the mole fraction of sorbate A (THIS sorbate) in the
        // gas phase, X_A the mole fraction of sorbate A (THIS sorbate) in the sorbed 
        // phase, Y_A, and isotherm model for species B, spreading_pressure_diff() 
        // calculates the difference in spreading pressure between species A (the 
        // species for THIS isotherm) & B.
        double spreading_pressure_diff ( 
                Isotherm B,   // isotherm for species B
                double P,     // pressure
                double X_A,   // mole fraction of sorbate A in the gas phase
                double Y_A    // mole fraction of sorbate A in the sorbed phase
        ) {
            double spreadingPressure_A = pi( P *      Y_A  /      X_A);
            double spreadingPressure_B = B.pi( P * (1.0-Y_A) / (1.0-X_A));
    
            return spreadingPressure_B - spreadingPressure_A;
        }



        // Return moles as a function of pressure, divided by said pressure
        // (Used for numerically integrating isotherm to find pi)
        double nOverP( double P ) {
    
            if( P<=0.0 ) 
                return 0.0;
    
            return n(P)/P; 
        }


        

        // Returns the spreading pressure using a numeric integration of n(P)/P
        double numeric_pi( double P ) {
    
            double sum = 0;
            int steps = 2000;
    
            for( int i=0; i<steps; i++ ) {
                double a =  P  *  (double)(i)   / steps;
                double b =  P  *  (double)(i+1) / steps;
                // Calculates the integral of this segment using Simpson's Rule
                sum += (b-a)/6.0 * ( nOverP(a) + nOverP(b) + 4.0*nOverP((a+b)/2.0)  );
            }
    
            return sum;
        }

        


        // Given a pressure, P, the mole fraction of sorbate A in the sorbed phase, Y_A, and
        // isotherm model for species B, x() narrows in on a gas phase mole fraction for sorbate 
        // A (THIS sorbate), X_A, accurate to the limits of machine precision.
        double x( 
                Isotherm B,   // isotherm for species B
                double P,     // pressure
                double Y_A    // mole fraction of sorbate A in the sorbed phase
        ) {
            // Bracket the root
            double x = 0;
            double lo_x = 1E-14;        // 0 + dx
            double hi_x = 1.0 - 1E-14;  // 1 - dx
     

            const double STEPS = 100;
            // factor will invert the value of the spreading pressure difference
            // function in the event that the values start out negative and go
            // positive. 
            double factor = (( spreading_pressure_diff( B, P, lo_x, Y_A ) >= 0)?( 1.0 ):( -1.0 ));
    
     
            if( factor * spreading_pressure_diff( B, P, hi_x, Y_A ) > 0 ) {
                bool foundInterval = false;
                for (int dx = 1; dx < 100; dx++)
                {
                    double diff = factor * spreading_pressure_diff(B, P, dx * 0.01 * (hi_x - lo_x), Y_A);
                    if (diff <= 0)
                    {
                        foundInterval = true;
                        hi_x = dx * 0.01;
                    }
                }
                if( !foundInterval )
                    throw( new System.Data.ConstraintException("No equilibrium condition found for pressure " + P ) );
            }

            // Test values of x in range (0,1 (or hi_x)) until the sign changes.
            //
            // (We are looking for a root and are trying to find the X_A
            // value where the difference in the spreading pressure for A & B
            // is 0, i.e. the equilibrium point.)
            //
            // Each time the sign changes, test values of x in the range of x
            // just before the sign of delta_of_spreading_pressure (spDiff) 
            // changed, and the x that caused the spDiff sign to change.
            // We will narrow this testing region by a factor of 'steps' a
            // total of MAX_ITER times.

            const int MAX_ITER = 10;
    
            for(int j=0; j<MAX_ITER; j++) {

                int i = 0;
                double spDiff = factor * spreading_pressure_diff( B, P, lo_x, Y_A );
        
                // Go until we've reached "steps" iterations or until spDiff
                // changes sign. 
                while( i <=STEPS  &&  spDiff >0 ) {
                    x = lo_x + (hi_x - lo_x)*(double)(i)/(double)(STEPS);
                    spDiff = factor * spreading_pressure_diff( B, P, x, Y_A );
                    i++;
                }

                lo_x = lo_x + (hi_x - lo_x)*(double)(i-2)/(double)(STEPS);
                hi_x = x;
            }
    
            double mid_x = (lo_x + hi_x) / 2.0;
    
            double lo  = Math.Abs(spreading_pressure_diff( B, P,  lo_x, Y_A ));
            double mid = Math.Abs(spreading_pressure_diff( B, P, mid_x, Y_A ));
            double hi  = Math.Abs(spreading_pressure_diff( B, P,  hi_x, Y_A ));
    
            if( lo < mid )
            {
                if( lo < hi )
                    return lo_x;
            } else {
                if( mid < hi )
                    return mid_x;
            }
    
            return hi_x;
        }
    }
}
