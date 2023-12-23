using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;

using WPF_Value_Coverters_View.Properties;

namespace WPF_Value_Coverters_View
{
    /*
     * WPF is mostly made of Binding, value converter and structure template 
     */
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region
        /// <summary>
        /// Default cnstructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();


        }
        #endregion

        #region Helper

        #region on loaded
        /// <summary>
        /// When the appliaction first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //GetLogicalDrive help in retrieving the name of logical drive in the computer form
            // Getting everything logical drive on the machine
            foreach (var drive in Directory.GetLogicalDrives())
            {
                //created new item for it
                var item = new TreeViewItem()
                {
                    //setting the header 
                    Header = drive, //setting it to the drive

                    //Getting a full path
                    Tag = drive //tag are use to store custom info about an element
                };

                  //Adding a dumy item 
                  item.Items.Add(null); //this give us inside dropdown expand icon

                //Listen out to item being expanded
                item.Expanded += Folder_Expanded;

                //Add to the main TreeView
                FolderView.Items.Add(item);
            }
        }
        #endregion

        /// <summary>
        /// when the folder is expanded we find the substring folder\files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region folder_expanded
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region initial_Check
            var item = (TreeViewItem)sender;

            // checking if the item only contain dumy folder
            if (item.Items.Count != 1 || item.Items[0] != null) return;

            //Try to clear our data if it is
            item.Items.Clear();

            //Getting folder name cast to string as our folder name contain string
            var fullpath = (string)item.Tag;

            #region Get_Folder
            //creating a blank list of directories
            var directories = new List<string>();

            //Trying get directories from the folder first and adding it to file
            //ignoring any issue why doing so
            try
            {
                var dire = Directory.GetDirectories(fullpath);
                if(dire.Length>0)
                {
                    directories.AddRange(dire);
                }
                
            }
            catch { }

            //iterating through the foreach directories....

            directories.ForEach(directoryPath =>
            {
                //Creating directory item
                var subItem = new TreeViewItem()
                {
                    // set header as folder name
                    Header = GetFileFolderName(directoryPath),
                    //add tag as full path 
                    Tag = directoryPath
                };
                
                //Add dumy SubItem so we can expand the folder
                subItem.Items.Add(null);

                //Listen out to subItem handle expanded
                subItem.Expanded += Folder_Expanded;

                //Adding these subItem to the parent item
                item.Items.Add(subItem);
            });
            #endregion

            #endregion

            #region Get_File
            //creating a blank list of directories
            var files = new List<string>();

            //Trying get directories from the folder first and adding it to file
            //ignoring any issue why doing so
            try
            {
                var fs = Directory.GetFiles(fullpath);
                if (fs.Length > 0)
                {
                    directories.AddRange(fs);
                }

            }
            catch { }

            //iterating through the foreach directories....

           files.ForEach(filePath =>
            {
                //Creating directory item
                var subItem = new TreeViewItem()
                {
                    // set header as file name
                    Header = GetFileFolderName(filePath),
                    //add tag as full path 
                    Tag = filePath
                };

                
                //Adding these subItem to the parent item
                item.Items.Add(subItem);
            });
            #endregion
        }
        #endregion

        /// <summary>
        /// find the file or folder from a full path
        /// </summary>
        /// <param name="path">The full path</param>
        /// <returns></returns>
        #region
        public static string GetFileFolderName(string path)
        {
            //C:\Something\a folder
            //C:\Something\a file.png

            //Next we check if we have no path return empty
            if (string.IsNullOrEmpty(path)) return string.Empty;

            //checking if our path is been open or window, linux or mac and make all slashes back slashes
            var normalizePath = path.Replace('/', '\\');

            //Next we find the last index in our file path
            var LastIndex = normalizePath.LastIndexOf('\\');

            //Next we check if we our path doesn't contain back slash then we try return the last file path
            if (LastIndex <= 0) return path;

            //Return name after the last back slash
            return path.Substring(LastIndex + 1);

        }
        #endregion

        #endregion


    }
}
