using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Sample_OpenTK
{
    public class Texture : IDisposable
    {
        public int Handle;

        bool isDisposed;
        public Texture(string path)
        {
            if(!File.Exists("Content/" + path))
            {
                throw new FileNotFoundException(nameof(path));
            }

            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            //float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

            Bitmap bitmap = new Bitmap("Content/" + path);

            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        ~Texture()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.DeleteTexture(Handle);

            GC.SuppressFinalize(this);
        }
    }
}
