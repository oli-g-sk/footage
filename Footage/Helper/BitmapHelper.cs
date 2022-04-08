namespace Footage.Helper;

using System.Drawing;

public static class BitmapHelper
{
    public static Image FromFileWithoutLock(string filePath)
    {
        using var bmpTemp = new Bitmap(filePath);
        Image img = new Bitmap(bmpTemp);
        return img;
    }
}