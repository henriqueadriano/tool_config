using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.models
{
    public class ObjFields
    {
        public string server { get; set; }
        public string database { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string path { get; set; }
        public bool IsValid { get; set; }
        public string StringConn { get; set; }
    }
}
