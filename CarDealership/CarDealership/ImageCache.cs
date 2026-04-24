using System.Drawing;
using System.IO;

namespace CarDealership
{
    public static class ImageCache
    {
        public static Image BgDark { get; private set; }
        public static Image BgLight { get; private set; }

        static ImageCache()
        {
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string darkPath = Path.Combine(baseDir, "sidebar_bg2.jpg");
            string lightPath = Path.Combine(baseDir, "sidebar_bg.jpg");

            if (File.Exists(darkPath)) BgDark = Image.FromFile(darkPath);
            if (File.Exists(lightPath)) BgLight = Image.FromFile(lightPath);
        }
    }
}