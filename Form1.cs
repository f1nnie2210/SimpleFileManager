using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleFileManager
{
    public partial class fileManagerForm : Form
    {
        private string filePath = "C:\\";
        private bool isFile = false;
        private string currentFileName = "";
        public fileManagerForm()
        {
            InitializeComponent();
        }

        private void fileManagerForm_Load(object sender, EventArgs e)
        {
            filePathTextBox.Text = filePath;
            LoadFiles();
        }

        private void LoadFiles()
        {
            DirectoryInfo directoryInfo;
            string tempFilePath = "";
            FileAttributes fileAttributes;
            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "/" + currentFileName;
                    FileInfo fileInfo = new FileInfo(tempFilePath);
                    fileNameLabel.Text = fileInfo.Name;
                    fileTypeLabel.Text = fileInfo.Extension;
                    fileAttributes = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileAttributes = File.GetAttributes(filePath);
                }

                if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    directoryInfo = new DirectoryInfo(filePath);
                    FileInfo[] files = directoryInfo.GetFiles();
                    DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                    string fileExt = "";
                    listView1.Items.Clear();

                    for (int i = 0; i < files.Length; i++)
                    {
                        fileExt = files[i].Extension.ToUpper();
                        switch (fileExt)
                        {
                            case ".MP3":
                                listView1.Items.Add(files[i].Name, 5);
                                break;
                            case ".EXE":
                                listView1.Items.Add(files[i].Name, 6);
                                break;
                            case ".MP4":
                            case ".AVI":
                            case ".MKV":
                                listView1.Items.Add(files[i].Name, 5);
                                break;

                            case ".PDF":
                                listView1.Items.Add(files[i].Name, 4);
                                break;
                            case ".TXT":
                            case ".DOC":
                            case ".DOCX":
                                listView1.Items.Add(files[i].Name, 3);
                                break;
                            case ".PNG":
                            case ".JPG":
                            case ".JPEG":
                                listView1.Items.Add(files[i].Name, 8);
                                break;
                            default:
                                listView1.Items.Add(files[i].Name, 7);
                                break;
                        }

                    }

                    for (int i = 0; i < dirs.Length; i++)
                    {
                        listView1.Items.Add(dirs[i].Name, 0);
                    }
                }
                else
                {
                    fileNameLabel.Text = currentFileName;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            LoadFilesAndDirectories();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentFileName = e.Item.Text;

            FileAttributes fileAttributes = File.GetAttributes(filePath + "/" + currentFileName);
            if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isFile = false;
                filePathTextBox.Text = filePath + "/" + currentFileName;
            }
            else
            {
                isFile = true;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            LoadFilesAndDirectories();
        }

        private void LoadFilesAndDirectories()
        {
            RemoveSlash();
            filePath = filePathTextBox.Text;
            LoadFiles();
            isFile = false;
        }

        private void RemoveSlash()
        {
            string path = filePathTextBox.Text;
            if (path.LastIndexOf("/") == path.Length - 1)
            {
                filePathTextBox.Text = path.Substring(0, path.Length - 1);
            }
        }

        private void GoBack()
        {
            try
            {
                RemoveSlash();
                string path = filePathTextBox.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                isFile = false;
                filePathTextBox.Text = path;
                RemoveSlash();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            GoBack();
            LoadFilesAndDirectories();
        }

    }
}
