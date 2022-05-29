using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Sample_OpenTK
{
    public struct Attribute
    {
        public int Index;
        public int Size;
        public VertexAttribPointerType PointerType;
        public bool IsNormalize;
        public int Offset;

        public Attribute(int index, int size, VertexAttribPointerType pointerType, bool isNormalize, int offset)
        {
            Index = index;
            Size = size;
            PointerType = pointerType;
            IsNormalize = isNormalize;
            Offset = offset;
        }
    }
    public struct VertexInfo
    {
        public Type Type;
        public int Size;
        public Attribute[] Attributes;

        public VertexInfo(Type type, int size, Attribute[] attributes)
        {
            Type = type;
            Size = size;
            Attributes = attributes;
        }
    }
    public struct VertexPositionColor
    {
        public Vector3 Position;
        public Vector4 Color;

        public static VertexInfo Info = new VertexInfo(typeof(VertexPositionColor), 7 * sizeof(float), new Attribute[] { new Attribute(0, 3, VertexAttribPointerType.Float, false, 0), new Attribute(1, 4, VertexAttribPointerType.Float, false, 3 * sizeof(float)) });

        public VertexPositionColor(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }

    public struct VertexPositionColorTexture
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector2 TexCoord;
        public float TexIndex;

        public static VertexInfo Info = new VertexInfo(typeof(VertexPositionColorTexture), 10 * sizeof(float), new Attribute[] { new Attribute(0, 3, VertexAttribPointerType.Float, false, 0), new Attribute(1, 4, VertexAttribPointerType.Float, false, 3 * sizeof(float)), new Attribute(2, 2, VertexAttribPointerType.Float, false, 7 * sizeof(float)), new Attribute(3, 1, VertexAttribPointerType.Float, false, 9 * sizeof(float)) });

        public VertexPositionColorTexture(Vector3 position, Color4 color, Vector2 textCoord, float texIndex)
        {
            Position = position;
            Color = new Vector4(color.R, color.G, color.B, color.A); ;
            TexCoord = textCoord;
            TexIndex = texIndex;
        }

        public VertexPositionColorTexture(Vector3 position, Color4 color, Vector2 textCoord)
        {
            Position = position;
            Color = new Vector4(color.R, color.G, color.B, color.A);
            TexCoord = textCoord;
            TexIndex = 0;
        }
    }

    public struct VertexPosition
    {
        public Vector3 Position;

        public static VertexInfo Info = new VertexInfo(typeof(VertexPosition), 3 * sizeof(float), new Attribute[] { new Attribute(0, 3, VertexAttribPointerType.Float, false, 0)});

        public VertexPosition(Vector3 position)
        {
            Position = position;
        }
    }

    public struct VertexPositionTexture
    {
        public Vector3 Position;

        public Vector2 Texture;

        public static VertexInfo Info = new VertexInfo(typeof(VertexPositionTexture), 3 * sizeof(float), new Attribute[] { new Attribute(0, 3, VertexAttribPointerType.Float, false, 0), new Attribute(1, 2, VertexAttribPointerType.Float, false, 3 * sizeof(float)) });

        public VertexPositionTexture(Vector3 position, Vector2 texture)
        {
            Position = position;
            Texture = texture;
        }
    }
}
