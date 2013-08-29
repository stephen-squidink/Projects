using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ExiaProject.com.game.camera;

namespace ExiaProject.com.game.objects
{
    public struct VertexPositionNormalColored
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public static int SizeInBytes = 7 * 4;
        public static VertexElement[] VertexElements = new VertexElement[]
              {
                  new VertexElement( 0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0 ),
                  new VertexElement( 0, sizeof(float) * 3, VertexElementFormat.Color, VertexElementMethod.Default, VertexElementUsage.Color, 0 ),
                  new VertexElement( 0, sizeof(float) * 4, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0 ),
              };
    }

    class ColoredTerrain
    {
        Texture2D _heightMap;

        public static Vector2 _size;

        public VertexBuffer _vertexBuffer;
        public IndexBuffer _indexBuffer;
        VertexDeclaration _vertexDeclaration;

        public List<BoundingSphere> _heightPoints;

       // VertexPositionNormalColored[] vertices;
        //int[] indices;

        Effect _effect;

        public static float[,] _heightData;

        public ColoredTerrain(Effect effect, Vector2 size, Texture2D heightMap)
        {
            _size = size;
            _effect = effect;
            _heightMap = heightMap;
        }

        public void intialize(GraphicsDevice device)
        {
            LoadVertices(device);
        }

        public void update(GameTime gameTime)
        {

        }

        public void render(GraphicsDevice device, ViewTransformations viewTrans)
        {
            _effect.CurrentTechnique = _effect.Techniques["Colored"];

            Matrix worldMatrix = Matrix.CreateTranslation(new Vector3(0, 0, _size.Y)) * Matrix.CreateScale(2);
            _effect.Parameters["xWorld"].SetValue(worldMatrix);
            _effect.Parameters["xView"].SetValue(viewTrans.viewMatrix);
            _effect.Parameters["xProjection"].SetValue(viewTrans.projectionMatrix);

            _effect.Parameters["xEnableLighting"].SetValue(true);
            Vector3 lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            _effect.Parameters["xLightDirection"].SetValue(lightDirection);
            _effect.Parameters["xAmbient"].SetValue(0.2f);

            _effect.Begin();
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Begin();


                device.Vertices[0].SetSource(_vertexBuffer, 0, VertexPositionNormalColored.SizeInBytes);
                device.Indices = _indexBuffer;
                device.VertexDeclaration = _vertexDeclaration;

                int noVertices = _vertexBuffer.SizeInBytes / VertexPositionNormalColored.SizeInBytes;
                int noTriangles = _indexBuffer.SizeInBytes / sizeof(int) / 3;
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, noVertices, 0, noTriangles);

                pass.End();
            }
            _effect.End();
        }

        private void LoadVertices(GraphicsDevice device)
        {
            _heightPoints = new List<BoundingSphere>();

            if(_heightMap != null)
                LoadHeightData(_heightMap);


            VertexPositionNormalColored[] terrainVertices = SetUpTerrainVertices();
            int[] terrainIndices = SetUpTerrainIndices();
            terrainVertices = CalculateNormals(terrainVertices, terrainIndices);
            CopyToTerrainBuffers(device, terrainVertices, terrainIndices);
            
            _vertexDeclaration = new VertexDeclaration(device, VertexPositionNormalColored.VertexElements);
        }

        private void LoadHeightData(Texture2D heightMap)
        {
            _size.X = heightMap.Width;
            _size.Y = heightMap.Height;

            Color[] heightMapColors = new Color[(int)_size.X * (int)_size.Y];
            heightMap.GetData(heightMapColors);

            _heightData = new float[(int)_size.X, (int)_size.Y];
            for (int x = 0; x < (int)_size.X; x++)
                for (int y = 0; y < (int)_size.Y; y++)
                {
                    float n = heightMapColors[x + y * (int)_size.X].R / 5.0f;
                    if (n < 2)
                        n = 0;

                    _heightData[x, y] = n;
                }
        }

        private VertexPositionNormalColored[] SetUpTerrainVertices()
        {
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;

            for (int x = 0; x < (int)_size.X; x++)
            {
                for (int y = 0; y < (int)_size.Y; y++)
                {
                    if (_heightData[x, y] < minHeight)
                        minHeight = _heightData[x, y];
                    if (_heightData[x, y] > maxHeight)
                        maxHeight = _heightData[x, y];
                }
            }

            VertexPositionNormalColored[] terrainVertices = new VertexPositionNormalColored[(int)_size.X * (int)_size.Y];

            for (int x = 0; x < _size.X; x++)
            {
                for (int y = 0; y < _size.Y; y++)
                {
                    terrainVertices[x + y * (int)_size.X].Position = new Vector3(x, _heightData[x, y], -y);

                    if (_heightData[x, y] == 0)
                        terrainVertices[x + y * (int)_size.X].Color = new Color(0.2f,0.2f,0.2f);
                    else if (_heightData[x, y] < minHeight + (maxHeight - minHeight) * 0.4 / 4)
                        terrainVertices[x + y * (int)_size.X].Color = new Color(0.2f,0.1f,0.0f);
                    else if (_heightData[x, y] < minHeight + (maxHeight - minHeight) * 2.8 / 4)
                        terrainVertices[x + y * (int)_size.X].Color = new Color(0.1f,0.3f,0.1f);
                    else if (_heightData[x, y] < minHeight + (maxHeight - minHeight) * 3.5 / 4)
                        terrainVertices[x + y * (int)_size.X].Color = new Color(0.1f,0.2f,0.1f);
                    else
                        terrainVertices[x + y * (int)_size.X].Color = Color.White;
                }
            }

            return terrainVertices;
        }

        static public float GetHeight(Vector3 pos)
        {
            float n = 0;

            pos.Z /= 2;
            pos.X /= 2;

            if ((int)pos.Z > 0 && (int)pos.X > 0 && (int)pos.X < 500 && (int)pos.Z < 500)
            {
                n = _heightData[(int)pos.X, 500-(int)pos.Z];

              //  if(n>0)
             //   Console.WriteLine(n);

            }

            return n; 
        }

        private int[] SetUpTerrainIndices()
        {
            int[] indices = new int[((int)_size.X - 1) * ((int)_size.Y - 1) * 6];
            int counter = 0;
            for (int y = 0; y < _size.Y - 1; y++)
            {
                for (int x = 0; x < _size.X - 1; x++)
                {
                    int lowerLeft = x + y * (int)_size.X;
                    int lowerRight = (x + 1) + y * (int)_size.X;
                    int topLeft = x + (y + 1) * (int)_size.X;
                    int topRight = (x + 1) + (y + 1) * (int)_size.X;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }

            return indices;
        }

        private VertexPositionNormalColored[] CalculateNormals(VertexPositionNormalColored[] vertices, int[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();

            return vertices;
        }

        private void CopyToTerrainBuffers(GraphicsDevice device, VertexPositionNormalColored[] vertices, int[] indices)
        {
            _vertexBuffer = new VertexBuffer(device, vertices.Length * VertexPositionNormalColored.SizeInBytes, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(vertices);

            _indexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }
    }
}
