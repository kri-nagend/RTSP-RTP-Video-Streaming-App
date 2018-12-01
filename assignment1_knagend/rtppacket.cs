using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment1_knagned
{
    class rtppacket
    {
        // initalizing the variables
        int response;
        byte[] myArray;

        // constructor w/ the initial response number set
        public rtppacket()
        {
            response = 200;
        }

        // function to allow the print header check to print the header
        public byte[] header()
        {
            return myArray;
        }
        
        // fucntion that assigns the frames to an array, which creates a packet
        public byte[] encap(byte[] frame)
        {
            byte[] ary = new byte[frame.Length+12];
            
            for (int i = 0; i < frame.Length; i++)
            {
                // assigns the frames because the first 12 are preset
                ary[12 + i] = frame[i];
            }
            // copies the created array to the array used for the header function
            myArray = ary;
            // returns the created array to the rtp class
            return ary;
        }
    }
}
