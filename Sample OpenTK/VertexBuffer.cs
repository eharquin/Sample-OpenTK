using OpenTK.Graphics.OpenGL4;

namespace Sample_OpenTK
{
    public class VertexBuffer : IDisposable
    {
        bool isDisposed;
        bool isStatic;

        int size;

        public readonly int Handle;

        public readonly VertexInfo VertexInfo;

        public static readonly int MinVertexCount = 3;
        public static readonly int MaxVertexCount = 100_000;


        public VertexBuffer(VertexInfo vertexInfo, int size = 1024, bool isStatic = true)
        {
            if (size < VertexBuffer.MinVertexCount)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (size > VertexBuffer.MaxVertexCount)
                throw new ArgumentOutOfRangeException(nameof(size));

            VertexInfo = vertexInfo;
            this.isStatic = isStatic;
            this.size = size;

            BufferUsageHint usageHint = BufferUsageHint.StaticDraw;
            if (!isStatic)
                usageHint = BufferUsageHint.DynamicDraw;

            Handle = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, size * VertexInfo.Size, IntPtr.Zero, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void SetData<T>(T[] vertices, int size) where T : struct
        {
            if (typeof(T) != VertexInfo.Type)
                throw new ArgumentException(nameof(vertices));

            if(vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            if(size > this.size || size < MinVertexCount)
                throw new ArgumentOutOfRangeException(nameof(size));

            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, size * VertexInfo.Size, vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Use()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        }

        ~VertexBuffer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if(isDisposed)
                return;

            isDisposed = true;

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(Handle);

            GC.SuppressFinalize(this);
        }
    }
}
