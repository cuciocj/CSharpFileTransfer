using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DownloadableFiles {

        private Dictionary<int, ServerFile> files;

        public DownloadableFiles() {
            
        }

        public void Add(ServerFile file) {
            if (files != null && files.Count > 0) {
                files.Add(files.Count + 1, file);
            } else {
                // files dictionary is empty
                files = new Dictionary<int, ServerFile> {
                    { 1, file }
                };
            }
        }

        public ServerFile Get(String key) {
            return files[int.Parse(key)];
        }

        public Dictionary<int, ServerFile> GetFiles() {
            return files;
        }
    }
