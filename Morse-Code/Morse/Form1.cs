using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Morse
{
    public partial class Form1 : Form
    {
        private const string PATH = @"G:\С#\Morse-Code\";
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string sC = null,
                rStr = null;
            char bStr = '\0';
            bool MesE = false;
            int cP = 0;
            string ps = PATH + textBox2.Text;
            string pr = PATH + textBox3.Text;
            string lge = "";
            if (comboBox1.Text == "Russian") { lge = "r"; } else { lge = "e"; }

            if (lge != "e" && lge != "r")
            {
            }
            else
            {
                if (lge == "e") lge = "en";
                if (lge == "r") lge = "rus";
                MC mes = new MC(lge);
                char a = '0';
                if (comboBox2.SelectedIndex == 0) { a = 'l'; } else { a = 'c'; }
                using (StreamWriter stW = new StreamWriter(pr, false, System.Text.Encoding.UTF8)) { }
                using (StreamReader stR = File.OpenText(ps))
                {
                    while (stR.Peek() != -1)
                    {
                        MesE = true;
                        if (a == 'c')
                        {
                            while (bStr != ' ')
                            {
                                bStr = (char)stR.Read();
                                if (bStr == '\uffff')
                                    break;
                                if (bStr != ' ')
                                    cP = 0;
                                else
                                    cP++;
                                if (bStr != ' ')
                                    sC += bStr;
                            }
                            if (sC != null)
                                rStr = Convert.ToString(mes.Decode(sC));
                            if (cP == 2)
                                rStr = Convert.ToString(mes.Decode(Convert.ToString(bStr)));
                            sC = null;
                            bStr = '\0';
                        }
                        else if (a == 'l')
                            rStr = mes.Code((char)stR.Read());
                        else
                            Console.WriteLine("You can choose only c or l!");
                        using (StreamWriter stW = new StreamWriter(pr, true, System.Text.Encoding.UTF8))
                            stW.Write(rStr);
                    }
                }
                if (MesE == true)
                    textBox1.Text = "Check the result file.";
                else
                    textBox1.Text = "File is empty!";
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }


    public class MC
    {
        private const string PATH = @"G:\С#\Morse-Code\";
        private string _lge;
        private Dictionary<string, string> aD = new Dictionary<string, string> { };
        private Dictionary<char, string> cD = new Dictionary<char, string> { };
        public delegate void ErrorMessage(string tOfE);
        public event ErrorMessage NE;

        public MC(String lge)
        {
            _lge = lge;
            switch (_lge)
            {
                case "en":
                    CrDic(PATH+"EnglishDictionary.txt");
                    break;
                case "rus":
                    CrDic(PATH+"RussianDictionary.txt");
                    break;
                default:
                    Console.WriteLine("Language uncorrect!");
                    break;
            }
        }
private void CrDic(string p)
        {
            String mFF = null, c = null;
            int ix = 2, lh;

            using (StreamReader fS = File.OpenText(p))
            {
                while (mFF != "///")
                {
                    mFF = fS.ReadLine();
                    if (mFF != "///")
                    {
                        lh = mFF.Length;

                        while (ix < lh)
                        {
                            c += mFF[ix];
                            ix++;
                        }
                        aD.Add(c, Convert.ToString(mFF[0]));
                        cD.Add(mFF[0], c);
                    }
                    ix = 2;
                    c = null;
                }
            }
        }
        private bool IsCode(char sl)
        {
            if (sl == '-' || sl == '.' || sl == ' ' || sl == '\n')
                return true;
            else
                return false;
        }
        private bool IsLe(char sl)
        {
            if (_lge == "en")
            {
                if ((sl >= 'A' && sl <= 'Z') ||
                    (sl >= 'a' && sl <= 'z') ||
                    (sl >= '0' && sl <= '9') ||
                    (sl == ' '))
                    return true;
            }
            if (_lge == "rus")
            {
                if ((sl >= 'А' && sl <= 'Я') ||
                    (sl >= 'а' && sl <= 'я') ||
                    (sl >= '0' && sl <= '9') ||
                    (sl == ' '))
                    return true;
            }
            return false;
        }

        private char ChToU(char sl)
        {
            if ((sl >= 'a' && sl <= 'z') || (sl >= 'а' && sl <= 'я'))
                return (char)(sl - ' ');
            else
                return sl;
        }
        public string Code(char sL)
        {
            sL = ChToU(sL);
            string resultString = null;
            if (IsLe(sL) == true)
            {
                try
                {
                    resultString = cD[sL];
                    resultString += " ";

                    return resultString;
                }
                catch (Exception)
                {
                    if (sL == ' ')
                        return " ";

                    return null;
                }
            }
            else
            {
                return Convert.ToString(sL) + " ";
            }
        }
        public string Decode(string sr)
        {
            if (IsCode(sr[0]) == true)
            {

                try
                {
                    return aD[sr];
                }

                catch (Exception)
                {
                    if (NE != null)
                        NE("Invalid code!");
                    else
                        Console.WriteLine("Invalid code!");

                    return sr;
                }
            }
            else
            {
                return sr;
            }
        }
    }
}
    
