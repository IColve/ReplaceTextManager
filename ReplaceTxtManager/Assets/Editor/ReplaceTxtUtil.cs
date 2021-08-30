using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReplaceTxtUtil : MonoBehaviour
{
    public static string ChooseFile(string type)
    {
        ReplaceTxtFile file = new ReplaceTxtFile();
        file.structSize = Marshal.SizeOf(file);
        file.filter = "文件(*." + type + ")\0*." + type + "";
        file.file = new string(new char[256]);
        file.maxFile = file.file.Length;
        file.fileTitle = new string(new char[64]);
        file.maxFileTitle = file.fileTitle.Length;
        file.initialDir = Application.streamingAssetsPath.Replace('/', '\\');
        file.title = "选择文件";
        file.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetFile(file))
        {
            return file.file;
        }

        return "";
    }

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    private static extern bool GetFile([In, Out] ReplaceTxtFile ofn);

    public static string ChooseFolder(string title = "选择文件夹路径")
    {
        ReplaceTxtFolder folder = new ReplaceTxtFolder();
        folder.pszDisplayName = new string(new char[2048]);
        folder.lpszTitle = title; // 标题  
        folder.ulFlags = 0x00000040; // 新的样式,带编辑框  
        IntPtr pidlPtr = GetFolder(folder);

        char[] charArray = new char[2048];

        for (int i = 0; i < 2048; i++)
        {
            charArray[i] = '\0';
        }

        GetPathIDList(pidlPtr, charArray);
        string res = new string(charArray);
        res = res.Substring(0, res.IndexOf('\0'));
        return res;
    }
    
    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    private static extern bool GetPathIDList([In] IntPtr pidl, [In, Out] char[] fileName);
    
    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    private static extern IntPtr GetFolder([In, Out] ReplaceTxtFolder ofn);
    
        
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class ReplaceTxtFile
    {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public String filter = null;
        public String customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public String file = null;
        public int maxFile = 0;
        public String fileTitle = null;
        public int maxFileTitle = 0;
        public String initialDir = null;
        public String title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public String defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public String templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class ReplaceTxtFolder
    {
        public IntPtr hwndOwner = IntPtr.Zero;
        public IntPtr pidlRoot = IntPtr.Zero;
        public String pszDisplayName = null;
        public String lpszTitle = null;
        public UInt32 ulFlags = 0;
        public IntPtr lpfn = IntPtr.Zero;
        public IntPtr lParam = IntPtr.Zero;
        public int iImage = 0;
    }

}
