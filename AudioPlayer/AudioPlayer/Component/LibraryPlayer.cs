using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AudioPlayer.Model;

namespace AudioPlayer.Component
{
    public class LibraryPlayer
    {
        private List<LibraryEntry> _playlist;

        public LibraryPlayer()
        {
            _playlist = new List<LibraryEntry>();

            NAudio.Mixer.Mixer s;
        }


    }
}
