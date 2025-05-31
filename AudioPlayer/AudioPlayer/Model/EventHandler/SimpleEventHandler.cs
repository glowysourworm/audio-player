using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.Model.EventHandler
{
    public delegate void SimpleHandler<T>(T parameter);
    public delegate void SimpleHandler<T1, T2>(T1 parameter1, T2 parameter2);
    public delegate void SimpleHandler<T1, T2, T3>(T1 parameter1, T2 parameter2, T3 parameter3);
}
