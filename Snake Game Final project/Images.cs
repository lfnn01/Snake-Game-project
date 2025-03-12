using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Snake_Game_Final_project
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Body = LoadImage("cs body.png");
        public readonly static ImageSource Head = LoadImage("cs head.png");
        public readonly static ImageSource Food = LoadImage("cs food.png");
        public readonly static ImageSource DeadBody = LoadImage("cs dead body.png");
        public readonly static ImageSource DeadHead = LoadImage("cs dead head.png");
        public readonly static ImageSource Background = LoadImage("Red Spider Lily.jpg");
        private static ImageSource LoadImage(string filename)
        {
            return new BitmapImage(new Uri($"Assets/{filename}", UriKind.Relative));
                
        }
    }
}
