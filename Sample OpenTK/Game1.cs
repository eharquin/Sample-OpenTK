using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK;
using OpenTK.Input;

namespace Sample_OpenTK
{
    internal class Game1 : GameWindow
    {
        Renderer renderer;

        ShaderProgram shaderProgram;

        VertexBuffer vertexBuffer;
        VertexArray vertexArray;
        IndexBuffer indexBuffer;
        Texture texture0;

        VertexBuffer vertexBuffer2;
        VertexArray vertexArray2;
        IndexBuffer indexBuffer2;
        Texture texture1;

        int vertexCount;
        int indexCount;

        int lineCount;
        int tileCount;

        int tileWidth;
        int tileHeight;

        int tileFaceWidth;
        int tileFaceHeight;
        int tileAngle;

        private double time;

        public int tileGreenHeight { get; private set; }
        public int startPosX { get; private set; }
        public int startPosY { get; private set; }

        public Game1(int width = 1280, int height = 720, string title = "Test")
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings()
                {
                    Title = title,
                    Size = new Vector2i(width, height),
                    //WindowBorder = WindowBorder.Fixed,
                    StartVisible = false,
                    StartFocused = false,
                    API = ContextAPI.OpenGL,
                    Profile = ContextProfile.Core,
                    APIVersion = new Version(4, 6),
                })
        {
            CenterWindow(new Vector2i(1280, 720));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            IsVisible = true;

            GL.ClearColor(Color4.LightGray);

            texture0 = new Texture("testTileThin.png");
            texture1 = new Texture("slide.png");

            renderer = new Renderer();


            tileWidth = 64;
            tileHeight = 64;

            tileFaceWidth = 48;
            tileFaceHeight = 16;

            tileAngle = 16;

            tileGreenHeight = 48;

            tileCount = 9;
            lineCount = 18;

            startPosX = 640 - (tileCount / 2) * (tileFaceWidth - tileAngle);
            startPosY = 360 + (lineCount / 2) * tileFaceHeight;

            //startPosX = 480;
            //startPosY = 480;

            vertexBuffer = new VertexBuffer(VertexPositionColorTexture.Info, 2048, true);
            vertexBuffer2 = new VertexBuffer(VertexPositionColorTexture.Info, 128, true);

            vertexArray = new VertexArray(vertexBuffer);
            vertexArray2 = new VertexArray(vertexBuffer2);

            indexBuffer = new IndexBuffer(6144, false);

            indexBuffer2 = new IndexBuffer(128 * 3, true);
            indexBuffer2.SetData(new int[] { 0, 1, 2, 0, 2, 3 }, 6);


            string vertexShaderCode =
                @"
                #version 460 core
                
                layout (location = 0) uniform vec2 viewport;

                layout (location = 1) uniform mat4 model;
                layout (location = 2) uniform mat4 view;
                layout (location = 3) uniform mat4 projection;

                layout (location = 0) in vec3 a_position;
                layout (location = 1) in vec4 a_color;
                layout (location = 2) in vec2 a_texture_coord;
                layout (location = 3) in float a_tex_index;

                out vec4 v_color;
                out vec2 v_texture_coord;
                out float v_tex_index;
                
                void main()
                {
                    float nx = a_position.x / viewport.x * 2.0 - 1.0;
                    float ny = a_position.y / viewport.y * 2.0 - 1.0;

                    gl_Position = vec4(nx, ny, 1.0, 1.0) * model * view * projection;
                    v_color = a_color;
                    v_texture_coord = a_texture_coord;
                    v_tex_index = a_tex_index;
                }
                ";

            string fragmentShaderCode =
                @"
                #version 460 core

                out vec4 f_color;

                in vec4 v_color;
                in vec2 v_texture_coord;
                in float v_tex_index;

                uniform sampler2D texture1; // (we'll bind to texture unit 1)
                uniform sampler2D texture2; // (we'll bind to texture unit 0)



                void main()
                {
                    int index = int(v_tex_index);

                    vec4 color = texture(texture1, v_texture_coord);

                    if(index == 1)
                    {
                        color = texture(texture2, v_texture_coord);
                    }

                    if(color != vec4(1.0, 1.0, 0.0, 1.0) && color != vec4(0.0,1.0,1.0,1.0))
                    {
                        f_color = color;
                    }  
                }
                ";

            shaderProgram = new ShaderProgram(vertexShaderCode, fragmentShaderCode);

            Matrix4 model = Matrix4.Identity;
            Matrix4 view = Matrix4.Identity;
            Matrix4 projection = Matrix4.Identity;

            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            shaderProgram.SetUniform("viewport", viewport[2], viewport[3]);

            shaderProgram.SetUniform("model", model); ;
            shaderProgram.SetUniform("view", view);
            shaderProgram.SetUniform("projection", projection);

            shaderProgram.SetUniform("texture1", 0);
            shaderProgram.SetUniform("texture2", 1);

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            vertexBuffer?.Dispose();
            shaderProgram?.Dispose();
            vertexArray?.Dispose();
            indexBuffer?.Dispose();

            texture0?.Dispose();

            vertexBuffer2?.Dispose();
            vertexArray2?.Dispose();
            indexBuffer2?.Dispose();

            texture1?.Dispose();

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            MouseState mouseState = MouseState.GetSnapshot();
            bool leftMouseDown = mouseState.IsButtonDown(MouseButton.Left);
            bool rightMouseDown = mouseState.IsButtonDown(MouseButton.Right);

            int mx = (int)(mouseState.X / Size.X * 1280); // value between 0 and 1279 (left to right)
            int my = (int)(-1f * ((mouseState.Y / Size.Y * 720) - 720)) - 1; // value between 0 and 719 (down to top)
            int mz = 0;

            int cx = ((int)mx / (48));
            int cy = ((int)my / 16);


            float gridXRatio = 48;
            float gridYRatio = 16;


            Vector2 iWorld = new Vector2(1, -1);
            Vector2 jWorld = new Vector2(0, -1);

            Matrix2 screenToWorld = new Matrix2(iWorld, jWorld);

            Vector2 screenPosition = new Vector2(mx, my) - new Vector2(startPosX, startPosY);

            Vector2 worldPosition = screenToWorld * screenPosition;

            if (worldPosition.X < 0 || worldPosition.Y < 0)
            {
                worldPosition = new Vector2(-48, -16);
            }

            int cwx = (int)worldPosition.X / 48;
            int cwy = (int)worldPosition.Y / 16;


            //time += args.Time;

            //if (time > (double)1 / 10)
            //{
            //    Console.Clear();
            //    Console.WriteLine("Mouse Screen Coordinate X = " + mx + " Y = " + my);
            //    Console.WriteLine("Mouse Grid Coordinate X = " + cx + " Y = " + cy);
            //    Console.WriteLine("Matrix  = " + screenToWorld);
            //    Console.WriteLine("Mouse world Coordinate  = " + worldPosition);
            //    Console.WriteLine("Mouse Grdi World Coordinate X = " + (int)worldPosition.X / 48 + " Y = " + (int)worldPosition.Y / 16);
            //    time = 0;
            //}


            vertexCount = 0;
            indexCount = 0;

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[tileCount * 4 * lineCount];
            int[] indices = new int[tileCount * 6 * lineCount];

            Random random = new Random();

            float[] tex_index = new float[] {0, 1};


            int xm = 0;
            int ym = 0;

            for (int j = 0; j < lineCount; j++)
            {
                xm = 0 - (tileAngle * j);
                ym = tileFaceHeight * j;

                for (int i = 0; i < tileCount; i++)
                {
                    float x = (startPosX - tileAngle) + (tileFaceWidth * i) + xm;
                    float y = (startPosY - tileHeight) - ym;

                    int width = tileWidth;
                    int height = tileHeight;

                    //if(j == cwy && i == cwx)
                    //{
                    //    y += 8;
                    //}

                    float index = 0f;

                    if (lineCount == j + 1)
                        index = 1f;

                    vertices[vertexCount + 0] = new VertexPositionColorTexture(new Vector3(x, y, 0f), Color4.White, new Vector2(0f, 0f), index);
                    vertices[vertexCount + 1] = new VertexPositionColorTexture(new Vector3(x + width, y, 0f), Color4.White, new Vector2(1f, 0f), index);
                    vertices[vertexCount + 2] = new VertexPositionColorTexture(new Vector3(x + width, y + height, 0f), Color4.White, new Vector2(1f, 1f), index);
                    vertices[vertexCount + 3] = new VertexPositionColorTexture(new Vector3(x, y + height, 0f), Color4.White, new Vector2(0f, 1f), index);

                    indices[indexCount + 0] = vertexCount + 0;
                    indices[indexCount + 1] = vertexCount + 1;
                    indices[indexCount + 2] = vertexCount + 2;
                    indices[indexCount + 3] = vertexCount + 0;
                    indices[indexCount + 4] = vertexCount + 2;
                    indices[indexCount + 5] = vertexCount + 3;

                    indexCount += 6;
                    vertexCount += 4;
                }
            }


            vertexBuffer.SetData(vertices, vertices.Length);
            indexBuffer.SetData(indices, indices.Length);




            //int tileW = 64;
            //int tileH = 64;

            //VertexPositionColorTexture[] vertices2 = new VertexPositionColorTexture[4];

            //vertices2[0] = new VertexPositionColorTexture(new Vector3(mx, my, 0f), Color4.White, new Vector2(0f, 0f));
            //vertices2[1] = new VertexPositionColorTexture(new Vector3(mx + tileW, my, 0f), Color4.White, new Vector2(1f, 0f));
            //vertices2[2] = new VertexPositionColorTexture(new Vector3(mx + tileW, my + tileH, 0f), Color4.White, new Vector2(1f, 1f));
            //vertices2[3] = new VertexPositionColorTexture(new Vector3(mx, my + tileH, 0f), Color4.White, new Vector2(0f, 1f));


            //vertexBuffer2.SetData(vertices2, vertices2.Length);

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            renderer.Clear();

            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture0.Handle);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, texture1.Handle);

            //int[] activateTexture = new int[10];
            //GL.GetInteger(GetPName.ActiveTexture, activateTexture);
            //Console.WriteLine(activateTexture[0] + " " +activateTexture[1]);

            renderer.Draw(vertexArray, indexBuffer, shaderProgram);

            //renderer.Draw(vertexArray2, indexBuffer2, shaderProgram, texture2);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
