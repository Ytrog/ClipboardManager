using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClipboardManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly int _maxItems = GetMaxItems();

        

        private void Form1_Load(object sender, EventArgs e)
        {
            timerMain.Enabled = true;
        }

        private void Poll()
        {
            string text = GetClipboardText();
            if (!string.IsNullOrWhiteSpace(text))
            {
                lbStrings.Items.Insert(0,text);
                Prune();
            }
        }

        private void Prune()
        {
            if (lbStrings.Items.Count > _maxItems)
            {
                lbStrings.Items.RemoveAt(lbStrings.Items.Count - 1);
            }
        }

        /// <summary>
        /// Gets the latest entry from the clipboard
        /// </summary>
        /// <returns></returns>
        private string GetClipboardText()
        {
            if  (!Clipboard.ContainsText() || (!string.IsNullOrWhiteSpace(LastEntry) && ItemsContainClipboard()))
            {
                return null;
            }
            return Clipboard.GetText();
        }

        private string LastEntry { get {return lbStrings.Items.Count > 0 ? lbStrings.Items[0] as string: null;} }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            Poll();
        }

        private void lbStrings_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Copy();
        }

        private bool ItemsContainClipboard()
        {
            string text = Clipboard.GetText();
            return lbStrings.Items.Contains(text);
        }

        private void Copy()
        {
            string text = lbStrings.SelectedItem as string;
            if (!string.IsNullOrWhiteSpace(text))
            {
                Clipboard.SetText(text);
            }
        }

        private static int GetMaxItems()
        {
            const string key = "maxItems";
            if(ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[key]);
            }
            return 5;
        }
    }
}
