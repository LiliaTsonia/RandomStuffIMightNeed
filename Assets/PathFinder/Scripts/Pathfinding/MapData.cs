using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pathfinding
{
    public class MapData : MonoBehaviour
    {
        private const string TEXT_RESOURCE_PATH = "mapdata";
        private const string IMG_RESOURCE_PATH = "mapdata/imageSource";

        [SerializeField] private int _width;
        [SerializeField] private int _height;

        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private Texture2D _textureMap;

        [SerializeField] private Camera _camera;

        private void Awake()
        {
            CheckAssets();
        }

        public int[,] CreateMap()
        {
            List<string> lines = new List<string>();

            if (_textureMap != null)
            {
                lines = GetMapFromTexture();
            }
            else
            {
                lines = GetMapFromTxtFile();
            }

            SetDimentions(lines);
            SetCamera();

            int[,] map = new int[_width, _height];
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    if (lines[y].Length > x)
                    {
                        map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                    }
                }
            }

            return map;
        }

        private void SetCamera()
        {
            float xPos = (_width - 1) / 2f;
            float zPos = (_height - 1) / 2f;

            _camera.transform.position = new Vector3(xPos, 15f, zPos);
            _camera.orthographicSize = zPos + 1f;
        }

        private void SetDimentions(List<string> textLines)
        {
            _height = textLines.Count;

            foreach (string line in textLines)
            {
                if (line.Length > _width)
                {
                    _width = line.Length;
                }
            }
        }

        private void CheckAssets()
        {
            string level = SceneManager.GetActiveScene().name;

            if (_textureMap == null)
            {
                _textureMap = Resources.Load<Texture2D>(IMG_RESOURCE_PATH + "/" + level);
            }

            if (_textAsset == null)
            {
                _textAsset = Resources.Load<TextAsset>(TEXT_RESOURCE_PATH + "/" + level);
            }
        }

        private List<string> GetMapFromTexture()
        {
            return GetMapFromTexture(_textureMap);
        }

        public List<string> GetMapFromTxtFile()
        {
            return GetMapFromTxtFile(_textAsset);
        }

        private List<string> GetMapFromTxtFile(TextAsset textAsset)
        {
            List<string> lines = new List<string>();

            if (textAsset != null)
            {
                string txtData = textAsset.text;
                string[] delimiters = { "\r\n", "\n" };
                lines.AddRange(txtData.Split(delimiters, System.StringSplitOptions.None));
                lines.Reverse();
            }
            else
            {
                Debug.LogError("MAPDATA GetMapFromTxtFile Error : invalid text asset");
            }

            return lines;
        }

        private List<string> GetMapFromTexture(Texture2D texture)
        {
            List<string> lines = new List<string>();

            if (_textureMap != null)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    string newLine = "";

                    for (int x = 0; x < texture.width; x++)
                    {
                        if (texture.GetPixel(x, y) == Color.black)
                        {
                            newLine += '1';
                        }
                        else if (texture.GetPixel(x, y) == Color.white)
                        {
                            newLine += '0';
                        }
                        else
                        {
                            newLine += ' ';
                        }
                    }

                    lines.Add(newLine);
                }
            }
            else
            {
                Debug.LogError("MAPDATA GetMapFromTexture Error : invalid texture asset");
            }

            return lines;
        }
    }
}
