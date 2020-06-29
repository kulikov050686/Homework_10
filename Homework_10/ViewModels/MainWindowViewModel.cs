using BaseClasses;
using System;
using System.Windows;
using System.Windows.Input;

namespace Homework_10
{
    /// <summary>
    /// Модель представления для главного окна
    /// </summary>
    public class MainWindowViewModel : BaseClassINPC
    {
        #region Закрытые поля

        private Window mWindow;
        private int mOuterMarginSize = 10;
        private int mWindowRadius = 10;

        ICommand close;

        #endregion

        #region Открытые поля

        /// <summary>
        /// Минимальная ширина окна
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        /// <summary>
        /// Минимальная высота окна
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// Размер рамки изменения размера вокруг окна
        /// </summary>
        public int ResizeBorder
        {
            get
            {
                return Borderless ? 0 : 6;
            }
        }

        /// <summary>
        /// Заполнение внутреннего содержимого главного окна
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// 
        /// </summary>
        public Thickness ResizeBorderThickness
        {
            get
            {
                return new Thickness(ResizeBorder + OuterMarginSize);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OuterMarginSize
        {
            get => mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            set => mOuterMarginSize = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public Thickness OuterMarginSizeThickness
        {
            get
            {
                return new Thickness(OuterMarginSize);
            }
        }

        /// <summary>
        /// Радиус краёв окна
        /// </summary>
        public int WindowRadius
        {
            get => mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            set => mWindowRadius = value;
        }

        /// <summary>
        /// Радиус краёв окна
        /// </summary>
        public CornerRadius WindowCornerRadius
        {
            get
            {
                return new CornerRadius(WindowRadius);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Borderless
        {
            get
            {
                return (mWindow.WindowState == WindowState.Maximized);
            }
        }

        /// <summary>
        /// Высота строки заголовка окна
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        /// <summary>
        /// Высота строки заголовка окна
        /// </summary>
        public GridLength TitleHeightGridLength
        {
            get
            {
                return new GridLength(TitleHeight + ResizeBorder);
            }
        }

        /// <summary>
        /// Страница отображаемая на главном окне
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Chat;

        /// <summary>
        /// Комманда закрытия приложения
        /// </summary>
        public ICommand Close 
        {
            get 
            {
                return close ?? (close = new RelayCommand((obj) => 
                {
                    mWindow.Close();
                }));
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindowViewModel()
        {
            mWindow = CurrentWindow();

            mWindow.StateChanged += MWindow_StateChanged;          
        }

        #region Закрытые методы

        /// <summary>
        /// 
        /// </summary>        
        private void MWindow_StateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }

        /// <summary>
        /// Определяет текущее окно
        /// </summary>        
        private Window CurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    return window;
                }
            }

            return null;
        }

        #endregion
    }
}
