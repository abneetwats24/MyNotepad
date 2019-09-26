

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using FileDialogExtenders;
using System.Linq;


namespace MyNotepad {
    public partial class SaveOpenDialog : FileDialogControlBase {
        private class EncodingComboBoxItem {
            public string Display { get; set; }
            public EncodingInfo Value { get; set; }
        }

        public SaveOpenDialog() {
            InitializeComponent();

            var items = new List<EncodingComboBoxItem>();

            controlEncodingComboBox.DataSource = new [] {
                new { Display = "ANSI", Value = Encoding.ASCII },
                new { Display = "Unicode", Value = Encoding.Unicode },
                new { Display = "Unicode big endian", Value = Encoding.BigEndianUnicode },
                new { Display = "UTF-8", Value = Encoding.UTF8 },
            };

            controlEncodingComboBox.SelectedIndex = 0;
        }

        public Encoding Encoding { 
            get { return (Encoding)controlEncodingComboBox.SelectedValue; } 
            set { controlEncodingComboBox.SelectedValue = value; } 
        }

        protected override void OnPrepareMSDialog() {
            base.OnPrepareMSDialog();

            // TODO: Assign any settings to the underlyining FileDialog (this.MSDialog)

            //if (Environment.OSVersion.Version.Major < 6)
            //    MSDialog.SetPlaces(new object[] { (int)Places.Desktop, (int)Places.Printers, (int)Places.Favorites, (int)Places.Programs, (int)Places.Fonts, });
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SaveOpenDialog_EventFileNameChanged(IWin32Window sender, string Filename) {
            #region Determine Encoding

            if (FileDlgType == Win32Types.FileDialogType.OpenFileDlg) {
                if (File.Exists(Filename)) {
                    using (var streamReader = new StreamReader(Filename, Encoding.ASCII, detectEncodingFromByteOrderMarks: true)) {
                        var text = streamReader.ReadToEnd();
                        controlEncodingComboBox.SelectedValue = streamReader.CurrentEncoding;
                    }
                }
            }

            #endregion

        }
    }
}

