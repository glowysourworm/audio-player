using AcoustID.Web;
using AudioPlayer.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.Component
{
    public static class AcoustIDClient
    {
        /// <summary>
        /// Calculates library entry by audio fingerprint using an online api.
        /// </summary>
        public static async Task<ILibraryMetaEntry> IdentifyFingerprint(ILibraryEntry entry)
        {
            var context = new AcoustID.ChromaContext();

            using (var service = new LookupService())
            {

            }
        }
    }
}
