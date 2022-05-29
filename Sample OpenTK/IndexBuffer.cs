using OpenTK.Graphics.OpenGL4;

namespace Sample_OpenTK
{
    public class IndexBuffer : IDisposable
    {
        bool isDisposed;
        bool isStatic;

        public readonly int Size;
        
        public readonly int Handle;

        public static readonly int MinIndexCount = 3;
        public static readonly int MaxIndexCount = 250_000;

        public IndexBuffer(int size, bool isStatic = true)
        {
            this.isStatic = isStatic;
            this.Size = size;

            BufferUsageHint usageHint = BufferUsageHint.StaticDraw;
            if (!isStatic)
                usageHint = BufferUsageHint.DynamicDraw;

            Handle = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, size * sizeof(int), IntPtr.Zero, usageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void SetData(int[] indices, int size)
        {
            if (indices == null)
                throw new ArgumentNullException(nameof(indices));

            if (size < VertexBuffer.MinVertexCount)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (size > this.Size)
                throw new ArgumentOutOfRangeException(nameof(size));


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, size * sizeof(int), indices);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Use()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
        }

        ~IndexBuffer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(Handle);

            GC.SuppressFinalize(this);
        }
    }
}