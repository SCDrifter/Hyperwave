using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hyperwave.Controller;
using Hyperwave.ViewModel;

namespace Hyperwave.MailWriter
{
    /// <summary>
    /// Interaction logic for MailWriter.xaml
    /// </summary>
    public partial class MailWriter : Window, IDraftWindow
    {
        DraftMail mDraft;
        DraftMessageSource mDraftSource;

        const double FONT_SIZE_MULTIPLIER = MailView.MailBodyView.BASE_FONT_SIZE / 12.0;

        public MailWriter()
        {
            InitializeComponent();
            cFromAccount.ItemsSource = GetClient().SendAccounts;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).AddProcessCount();
        }

        private static Controller.EveMailClient GetClient()
        {
            return ((App)Application.Current).Client;
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = (FrameworkElement)sender;
            Grid parent = button.Tag as Grid;
            var item = parent.DataContext as DraftMailRecipient;

            item.StartEdit();

            TextBox text = parent.FindName("cTextBox") as TextBox;
            text.Focus();
        }

        private void cTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;

            var item = text.DataContext as DraftMailRecipient;

            item.CancelEdit();
        }

        private void cTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            FrameworkElement text = sender as FrameworkElement;
            var item = text.DataContext as DraftMailRecipient;

            if (e.Key == Key.Return)
            {
                item.Editor.Select();
                e.Handled = true;
            }
        }

        private void Selection_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement text = sender as FrameworkElement;
            var item = text.DataContext as UserCache.EntityInfo;

            var editor = text.Tag as ReciepientEditor;

            editor.Select(item);
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            var mail = DataContext as DraftMail;
            var recipient = control.DataContext as ViewModel.DraftMailRecipient;

            mail.Recipients.Remove(recipient);

            
        }

        private async void SearchOnline_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            var editor = control.DataContext as ReciepientEditor;
            await editor.SearchOnline();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            foreach(var i in mDraft.EditableReceipients)
            {
                i.CancelEdit();
            }
        }

        private void FetchCharacters_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            var editor = control.DataContext as ReciepientEditor;
            editor.GetMoreCharacters();
        }

        private void FetchMailinglists_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            var editor = control.DataContext as ReciepientEditor;
            editor.GetMoreMailinglists();
        }

        private void FetchCorporations_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            var editor = control.DataContext as ReciepientEditor;
            editor.GetMoreCorporations();
        }

        private void FetchAlliances_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            var editor = control.DataContext as ReciepientEditor;
            editor.GetMoreAlliances();
        }

        void RemoveAllDynamicContextItems(ContextMenu menu)
        {
            List<Control> items = new List<Control>();
            foreach (Control i in menu.Items)
            {
                if (i.Tag == null)
                    items.Add(i);
            }

            foreach(Control i in items)
            {
                menu.Items.Remove(i);
            }
        }

        private void cContent_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var errors = cContent.GetSpellingError(cContent.CaretPosition);
            ContextMenu menu = cContent.ContextMenu;
            GetSpellingSuggestions(errors,cContent, menu);
        }

        private void GetSpellingSuggestions(SpellingError errors,IInputElement source, ContextMenu menu)
        {
            RemoveAllDynamicContextItems(menu);

            if (errors == null)
                return;

            int index = 0;

            foreach (var i in errors.Suggestions)
            {
                MenuItem item = new MenuItem()
                {
                    Header = i,
                    FontWeight = FontWeights.Bold,
                    Command = EditingCommands.CorrectSpellingError,
                    CommandParameter = i,
                    CommandTarget = source
                };

                menu.Items.Insert(index++, item);
            }
            menu.Items.Insert(index++, new Separator());

            MenuItem ignore = new MenuItem()
            {
                Header = "Ignore All",
                Command = EditingCommands.IgnoreSpellingError,
                CommandTarget = source
            };
            menu.Items.Insert(index++, ignore);

            menu.Items.Insert(index++, new Separator());
        }

        private void cSubject_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var errors = cSubject.GetSpellingError(cSubject.CaretIndex);
            ContextMenu menu = cSubject.ContextMenu;
            GetSpellingSuggestions(errors,cSubject, menu);
        }
        bool mReEntry = false;
        private void cContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            mReEntry = true;
            try
            {
                object temp;
                temp = cContent.Selection.GetPropertyValue(Inline.FontWeightProperty);

                cToggleBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

                temp = cContent.Selection.GetPropertyValue(Inline.FontStyleProperty);
                cToggleItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

                temp = cContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                cToggleUnderline.IsChecked = (temp != DependencyProperty.UnsetValue && temp != null) && (temp.Equals(TextDecorations.Underline));

                temp = cContent.Selection.GetPropertyValue(Inline.FontSizeProperty);

                if (temp == null || temp == DependencyProperty.UnsetValue)
                    cFontSize.SelectedItem = null;
                else
                    cFontSize.SelectedItem = ((double)temp / FONT_SIZE_MULTIPLIER);

                temp = cContent.Selection.GetPropertyValue(Inline.ForegroundProperty);

                if (temp == null || temp == DependencyProperty.UnsetValue)
                    cColor.Fill = Brushes.Transparent;
                else
                    cColor.Fill = (Brush)temp;
            }
            finally
            {
                mReEntry = false;
            }
        }

        private void cFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cContent != null && !mReEntry)
                cContent.Selection.ApplyPropertyValue(Inline.FontSizeProperty, (double)cFontSize.SelectedItem * FONT_SIZE_MULTIPLIER);
        }

        private void cColor_Click(object sender, RoutedEventArgs e)
        {
            if (mReEntry)
                return;

            Color color = (Color)((FrameworkElement)sender).Tag;
            cColor.Fill = new SolidColorBrush(color);
            cColorMenu.IsChecked = false;

            cContent.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, cColor.Fill);
        }

        private void cHyperlink_Click(object sender, RoutedEventArgs e)
        {
            EnterUrlDialog dlg = new EnterUrlDialog();
            dlg.Url.Description = cContent.Selection.Text;
            dlg.Owner = this;
            if(!dlg.ShowDialog().GetValueOrDefault())
                return;

            Hyperlink link = new Hyperlink(cContent.Selection.Start, cContent.Selection.End);
            link.Inlines.Clear();
            link.Inlines.Add(dlg.Url.Description);
            link.Foreground = new SolidColorBrush(Color.FromArgb(255, 0xff, 0xa6, 0x00));
            link.NavigateUri = new Uri(dlg.Url.Url);
        }

        void IDraftWindow.SetDraft(DraftMessageSource mail)
        {
            mDraftSource = mail;
            DataContext = mDraft = new DraftMail(mDraftSource);
            Hyperlink[] links;
            cContent.Document = EveMarkupLanguage.ConvertToFlowDocument(mail.Body, out links, cSubject.FontFamily, Application.Current.Resources["WebBrowserBackgroundBrush"] as Brush, MailView.MailBodyView.BASE_FONT_SIZE);
        }

        void IDraftWindow.SetFocus()
        {
            Activate();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GetClient().CloseDraft(mDraftSource);
            ((App)App.Current).RemoveProcessCount();
        }

        private void cSaveDraft_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        public void Save()
        {
            mDraft.Body = EveMarkupLanguage.ConvertFromFlowDocument(cContent.Document, MailView.MailBodyView.BASE_FONT_SIZE);
            mDraft.Save(mDraftSource);
        }

        private void cSend_Click(object sender, RoutedEventArgs e)
        {
            Save();
            SendMailWindow window = new SendMailWindow();
            Task discard = window.SendMail(mDraftSource);
            Close();
        }

        void IDraftWindow.DraftDeleted()
        {
            Close();
        }
    }


    class ColorCollection : List<Color>
    {
    }
}
