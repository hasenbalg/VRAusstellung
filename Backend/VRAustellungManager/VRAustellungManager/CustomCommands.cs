using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VRAustellungManager
{
    public static class CustomCommands
    {
        //http://www.wpf-tutorial.com/commands/implementing-custom-commands/
        public static readonly RoutedUICommand Exit = new RoutedUICommand
                (
                        "Exit",
                        "Exit",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                                        new KeyGesture(Key.F4, ModifierKeys.Alt)
                        }
                );

        public static readonly RoutedUICommand Export = new RoutedUICommand
                (
                        "Export",
                        "Export",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                                        new KeyGesture(Key.E, ModifierKeys.Control)
                        }
                );

        public static readonly RoutedUICommand Import = new RoutedUICommand
                (
                        "Import",
                        "Import",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                        {
                                        new KeyGesture(Key.I, ModifierKeys.Control)
                        }
                );
    }

}
