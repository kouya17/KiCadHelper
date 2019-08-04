using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace KiCadHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //  プロジェクトフォルダ読み込み
            if(textBox1.Text == "")
            {
                textBox3.Text = "<ERROR> プロジェクトフォルダが指定されていません。\r\n";
                return;
            }
            string project_path = textBox1.Text;
            textBox3.Text += "プロジェクトフォルダ " + project_path + "\r\n";
            //  作業フォルダ作成
            string product_name = "elecrow-" + System.IO.Path.GetFileName(project_path) + "-" + System.DateTime.Now.ToString("MMdd_hhmm");
            string work_path = project_path + @"\" + product_name;
            textBox3.Text += "作業フォルダ " + work_path + " 作成\r\n";
            System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(work_path);
            //  対象ファイルコピー+リネーム
            string[] target_extensions = { "gtl", "gbl", "gto", "gbo", "gts", "gbs", "gm1", "drl" };
            for (int i = 0; i < target_extensions.Length; i++)
            {
                //  対象ファイル検索
                string[] fileList = System.IO.Directory.GetFileSystemEntries(project_path, @"*." + target_extensions[i]);
                if(fileList.Length != 1)
                {
                    textBox3.Text += "<ERROR> ファイル数が不正です。(拡張子=" + target_extensions[i] + ", ファイル数=" + fileList.Length.ToString() + ")";
                    return;
                }
                //  リネームしつつファイルコピー
                string dest_extention = System.IO.Path.GetExtension(fileList[0]);
                if (target_extensions[i] == "gm1")
                {
                    dest_extention = ".gml";
                }else if(target_extensions[i] == "drl")
                {
                    dest_extention = ".txt";
                }
                string file_name = product_name + dest_extention;
                textBox3.Text += "ファイルコピー " + fileList[0] + "\r\n"
                    + " -> " + work_path + @"\" + file_name + "\r\n";
                System.IO.File.Copy(fileList[0], work_path + @"\" + file_name);
            }
            // 圧縮
            if (textBox2.Text == "")
            {
                textBox3.Text = "<ERROR> zipファイル出力先が指定されていません。\r\n";
                return;
            }
            string zip_path = textBox2.Text + @"\" + product_name + ".zip";
            textBox3.Text += "zip生成 " + work_path + "\r\n"
                + " -> " + zip_path + "\r\n";
            ZipFile.CreateFromDirectory(work_path, zip_path);
        }
    }
}
