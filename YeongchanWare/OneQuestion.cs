using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongchanWare
{
    public struct OneQuestion
    {
        public string Text;
        public string S1, S2, S3, S4;

        public int Answer;

        public string Description;

        public OneQuestion(string text, string s1, string s2, string s3, string s4, int answer, string des)
        {
            Text = text;
            S1 = s1;
            S2 = s2;
            S3 = s3;
            S4 = s4;
            Answer = answer;
            Description = des;
        }
    }
}
