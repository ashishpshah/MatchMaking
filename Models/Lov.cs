using System;
using System.Collections.Generic;

namespace MatchMaking.Models
{
    public partial class Lov
    {
        public string LovColumn { get; set; } = null!;
        public string LovCode { get; set; } = null!;
        public string LovDesc { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
}
