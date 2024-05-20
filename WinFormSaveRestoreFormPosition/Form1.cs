using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Ambiesoft;

namespace WinFormSaveRestoreFormPosition
{
    public partial class Form1 : Form
    {
        const string SECTION_LOCATION = "Location";
        const string KEY_X = "X";
        const string KEY_Y = "Y";
        const string KEY_WIDTH = "Width";
        const string KEY_HEIGHT = "Height";
        const string KEY_MAXIMIZED = "Maximized";

        bool _maxed;
        int _x, _y, _width, _height;
        FormWindowState _lastWindowState;
        bool _isMinimizedFromMaximized;
        string IniPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                    Application.ProductName + ".ini");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            _isMinimizedFromMaximized = false;
            if (WindowState == FormWindowState.Minimized)
            {
                if(_lastWindowState == FormWindowState.Maximized)
                {
                    _isMinimizedFromMaximized = true;
                }
            }
            _lastWindowState = WindowState;
        }

        public Form1()
        {
            InitializeComponent();

            Profile.GetBool(SECTION_LOCATION, KEY_MAXIMIZED, false, out _maxed, IniPath);
            Profile.GetInt(SECTION_LOCATION, KEY_X, 0, out _x, IniPath);
            Profile.GetInt(SECTION_LOCATION, KEY_Y, 0, out _y, IniPath);
            Profile.GetInt(SECTION_LOCATION, KEY_WIDTH, 640, out _width, IniPath);
            Profile.GetInt(SECTION_LOCATION, KEY_HEIGHT, 480, out _height, IniPath);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Profile.WriteBool(SECTION_LOCATION, KEY_MAXIMIZED, 
                _isMinimizedFromMaximized || WindowState == FormWindowState.Maximized, IniPath);

            int x,y,width,height;
            if (WindowState == FormWindowState.Normal)
            {
                x = Location.X;
                y = Location.Y;
                width = Width;
                height = Height;
            }
            else
            {
                x = RestoreBounds.X;
                y = RestoreBounds.Y;
                width = RestoreBounds.Width;
                height = RestoreBounds.Height;
            }
            Profile.WriteInt(SECTION_LOCATION, KEY_X, x, IniPath);
            Profile.WriteInt(SECTION_LOCATION, KEY_Y, y, IniPath);
            Profile.WriteInt(SECTION_LOCATION, KEY_WIDTH, width, IniPath);
            Profile.WriteInt(SECTION_LOCATION, KEY_HEIGHT, height, IniPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_maxed)
            {
                Location = new Point(_x, _y);
                Size = new Size(_width, _height);
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                Location = Location = new Point(_x, _y);
                Size = new Size(_width, _height);
            }
        }
    }
}
