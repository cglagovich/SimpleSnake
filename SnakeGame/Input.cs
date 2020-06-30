using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    class Input
    {
        private static Dictionary<Keys, bool> keyTable = new Dictionary<Keys, bool>();
        public static Keys? LastPressed = null;
        public static bool KeyPress(Keys key)
        {
            if (keyTable.ContainsKey(key))
            {
                return keyTable[key];
            }
            else
            {
                return false;
            }
        }

        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
            if (state)
            {
                LastPressed = key;
            }
        }
        
    }
}
