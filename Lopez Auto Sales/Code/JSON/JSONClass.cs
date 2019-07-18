using System.Collections.Generic;

namespace Lopez_Auto_Sales.JSON
{
    public class JSONClass
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }
        public IList<JSONResult> Results { get; set; }
    }

    public class JSONResult
    {
        public string Value { get; set; }
        public string ValueId { get; set; }
        public string Variable { get; set; }
        public int VariableId { get; set; }
    }
}