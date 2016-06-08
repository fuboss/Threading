using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Threading
{
    public partial class View : Form
    {
        public View()
        {
            InitializeComponent();

            _folderBrowser = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            //create folder for xml files
            Directory.CreateDirectory(_xmlFolderPath);
        }

        private readonly string ViewName = "View";
        private readonly FolderBrowserDialog _folderBrowser;
        private ModelHolder _holder;
        private readonly object _threadLock = new object();

        private string _xmlFolderPath { get { return Directory.GetCurrentDirectory() + "\\XML"; } }

        private void ShowMesssage(string message)
        {
            MessageBox.Show(message);
        }

        private bool ValidateDirectoryPath(string dir)
        {
            try
            {
                new DirectoryInfo(dir);
            }
            catch (DirectoryNotFoundException ex)
            {
                //directory path isn't correct
                ShowMesssage(string.Format("Directory {0} not found", dir));
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                //application have no access to this folder
                ShowMesssage("Application have no access to this folder");
                return false;
            }

            return true;
        }

        //Recursive scan of folders and files
        private void TreeScan(string directory, DirectoryInfoWrapper parent=null)
        {
            if(!ValidateDirectoryPath(directory)) return;

            var fileInfos = new List<FileInfoWrapper>();
            string[] files;
            try
            {
                files = Directory.GetFiles(directory);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowMesssage("Application have no access to some subfolder in selected root");
                return;
            }

            var dir = new DirectoryInfoWrapper(directory);
            lock (_threadLock)
            {
                //remember root directory
                if (parent == null)
                    _holder = new ModelHolder(dir);
                else
                    parent.RegisterDir(dir);
            }

            //scaning for files
            foreach (string f in files)
            {
                FileInfoWrapper info;
                try
                {
                    info = new FileInfoWrapper(f);
                }
                catch (FileNotFoundException e)
                {
                    //we do not have to show to user this exception
                    continue;
                }

                fileInfos.Add(info);
            }


            //register those files to current directory
            lock (_threadLock)
                dir.RegisterFiles(fileInfos);

            foreach (string d in Directory.GetDirectories(directory))
                TreeScan(d,dir);
        }

        private async Task CollectInfo(string selectedPath)
        {
            //do not use ThreadPool, 'cause this operation may take a long time
            var task = Task.Factory.StartNew(() => TreeScan(selectedPath), TaskCreationOptions.LongRunning);
            await task;

        }

        private async Task BuildTree()
        {
            if(_holder.Hierarchy == null) return;
            TreeView.Nodes.Clear();
            await Task.Run(() =>
            {
                TreeView.InvokeEx(tw=>tw.Nodes.Add(GetTreeNode(_holder.Hierarchy)));
            });
        }

        //recursive treeNode building
        private TreeNode GetTreeNode(DirectoryInfoWrapper dir)
        {
            var node = new TreeNode(dir.Name);
            foreach (var file in dir.Files)
                node.Nodes.Add(file.Name);
            foreach (var subDirectory in dir.SubDirectories)
                node.Nodes.Add(GetTreeNode(subDirectory));

            return node;
        }

        private async Task ConvertModelToXML()
        {
            var xml = _holder.Hierarchy.Serialize();
            var filePath = string.Format("{0}\\{1}{2}", _xmlFolderPath, _holder.Hierarchy.Name, ".xml");
            //validate
            filePath = filePath.Trim(Path.GetInvalidFileNameChars());
            await Task.Run(() => File.WriteAllText(filePath, xml));

            if (MessageBox.Show("Open folder with generated XML file?", "XML file was generated",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(_xmlFolderPath, string.Format("{0},/select", filePath));
            }
        }
        
        private async void ChooseButton_Click(object sender, EventArgs e)
        {
            if (_folderBrowser.ShowDialog() == DialogResult.OK)
            {
                var selectedPath = _folderBrowser.SelectedPath;
                Text = string.Format("{0} {1}", ViewName, selectedPath);

                FolderButton.Enabled = false;
                await CollectInfo(selectedPath);
                
                await BuildTree();
                await ConvertModelToXML();
                FolderButton.Enabled = true;
            }
        }
    }
}
