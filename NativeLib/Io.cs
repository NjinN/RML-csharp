using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;
using RML.Lang;
using System.Text.RegularExpressions;

namespace RML.NativeLib {
    [Serializable]
    class Rexists : Rnative {
        public Rexists() {
            name = "exists";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.File)) {
                string path = args[0].GetFilePath();
                if (path.EndsWith('/')) {
                    return new Rtoken(Rtype.Bool, Directory.Exists(path));
                } else {
                    return new Rtoken(Rtype.Bool, File.Exists(path));
                }
                
            }
            return ErrorInfo(args);
        }

    }


    [Serializable]
    class Rread : Rnative {
        public Rread() {
            name = "_read";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp) switch {
                (Rtype.File, Rtype.Datatype) => ReadFile(args),

                _ => ErrorInfo(args)
            };
        }

        public Rtoken ReadFile(List<Rtoken> args) {
            if (!File.Exists(args[0].GetStr())) {
                return new Rtoken(Rtype.Err, "Error: File " + args[0].ToStr() + " is not exists");
            }

            List<byte> bts = new List<byte>();
            using(BinaryReader br = new BinaryReader(new FileStream(args[0].GetFilePath(), FileMode.Open))) {
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                
                while(br.PeekChar() > -1) {
                    bts.Add(br.ReadByte());
                }
            }

            if (args[1].GetRtype().Equals(Rtype.Bin)) {
                return new Rtoken(Rtype.Bin, bts);
            }else if (args[1].GetRtype().Equals(Rtype.Str)) {
                return new Rtoken(Rtype.Str, System.Text.Encoding.Default.GetString(bts.ToArray()));
            }


            return new Rtoken(Rtype.Err, "Error: " + args[1].ToStr() + " is not supported for native::_read");
        }



    }


    [Serializable]
    class Rwrite : Rnative {
        public Rwrite() {
            name = "_write";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return (args[0].tp, args[1].tp, args[2].tp) switch {
                (Rtype.File, Rtype.Bin, Rtype.Bool) => WriteFile(args),
                (Rtype.File, Rtype.Str, Rtype.Bool) => WriteFile(args),

                _ => ErrorInfo(args)
            };
        }

        public Rtoken WriteFile(List<Rtoken> args) {
            bool finsh = false;
            using (BinaryWriter bw = new BinaryWriter(new FileStream(args[0].GetFilePath(), args[2].GetBool() ? FileMode.Create | FileMode.Append : FileMode.Create, FileAccess.Write))) {
                if (args[1].tp.Equals(Rtype.Bin)) {
                    bw.Write(args[1].GetBin().ToArray());
                    finsh = true;
                }else if (args[1].tp.Equals(Rtype.Str)) {
                    bw.Write(System.Text.Encoding.Default.GetBytes(args[1].GetStr()));
                    finsh = true;
                }
            }
            if (finsh) {
                return new Rtoken(Rtype.Bool, true);
            }

            return ErrorInfo(args);
        }


    }



    [Serializable]
    class Rfrename : Rnative {
        public Rfrename() {
            name = "frename";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.File) && args[1].tp.Equals(Rtype.Str)) {
                string path = args[0].GetFilePath();
                if (path.EndsWith('/') && Directory.Exists(path)) {

                    try {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                            FileSystem.Rename(path, path + "../" + args[1].GetStr());
                        } else {
                            DirectoryInfo di = new DirectoryInfo(path);
                            di.MoveTo(path + "../" + args[1].GetStr());
                        }
                    } catch (Exception e) {
                        return new Rtoken(Rtype.Err, e.Message);
                    }

                    return new Rtoken(Rtype.Bool, true);
                } else if (!path.EndsWith('/') && File.Exists(path)) {

                    try {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                            FileSystem.Rename(path, path + "/../" + args[1].GetStr());
                        } else {
                            FileInfo fi = new FileInfo(path);
                            fi.MoveTo(path + "/../" + args[1].GetStr());
                        }
                    } catch (Exception e) {
                        return new Rtoken(Rtype.Err, e.Message);
                    }

                    return new Rtoken(Rtype.Bool, true);

                } else {
                    return new Rtoken(Rtype.Bool, false);
                }
            }
            return ErrorInfo(args);
        }

    }


    [Serializable]
    class Rfmove : Rnative {
        public Rfmove() {
            name = "fmove";
            argsLen = 2;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.File) && args[1].tp.Equals(Rtype.File)) {
                string fp0 = args[0].GetFilePath();
                string fp1 = args[1].GetFilePath();

                if (fp0.EndsWith('/') && Directory.Exists(fp0)) {
                    try {
                        DirectoryInfo di = new DirectoryInfo(fp0);
                        di.MoveTo(fp1); 
                    } catch (Exception e) {
                        return new Rtoken(Rtype.Err, e.Message);
                    }

                    return new Rtoken(Rtype.Bool, true);

                } else if (!fp0.EndsWith('/') && File.Exists(fp0)) {
                    try {
                        FileInfo fi = new FileInfo(fp0);
                        fi.MoveTo(fp1);
 
                    } catch (Exception e) {
                        return new Rtoken(Rtype.Err, e.Message);
                    }

                    return new Rtoken(Rtype.Bool, true);
                } else {
                    return new Rtoken(Rtype.Bool, false);
                }
            }
            return ErrorInfo(args);
        }

    }


    [Serializable]
    class Rfcopy : Rnative {
        public Rfcopy() {
            name = "_fcopy";
            argsLen = 3;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.File) && args[1].tp.Equals(Rtype.File) && args[2].tp.Equals(Rtype.Bool)) {
                string fp0 = args[0].GetFilePath();
                string fp1 = args[1].GetFilePath();

                if (fp0.EndsWith('/') && fp1.EndsWith('/') && Directory.Exists(fp0)) {
                    return CopyFolder(fp0, fp1, args[2].ToBool());

                } else if (!fp0.EndsWith('/') && !fp1.EndsWith('/') && File.Exists(fp0)) {
                    try {
                        System.IO.File.Copy(fp0, fp1, args[2].ToBool());

                    } catch (Exception e) {
                        return new Rtoken(Rtype.Err, e.Message);
                    }

                    return new Rtoken(Rtype.Bool, true);
                } else {
                    return new Rtoken(Rtype.Bool, false);
                }
            }
            return ErrorInfo(args);
        }



        public Rtoken CopyFolder(string sourceFolder, string destFolder, bool rewrite) {
            try {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder)) {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files) {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest, rewrite);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders) {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest, rewrite);//构建目标路径,递归复制文件
                }
                return new Rtoken(Rtype.Bool, true);
            } catch (Exception e) {
                return new Rtoken(Rtype.Err, e.Message);
            }

        }

    }




    [Serializable]
    class Rfdelete : Rnative {
        public Rfdelete() {
            name = "fdelete";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.File)) {
                string path = args[0].GetFilePath();
                if (path.EndsWith('/') && Directory.Exists(path)) {
                    try {
                        DirectoryInfo di = new DirectoryInfo(args[0].GetFilePath());
                        di.Delete(true);
 
                    } catch (Exception e) {
                        return new Rtoken(Rtype.Err, e.Message);
                    }

                    return new Rtoken(Rtype.Bool, true);
                } else if (!path.EndsWith(path) && File.Exists(path)) {
                    try {
                        FileInfo fi = new FileInfo(args[0].GetFilePath());
                        fi.Delete();

                    } catch (Exception e) {
                        return new Rtoken(Rtype.Err, e.Message);
                    }

                    return new Rtoken(Rtype.Bool, true);

                } else {
                    return new Rtoken(Rtype.Bool, false);
                }
            }
            return ErrorInfo(args);
        }

    }



    [Serializable]
    class Rfcwd : Rnative {
        public Rfcwd() {
            name = "fcwd";
            argsLen = 0;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            return new Rtoken(Rtype.File, "/" + Directory.GetCurrentDirectory() + "/");
        }

    }


    [Serializable]
    class Rfcd : Rnative {
        public Rfcd() {
            name = "fcd";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[0].tp.Equals(Rtype.File)) {
                return ErrorInfo(args);
            }
            string path = args[0].GetFilePath();
            if (!path.EndsWith('/')) {
                return new Rtoken(Rtype.File, "Error: must be a dir path for native::fcd");
            }

            if (Directory.Exists(path)) {
                Directory.SetCurrentDirectory(path);
                return new Rtoken(Rtype.Bool, true);
            } else {
                return new Rtoken(Rtype.Bool, false);
            }
            
        }

    }


    [Serializable]
    class Rfdir : Rnative {
        public Rfdir() {
            name = "fdir";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (!args[0].tp.Equals(Rtype.File)) {
                return ErrorInfo(args);
            }
            string path = args[0].GetFilePath();
            if (!path.EndsWith('/')) {
                return new Rtoken(Rtype.File, "Error: must be a dir path for native::fdir");
            }

            if (Directory.Exists(path)) {
                return new Rtoken(Rtype.Bool, false);
            } else {
                Directory.CreateDirectory(path);
                return new Rtoken(Rtype.Bool, true);
            }

        }

    }

    [Serializable]
    class Rfls : Rnative {
        public Rfls() {
            name = "_fls";
            argsLen = 1;
        }

        public override Rtoken Run(List<Rtoken> args, Rtable ctx) {
            if (args[0].tp.Equals(Rtype.File)) {
                string path = args[0].GetFilePath();
                if (!Directory.Exists(path)) {
                    return new Rtoken(Rtype.Err, "Error: No such dir of " + args[0].ToStr());
                }

                string[] files = Directory.GetFiles(path);
                string[] dirs = Directory.GetDirectories(path);
                List<Rtoken> list = new List<Rtoken>();

                foreach(var item in files) {
                    list.Add(new Rtoken(Rtype.File, "/" + item));
                }
                foreach (var item in dirs) {
                    list.Add(new Rtoken(Rtype.File, "/" + item + "/"));
                }

                return new Rtoken(Rtype.Block, list);
            }
            
            return ErrorInfo(args);

        }

    }

}
