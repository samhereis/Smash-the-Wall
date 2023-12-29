using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    public class PaintOnTexture : MonoBehaviour
    {
        [FoldoutGroup("Settings"), ShowInInspector] private Color _ccurrentColor;
        [FoldoutGroup("Settings"), ShowInInspector] private int _size = 150;

        [FoldoutGroup("Debug - Current Image"), ShowInInspector] private Texture2D _paintTexture;
        [FoldoutGroup("Debug - Current Image"), ShowInInspector] private Texture2D _originalTexture;
        [FoldoutGroup("Debug - Current Image"), ShowInInspector] private SpriteRenderer _spriteRenderer;
        [FoldoutGroup("Debug - Current Image"), ShowInInspector] private Color _originalColor;

        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private Vector3 _offset;
        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private int _rectWidth;
        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private int _rectHeight;
        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private int _textureWidth;
        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private int _textureHeight;
        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private float _pixelPerUnit;
        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private Vector2 _pivot;
        [FoldoutGroup("Debug - Current Image Properties"), ShowInInspector] private Rect _spriteRect;

        private Color32[] _allPixels;

        private void Start()
        {
            _originalTexture = _spriteRenderer.sprite.texture;

            _paintTexture = new Texture2D(_originalTexture.width, _originalTexture.height);
            Sprite sprite = Sprite.Create(_paintTexture, new Rect(_spriteRenderer.sprite.rect.position, new Vector2(_originalTexture.width, _originalTexture.height)), new Vector2(0.5f, 0.5f));
            _spriteRenderer.sprite = sprite;

            _paintTexture.SetPixels(_originalTexture.GetPixels());
            _paintTexture.Apply();

            _originalColor = _originalTexture.GetPixel(_originalTexture.width / 2, _originalTexture.height / 2);

            _rectWidth = (int)_spriteRenderer.sprite.rect.width;
            _rectHeight = (int)_spriteRenderer.sprite.rect.height;

            _textureWidth = _spriteRenderer.sprite.texture.width;
            _textureHeight = _spriteRenderer.sprite.texture.height;

            _pixelPerUnit = _spriteRenderer.sprite.pixelsPerUnit;
            _pivot = _spriteRenderer.sprite.pivot;

            _spriteRect = _spriteRenderer.sprite.rect;

            _allPixels = _paintTexture.GetPixels32();
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0))
            {
                _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
                newPosition.z = 0;

                Vector2 localPos = _spriteRenderer.transform.InverseTransformPoint(newPosition) * _pixelPerUnit;
                Vector2 coords = TextureSpaceCoord(localPos);

                var pixel = new Vector2Int((int)coords.x, (int)coords.y);

                bool isValidPosition = false;

                if ((pixel.x < _rectWidth && pixel.x > 0) & (pixel.y < _rectHeight && pixel.y > 0))
                {
                    int array_pos = pixel.y * _rectWidth + pixel.x;

                    if (array_pos >= 0 && array_pos < _allPixels.Length && _allPixels[array_pos].a > 0)
                    {
                        isValidPosition = true;
                    }
                }

                if (isValidPosition)
                {
                    MarkPixelsToColour(pixel);
                }
                else
                {
                    _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }

        private void LateUpdate()
        {
            ApplyTextureChanges();
        }

        public async void MarkPixelsToColour(Vector2Int pixelCoordinates)
        {
            await Task.Run(() =>
            {
                for (int x = pixelCoordinates.x - _size; x <= pixelCoordinates.x + _size; x++)
                {
                    if (x >= _rectWidth || x < 0) continue;

                    for (int y = pixelCoordinates.y - _size; y <= pixelCoordinates.y + _size; y++)
                    {
                        int array_pos = y * _rectWidth + x;

                        if (_allPixels.Length < 1) return;

                        try
                        {
                            if (array_pos >= _allPixels.Length || array_pos < 0) continue;

                            if (Vector2.Distance(pixelCoordinates, new Vector2(x, y)) < _size)
                            {
                                if (IsCorrectColor(ref _allPixels[array_pos]))
                                {
                                    _allPixels[array_pos] = _ccurrentColor;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            });
        }

        private void ApplyTextureChanges()
        {
            _paintTexture.SetPixels32(_allPixels);
            _paintTexture.Apply();
        }

        public Vector2 TextureSpaceCoord(Vector2 localPos)
        {
            var texSpacePivot = new Vector2(_spriteRect.x, _spriteRect.y) + _pivot;
            Vector2 texSpaceCoord = texSpacePivot + localPos;

            return texSpaceCoord;
        }

        public Vector2 TextureSpaceUV(ref Vector3 worldPos)
        {
            Vector2 texSpaceCoord = TextureSpaceCoord(worldPos);

            Vector2 uvs = texSpaceCoord;
            uvs.x /= _textureWidth;
            uvs.y /= _textureWidth;

            return uvs;
        }

        public bool IsCorrectColor(ref Color color)
        {
            return (color == _originalColor && color.a > 0);
        }

        public bool IsCorrectColor(ref Color32 color)
        {
            return (color == _originalColor && color.a > 0);
        }

        private bool AreAlmostTheSameColors(Color color1, Color color2, float threshold = 1.5f)
        {
            var difference = GetColorDifference(color1, color2);
            return difference < threshold;
        }

        private float GetColorDifference(Color color1, Color color2)
        {
            float r = color1.r - color2.r;
            float g = color1.g - color2.g;
            float b = color1.b - color2.b;
            return Mathf.Sqrt(r * r + g * g + b * b);
        }
    }
}