using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hyperwave.LabelEditor
{
    /// <summary>
    /// Interaction logic for ManageLabelsDialog.xaml
    /// </summary>
    public partial class ManageLabelsDialog : Window
    {
        public ManageLabelsDialog()
        {
            InitializeComponent();

            mEditor = cDataRoot.DataContext as DataModel.LabelEditor;
        }

        DataModel.LabelEditor mEditor;

        public ViewAccount Account { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mEditor.Load(Account);
        }

        private void cText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var textbox = (sender as TextBlock).Tag as TextBox;
            var item = textbox.Tag as DataModel.LabelItem;

            mEditor.CancelEdit();

            if (!item.CanEdit)
                return;

            mEditor.EditItem(item);

            textbox.Focus();
            textbox.SelectAll();
        }

        private void cSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Control).Tag as DataModel.LabelItem;
            item.SaveEdit();
        }

        private void cCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Control).Tag as DataModel.LabelItem;
            item.CancelEdit();
        }

        private void cNewItem_GotFocus(object sender, RoutedEventArgs e)
        {
            mEditor.CancelEdit();
        }

        private void cAddItem_Click(object sender, RoutedEventArgs e)
        {
            var item = cNewItemPanel.DataContext as DataModel.NewItem;
            mEditor.AddItem(item.Text);
            item.Text = "";
            cNewItem.Focus();
            
        }

        private void cDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Control).Tag as DataModel.LabelItem;
            mEditor.DeleteItem(item);
        }

        private void cRedo_Click(object sender, RoutedEventArgs e)
        {
            mEditor.Redo();
        }

        private void cUndo_Click(object sender, RoutedEventArgs e)
        {
            mEditor.Undo();
        }

        private async void cOk_Click(object sender, RoutedEventArgs e)
        {
            EnableClose = false;
            await mEditor.Apply();
            Close();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint SC_CLOSE = 0xF060;

        bool mEnableClose = true;

        bool EnableClose
        {
            get
            {
                return mEnableClose;
            }
            set
            {
                if (mEnableClose == value)
                    return;

                mEnableClose = value;

                var hWnd = new WindowInteropHelper(this);
                var sysMenu = GetSystemMenu(hWnd.Handle, false);

                if (mEnableClose)
                    EnableMenuItem(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                else
                    EnableMenuItem(sysMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
        }
    }
}
