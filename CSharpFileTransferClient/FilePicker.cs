using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FileTransferClient {
    
    public partial class FilePicker : Form {
        
        public FilePicker(Dictionary<int, CSharpFileTransferClient.File> dirFiles) {
            InitializeComponent();
            InitializeTable(dirFiles);
        }

        public void InitializeTable(Dictionary<int, CSharpFileTransferClient.File> dirFiles) {
            Console.WriteLine("init tableeeee");
            foreach (var file in dirFiles) {
                Console.WriteLine("{0}: {1}", file.Key, file.Value.name);
            }
        }
    }
}
