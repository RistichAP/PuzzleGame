using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace shabl
{
    public class Song 
    {      
        public String SongName;      
        public Song(string s)
        {
            SongName = s;
        }

    }

    public class MusicPlayer
    {
        public String NowPlayingSong { get; set; }
        public List<Song> playList { get; set; }
        public MusicPlayer()
        {
            playList = new List<Song>();
            playList.Add(new Song(@"Resources\Carlos Gardel – Por Una Cabeza.mp3"));
            playList.Add(new Song(@"Resources\Fabrizio Paterlini – Soffia la notte.mp3"));
            playList.Add(new Song(@"Resources\Franz Liszt – Liebestraum No. 3 In A Flat Major.mp3"));
            playList.Add(new Song(@"Resources\Морис Равель – Болеро.mp3"));
            playList.Add(new Song(@"Resources\Моцарт – Фантазия.mp3"));
            playList.Add(new Song(@"Resources\Ф. Шопен – Waltz In A Minor, Op. posth..mp3"));

        }
        public void ChangeSong()
        {
            Random randomizeSong = new Random();
            NowPlayingSong = (playList[randomizeSong.Next(0, playList.Count())]).SongName;
        }

    }

}




