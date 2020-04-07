using System;
using System.Collections.Generic;
using System.Text;

namespace AudioPlayer.Event
{
    public delegate void SimpleEventHandler();
    public delegate void SimpleEventHandler<T>(T payload);
    public delegate void SimpleEventHandler<T1, T2>(T1 item1, T2 item2);
    public delegate void SimpleEventHandler<T1, T2, T3>(T1 item1, T2 item2, T3 item3);
}
