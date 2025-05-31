using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AudioPlayer.Extension;

namespace AudioPlayer.Model
{
    public class Album : ModelBase
    {
        string _name;
        uint _year;
        SortedObservableCollection<Artist> _artists;
    }
}
