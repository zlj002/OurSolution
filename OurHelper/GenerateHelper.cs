using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OurHelper
{
    public class GenerateHelper
    {
        public static string GenerateRandomNumber(int Length)
        {
            char[] constant =   
              {   
                '0','1','2','3','4','5','6','7','8','9' 
              };
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

    }
}
