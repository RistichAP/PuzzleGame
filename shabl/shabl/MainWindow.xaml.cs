using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.Collections;
using System.Windows.Threading;



namespace shabl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Puzzle p;
        public PuzzleViewModel pvm;

        #region PuzzleMethods
        public void DoEPuzzle()
        {
            p = new Puzzle();
            p.CreateEasyPuzzle();
            pvm = new PuzzleViewModel(p);
            pvm.NotificatorEvent += this.HandleNotificatorEvent;
            this.DataContext = pvm;
        }
        public void DoNPuzzle()
        {
            p = new Puzzle();
            p.CreateNormalPuzzle();
            pvm = new PuzzleViewModel(p);
            pvm.NotificatorEvent += this.HandleNotificatorEvent;
            this.DataContext = pvm;
        }
        #endregion

        #region Buttons
        private void ChooseDiffActive(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1();
            w.Show();
            this.Close();
        }

        private void StartTimer(object sender, RoutedEventArgs e)
        {
            ForTimeCount time = new ForTimeCount();
            time.Timer();
            timerlabel.DataContext = time;
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            wpfplayer.Pause();
            MessageBox.Show("Game Paused", "Pause", MessageBoxButton.OK);
        }
        #endregion

        #region Music

        MusicPlayer mp;
        private void ChangeMusic(object sender, RoutedEventArgs e)
        {
            mp= new MusicPlayer();
            mp.ChangeSong();
            MusicPlayerViewModel mpvm = new MusicPlayerViewModel(mp);
            wpfplayer.DataContext = mpvm;
        }


        private void WindowUnloaded(object sender, RoutedEventArgs e)
        {
            wpfplayer.Stop();
        }


        private void IsActivated(object sender, EventArgs e)
        {
            wpfplayer.Play();
        }
        private void ChangeSong(object sender, RoutedEventArgs e)
        {
            mp.ChangeSong();
            MusicPlayerViewModel mpvm = new MusicPlayerViewModel(mp);
            wpfplayer.DataContext = mpvm;
        }

        #endregion            

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            if (PuzzleViewModel.Counter == PuzzleViewModel.MaxCounter)
            {
                IImageSaver isr = new ImageSaver(p.WholeImageSource, p.ImageHeight, p.ImageWidth);
                SaverImage.DataContext = isr;
                IImageSaverImp isri = new ImageSaverImp(isr);
            }
        }

        #region PieceMoving
            private Boolean isPieceDragged;
            private Point _startPoint;
            private double _originalLeft;
            private double _originalTop;
            private bool _isDown;
            private UIElement _originalElement;
            private SimpleCircleAdorner _overlayElement;

            public String PieceThis;
   
            private void PieceDrag(object sender, MouseButtonEventArgs e)
            {                 
                    _isDown = true;
                    _startPoint = e.GetPosition(PuzzleCanvas);
                    _originalElement = e.Source as UIElement;
                    _originalElement.CaptureMouse();
                    e.Handled = true;

                    PieceThis = (sender as Rectangle).Tag.ToString();
            }

            private void PieceDrop(object sender, MouseButtonEventArgs e)
            {
                if (_isDown)
                {
                    DragFinished(e);
                    e.Handled = true;
                }
                
            }
            void HandleNotificatorEvent(object sender, PuzzleNotificatorEventArgs e)
            {
                PuzzleCanvas.Children.Remove(PieceOnCanvas);
            }

            public Rectangle PieceOnCanvas;
            private void PieceMove(object sender, MouseEventArgs e)
            {

                if (_isDown)
                {
                    if ((isPieceDragged == false) && ((Math.Abs(e.GetPosition(PuzzleCanvas).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                        (Math.Abs(e.GetPosition(PuzzleCanvas).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    {
                        DragStarted();
                    }
                    if (isPieceDragged)
                    {
                        DragMoved();

                    }
                }

            }
            private void DragFinished(MouseEventArgs o)
            {
                System.Windows.Input.Mouse.Capture(null);
                if (isPieceDragged)
                {
                    AdornerLayer.GetAdornerLayer(_overlayElement.AdornedElement).Remove(_overlayElement);

                    PieceOnCanvas = new Rectangle();
                    PuzzleCanvas.Children.Add(PieceOnCanvas);
                    PieceOnCanvas.Width = ((Rectangle)_originalElement).Width;
                    PieceOnCanvas.Height = ((Rectangle)_originalElement).Height;
                    PieceOnCanvas.Fill = ((Rectangle)_originalElement).Fill;
                    Canvas.SetTop(PieceOnCanvas, (o.GetPosition(PuzzleCanvas).Y - o.GetPosition(_overlayElement).Y));
                    Canvas.SetLeft(PieceOnCanvas, (o.GetPosition(PuzzleCanvas).X - o.GetPosition(_overlayElement).X));
                    AdornerLayer.GetAdornerLayer(_overlayElement).Remove(_overlayElement);
                    
                    pvm.Checker(Convert.ToInt32(PieceThis), Canvas.GetLeft(PieceOnCanvas), Canvas.GetTop(PieceOnCanvas));
                    if (pvm.WinChecker())
                    {
                        MessageBox.Show("You won! You can save your image now.", "Congratulations!", MessageBoxButton.OK);
                    }
                    PieceOnCanvas = null;
                    _overlayElement = null;

                }

                isPieceDragged = false;
                _isDown = false;

            }
            private void DragStarted()
            {
                DragStart();

                _overlayElement = new SimpleCircleAdorner(_originalElement);
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(PuzzleCanvas);
                layer.Add(_overlayElement);

            }
            private void DragStart()
            {
                isPieceDragged = true;
                _originalLeft = Canvas.GetLeft(_originalElement);
                _originalTop = Canvas.GetTop(_originalElement);
            }
            private void DragStartedOnCanvas()
            {
                DragStart();

                _overlayElement = new SimpleCircleAdorner(_originalElement);
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(_originalElement);
                layer.Add(_overlayElement);

            }
            private void DragMoved()
            {
                Point CurrentPosition = System.Windows.Input.Mouse.GetPosition(PuzzleCanvas);

                _overlayElement.LeftOffset = CurrentPosition.X - _startPoint.X;
                _overlayElement.TopOffset = CurrentPosition.Y - _startPoint.Y;


            }
            private void PieceMovingOnCanvas(object sender, MouseEventArgs e)
            {
                if (_isDown)
                {
                    isPieceDragged = false;
                    if ((isPieceDragged == false) && ((Math.Abs(e.GetPosition(PuzzleCanvas).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                                        (Math.Abs(e.GetPosition(PuzzleCanvas).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    {
                        DragStartedOnCanvas();
                    }
                    if (isPieceDragged)
                    {
                        DragMoved();
                    }
                }
            }


        }
        public class SimpleCircleAdorner : Adorner
        {
            public DoubleAnimation animation;
            public SimpleCircleAdorner(UIElement adornedElement)
                : base(adornedElement)
            {
                VisualBrush _brush = new VisualBrush(adornedElement);

                _child = new Rectangle();
                _child.Width = adornedElement.RenderSize.Width;
                _child.Height = adornedElement.RenderSize.Height;


                animation = new DoubleAnimation(0.3, 1, new Duration(TimeSpan.FromSeconds(1)));
                animation.AutoReverse = true;
                animation.RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever;
                _brush.BeginAnimation(System.Windows.Media.Brush.OpacityProperty, animation);

                _child.Fill = _brush;
            }


            protected override void OnRender(DrawingContext drawingContext)
            {

                Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

                SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
                renderBrush.Opacity = 0.2;
                Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
                double renderRadius = 5.0;


                drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
            }

            protected override Size MeasureOverride(Size constraint)
            {
                _child.Measure(constraint);
                return _child.DesiredSize;
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                _child.Arrange(new Rect(finalSize));
                return finalSize;
            }

            protected override Visual GetVisualChild(int index)
            {
                return _child;
            }

            protected override int VisualChildrenCount
            {
                get
                {
                    return 1;
                }
            }

            public double LeftOffset
            {
                get
                {
                    return _leftOffset;
                }
                set
                {
                    _leftOffset = value;
                    UpdatePosition();
                }
            }

            public double TopOffset
            {
                get
                {
                    return _topOffset;
                }
                set
                {
                    _topOffset = value;
                    UpdatePosition();

                }
            }

            private void UpdatePosition()
            {
                AdornerLayer adornerLayer = this.Parent as AdornerLayer;
                if (adornerLayer != null)
                {
                    adornerLayer.Update(AdornedElement);
                }
            }

            public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
            {
                GeneralTransformGroup result = new GeneralTransformGroup();
                result.Children.Add(base.GetDesiredTransform(transform));
                result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));
                return result;
            }

            private Rectangle _child = null;
            private double _leftOffset = 0;
            private double _topOffset = 0;

        }
            #endregion


    }





