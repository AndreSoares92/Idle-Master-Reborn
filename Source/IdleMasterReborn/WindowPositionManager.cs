using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace IdleMasterReborn
{
    public class SavedWindowPosition
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    internal static class WindowPositionManager
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "IdleMasterReborn",
            "windows.xml");

        internal static void Attach(Form form)
        {
            if (form == null)
            {
                return;
            }

            string key = form.GetType().Name;

            form.Load += (s, e) =>
            {
                var pos = LoadAll().FirstOrDefault(p => p.Name == key);
                if (pos == null)
                {
                    return;
                }

                var rect = new Rectangle(pos.X, pos.Y, Math.Max(100, form.Width), Math.Max(80, form.Height));
                bool visibleOnScreen = Screen.AllScreens.Any(scr => scr.WorkingArea.IntersectsWith(rect));
                if (visibleOnScreen)
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(pos.X, pos.Y);
                }
            };

            form.FormClosing += (s, e) =>
            {
                if (form.WindowState != FormWindowState.Normal)
                {
                    return;
                }

                var list = LoadAll();
                var existing = list.FirstOrDefault(p => p.Name == key);
                if (existing == null)
                {
                    existing = new SavedWindowPosition { Name = key };
                    list.Add(existing);
                }
                existing.X = form.Location.X;
                existing.Y = form.Location.Y;
                SaveAll(list);
            };
        }

        private static List<SavedWindowPosition> LoadAll()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<SavedWindowPosition>();
                }

                var serializer = new XmlSerializer(typeof(List<SavedWindowPosition>));
                using (var stream = File.OpenRead(FilePath))
                {
                    var list = serializer.Deserialize(stream) as List<SavedWindowPosition>;
                    return list ?? new List<SavedWindowPosition>();
                }
            }
            catch
            {
                return new List<SavedWindowPosition>();
            }
        }

        private static void SaveAll(List<SavedWindowPosition> list)
        {
            try
            {
                var dir = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var serializer = new XmlSerializer(typeof(List<SavedWindowPosition>));
                using (var stream = File.Create(FilePath))
                {
                    serializer.Serialize(stream, list ?? new List<SavedWindowPosition>());
                }
            }
            catch
            {
            }
        }
    }
}
