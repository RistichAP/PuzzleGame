using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shabl
{
    public class MusicPlayerViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected string _nowPlayingSong;
        public String NowPlayingSong
        {
            get { return _nowPlayingSong; }
            set
            {
                _nowPlayingSong = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("NowPlayingSong")); }
            }
        }
        public List<SongViewModel> PlayList = new List<SongViewModel>();
        public MusicPlayerViewModel(MusicPlayer model)
        {
            this.NowPlayingSong = model.NowPlayingSong;
            foreach (var i in model.playList)
            {
                this.PlayList.Add(new SongViewModel(i));
            }
        }
    }
    public class SongViewModel
    {
        public String SongName { get; set; }
        public SongViewModel(Song model)
        {
            this.SongName = model.SongName;
        }
    }
}
