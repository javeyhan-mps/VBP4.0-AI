namespace VirtualBenchPro_v4
{
    public class Parameter
    {
        public string Name { get; set; }
        public string InfoParameter { get; set; }
        public string BitControl_Ref { get; set; }

        public Parameter(string name, string infoParameter, string bitControlRef)
        {
            Name = name;
            InfoParameter = infoParameter;
            BitControl_Ref = bitControlRef;
        }
    }
}
