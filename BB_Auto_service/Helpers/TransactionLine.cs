namespace Gateway.Helpers
{
    public class TransactionLine
    {
        public string IdTerminal { get; set; }
        public string TransactionReference { get; set; }
        public string AuthDate { get; set; }
        public string ReferenceOT { get; set; }
        public string DateRefOT { get; set; }
        public string Amount { get; set; }
        public string Operation { get; set; }
        public bool FormatTRN { get; set; }
        public TransactionLine(string line)
        {
            char delimiter = ',';
            string[] substrings = line.Split(delimiter);
            if (substrings.Length == 7)
            {
                IdTerminal = substrings[0].Replace("$", "");
                TransactionReference = substrings[1];
                AuthDate = substrings[2];
                ReferenceOT = substrings[3];
                DateRefOT = substrings[4];
                Amount = substrings[5];
                Operation = substrings[6];

                if (IdTerminal.Length == 10 
                    && TransactionReference.Length == 6 
                    && AuthDate.Length == 8 
                    && ReferenceOT.Length == 8 
                    && DateRefOT.Length == 8
                    && Amount.Length == 11 
                    && Operation.Length == 1
                    && (Operation == "P" || Operation == "A"))
                    FormatTRN = true;
                else
                    FormatTRN = false;
            }
            else
            {
                FormatTRN = false;

            }
        }
    }
}