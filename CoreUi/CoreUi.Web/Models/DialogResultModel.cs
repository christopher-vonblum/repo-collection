using System;
using Newtonsoft.Json.Linq;

namespace CoreUi.Web.Models
{
    public class DialogResultModel
    {
        public Guid ResponseToken { get; set; }
        public JToken Data { get; set; }
    }
}