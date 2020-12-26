using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Xml;
using HtmlAgilityPack;

namespace RuneImporter
{

    public partial class Form1 : Form
    {
        string ExportBuildString = "";
        Button[,] skillOrderArray = new Button[4,18];
        int[,] skillorder = new int[4,18];
        int selected = -1;
        [DllImport("kernel32.dll")]
        static extern int GetProcessId(IntPtr handle);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public Form1()
        {
            InitializeComponent();
            
        }
        string searchString(string runesSection, string search)
        {
            string returnString = "";
            if (runesSection.Contains(search))
            {
                int index = runesSection.IndexOf(search);
                while (runesSection[index] != '>')
                {
                    index++;
                }
                index++;
                while (runesSection[index] != '<')
                {
                    returnString += runesSection[index];
                    index++;
                }
            }
            return returnString;
        }
        string cutString(string runesSection, string search)
        {
            string returnString = "";
            if (runesSection.Contains(search))
            {
                int index = runesSection.IndexOf(search);
                while (runesSection[index] != '>')
                {
                    index++;
                }
                index++;
                while (runesSection[index] != '<')
                {
                    returnString += runesSection[index];
                    index++;
                }
               runesSection = runesSection.Substring(index,runesSection.Length-index);
            }
            return runesSection;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ExportBuildString = "{\"accountId\":212929151,\"itemSets\":";
            Button[,] skillOrderArray =
        {
            {Q1,Q2,Q3,Q4,Q5,Q6,Q7,Q8,Q9,Q10,Q11,Q12,Q13,Q14,Q15,Q16,Q17,Q18},
            {W1,W2,W3,W4,W5,W6,W7,W8,W9,W10,W11,W12,W13,W14,W15,W16,W17,W18},
            {E1,E2,E3,E4,E5,E6,E7,E8,E9,E10,E11,E12,E13,E14,E15,E16,E17,E18},
            {R1,R2,R3,R4,R5,R6,R7,R8,R9,R10,R11,R12,R13,R14,R15,R16,R17,R18}
        };
            
            PictureBox[] Frequentbuild = new PictureBox[] { pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7};
            
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc;
            try
            {
                doc = web.Load("https://champion.gg/champion/" + champText.Text + "/" + positionText.Text);
            }
            catch
            {
                MessageBox.Show("That Champion Doesn't exist!");
                return;
            }
            
            HtmlNodeNavigator navigator = (HtmlAgilityPack.HtmlNodeNavigator)doc.CreateNavigator();
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    var test = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[3]/div[1]/div[2]/div[" + (i + 1) + "]/div[1]/div[" + (j + 1)+ "]/div[1]/div");
                    if (test != null) {
                        skillOrderArray[i, j].BackColor = Color.FromArgb(51, 255, 255);
                        skillOrderArray[i, j].Text = "";
                    }
                    else { 
                        skillOrderArray[i, j].BackColor = Color.FromArgb(38, 51, 66);
                        skillOrderArray[i, j].Text = "";
                    }
                }
            }
            //Frequentbuild[0].LoadAsync
            for(int i = 0; i < 6; i++)
            {
                var node = (navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[1]/div[3]/div/div/div[" + (i + 1) + "]/div[1]/img"));
                Frequentbuild[i].LoadAsync(node.GetAttribute("src", node.NamespaceURI));
                Frequentbuild[i].SizeMode = PictureBoxSizeMode.StretchImage;
            }
            
            string keyStoneTitle = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[1]/div[4]/div/span").Value;
            var keyStone1 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[1]/div[5]/div/span").Value;
            string keyStone2 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[1]/div[6]/div/span").Value;
            string keyStone3 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[1]/div[7]/div/span").Value;
            string keyStone4 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[1]/div[8]/div/span").Value;
            string secondaryStoneTitle = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[2]/div[2]/div/span").Value;
            string secondaryStone1 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[2]/div[3]/div/span").Value;
            string secondaryStone2 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[2]/div[4]/div/span").Value;
            string extra1 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[2]/div[7]/div/span").Value;
            string extra2 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[2]/div[8]/div/span").Value;
            string extra3 = navigator.SelectSingleNode("/html/body/div/div/div[2]/div[3]/div/div/div[1]/div/div[1]/div[3]/div[2]/div[2]/div[2]/div[3]/div[1]/div[2]/div/div/div[2]/div[9]/div/span").Value;
            RuneTitle.Text = keyStoneTitle;
            Rune1.Text = keyStone1;
            Rune2.Text = keyStone2;
            Rune3.Text = keyStone3;
            Rune4.Text = keyStone4;
            SecondaryPathTitle.Text = secondaryStoneTitle;
            SecondaryPath1.Text = secondaryStone1;
            SecondaryPath2.Text = secondaryStone2;
            extrarune1.Text = extra1;
            extrarune2.Text = extra2;
            extrarune3.Text = extra3;
            string temp = champText.Text.ToLower();
            char[] tempChar = temp.ToCharArray();
            for (int i = 0; i < tempChar.Length; i++)
            {
                if(i-1 > 0)
                {
                    if (tempChar[i - 1] == ' ') {
                        tempChar[i] = char.ToUpper(tempChar[i]);
                    }
                }else if(i == 0)
                {
                    tempChar[i] = char.ToUpper(tempChar[i]);
                }
            }
            temp = new string(tempChar);
            temp = temp.Replace(" ", "");
            temp = temp.Replace("\'","");
            pictureBox1.WaitOnLoad = false;
            pictureBox1.LoadAsync("http://ddragon.leagueoflegends.com/cdn/10.12.1/img/champion/" + temp + ".png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

        }

        private void Import_Click(object sender, EventArgs e)
        {
            Process[] processlist = Process.GetProcesses();
            IntPtr h = IntPtr.Zero;
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                    if (process.ProcessName == "LeagueClientUx")
                    {
                        h = process.MainWindowHandle;
                    }
                }
            }
            
            //IntPtr h = FindWindow(null, "LeagueClientUx");
            Console.WriteLine("Hi: " + h.ToString());
            SetForegroundWindow(h);
            System.Threading.Thread.Sleep(200);
            bool runePageSelected = false;
            //Precision
            //Precision
            if (getpixelColor(283, 237,174,167,137))
            {
                runePageSelected = true;
            }
            //Domination
            if (getpixelColor(290, 236,212,66,66))
            {
                runePageSelected = true;
            }
            //Sorcery
            if (getpixelColor(287, 238,157,168,248))
            {
                runePageSelected = true;
            }
            //Resolve
            if (getpixelColor(284, 237,161,213,134))
            {
                runePageSelected = true;
            }
            //Inspiration
            if (getpixelColor(294, 239,73,170,185))
            {
                runePageSelected = true;
            }
            if (!runePageSelected)
            {
                click(553, 853);
                System.Threading.Thread.Sleep(1000);
            }
            fixRuneSelectionBug();
            click(211,264);
            
            String[] keyStones = { "Precision", "Domination", "Sorcery", "Resolve", "Inspiration" };
            if (selected != -1) {
                if (keyStones[selected] != RuneTitle.Text) {
                    for (int i = 0; i < keyStones.Length; i++)
                    {
                        if (RuneTitle.Text == keyStones[i]) {
                            click(300 + (i * 50), 264);
                            keyStones[i] = "";
                        }
                    }
                }
                else
                {
                    keyStones[selected] = "";
                }
            }
            else
            {
                for (int i = 0; i < keyStones.Length; i++)
                {
                    if (RuneTitle.Text == keyStones[i])
                    {
                        click(300 + (i * 50), 264);
                        keyStones[i] = "";
                    }
                }
            }
           
            String[,] rune1_3 = { { "Summon Aery","Grasp of the Undying","Glacial Augment" },{"Arcane Comet","Aftershock","Unsealed Spellbook" },{"Phase Rush","Guardian","Prototype: Omnistone" } };
            for (int i =0; i < 3; i++)
            {
                for(int j = 0; j <3; j++)
                {
                    if (Rune1.Text == rune1_3[i,j])
                    {
                        click(300 + (i * 100),412);
                    }
                }
            }
            String[,] rune1_4 = { { "Press the Attack", "Electrocute" },{"Lethal Tempo","Predator"},{ "Fleet Footwork", "Dark Harvest" },{ "Conqueror", "Hail of Blades" } };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (Rune1.Text == rune1_4[i, j])
                    {
                        click(300 + (i * 70), 412);
                    }
                }
            }
            String[,] rune2_5 = { 
                {"Overheal","Cheap Shot","Nullifying Orb","Demolish","Hextech Flashtraption"},
                { "Triumph", "Taste of Blood", "Manaflow Band", "Font of Life", "Magical Footwear"},
                { "Presence of Mind", "Sudden Impact", "Nimbus Cloak", "Shield Bash", "Perfect Timing" }
            };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Rune2.Text == rune2_5[i, j])
                    {
                        click(318 + (i * 75), 536);
                    }
                }
            }
            String[,] rune3_5 = {
                {"Legend: Alacrity","Zombie Ward","Transcendence","Conditioning","Future's Market"},
                {"Legend: Tenacity","Ghost Poro","Celerity","Second Wind","Minion Dematerializer"},
                {"Legend: Bloodline","Eyeball Collection","Absolute Focus","Bone Plating","Biscuit Delivery"}
            };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Rune3.Text == rune3_5[i, j])
                    {
                        click(318 + (i * 75), 645);
                    }
                }
            }
            String[,] rune4_4 = {
                {"Coup de Grace","Scorch","Overgrowth","Cosmic Insight"},
                {"Cut Down","Waterwalking","Revitalize","Approach Velocity"},
                {"Last Stand","Gathering Storm","Unflinching","Time Warp Tonic"}
            };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Rune4.Text == rune4_4[i, j])
                    {
                        click(318 + (i * 75), 753);
                    }
                }
            }
            String[,] rune4_1 =
            {
                {"Ravenous Hunter" },{"Ingenious Hunter" },{"Relentless Hunter" },{"Ultimate Hunter"}
            };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (Rune4.Text == rune4_1[i, j])
                    {
                        click(307 + (i * 65), 753);
                    }
                }
            }
            
            System.Threading.Thread.Sleep(200);
            //secondary Name
            bool sub = false;
            for (int i = 0; i < keyStones.Length; i++)
            {
                if (SecondaryPathTitle.Text == keyStones[i])
                {
                    if (sub)
                    {
                        click(714 + ((i-1) * 55), 264);
                    }
                    else
                    {
                        click(714 + (i * 55), 264);
                    }
                }
                if (keyStones[i] == "")
                {
                    sub = true;
                }
            }
            //System.Threading.Thread.Sleep(200);
            //click(623, 270);
            //rune1 secondary
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (SecondaryPath1.Text == rune2_5[i, j] || SecondaryPath2.Text == rune2_5[i, j])
                    {
                        click(726 + (i * 75), 371);
                    }
                }
            }
            //rune2 secondary
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (SecondaryPath1.Text == rune3_5[i, j] || SecondaryPath2.Text == rune3_5[i, j])
                    {
                        click(726 + (i * 75), 469);
                    }
                }
            }
            //rune3 secondary
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (SecondaryPath1.Text == rune4_4[i, j] || SecondaryPath2.Text == rune4_4[i, j])
                    {
                        click(726 + (i * 75), 568);
                    }
                }
            }
            //rune3 secondary
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (SecondaryPath1.Text == rune4_1[i, j] || SecondaryPath2.Text == rune4_1[i, j])
                    {
                        click(714 + (i * 65), 568);
                    }
                }
            }
            System.Threading.Thread.Sleep(200);
            click(616, 643);
            String[,] extrarune3_1 =
            {
                {"Adaptive Force"},{"Attack Speed"},{"Scaling Cooldown Reduction"}
            };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (extrarune1.Text == extrarune3_1[i, j])
                    {
                        click(725 + (i * 82), 643);
                    }
                }
            }
            click(616, 700);
            String[,] extrarune23_1 =
            {
                {"Adaptive Force"},{"Armor"},{"Magic Resist"}
            };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (extrarune2.Text == extrarune23_1[i, j])
                    {
                        click(725 + (i * 82), 700);
                    }
                }
            }
            click(616, 756);
            String[,] extrarune33_1 =
            {
                {"Scaling Health"},{"Armor"},{"Magic Resist"}
            };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (extrarune3.Text == extrarune33_1[i, j])
                    {
                        click(725 + (i * 82), 756);
                    }
                }
            }
            System.Threading.Thread.Sleep(400);
            click(636, 157);
            System.Threading.Thread.Sleep(400);
            click(1457, 94);
            
        }
        private void click(int x, int y)
        {
            RECT rct = new RECT();
            GetWindowRect(GetForegroundWindow(), ref rct);
            //Gives me position of window
            //1600x900 is the window size that I have created this in.
            //I need to scale the given points by the current window size
            //Ratio of 1600:current?
            Point pt = new Point(x * (rct.Right - rct.Left) / 1600 + rct.Left, y * (rct.Bottom - rct.Top) / 900 + rct.Top);
            Cursor.Position = pt;
            System.Threading.Thread.Sleep(200);
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            System.Threading.Thread.Sleep(200);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            RECT rct = new RECT();
            GetWindowRect(GetForegroundWindow(), ref rct);
            Position.Text = "X: " + (Cursor.Position.X-rct.Left) + " Y:" + (Cursor.Position.Y - rct.Top);
            //ColorDebug.Text = "R:" + getpixelColor(Cursor.Position.X - rct.Left, Cursor.Position.Y - rct.Top).R + ", G:" + getpixelColor(Cursor.Position.X - rct.Left, Cursor.Position.Y - rct.Top).G+ ", B:" + getpixelColor(Cursor.Position.X - rct.Left, Cursor.Position.Y - rct.Top).B;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                //do stuff
            }
        }

        private void MostFreq_CheckedChanged(object sender, EventArgs e)
        {
            if (MostFreq.Checked)
            {
                MostFreq.Text = "Most Frequent Runes";
            }
            else
            {
                MostFreq.Text = "Highest Win % Runes";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           Import.FlatAppearance.BorderSize = 5;
           Import.FlatAppearance.BorderColor = Color.FromArgb(51, 204, 255);
           Import.FlatAppearance.MouseDownBackColor = Color.FromArgb(153, 231, 255);
           Import.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 134, 179);
           Import.BackColor = Color.FromArgb(30, 35, 40);
           button1.BackColor = Color.FromArgb(30, 35, 40);
           Background.BackColor = Color.FromArgb(11, 13, 15);
            Button[,] skillOrderArray =
       {
            {Q1,Q2,Q3,Q4,Q5,Q6,Q7,Q8,Q9,Q10,Q11,Q12,Q13,Q14,Q15,Q16,Q17,Q18},
            {W1,W2,W3,W4,W5,W6,W7,W8,W9,W10,W11,W12,W13,W14,W15,W16,W17,W18},
            {E1,E2,E3,E4,E5,E6,E7,E8,E9,E10,E11,E12,E13,E14,E15,E16,E17,E18},
            {R1,R2,R3,R4,R5,R6,R7,R8,R9,R10,R11,R12,R13,R14,R15,R16,R17,R18}
        };
            for (int i = 0; i < 4; i++)
           {
               for (int j = 0; j < 18; j++)
               {
                   skillOrderArray[i, j].BackColor = Color.FromArgb(38, 51, 66);
                    skillOrderArray[i, j].Text = "";
               }
           }
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void RuneTitle_TextChanged(object sender, EventArgs e)
        {
            if (String.Equals(RuneTitle.Text,"Precision"))
            {
                RuneTitle.ForeColor = Color.LightGoldenrodYellow;
            }
            else if (String.Equals(RuneTitle.Text, "Domination"))
            {
                RuneTitle.ForeColor = Color.DarkRed;
            }
            else if (String.Equals(RuneTitle.Text, "Sorcery"))
            {
                RuneTitle.ForeColor = Color.DarkBlue;
            }
            else if (String.Equals(RuneTitle.Text, "Resolve"))
            {
                RuneTitle.ForeColor = Color.DarkGreen;
            }
            else if (String.Equals(RuneTitle.Text, "Inspiration"))
            {
                RuneTitle.ForeColor = Color.LightSteelBlue;
            }
        }

        private void SecondaryPathTitle_TextChanged(object sender, EventArgs e)
        {
            if (String.Equals(SecondaryPathTitle.Text, "Precision"))
            {
                SecondaryPathTitle.ForeColor = Color.LightGoldenrodYellow;
            }
            else if (String.Equals(SecondaryPathTitle.Text, "Domination"))
            {
                SecondaryPathTitle.ForeColor = Color.DarkRed;
            }
            else if (String.Equals(SecondaryPathTitle.Text, "Sorcery"))
            {
                SecondaryPathTitle.ForeColor = Color.DarkBlue;
            }
            else if (String.Equals(SecondaryPathTitle.Text, "Resolve"))
            {
                SecondaryPathTitle.ForeColor = Color.DarkGreen;
            }
            else if (String.Equals(SecondaryPathTitle.Text, "Inspiration"))
            {
                SecondaryPathTitle.ForeColor = Color.LightSteelBlue;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Point pt = new Point(212, 264);
            Cursor.Position = pt;
        }
        private void fixRuneSelectionBug()
        {
            
            /*String[] keyStones = { "Precision", "Domination", "Sorcery", "Resolve", "Inspiration" };
            for (int i = 0; i < keyStones.Length; i++)
            {
                if (RuneTitle.Text == keyStones[i])
                {
                    click(300 + (i * 50), 264);
                    keyStones[i] = "";
                }
            }*/
            //Precision
            if (getpixelColor(689, 236,174,167,137))
            {
                click(211, 264);
                click(300, 264);
                selected = 0;
            }
            //Domination
            if (getpixelColor(708, 236,212,66,66))
            {
                click(211, 264);
                click(350, 264);
                selected = 1;
            }
            //Sorcery
            if (getpixelColor(705, 237,159,170,252))
            {
                click(211, 264);
                click(400, 264);
                selected = 2;
            }
            //Resolve
            if (getpixelColor(689, 234,161,213,134))
            {
                click(211, 264);
                click(450, 264);
                selected = 3;
            }
            //Inspiration
            if (getpixelColor(701, 238,73,170,185))
            {
                click(211, 264);
                click(500, 264);
                selected = 4;
            }
            //int centerX2 = 621 + rct.Left;
            //int centerY2 = 257 + rct.Top; sorc and resolve
            //Domination: 616 256
            //Inspiration: 615 267
            //Precision: 623 265
        }
       private bool getpixelColor(int x, int y,int R, int G, int B)
        {
            Console.WriteLine("true");
            Bitmap bmpScreenshot = new Bitmap(50, 50);
            Graphics g = Graphics.FromImage(bmpScreenshot);
            RECT rct = new RECT();
            GetWindowRect(GetForegroundWindow(), ref rct);
            int centerX = x * (rct.Right - rct.Left) / 1600 + rct.Left ;
            int centerY = y * (rct.Bottom - rct.Top) / 900 + rct.Top ;
            g.CopyFromScreen(centerX-25, centerY-25, 0, 0, new Size(50, 50));
            for(int x1 = 0; x1 < 50; x1++)
            {
                for (int y1 = 0; y1 < 50; y1++)
                {
                    if (bmpScreenshot.GetPixel(x1, y1).R == R && bmpScreenshot.GetPixel(x1, y1).G == G && bmpScreenshot.GetPixel(x1, y1).B == B)
                    {
                        Console.WriteLine("true");
                        return true;
                    }
                }
            }
            return false;
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = "Most Frequent Build";
            }
            else
            {
                checkBox1.Text = "Highest Win % Build";
            }
        }

        private void champText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

    }
    


}
