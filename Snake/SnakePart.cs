using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Snake
{
    internal class SnakePart
    {
        public Rectangle UiElement { get; set; }
        public Point Position { get; set; }
        public bool IsHead { get; set; }
        public bool IsTail { get; set; }
    }
}
