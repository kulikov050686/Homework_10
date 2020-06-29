using BaseClasses;
using Models;
using Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Homework_10
{
    /// <summary>
    /// Модель представления для страницы с ботом
    /// </summary>
    public class ChatBotViewModel : BaseClassINPC
    {
        #region Закрытые поля

        Window window;
        TelegramBotClient bot;        
        ObservableCollection<MessageLog> listMessages;
        string token;
        bool openApp;
        string inputText;
        int indexElement;

        ICommand openFileToken;
        ICommand addText;
        ICommand addFileDocument;
        ICommand addFileImage;
        ICommand addFileAudio;
        ICommand addFileVideo;
        ICommand saveAs;
        ICommand open;

        #endregion

        #region Открытые поля

        /// <summary>
        /// Ввод текстовой информации
        /// </summary>
        public string InputText
        {
            get => inputText;

            set
            {
                inputText = value;
                OnPropertyChanged("InputText");
            }
        }

        /// <summary>
        /// Номер выбранного элемента списка
        /// </summary>
        public int IndexElement
        {
            get => indexElement;

            set
            {
                indexElement = value;
                OnPropertyChanged("IndexElement");
            }
        }

        /// <summary>
        /// Список сообщений
        /// </summary>
        public ObservableCollection<MessageLog> ListMessages
        {
            get => listMessages;

            set
            {
                listMessages = value;
                OnPropertyChanged("ListMessages");
            }
        }

        /// <summary>
        /// Команда открытия токена бота
        /// </summary>
        public ICommand OpenFileToken
        {
            get
            {
                return openFileToken ?? (openFileToken = new RelayCommand((obj) =>
                {
                    var temp = FileDialog.OpenFileDialogToken();

                    if (!string.IsNullOrWhiteSpace(temp))
                    {
                        token = temp;
                        openApp = true;

                        bot = new TelegramBotClient(token);

                        bot.OnMessage += MessageListener;

                        bot.StartReceiving();
                    }
                }, (obj) => !openApp));
            }
        }

        /// <summary>
        /// Добавить текст
        /// </summary>
        public ICommand AddText
        {
            get
            {
                return addText ?? (addText = new RelayCommand((obj) =>
                {
                    ListMessages.Add(new MessageLog(DateTime.Now.ToLongTimeString(), InputText, "Pavel", 23));

                    if (bot != null)
                    {
                        SendMessage(ListMessages[IndexElement].Id, InputText);
                    }

                    InputText = "";
                }, (obj) => openApp));
            }
        }

        /// <summary>
        /// Добавить документ
        /// </summary>
        public ICommand AddFileDocument
        {
            get
            {
                return addFileDocument ?? (addFileDocument = new RelayCommand((obj) =>
                {
                    if (bot != null)
                    {
                        string path = FileDialog.SendFileDialog();

                        if (path != null)
                        {
                            Send(ListMessages[IndexElement].Id, path, DownloadAddFile.Document);
                        }
                    }

                }, (obj) => openApp));
            }
        }

        /// <summary>
        /// Добавить картинку
        /// </summary>
        public ICommand AddFileImage
        { 
            get 
            {
                return addFileImage ?? (addFileImage = new RelayCommand((obj) => 
                {
                    if (bot != null)
                    {
                        string path = FileDialog.SendFileDialog();

                        if (path != null)
                        {
                            Send(ListMessages[IndexElement].Id, path, DownloadAddFile.Image);
                        }
                    }
                }, (obj) => openApp)); 
            }
        }

        /// <summary>
        /// Добавить аудио
        /// </summary>
        public ICommand AddFileAudio
        {
            get 
            {
                return addFileAudio ?? (addFileAudio = new RelayCommand((obj) => 
                {
                    if (bot != null)
                    {
                        string path = FileDialog.SendFileDialog();

                        if (path != null)
                        {
                            Send(ListMessages[IndexElement].Id, path, DownloadAddFile.Audio);
                        }
                    }
                }, (obj) => openApp)); 
            } 
        }

        /// <summary>
        /// Добавить видео
        /// </summary>
        public ICommand AddFileVideo
        {
            get 
            {
                return addFileVideo ?? (addFileVideo = new RelayCommand((obj) =>
                {
                    if (bot != null)
                    {
                        string path = FileDialog.SendFileDialog();

                        if (path != null)
                        {
                            Send(ListMessages[IndexElement].Id, path, DownloadAddFile.Video);
                        }
                    }
                }, (obj) => openApp)); 
            } 
        }

        /// <summary>
        /// Сохранить историю
        /// </summary>
        public ICommand SaveAs
        {
            get
            {
                return saveAs ?? (saveAs = new RelayCommand((obj) =>
                {
                    FileDialog.SaveFileDialog(ListMessages);
                }, (obj) => openApp));
            }
        }

        /// <summary>
        /// Открыть сохранённую историю 
        /// </summary>
        public ICommand Open
        {
            get
            {
                return open ?? (open = new RelayCommand((obj) =>
                {
                    var temp = FileDialog.OpenFileDialog();

                    if (temp != null)
                    {
                        ListMessages = temp;
                    }
                }));
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChatBotViewModel()
        {
            openApp = false;
            listMessages = new ObservableCollection<MessageLog>();
            window = CurrentWindow();
        }

        #region Закрытые методы

        /// <summary>
        /// Обработчик события получения сообщения
        /// </summary>        
        private void MessageListener(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            switch(e.Message.Type)
            {
                case MessageType.Document:
                    {
                        DownloadFileYesNo("Документ", DownloadAddFile.Document, e);
                    }                    
                    break;                
            }            

            if (e.Message.Text == null) return;

            var messageText = e.Message.Text;

            window.Dispatcher.Invoke(() =>
            {
                ListMessages.Add(new MessageLog(DateTime.Now.ToLongTimeString(), messageText, e.Message.Chat.FirstName, e.Message.Chat.Id));
            });
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="text"> Текст сообщения </param>
        /// <param name="id"> Идентификатор </param>
        private void SendMessage(long id, string text)
        {
            bot.SendTextMessageAsync(id, text);
        }

        /// <summary>
        /// Выбор скачивать пришедший файл или нет
        /// </summary>        
        private void DownloadFileYesNo(string textMessage, DownloadAddFile downloadFile, Telegram.Bot.Args.MessageEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Выберите один из вариантов: ", "Сохранить " + textMessage + " ?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                switch (downloadFile)
                {
                    case DownloadAddFile.Document:
                        {
                            string path = FileDialog.DownloadFileDialog(e.Message.Document.FileName);

                            if (path != null)
                            {
                                Download(e.Message.Document.FileId, path);
                            }
                        }
                        break;                    
                }
            }
        }

        /// <summary>
        /// Сохранение файла из чата
        /// </summary>        
        private async void Download(string fileId, string path)
        {
            var file = await bot.GetFileAsync(fileId);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await bot.DownloadFileAsync(file.FilePath, fs);                
            }
        }

        /// <summary>
        /// Загрузка файла в чат
        /// </summary>       
        private async void Send(long id, string path, DownloadAddFile addFile)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var file = new InputOnlineFile(fs, path);

                switch(addFile)
                {
                    case DownloadAddFile.Document:
                        await bot.SendDocumentAsync(id, file);
                        break;
                    case DownloadAddFile.Image:
                        await bot.SendPhotoAsync(id, file);
                        break;
                    case DownloadAddFile.Audio:
                        await bot.SendAudioAsync(id, file);
                        break;
                    case DownloadAddFile.Video:
                        await bot.SendVideoAsync(id, file);
                        break;
                }                
            }
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
