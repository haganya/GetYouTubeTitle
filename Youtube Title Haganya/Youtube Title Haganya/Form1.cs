/*

23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Youtube_Title_Haganya
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtKayitYolu.Text = Properties.Settings.Default.Path;

        }
        

        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("User32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr windowHandle, StringBuilder stringBuilder, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", SetLastError = true)]
        internal static extern int GetWindowTextLength(IntPtr hwnd);

        private static List<IntPtr> windowList;
        private static string _className;
        private static StringBuilder apiResult = new StringBuilder(256);
        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
        public string Path { get; set; }
        public bool Yazabilir { get; set; }


        private void Hakkimda_Click(object sender, EventArgs e)
        {
            HakkimdaForm hkf = new HakkimdaForm();
            hkf.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Path == "")
            {
                Durdur();
                MessageBox.Show("Kayıt Dosyası Seçmelisin !", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DosyaSec();
            }
            else Baslat();
        }

        void Durdur()
        {
            timer1.Enabled = false;
            lblDurum.BackColor = System.Drawing.Color.Red;
            baslatToolStripMenuItem.Enabled = true;
            durdurToolStripMenuItem.Enabled = false;
            button1.Text = "Başla";
            richTitle.Text = "";
        }
        void Baslat()
        {
            timer1.Enabled = true;
            lblDurum.BackColor = System.Drawing.Color.Green;
            baslatToolStripMenuItem.Enabled = false;
            durdurToolStripMenuItem.Enabled = true;
            button1.Text = "Durdur";
        }

        
        void dosyayaYaz()
        {
            try
            {
                System.IO.File.WriteAllText(Properties.Settings.Default.Path, richTitle.Text, System.Text.Encoding.UTF8);
                Yazabilir = true;
            }
            catch
            {
                Yazabilir = false;
                Durdur();
                MessageBox.Show("Kayıt Dosyası Seçmelisin !", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DosyaSec();
            }
        }


        void getTitle()
        {

            //listBox1.Items.Clear();
            
            GC.Collect();
            richTitle.Text = "";

            List<IntPtr> ChromeWindows = WindowsFinder("Chrome_WidgetWin_1", "chrome");
            foreach (IntPtr windowHandle in ChromeWindows)
            {
                int length = GetWindowTextLength(windowHandle);
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(windowHandle, sb, sb.Capacity);


                if (sb.ToString().IndexOf("YouTube") >= 0)
                {
                    if (sb.ToString().Substring(0, sb.ToString().Length - 23).IndexOf(" - ") >= 0)
                    {
                        //// AAAAAA - YOUTUBE - GOOGLE CHROME
                        richTitle.Text += (sb.ToString().Substring(0, sb.ToString().IndexOf("Google Chrome")-3));
                        //listBox1.Items.Add(sb.ToString().Substring(0, sb.ToString().Length - 16));

                        dosyayaYaz();

                        return;
                        
                    }
                }               
            }

        }

        private static List<IntPtr> WindowsFinder(string className, string process)
        {
            _className = className;
            windowList = new List<IntPtr>();

            Process[] chromeList = Process.GetProcessesByName(process);

            if (chromeList.Length > 0)
            {
                foreach (Process chrome in chromeList)
                {
                    if (chrome.MainWindowHandle != IntPtr.Zero)
                    {
                        foreach (ProcessThread thread in chrome.Threads)
                        {
                            EnumThreadWindows((uint)thread.Id, new EnumThreadDelegate(EnumThreadCallback), IntPtr.Zero);
                        }
                    }
                }
            }

            return windowList;
        }

        static bool EnumThreadCallback(IntPtr hWnd, IntPtr lParam)
        {
            if (GetClassName(hWnd, apiResult, apiResult.Capacity) != 0)
            {
                if (string.CompareOrdinal(apiResult.ToString(), _className) == 0)
                {
                    windowList.Add(hWnd);
                }
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Başla")
            {
                Baslat();
            }
            else
            {
                Durdur();

                
                if (Yazabilir)
                {
                    System.IO.File.WriteAllText(Properties.Settings.Default.Path, richTitle.Text, System.Text.Encoding.UTF8);
                }

            }
        }

        void DosyaSec()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Metin Dosyası|*.txt";
            if (file.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(file.FileName);
                if (fi.Exists)
                {
                    txtKayitYolu.Text = (file.FileName);

                    try
                    {
                        Properties.Settings.Default.Path = txtKayitYolu.Text;
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Kaydedildi : " + Properties.Settings.Default.Path, "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Sorun Var!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DosyaSec();
        }

        private void btnOnizleme_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Settings.Default.Path);

            //ayarları sıfırlar
            //Properties.Settings.Default.Path = "";
            //Properties.Settings.Default.Save();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getTitle();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(500);
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            //notifyIcon1.Visible = false;
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.ExitThread();
            Application.Exit();
        }

        private void baslatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Baslat();
        }

        private void durdurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Durdur();
        }

        private void cikisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.ExitThread();
            Application.Exit();
        }
    }
}
