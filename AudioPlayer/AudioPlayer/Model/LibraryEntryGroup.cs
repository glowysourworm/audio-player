using System.Collections.Generic;

namespace AudioPlayer.Model
{
    public class LibraryEntryGroup
    {
        readonly IList<LibraryEntry> _titles;
        readonly string _name;

        public IEnumerable<LibraryEntry> Titles
        {
            get { return _titles; }
        }

        public string Name
        {
            get { return _name; }
        }

        public LibraryEntryGroup(string name, IList<LibraryEntry> titles)
        {
            _name = name;
            _titles = titles;
        }

        public void AddTitle(LibraryEntry entry)
        {
            _titles.Add(entry);
        }
    }
}
