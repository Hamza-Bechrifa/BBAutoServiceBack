namespace Gateway.Helpers
{
    public class LastLine
    {
        //public string numberA { get; set; }
        //public string numberU { get; set; }
        //public string numberR { get; set; }
        //public string numberD { get; set; }
        public string numberT { get; set; }
        public bool FormatLastLine { get; set; }
        public LastLine(string line)
        {
            char delimiter = ',';
            string[] substrings = line.Split(delimiter);
            if (substrings.Length == 3)
            {
                numberT = substrings[2];

                if (numberT.Length >= 1)
                    FormatLastLine = true;
                else
                    FormatLastLine = false;
            }
            else
            {
                FormatLastLine = false;

            }
            ////Char delimiter = ',';
            //if (line.Length >= 6)
            //{

            //}
            //else
            //{
            //    FormatLastLine = false;
            //}
            //string substrings = line.Substring(5, 1);
            //if (substrings.Length == 9)
            //{
            //    //numberA = substrings[0].Replace("&", "");
            //    //numberU = substrings[1];
            //    //numberR = substrings[2];
            //    //numberD = substrings[3];
            //    numberT = substrings;

            //    FormatLastLine = true;

            //}
            //else
            //{
            //    FormatLastLine = false;

            //}

            //   if (ControleFileNameFormat()) { throw new Exception("Error format file name."); }
        }
    }
}