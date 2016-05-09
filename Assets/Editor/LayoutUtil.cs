using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Editor
{
    public static class LayoutUtil
    {
        public static bool SizedButton(string content, int width = 0, int height = 0)
        {
            return SizedButton(new GUIContent(content), width, height);
        }

        public static bool SizedButton(GUIContent content, int width = 0, int height = 0)
        {
            var layouts = new List<GUILayoutOption>();
            if (width > 0)
            {
                layouts.Add(GUILayout.Width(width));
            }
            if (height > 0)
            {
                layouts.Add(GUILayout.Height(height));
            }
            return GUILayout.Button(content, layouts.ToArray());
        }
    }
}
