using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_knagend
{
    class rtppacket
    {
        // initializes the variables
        byte[] head;

        // class contructor 
        public rtppacket()
        {
            head = new byte[12];

        }

        // function returns the header info as a string
        public string Header()
        {
            String header = string.Join(" ", head.Select(b => Convert.ToString(b, 2).PadLeft(8, '0').ToUpper()));
            header += "\r\n";
            return header;
        }

        // function to seperate header info from data
        public byte[] seperate(byte[] info)
        {
            // this array stores the info except the header
            byte[] parsed = new byte[999988];

            // stores the header information 
            for (int i = 0; i < 12; i++)
            {
                head[i] = info[i];
            }
            // stores the info without the header
            for (int i = 12; i < info.Length; i++)
            {
                parsed[i - 12] = info[i];
            }
            return parsed;
        }

       
    }
}
