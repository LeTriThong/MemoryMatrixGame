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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeTriThong_MemoryMatrixGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int side_length = 50; //Kích thước của 1 ô vuông  
        const int line_thickness = 5;   //Bề dày của 1 ô vuông

        const int maxTile_Width = 8;    //Số lượng ô vuông tối đa theo chiều dài
        const int maxTile_Height = 6;   //Số lượng ô vuông tối đa theo chiều rộng

        const int max_level = 24;   //Cấp độ tối đa
        const int max_lives = 3;    //Số mạng tối đa

        //Margin của bảng ô vuông
        const int margin_left = 800 / 2 - (maxTile_Width / 2) * (side_length + line_thickness); 
        const int margin_top = 600 / 2 - (maxTile_Height / 2) * (side_length + line_thickness);
      
        //Kích thước tối đa của bảng ô vuông
        const int rec_maxWidth = side_length * maxTile_Width + (maxTile_Width + 1) * line_thickness;
        const int rec_maxHeight = side_length * maxTile_Height + (maxTile_Height + 1) * line_thickness;

        const int score_per_tiles = 500;    //Điểm nhận được với mỗi ô chính xác
        const int bonus_per_level = 200;    //Điểm cộng thêm với mỗi cấp độ
      
        //Padding của bảng ô vuông so với bảng tối đa
        int padding_top = 0;
        int padding_left = 0;

        
        int width_tiles = 0;    //Chiều dài bảng hiện tại (tính theo ô)
        int height_tiles = 0;   //Chiều rộng bảng hiện tại (tính theo ô) 
        int level_number = 0;   //Cấp độ hiện tại
        int numberOfChosenTiles = 0;    //Số ô đã được chọn hiện tại
        int lives = 0; //Số mạng còn lại
        int player_score = 0; //Điểm hiện tại

        int[,] _numberMatrix;   //Ma trận chứa trạng thái các ô vuông (0: không được đánh dấu; 1: được đánh dấu; 2: được đánh dấu và được chọn)
        List<Rectangle> _rectangles = new List<Rectangle>(); //Danh sách chứa các ô vuông

        Image imgFinalResult = new Image(); //Hình thể hiện kết quả cuối cùng của 1 level

        bool isWin = false; //Trạng thái cho biết người chơi đã thắng hay chưa  
        bool _isPlayable = false;   //Trạng thái cho biết người chơi có thể tiếp tục click vào các ô vuông hay không

        Random tilesRandom = new Random();  //Biến random các ô được chọn

        System.Windows.Threading.DispatcherTimer delayBeforeShowingBoardTilesTimer = new System.Windows.Threading.DispatcherTimer(); //Thời gian chờ trước khi hiện bảng ô vuông
        System.Windows.Threading.DispatcherTimer delayBeforeShowingTilesTimer = new System.Windows.Threading.DispatcherTimer();   //Thời gian chờ trước khi hiện các ô vuông được đánh dấu
        System.Windows.Threading.DispatcherTimer showingTimer = new System.Windows.Threading.DispatcherTimer(); //Thời gian hiện các ô được chọn
        System.Windows.Threading.DispatcherTimer delayBeforeNextLevelTimer = new System.Windows.Threading.DispatcherTimer(); //Thời gian chờ trước khi vào màn tiếp theo
        System.Windows.Threading.DispatcherTimer delayBeforeGameEndsTimer = new System.Windows.Threading.DispatcherTimer(); //Thời gian chờ trước khi màn hình kết thúc game xuất hiện


        SolidColorBrush chosenTileBrush = new SolidColorBrush(Color.FromRgb(74, 179, 184)); //Màu của ô được chọn chính xác
        SolidColorBrush unchosenTileBrush = new SolidColorBrush(Color.FromRgb(113, 74, 67)); //Màu của ô chưa được chọn
        SolidColorBrush correctTileBrush = new SolidColorBrush(Color.FromRgb(80, 185, 72)); //Màu của ô được chọn chính xác cuối cùng
        SolidColorBrush incorrectTileBrush = new SolidColorBrush(Color.FromRgb(236, 96, 21)); //Màu của ô chọn sai
        SolidColorBrush transparentBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)); //Màu trong suốt
        SolidColorBrush marginBrush = new SolidColorBrush(Color.FromRgb(61, 44, 39)); //Màu của viền các ô trống


        public MainWindow()
        {
            InitializeComponent();
        }

        //-----------------------------------------
        //-------- Xử lý giao diện ----------------
        //-----------------------------------------

        /// <summary>
        /// Hiện số mạng còn lại của người chơi
        /// </summary>
        /// <param name="l">Số mạng</param>
        private void ShowPlayerLives(int l)
        {
            if (l == 3)
            {
                rtgSeriesNum1.Fill = correctTileBrush;
                rtgSeriesNum2.Fill = correctTileBrush;
                rtgSeriesNum3.Fill = correctTileBrush;
            }
            else if (l == 2)
            {
                rtgSeriesNum3.Fill = transparentBrush;
            }
            else if (l == 1)
            {
                rtgSeriesNum2.Fill = transparentBrush;
            }
            else
            {
                rtgSeriesNum1.Fill = transparentBrush;
                _isPlayable = false;
            }

        }

        /// <summary>
        /// Hiện các ô vuông đã được chọn trong bảng
        /// </summary>
        private void ShowChosenTiles()
        {
            for (int i = 0; i < height_tiles; i++)
            {
                for (int j = 0; j < width_tiles; j++)
                {
                    if (_numberMatrix[i, j] == 1)
                        LightenOneTile(i, j);
                }
            }
        }

        /// <summary>
        /// Ẩn các ô vuông đã được chọn trong bảng
        /// </summary>
        private void HideChosenTiles()
        {
            for (int i = 0; i < height_tiles; i++)
            {
                for (int j = 0; j < width_tiles; j++)
                {
                    if (_numberMatrix[i, j] == 1)
                        UnlightenOneTile(i, j);
                }
            }
        }

        /// <summary>
        /// Hiện một ô vuông tại hàng h, cột w
        /// </summary>
        /// <param name="w">vị trí cột</param>
        /// <param name="h">vị trí hàng</param>
        private void LightenOneTile(int w, int h)
        {
            canvas.Children.Remove(_rectangles[w * width_tiles + h]);
            _rectangles[w * width_tiles + h].Fill = chosenTileBrush;
            canvas.Children.Add(_rectangles[w * width_tiles + h]);
        }

        /// <summary>
        /// Ẩn một ô vuông tại hàng h, cột w
        /// </summary>
        /// <param name="w">vị trí cột</param>
        /// <param name="h">vị trí hàng</param>
        private void UnlightenOneTile(int w, int h)
        {
            canvas.Children.Remove(_rectangles[w * width_tiles + h]);
            _rectangles[w * width_tiles + h].Fill = unchosenTileBrush;
            canvas.Children.Add(_rectangles[w * width_tiles + h]);
        }

        /// <summary>
        /// Hiện bảng các ô vuông lên màn hình
        /// </summary>
        /// <param name="w_tiles">Số cột</param>
        /// <param name="h_tiles">Số hàng</param>
        private void CreateBoardTiles(int w_tiles, int h_tiles)
        {          
            width_tiles = w_tiles;
            height_tiles = h_tiles;

            int board_width = w_tiles * side_length + (w_tiles + 1) * line_thickness;
            int board_height = h_tiles * side_length + (h_tiles + 1) * line_thickness;

            padding_top = (rec_maxHeight - board_height) / 2;
            padding_left = (rec_maxWidth - board_width) / 2;

            DeleteBoardTiles();

            for (int j = 0; j < h_tiles; j++)
            {
                for (int i = 0; i < w_tiles; i++)
                {
                    Rectangle boardTile = new Rectangle();
                    boardTile.Width = 60;
                    boardTile.Height = 60;
                    boardTile.StrokeThickness = 5;
                    boardTile.Stroke = marginBrush;
                    boardTile.Fill = unchosenTileBrush;

                    Canvas.SetLeft(boardTile, margin_left + padding_left + 55 * i);
                    Canvas.SetTop(boardTile, margin_top + padding_top + 55 * j);
                    _rectangles.Add(boardTile);
                }
            }

            foreach (var rect in _rectangles)
                canvas.Children.Add(rect);
        }

        /// <summary>
        /// Xóa toàn bộ bảng
        /// </summary>
        private void DeleteBoardTiles()
        {
            if (canvas.Children.Contains(imgFinalResult))
                canvas.Children.Remove(imgFinalResult);
            if (_rectangles.Count > 0)
            {
                foreach (var rect in _rectangles)
                {
                    canvas.Children.Remove(rect);
                }
                _rectangles.Clear();
            }
        }

        /// <summary>
        /// Thiết lập màn hình trong game
        /// </summary>
        private void SetupGameScreen()
        {
            lblGameTitle.Content = "";
            lblLevelResult.Content = "";
            lblScoreResult.Content = "";
            btnPlay.Visibility = System.Windows.Visibility.Hidden;

            lblCurrentLevel.Content = level_number;
            lblCurrentScore.Content = player_score;

            lblLevelTitle.Visibility = System.Windows.Visibility.Visible;
            lblCurrentLevel.Visibility = System.Windows.Visibility.Visible;
            lblScoreTitle.Visibility = System.Windows.Visibility.Visible;
            lblCurrentScore.Visibility = System.Windows.Visibility.Visible;
            lblBonusScore.Visibility = System.Windows.Visibility.Visible;
            stackpanelLives.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowTheTileAfterClicking(int i, int j, int result)
        {
            if (result == 0)
            {
                canvas.Children.Remove(_rectangles[i * width_tiles + j]);

                imgFinalResult.Source = new BitmapImage(new Uri("Images/incorrectTile.png", UriKind.Relative));
                imgFinalResult.Height = 50;
                imgFinalResult.Width = 50;

                Canvas.SetLeft(imgFinalResult, margin_left + padding_left + 55 * j + 5);
                Canvas.SetTop(imgFinalResult, margin_top + padding_top + 55 * i + 5);

                _rectangles[i * width_tiles + j].Fill = incorrectTileBrush;
                canvas.Children.Add(_rectangles[i * width_tiles + j]);
                canvas.Children.Add(imgFinalResult);
            }
            else if (result == 1)
            {
                canvas.Children.Remove(_rectangles[i * width_tiles + j]);

                imgFinalResult.Source = new BitmapImage(new Uri("Images/correctTile.png", UriKind.Relative));
                imgFinalResult.Height = 50;
                imgFinalResult.Width = 50;

                Canvas.SetLeft(imgFinalResult, margin_left + padding_left + 55 * j + 5);
                Canvas.SetTop(imgFinalResult, margin_top + padding_top + 55 * i + 5);
                _rectangles[i * width_tiles + j].Fill = correctTileBrush;
                lblCurrentScore.Content = player_score;
                lblBonusScore.Content = "+" + (200 * level_number).ToString();

                canvas.Children.Add(_rectangles[i * width_tiles + j]);
                canvas.Children.Add(imgFinalResult);
            }
            else if (result == 2)
            {
                canvas.Children.Remove(_rectangles[i * width_tiles + j]);
                _rectangles[i * width_tiles + j].Fill = chosenTileBrush;
                lblCurrentScore.Content = player_score;
                canvas.Children.Add(_rectangles[i * width_tiles + j]);
            }

        }

        
        /// <summary>
        /// Hiện màn hình chiến thắng
        /// </summary>
        private void ShowLosingScreen()
        {
            DeleteBoardTiles();
            lblCurrentLevel.Visibility = System.Windows.Visibility.Hidden;
            lblCurrentScore.Visibility = System.Windows.Visibility.Hidden;
            lblScoreTitle.Visibility = System.Windows.Visibility.Hidden;
            lblLevelTitle.Visibility = System.Windows.Visibility.Hidden;
            lblBonusScore.Visibility = System.Windows.Visibility.Hidden;

            stackpanelLives.Visibility = System.Windows.Visibility.Hidden;

            lblGameTitle.Visibility = System.Windows.Visibility.Visible;
            lblGameTitle.Content = "Game over";

            lblLevelResult.Visibility = System.Windows.Visibility.Visible;
            lblLevelResult.Content = "Tiles: " + level_number;

            lblScoreResult.Visibility = System.Windows.Visibility.Visible;
            lblScoreResult.Content = "Score: " + player_score;

            btnPlay.Visibility = System.Windows.Visibility.Visible;
            btnPlay.Content = "Play again";
        }

        /// <summary>
        /// Hiện màn hình thua cuộc
        /// </summary>
        private void ShowWinningScreen()
        {
            DeleteBoardTiles();
            lblCurrentLevel.Visibility = System.Windows.Visibility.Hidden;
            lblCurrentScore.Visibility = System.Windows.Visibility.Hidden;
            lblScoreTitle.Visibility = System.Windows.Visibility.Hidden;
            lblLevelTitle.Visibility = System.Windows.Visibility.Hidden;
            lblBonusScore.Visibility = System.Windows.Visibility.Hidden;

            stackpanelLives.Visibility = System.Windows.Visibility.Hidden;

            lblGameTitle.Visibility = System.Windows.Visibility.Visible;
            lblGameTitle.Content = "You win!";

            lblScoreResult.Visibility = System.Windows.Visibility.Visible;
            lblScoreResult.Content = "Score: " + player_score;

            btnPlay.Visibility = System.Windows.Visibility.Visible;
            btnPlay.Content = "Play again";
        }

        //---------------------------------
        //------ Xử lý nghiệp vụ ----------
        //---------------------------------

        /// <summary>
        /// Xử lý sự kiện bấm nút Play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            SetupGameScreen();

        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn chuột trái
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isPlayable == true)
            {
                var position = e.GetPosition(this);
                int i = ((int)position.Y - padding_top - margin_top);
                int j = ((int)position.X - padding_left - margin_left);
                if ((i < 0) || (j < 0))
                    return;
                i = i / 55;
                j = j / 55;

                

                if ((i < height_tiles) && (j < width_tiles))
                {
                    if (_numberMatrix[i, j] == 2)
                        return;

                    numberOfChosenTiles++;

                    if (_numberMatrix[i, j] == 0)
                    {
                        _isPlayable = false;
                        ShowTheTileAfterClicking(i, j, 0);
                        ShowChosenTiles();
                        ShowPlayerLives(--lives);

                        if (lives != 0)
                            delayBeforeNextLevelTimer.Start();
                        else
                            delayBeforeGameEndsTimer.Start();
                    }
                    else
                    {
                        _numberMatrix[i, j] = 2;
                        if (numberOfChosenTiles == level_number)
                        {
                            _isPlayable = false;
                            player_score = player_score + score_per_tiles + bonus_per_level * level_number;

                            ShowTheTileAfterClicking(i, j, 1);

                            level_number++;
                            if (level_number > max_level)
                            {
                                isWin = true;
                                delayBeforeGameEndsTimer.Start();
                            }
                            else
                                delayBeforeNextLevelTimer.Start();
                        }
                        else
                        {
                            player_score += score_per_tiles;
                            ShowTheTileAfterClicking(i, j, 2);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Cài đặt thời gian hiện ô đã được chọn tùy theo cấp độ
        /// </summary>
        /// <param name="level">Cấp độ</param>
        private void SetupShowingTimePerLevel(int level)
        {           
            showingTimer.Interval = new TimeSpan(0, 0, 0, 0, 1250 + (level - 3) / 3 * 250);
        }

        /// <summary>
        /// Cài đặt các đồng hồ
        /// </summary>
        private void SetupTimer()
        {
            showingTimer.Tick += new EventHandler(showingTimer_Tick);

            delayBeforeShowingTilesTimer.Tick += new EventHandler(delayBeforeShowingTilesTimer_Tick);
            delayBeforeShowingTilesTimer.Interval = new TimeSpan(0, 0, 1);

            delayBeforeNextLevelTimer.Tick += new EventHandler(delayBeforeNextLevelTimer_Tick);
            delayBeforeNextLevelTimer.Interval = new TimeSpan(0, 0, 2);

            delayBeforeShowingBoardTilesTimer.Tick += new EventHandler(delayBeforeShowingBoardTilesTimer_Tick);
            delayBeforeShowingBoardTilesTimer.Interval = new TimeSpan(0, 0, 2);

            delayBeforeGameEndsTimer.Tick += new EventHandler(delayBeforeGameEndsTimer_Tick);
            delayBeforeGameEndsTimer.Interval = new TimeSpan(0, 0, 0, 1, 500);
        }

        /// <summary>
        /// Khởi chạy khi chương trình bắt đầu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupTimer();          
        }

        /// <summary>
        /// Tạo ma trận số thể hiện trạng thái các ô tương ứng với ô vuông
        /// </summary>
        private void CreateNumberMatrix()
        {
            _numberMatrix = new int[height_tiles, width_tiles];

            for (int i = 0; i < height_tiles; i++)
            {
                for (int j = 0; j < width_tiles; j++)
                {
                    _numberMatrix[i, j] = 0;
                }
            }

            int total = width_tiles * height_tiles;
            List<int> possible = Enumerable.Range(0, total).ToList();

            for (int i = 0; i < level_number; i++)
            {
                int index = tilesRandom.Next(0, possible.Count);
                _numberMatrix[possible[index] / width_tiles, possible[index] % width_tiles] = 1;
                possible.RemoveAt(index);

            }
        }

        /// <summary>
        /// Thiết lập các thông số ban đầu của game
        /// </summary>
        private void InitializeGame()
        {
            isWin = false;
            level_number = 5;
            lives = max_lives;
            player_score = 0;
            ShowPlayerLives(lives);
            Play();
        }

        /// <summary>
        /// Chọn ra kích thước bảng tùy thuộc theo cấp độ
        /// </summary>
        /// <param name="level">Cấp độ trò chơi</param>
        private void ChooseBoardTiles(int level)
        {
            if (level == 5)
            {
                width_tiles = 4;
                height_tiles = 4;
            }
            else if (level == 6)
            {
                width_tiles = 5;
                height_tiles = 4;
            }
            else if (level == 7)
            {
                width_tiles = 5;
                height_tiles = 5;
            }
            else if (level == 8)
            {
                width_tiles = 6;
                height_tiles = 5;
            }
            else if ((level >= 9) && (level <= 18))
            {
                width_tiles = 6;
                height_tiles = 6;
            }
            else if ((level >= 19) && (level <= 21))
            {
                width_tiles = 7;
                height_tiles = 6;
            }
            else
            {
                width_tiles = 8;
                height_tiles = 6;
            }

            CreateBoardTiles(width_tiles, height_tiles);
        }

        /// <summary>
        /// Xử lý sự kiện khi hết thời gian chờ lúc game kết thúc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delayBeforeGameEndsTimer_Tick(object sender, EventArgs e)
        {
            delayBeforeGameEndsTimer.Stop();
            if (isWin == true)
                ShowWinningScreen();
            else
                ShowLosingScreen();
        }

        /// <summary>
        /// Xử lý sự kiện khi hết thời gian chờ trước khi các ô đã chọn được đánh dấu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delayBeforeShowingBoardTilesTimer_Tick(object sender, EventArgs e)
        {
            delayBeforeShowingBoardTilesTimer.Stop();
            delayBeforeShowingTilesTimer.Start();
        }

        /// <summary>
        /// Xử lý sự kiện khi hết thời gian chờ lúc chuẩn bị màn tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delayBeforeNextLevelTimer_Tick(object sender, EventArgs e)
        {
            delayBeforeNextLevelTimer.Stop();
            lblBonusScore.Content = "";
            Play();
        }

        /// <summary>
        /// Xử lý sự kiện khi kết thúc thời gian ghi nhớ các ô vuông
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showingTimer_Tick(object sender, EventArgs e)
        {
            showingTimer.Stop();
            HideChosenTiles();
            
            _isPlayable = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delayBeforeShowingTilesTimer_Tick(object sender, EventArgs e)
        {
            delayBeforeShowingTilesTimer.Stop();
            ShowChosenTiles();
            SetupShowingTimePerLevel(level_number);
            showingTimer.Start();
        }

        /// <summary>
        /// Bắt đầu chạy game
        /// </summary>
        private void Play()
        {
            if (lives > 0)
            {
                numberOfChosenTiles = 0;
                ChooseBoardTiles(level_number);
                CreateNumberMatrix();
                lblCurrentLevel.Content = level_number;
                delayBeforeShowingBoardTilesTimer.Start();
            }
        }
    }
}
