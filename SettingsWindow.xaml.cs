using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ScreenTranslatorApp.Properties;

namespace ScreenTranslatorApp
{
    public partial class SettingsWindow : Page
    {
        private readonly AppConfig _config;

        public SettingsWindow()
        {
            InitializeComponent();

            _config = AppConfig.Load();

            HotkeyTextBoxForMethod1.Text = _config.HotkeyForMethod1;
            HotkeyTextBoxForCancel.Text = _config.HotkeyForCancel;
            foreach (ComboBoxItem item in LanguageComboBox.Items)
            {
                if ((string)item.Tag == _config.TargetLanguage)
                {
                    LanguageComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _config.HotkeyForMethod1 = HotkeyTextBoxForMethod1.Text;
            _config.HotkeyForCancel = HotkeyTextBoxForCancel.Text;

            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
                _config.TargetLanguage = (string)selectedItem.Tag;

            _config.Save();
        }

        private void HotkeyTextMethod1Box_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var modifiers = CatchHotKeyPressedButton(e);
            if (modifiers is null)
            {
                return;
            }

            HotkeyTextBoxForMethod1.Text = string.Join("+", modifiers);
        }
        
        private void HotkeyTextCancelBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var modifiers = CatchHotKeyPressedButton(e);
            if (modifiers is null)
            {
                return;
            }

            HotkeyTextBoxForCancel.Text = string.Join("+", modifiers);
        }

        private List<string>? CatchHotKeyPressedButton(KeyEventArgs e)
        {
            var modifiers = new List<string>();
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
                modifiers.Add("Ctrl");
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
                modifiers.Add("Alt");
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
                modifiers.Add("Shift");
            if ((Keyboard.Modifiers & ModifierKeys.Windows) != 0)
                modifiers.Add("Win");

            if (e.Key is Key.LeftCtrl or Key.RightCtrl or Key.LeftAlt or Key.RightAlt or Key.LeftShift or Key.RightShift or Key.LWin or Key.RWin)
                return null;

            var key = e.Key == Key.System ? e.SystemKey : e.Key;
            modifiers.Add(key.ToString());
            return modifiers;
        }
    }
}
