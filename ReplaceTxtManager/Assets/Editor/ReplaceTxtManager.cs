using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ReplaceTxtManager : EditorWindow
{
    private static bool _isInit;
    private string _configFilePath;
    private string _configFolderPath;
    private List<string> _configsList;
    
    [MenuItem("ColveTools/ReplaceTxt")]
    public static void OpenWindow()
    {
        GetWindow(typeof(ReplaceTxtManager), false);
    }

    private void OnGUI()
    {
        if (!_isInit)
        {
            _isInit = true;
            
            _configFilePath = PlayerPrefs.GetString("ReplaceFilePath");
            _configFolderPath = PlayerPrefs.GetString("ReplaceFolderPath");
            RefreshConfigList();
        }
        
        GUILayout.BeginVertical();
        
        if (GUILayout.Button("ChooseFile"))
        {
            string path = ReplaceTxtUtil.ChooseFile("txt");
            PlayerPrefs.SetString("ReplaceFilePath", path);
            _isInit = false;
            Debug.Log($"choose file success :{path}");
        }
        
        if (GUILayout.Button("ChooseFolder"))
        {
            string path = ReplaceTxtUtil.ChooseFolder();
            PlayerPrefs.SetString("ReplaceFolderPath", path);
            Debug.Log($"choose folder success :{path}");
        }

        if (GUILayout.Button("Refresh"))
        {
            RefreshConfigList();
        }

        _configsList.ForEach(x =>
        {
            if (GUILayout.Button(x))
            {
                string fileName = x;
                string path = $"{_configFolderPath}/{fileName}";
                if (File.Exists(path))
                {
                    if (File.Exists(_configFilePath))
                    {
                        ReplaceText(path);
                    }
                    else
                    {
                        Debug.Log($"config path is not exist: {_configFilePath}");
                    }
                }
                else
                {
                    Debug.Log($"select path is not exist: {path}");
                }
            }
        });
        
        GUILayout.EndVertical();
    }

    private void ReplaceText(string selectTxtPath)
    {
        try
        {
            StreamReader reader = new StreamReader(selectTxtPath);
            string str = reader.ReadToEnd();
            reader.Close();

            StreamWriter writer = new StreamWriter(_configFilePath);
            writer.Write(str);
            writer.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        Debug.Log("Replace Txt Success");
    }

    private void RefreshConfigList()
    {
        if (_configsList == null)
        {
            _configsList = new List<string>();
        }
        
        _configsList.Clear();
        
        if (!Directory.Exists(_configFolderPath))
        {
            Debug.Log($"folder path is not exist : {_configFolderPath}");
        }
        else
        {
            DirectoryInfo direction = new DirectoryInfo(_configFolderPath);
            FileInfo[] files = direction.GetFiles("*");
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".txt"))
                {
                    _configsList.Add(files[i].Name);
                }
            }
        }
    }
}
