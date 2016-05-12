using System;
using UnityEditor;
using UnityEngine;

namespace HlLib.Unity
{
    public abstract class DisposableLayout : IDisposable
    {
        ~DisposableLayout()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (OnDisposed != null)
            {
                OnDisposed();
            }

            _disposed = true;
        }

        protected Action OnDisposed { get; set; }

        private bool _disposed;
    }

    public class ScopedHorizontal : DisposableLayout
    {
        public ScopedHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            OnDisposed = () => EditorGUILayout.EndHorizontal();
        }
    }

    public class ScopedVertical : DisposableLayout
    {
        public ScopedVertical()
        {
            EditorGUILayout.BeginVertical();
            OnDisposed = () => EditorGUILayout.EndVertical();
        }
    }

    public class ScopedGuiEnabled : DisposableLayout
    {
        public ScopedGuiEnabled(bool enabled = true)
        {
            _initialState = GUI.enabled;
            GUI.enabled = enabled;
            OnDisposed = () => GUI.enabled = _initialState;
        }

        private bool _initialState;
    }

    public enum Alignment
    {
        Center,
        Left,
        Right,
    }

    public class ScopedHorizontalAlignment : DisposableLayout
    {
        public ScopedHorizontalAlignment(Alignment align)
        {
            switch (align)
            {
                case Alignment.Center:
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    OnDisposed = () =>
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                    };
                    break;

                case Alignment.Left:
                    EditorGUILayout.BeginHorizontal();
                    OnDisposed = () =>
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                    };
                    break;

                case Alignment.Right:
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    OnDisposed = () =>
                    {
                        EditorGUILayout.EndHorizontal();
                    };
                    break;

                default:
                    throw new ArgumentException("invalid alignment: " + align.ToString());
            }
        }
    }
}
