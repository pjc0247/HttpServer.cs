using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public class TargetProtocolAttribute : Attribute
    {
        public Type type { get; private set; }

        public TargetProtocolAttribute(Type type)
        {
            this.type = type;
        }
    }
}
