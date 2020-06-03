using UnityEngine;

namespace MonoSecurity.Drawing
{
    public static class Render //Credit to Zat from UC
    {
        public static Material DrawMaterial;

        private static Texture2D texture;

        private static Color textureColor;

        public static GUIStyle StringStyle { get; set; } = new GUIStyle(GUI.skin.label);

        public static Color Color
        {
            get
            {
                return GUI.color;
            }
            set
            {
                GUI.color = value;
            }
        }

        public static void DrawLine(Vector2 from, Vector2 to, float thickness, Color color)
        {
            Color = color;
            DrawLine(from, to, thickness);
        }

        public static void DrawBackground(Rect rect, Color color)
        {
            Color = color;
            DrawBackground(rect);
        }

        public static void DrawBackground(Rect rect)
        {
            if (!texture) { texture = new Texture2D(1, 1); texture.filterMode = FilterMode.Point; }

            GUI.DrawTexture(rect, texture);
        }

        public static void DrawLine(Vector2 from, Vector2 to, float thickness)
        {
            if (!texture) { texture = new Texture2D(1, 1); texture.filterMode = FilterMode.Point; }

            var vector = to - from;
            float fovang = 57.29578f * Mathf.Atan(vector.y / vector.x);
            if (vector.x < 0f)
            {
                fovang += 180f;
            }

            float yOffset = Mathf.Ceil(thickness / 2);
            GUIUtility.RotateAroundPivot(fovang, from);

            GUI.DrawTexture(new Rect(from.x, from.y - yOffset, vector.magnitude, thickness), texture);

            GUIUtility.RotateAroundPivot(-fovang, from);

        }

        public static void DrawBox(float x, float y, float w, float h, float thickness, Color color)
        {
            Color = color;
            DrawLine(new Vector2(x, y), new Vector2(x + w, y), thickness, color);
            DrawLine(new Vector2(x, y), new Vector2(x, y + h), thickness, color);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), thickness, color);
            DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), thickness, color);
        }

        public static void DrawBoxCorners(float x, float y, float w, float h, float thickness, Color color)
        {
            Color = color;

            float lineW = w / 5;
            float lineH = h / 6;

            DrawLine(new Vector2(x, y), new Vector2(x, y + lineH), thickness, color);
            DrawLine(new Vector2(x, y), new Vector2(x + lineW, y), thickness, color);
            DrawLine(new Vector2(x + w - lineW, y), new Vector2(x + w, y), thickness, color);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + lineH), thickness, color);
            DrawLine(new Vector2(x, y + h - lineH), new Vector2(x, y + h), thickness, color);
            DrawLine(new Vector2(x, y + h), new Vector2(x + lineW, y + h), thickness, color);
            DrawLine(new Vector2(x + w - lineW, y + h), new Vector2(x + w, y + h), thickness, color);
            DrawLine(new Vector2(x + w, y + h - lineH), new Vector2(x + w, y + h), thickness, color);
        }

        public static void DrawBox(Vector2 position, Vector2 size, float thickness, Color color, bool centered = true)
        {
            Color = color;
            if (textureColor != color || textureColor == null)
            {
                textureColor = color;
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, color);
                texture.wrapMode = 0;
                texture.Apply();
            }
            if (centered)
            {
                position -= size / 2f;
            }
            GUI.DrawTexture(new Rect(position.x, position.y, size.x, thickness), texture);
            GUI.DrawTexture(new Rect(position.x, position.y, thickness, size.y), texture);
            GUI.DrawTexture(new Rect(position.x + size.x, position.y, thickness, size.y), texture);
            GUI.DrawTexture(new Rect(position.x, position.y + size.y, size.x + thickness, thickness), texture);
        }

        public static void CornerBox(Vector2 Head, float Width, float Height, float thickness)
        {
            float num = Width / 4f;

            RectFilled(Head.x - Width / 2f, Head.y, num, 1f);
            RectFilled(Head.x - Width / 2f, Head.y, 1f, num);
            RectFilled(Head.x + Width / 2f - num, Head.y, num, 1f);
            RectFilled(Head.x + Width / 2f, Head.y, 1f, num);
            RectFilled(Head.x - Width / 2f, Head.y + Height - 3f, num, 1f);
            RectFilled(Head.x - Width / 2f, Head.y + Height - num - 3f, 1f, num);
            RectFilled(Head.x + Width / 2f - num, Head.y + Height - 3f, num, 1f);
            RectFilled(Head.x + Width / 2f, Head.y + Height - num - 3f, 1f, num + 1);
        }

        public static void CornerBox(Vector2 Head, float Width, float Height, float thickness, Color color)
        {
            Color = color;
            CornerBox(Head, Width, Height, thickness);
        }

        public static void DrawSwastika(Color color)
        {
            int drX = Screen.width / 2;
            int drY = Screen.height / 2;
            DrawLine(new Vector2(drX, drY), new Vector2(drX, drY - 10), 1f, color);
            DrawLine(new Vector2(drX, drY - 10), new Vector2(drX + 10, drY - 10), 1f, color);
            DrawLine(new Vector2(drX, drY), new Vector2(drX + 10, drY), 1f, color);
            DrawLine(new Vector2(drX + 10, drY), new Vector2(drX + 10, drY + 10), 1f, color);

            DrawLine(new Vector2(drX, drY), new Vector2(drX, drY + 10), 1f, color);
            DrawLine(new Vector2(drX, drY + 10), new Vector2(drX - 10, drY + 10), 1f, color);

            DrawLine(new Vector2(drX, drY), new Vector2(drX - 10, drY), 1f, color);
            DrawLine(new Vector2(drX - 10, drY), new Vector2(drX - 10, drY - 10), 1f, color);
        }

        public static void DrawCross(Vector2 position, Vector2 size, float thickness)
        {
            GUI.DrawTexture(new Rect(position.x - size.x / 2f, position.y, size.x, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x, position.y - size.y / 2f, thickness, size.y), Texture2D.whiteTexture);
        }

        internal static void DrawLabel(Rect rect, string label, Color color)
        {
            Color = color;
            GUIStyle guistyle = new GUIStyle();
            if (guistyle.normal.textColor != color)
            {
                guistyle.normal.textColor = color;
            }
            guistyle.font = Main.Consolas;
            GUI.Label(rect, label, guistyle);
        }

        public static void DrawString(Vector2 position, string label, Color color, bool centered = true, int size = 12, FontStyle fontStyle = FontStyle.Bold)
        {
            Color = color;
            DrawString(position, label, centered, size, fontStyle);
        }

        public static void DrawString(Vector2 position, string label, bool centered = true, int size = 12, FontStyle fontStyle = FontStyle.Bold)
        {

            StringStyle.fontSize = size;
            StringStyle.richText = true;
            StringStyle.font = Main.Consolas;
            StringStyle.fontStyle = fontStyle;
            GUIContent guicontent = new GUIContent(label);
            if (centered)
            {
                position.x -= StringStyle.CalcSize(guicontent).x / 2f;
            }
            GUI.Label(new Rect(position.x, position.y, 300f, 25f), guicontent, StringStyle);

        }

        public static void RectFilled(float x, float y, float width, float height, Color color)
        {
            Color = color;
            RectFilled(x, y, width, height);
        }

        public static void RectFilled(float x, float y, float width, float height)
        {
            GUI.DrawTexture(new Rect(x, y, width, height), texture);
        }
    }

}

