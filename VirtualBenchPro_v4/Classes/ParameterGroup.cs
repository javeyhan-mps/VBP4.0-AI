using System.Collections.Generic;

namespace VirtualBenchPro_v4
{
    public class ParameterGroup
    {

        public string Name { get; set; }
        public string InfoParameterGroup { get; set; }
        public List<Parameter> Parameters { get; set; }

        public ParameterGroup(string name, string infoParameterGroup, List<Parameter> parameters)
        {
            Name = name;
            InfoParameterGroup = infoParameterGroup;
            Parameters = parameters;
        }
    }
}
