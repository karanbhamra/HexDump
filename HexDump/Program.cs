using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HexConverter
{
    class Program
    {
        const string HexAlphabet = "0123456789ABCDEF";

        static string ReverseString(string input)
        {
            var charArray = input.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }

        static string DecToHex(int dec)
        {
            StringBuilder output = new StringBuilder();

            while (dec != 0)
            {
                int remainder = dec % 16;

                output.Append(HexAlphabet[remainder]);

                dec /= 16;
            }


            return ReverseString(output.ToString());
        }

        static int HexToDec(string hex)
        {
            int sum = 0;
            int mult = 1;
            for (int i = hex.Length - 1; i >= 0; i--)
            {
                int value = HexAlphabet.IndexOf(hex[i]) * mult;

                mult *= 16;

                sum += value;
            }

            return sum;
        }

        static string PadHexVal(string hex, int paddingSize)
        {
            while (hex.Length < paddingSize)
            {
                hex = "0" + hex;
            }

            return hex;
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please pass the filename as the parameter.");
                return;
            }

            string filename = args[0];
            var bytes = File.ReadAllBytes(filename);

            // read 16 bytes at a time
            for (int i = 0; i < bytes.Length; i += 16)
            {
                var bytesList = new List<string>();
                var asciiList = new List<string>();

                for (int j = i; j < i + 16; j++)
                {
                    if (j < bytes.Length)
                    {
                        // get the current byte and convert it to hex and convert it to its char representation
                        byte current = bytes[j];
                        string hexVal = DecToHex(current);
                        string paddedHexVal = PadHexVal(hexVal, 2);
                        bytesList.Add(paddedHexVal);

                        string asciiValue = ((char) current).ToString();

                        // any unreadable character is displayed as a period
                        if (current < 33 || current > 126)
                        {
                            asciiList.Add(".");
                        }
                        else
                        {
                            asciiList.Add(asciiValue);
                        }
                    }
                    else
                    {
                        // if we run out, pad it with empty space
                        bytesList.Add("  ");    
                    }
                }

                // pad the line number to be length of 8
                string lineCountHex = PadHexVal(DecToHex(i), 8);

                Console.Write(lineCountHex + "\t");

                // print out the bytes
                int count = 0;
                foreach (var byteString in bytesList)
                {
                    Console.Write(byteString + " ");

                    // Add extra space to split up the 16 byte line into 8 and 8 bytes for better readability
                    if (count == 7)
                    {
                        Console.Write(" ");
                    }

                    count++;
                }
                
                // display the ascii values of the bytes in the same line
                Console.Write("\t|");

                foreach (var asciiVal in asciiList)
                {
                    Console.Write(asciiVal);
                }

                Console.WriteLine("|");
            }
        }
    }
}