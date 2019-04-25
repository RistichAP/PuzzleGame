using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shabl
{
    public class Puzzle
    {
        public int MaxCounter;
        public String WholeImageSource { get; set; }
        public Int32 ImageWidth { get; set; }
        public Int32 ImageHeight { get; set; }
        public ObservableCollection<PuzzlePiece> PuzzleList { get; set; }
        private List<int> collection = new List<int>();
        private Random r = new Random();
        public void CreateNormalPuzzle()
        {
            MaxCounter = 54;
            PuzzleList = new ObservableCollection<PuzzlePiece>();
            ImageHeight = 480;
            ImageWidth = 240;
            int n = r.Next(1, 3);
            WholeImageSource = String.Format(@"C:\Users\pc\Source\Repos\NewRepo\shabl\shabl\normal_whole{0}.jpg", n);
            PuzzlePiece piecefirstsmall;
            piecefirstsmall = new PuzzlePieceTypeNormalSmall(String.Empty, 0, 0, String.Empty);
            int f = 0;
            for (int i = 0; i < 36; i++)
            {
                while (collection.Contains(f))
                {
                    f = r.Next(0, 36);
                }
                collection.Add(f);
                PuzzleList.Add(piecefirstsmall.PuzzlePieceClone(String.Format(@"C:\Users\pc\Source\Repos\NewRepo\shabl\shabl\Images\norm{0}_1_ ({1}).jpeg", n, f + 1), (f % 6) * 50, (f / 6 + 2 + 2 * (f / 12)) * 50, i.ToString()));
            }
            collection.Clear();
            piecefirstsmall = new PuzzlePieceTypeNormalBig(String.Empty, 0, 0, String.Empty);
            for (int i = 0; i < 18; i++)
            {
                while (collection.Contains(f))
                {
                    f = r.Next(0, 18);
                }
                collection.Add(f);
                PuzzleList.Add(piecefirstsmall.PuzzlePieceClone(String.Format(@"C:\Users\pc\Source\Repos\NewRepo\shabl\shabl\Images\norm{0}_2_ ({1}).jpeg", n, f + 1), (f % 6) * 50, 400 * (f / 6), (i+36).ToString()));
            }

        }
        public void CreateEasyPuzzle()
        {
            int n = r.Next(1, 4);
            MaxCounter = 20;
            PuzzleList = new ObservableCollection<PuzzlePiece>();
            WholeImageSource = String.Format(@"C:\Users\pc\Source\Repos\NewRepo\shabl\shabl\easy_whole{0}.jpeg", n);
            ImageHeight = 498;
            ImageWidth = 403;
            PuzzlePiece piecefirst = new PuzzlePieceTypeEasy(String.Empty, 0, 0, String.Empty);
            int f = 0;
            for (int i = 0; i < 20; i++)
            {
                while (collection.Contains(f))
                {
                    f = r.Next(0, 20);
                }
                collection.Add(f);
                PuzzleList.Add(piecefirst.PuzzlePieceClone(String.Format(@"C:\Users\pc\Source\Repos\NewRepo\shabl\shabl\Images\easy{0}_ ({1}).jpeg", n, f + 1), (f % 4) * 100, (f / 4) * 100, i.ToString()));
            }
        }
    }

    public abstract class PuzzlePiece
    {
        public String NamePiece { get; set; }
        public Int32 PieceWidth { get; set; }
        public Int32 PieceHeight { get; set; }
        public String PieceSourse { get; set; }
        public Double X { get; set; }
        public Double Y { get; set; }

        public PuzzlePiece(string s, double x, double y, string a)
        {
            PieceSourse = s;
            X = x;
            Y = y;
            NamePiece = a;
        }
        public abstract PuzzlePiece PuzzlePieceClone(string s, double x, double y, string a);
    }

    public class PuzzlePieceTypeNormalSmall: PuzzlePiece
    {
        public PuzzlePieceTypeNormalSmall(string s, double x, double y, string a)
            : base(s, x, y, a)
        {
            PieceWidth = 40;
            PieceHeight = 40;
        }

        public override PuzzlePiece PuzzlePieceClone(string s, double x, double y, string a)
        {
            return new PuzzlePieceTypeNormalSmall(s, x, y, a);
        }

    }
    public class PuzzlePieceTypeNormalBig : PuzzlePiece
    {
        public PuzzlePieceTypeNormalBig(string s, double x, double y, string a)
            : base(s, x, y, a)
        {
            PieceWidth = 40;
            PieceHeight = 80;
        }

        public override PuzzlePiece PuzzlePieceClone(string s, double x, double y, string a)
        {
            return new PuzzlePieceTypeNormalBig(s, x, y, a);
        }

    }
    public class PuzzlePieceTypeEasy : PuzzlePiece
    {
        public PuzzlePieceTypeEasy(string s, double x, double y, string a)
            : base(s, x, y, a)
        {
            PieceWidth = 100;
            PieceHeight = 100;
        }

        public override PuzzlePiece PuzzlePieceClone(string s, double x, double y, string a)
        {
            return new PuzzlePieceTypeEasy(s, x, y, a);
        }
    }
}
