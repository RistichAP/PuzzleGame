using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace shabl
{
    public interface IImageSaver
    {
         Int32 ImageWidth { get; set; }
         Int32 ImageHeight { get; set; }
         String SaverImage { get; set; }
         String ImageVisibility { get; set; }
        
    }
    public interface IImageSaverImp
    {       
         void Saver();
    }
    public class ImageSaver:IImageSaver, INotifyPropertyChanged
    {
        private string _saverImage;
        private string _imageVis;
        private int _imW;
        private int _imH;
        public Int32 ImageWidth
        {
            get { return _imW; }
            set { _imW = value; OnPropertyChanged("ImageWidth"); }
        }
        public Int32 ImageHeight
        {
            get { return _imH; }
            set { _imH = value; OnPropertyChanged("ImageHeight"); }
        }

        public String SaverImage
        {
            get { return _saverImage; }
            set { _saverImage = value; OnPropertyChanged("SaverImage"); }
        }
        public String ImageVisibility
        {
            get { return _imageVis; }
            set { _imageVis = value; OnPropertyChanged("SaverVisibility"); }
        }
        private void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(s)); }
        }

        public ImageSaver(string s, int a, int b)
        {
            SaverImage = s;
            ImageHeight = a;
            ImageWidth = b;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
    public class ImageSaverImp:IImageSaverImp
    {

        private IImageSaver _imp;
        public ImageSaverImp(IImageSaver i)
        {
            _imp = i;
            Saver();        
        }

        public void Saver()
        {
            BitmapImage image = new BitmapImage(new Uri(_imp.SaverImage, UriKind.RelativeOrAbsolute));
            JpegBitmapEncoder JBE = new JpegBitmapEncoder();
            JBE.QualityLevel = 50;
            JBE.Frames.Add(BitmapFrame.Create(image));
            string fileName = @"D:\PuzzleImage.jpg";
            if (File.Exists(fileName)) { File.Delete(fileName); }
            FileStream fStream = new FileStream(fileName, FileMode.CreateNew);
            JBE.Save(fStream);
            fStream.Close();
        }
       
    }
}
