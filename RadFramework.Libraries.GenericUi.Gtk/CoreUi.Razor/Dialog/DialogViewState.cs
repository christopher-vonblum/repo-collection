using System;
using Newtonsoft.Json;

namespace CoreUi.Razor.Dialog
{
    public class DialogViewState
    {
        [JsonIgnore] public Guid DialogId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Z { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}