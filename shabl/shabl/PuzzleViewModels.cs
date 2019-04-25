using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shabl
{
    
    public delegate void NotificatorEventHandler(object sender, PuzzleNotificatorEventArgs e);
    public class PuzzleNotificatorEventArgs : EventArgs
    {
        public PuzzleNotificatorEventArgs(object ob)
        {
            piece = (PuzzlePieceViewModel)ob;
        }
        private PuzzlePieceViewModel piece;
        public PuzzlePieceViewModel Piece
        {
            get { return piece; }
        }
    }

   public class PuzzleViewModel:INotifyPropertyChanged
    {
       public static int Counter = 0;
       public static int MaxCounter;

       private ObservableCollection<PuzzlePieceViewModel> _puzzleList;
       public ObservableCollection<PuzzlePieceViewModel> PuzzleList
       {
           get
           { return _puzzleList; }
           set
           {
               _puzzleList = value;
               if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("PuzzleList")); }
           }
       }
       private String _imagesource;
       private Int32 _imageHeight;
       private Int32 _imageWidth;
      
       public Int32 ImageHeight
       {
           get { return _imageHeight; }
           set { _imageHeight = value; OnPropertyChanged("ImageHeight"); }
       }
       public Int32 ImageWidth
       {
           get { return _imageWidth; }
           set { _imageWidth = value; OnPropertyChanged("ImageWidth"); }
       }     
       public String WholeImageSource 
       { 
           get { return _imagesource; }
           set 
           { 
               _imagesource = value;
               OnPropertyChanged("WholeImageSource");
           }
       }

       private void OnPropertyChanged(string s)
       {
           if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(s)); }
       }

       public PuzzleViewModel(Puzzle model)
       {
           this.ImageHeight = model.ImageHeight;
           this.ImageWidth = model.ImageWidth;
           this.WholeImageSource = model.WholeImageSource;
           MaxCounter = model.MaxCounter;
           PuzzleList = new ObservableCollection<PuzzlePieceViewModel>();
           foreach (var e in model.PuzzleList)
           {
               this.PuzzleList.Add(new PuzzlePieceViewModel(e));
           }
      
       }

       public event PropertyChangedEventHandler PropertyChanged;

       public event NotificatorEventHandler NotificatorEvent;

       public void Checker(Int32 index, Nullable<double> a, Nullable<double> b)
       {
          
           if ((a < (PuzzleList[index]._x + 7)) && (a > 0) && (a > PuzzleList[index]._x - 7) && (b < PuzzleList[index]._y + 7) && (b > 0) && (b > PuzzleList[index]._y - 7)) 
           {
               Counter++;
               PuzzleList.RemoveAt(index);
               foreach (var e in PuzzleList)
               {
                   e.PieceName = PuzzleList.IndexOf(e).ToString();
               }
           }
           else { OnNotificatorEvent(new PuzzleNotificatorEventArgs(PuzzleList[index])); }
       }
       protected virtual void OnNotificatorEvent(PuzzleNotificatorEventArgs e)
       {
           NotificatorEventHandler handler = NotificatorEvent;
           if (handler != null) { handler(this, e); }
       }
       public Boolean WinChecker()
       {
           if (MaxCounter == Counter) { return true; }
           else { return false; }
       }
    }

   public class PuzzlePieceViewModel:INotifyPropertyChanged
   {
       protected int _width;
       protected int _height;
       protected string _source;
       public double _x;
       public double _y;
       private string _pname;
       public String PieceName
       {
           get { return _pname; }
           set { _pname = value; OnPropertyCanged("PieceName"); }
       }
       public Int32 PieceWidth
       { 
           get
           {
               return _width;
           } 
           set
           {
               _width=value;
               OnPropertyCanged("Widht");
           } 
       }
       public Int32 PieceHeight
       {
           get
           {
               return _height;
           }
           set
           {
               _height = value;
               OnPropertyCanged("Height");
           }
       }
       public String PieceSourse
       {
           get
           {
               return _source;
           }
           set
           {
               _source = value;
               OnPropertyCanged("Sourse");
           }
       }
       public PuzzlePieceViewModel(PuzzlePiece model)
       {
           this.PieceSourse = model.PieceSourse;
           this._x = model.X;
           this._y = model.Y;
           this.PieceHeight = model.PieceHeight;
           this.PieceWidth = model.PieceWidth;
           this.PieceName = model.NamePiece;
       }       
       public void OnPropertyCanged(string q)
       {
           if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(q)); }
       }

       public event PropertyChangedEventHandler PropertyChanged;
   }

}
