using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDDDSample.RegisterApp.Views
{
    using System.Windows;

    public class MessageBoxCreator : IMessageBoxCreator
    {
        public void ShowMessageBox(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
