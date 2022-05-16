using OpenTK.Graphics.OpenGL4;

namespace Sample_OpenTK
{
    public class VertexBuffer : IDisposable
    {
        bool isDisposed;
        bool isStatic;

        public readonly int Handle;

        public readonly VertexInfo VertexInfo;

        public static readonly int MinVertexCount = 3;
        public static readonly int MaxVertexCount = 100_000;


        public VertexBuffer(VertexInfo vertexInfo, bool isStatic = true)
        {
            VertexInfo = vertexInfo;
            this.isStatic = isStatic;

            Handle = GL.GenBuffer();
        }

        public void SetData<T>(T[] vertices, int count) where T : struct
        {
            if (typeof(T) != VertexInfo.Type)
                throw new ArgumentException(nameof(vertices));

            if(vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            if(count < VertexBuffer.MinVertexCount)
                throw new ArgumentOutOfRangeException(nameof(count));

            if(count > VertexBuffer.MaxVertexCount)
                throw new ArgumentOutOfRangeException(nameof(count));


            BufferUsageHint usageHint = BufferUsageHint.StaticDraw;
            if (!isStatic)
                usageHint = BufferUsageHint.DynamicDraw;

            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, count * VertexInfo.Size, vertices, usageHint);
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
