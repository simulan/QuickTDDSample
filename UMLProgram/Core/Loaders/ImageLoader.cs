using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders {
    public class ImageLoader {
        public static int Load(string filename) {
            try {
                Bitmap file = new Bitmap(filename);
                return Load(file);
            } catch (FileNotFoundException e) {
                return -1;
            }
        }
        public static int Load(Bitmap image) {
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
            Rectangle size = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData data = image.LockBits(size, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return texID;
        }
    }
}
