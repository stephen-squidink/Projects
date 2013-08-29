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
    class Terrain
    {
        Texture2D _terrainMap;
        Texture2D _heightMap;

        Vector2 _size;

        VertexBuffer _vertexBuffer;
        IndexBuffer _indexBuffer;
        VertexDeclaration _vertexDeclaration;

        Effect _effect;

        float[,] _heightData;

        public Terrain(Texture2D terrainMap, Effect effect, Vector2 size, Texture2D heightMap)
        {
            _size = size;
            _effect = effect;
            _terrainMap = terrainMap;
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
            _effect.CurrentTechnique = _effect.Techniques["Textured"];
            _effect.Parameters["xTexture"].SetValue(_terrainMap);

            Matrix worldMatrix = Matrix.CreateTranslation(new Vector3(-_size.X / 2, 0, _size.Y / 2)) * Matrix.CreateScale(2);
            _effect.Parameters["xWorld"].SetValue(worldMatrix);
            _effect.Parameters["xView"].SetValue(viewTrans.viewMatrix);
            _effect.Parameters["xProjection"].SetValue(viewTrans.projectionMatrix);

            _effect.Parameters["xEnableLighting"].SetValue(true);
            _effect.Parameters["xAmbient"].SetValue(0.4f);
            _effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

            _effect.Begin();
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                device.Vertices[0].SetSource(_vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
                device.Indices = _indexBuffer;
                device.VertexDeclaration = _vertexDeclaration;

                int noVertices = _vertexBuffer.SizeInBytes / VertexPositionNormalTexture.SizeInBytes;
                int noTriangles = _indexBuffer.SizeInBytes / sizeof(int) / 3;
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, noVertices, 0, noTriangles);

                pass.End();
            }
            _effect.End();
        }

        private void LoadVertices(GraphicsDevice device)
        {
            if(_heightMap != null)
                LoadHeightData(_heightMap);
            

            VertexPositionNormalTexture[] terrainVertices = SetUpTerrainVertices();
            int[] terrainIndices = SetUpTerrainIndices();
            terrainVertices = CalculateNormals(terrainVertices, terrainIndices);
            CopyToTerrainBuffers(device, terrainVertices, terrainIndices);
            _vertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
        }

        private void LoadHeightData(Texture2D heightMap)
        {
            float minimumHeight = float.MaxValue;
            float maximumHeight = float.MinValue;

            _size.X = heightMap.Width;
            _size.Y = heightMap.Height;

            Color[] heightMapColors = new Color[(int)_size.X * (int)_size.Y];
            heightMap.GetData(heightMapColors);

            _heightData = new float[(int)_size.X, (int)_size.Y];
            for (int x = 0; x < _size.X; x++)
                for (int y = 0; y < _size.Y; y++)
                {
                    _heightData[x, y] = heightMapColors[x + y * (int)_size.X].R;
                    if (_heightData[x, y] < minimumHeight) minimumHeight = _heightData[x, y];
                    if (_heightData[x, y] > maximumHeight) maximumHeight = _heightData[x, y];
                }

            for (int x = 0; x < _size.X; x++)
                for (int y = 0; y < _size.Y; y++)
                    _heightData[x, y] = (_heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 30.0f;
        }

        private VertexPositionNormalTexture[] SetUpTerrainVertices()
        {
            VertexPositionNormalTexture[] terrainVertices = new VertexPositionNormalTexture[(int)_size.X * (int)_size.Y];
            
            float heightInfo = 0;

            for (int x = 0; x < _size.X; x++)
            {
                for (int y = 0; y < _size.Y; y++)
                {
                    if (_heightData != null)
                        heightInfo = _heightData[x, y];
                    else
                        heightInfo = 0;

                    terrainVertices[x + y * (int)_size.X].Position = new Vector3(x, heightInfo, -y);
                    terrainVertices[x + y * (int)_size.X].TextureCoordinate.X = (float)x / 30.0f;
                    terrainVertices[x + y * (int)_size.X].TextureCoordinate.Y = (float)y / 30.0f;
                }
            }

            return terrainVertices;
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

        private VertexPositionNormalTexture[] CalculateNormals(VertexPositionNormalTexture[] vertices, int[] indices)
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

        private void CopyToTerrainBuffers(GraphicsDevice device, VertexPositionNormalTexture[] vertices, int[] indices)
        {
            _vertexBuffer = new VertexBuffer(device, vertices.Length * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(vertices);

            _indexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }
    }
}
