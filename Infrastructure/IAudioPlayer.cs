using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tikisoft.DesktopAlertsDemo.Infrastructure
{
    interface IAudioPlayer
    {
        bool PlayAudio(string audioFilePath, bool loop);
        void StopPlaying();
    }
}
