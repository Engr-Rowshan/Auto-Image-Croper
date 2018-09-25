using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;


namespace Auto_Image_Croper.Classes
{
    class ImageProcessor
    {
        private Bitmap _b = null;

        public ImageProcessor(string FilePath)
        {
            string p = Path.GetExtension(FilePath).ToLower();
            if (p == ".jpg" || p == ".png")
            {

            }
        }

    }
}
