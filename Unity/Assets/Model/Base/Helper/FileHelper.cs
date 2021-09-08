using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ETModel
{
	public static class FileHelper
	{
		public static KeyValuePair<string, string> ConventToSize(float size)
		{
			KeyValuePair<string, string> pair=new KeyValuePair<string, string>();
			float sizef = size;
			int count = 0;
			while (size > 1024)
			{
				size /= 1024;
				count++;
			}
			string value = size.ToString("f2");
			switch (count)
			{
				case 0:
					pair = new KeyValuePair<string, string>(value, "B");
					break;
				case 1:
					pair = new KeyValuePair<string, string>(value, "KB");
					break;
				case 2:
					pair = new KeyValuePair<string, string>(value, "MB");
					break;
				case 3:
					pair = new KeyValuePair<string, string>(value, "G");
					break;
			}
			return pair;
		}

		public static string ConventToSize(long size)
		{
			float sizef = size;
			for (int i = 0; i < 2; i++)
			{
				sizef /= 1024;
			}
			return sizef.ToString("f2") + "MB";
		}
		public static void GetAllFiles(List<string> files, string dir)
		{
			string[] fls = Directory.GetFiles(dir);
			foreach (string fl in fls)
			{
				files.Add(fl);
			}

			string[] subDirs = Directory.GetDirectories(dir);
			foreach (string subDir in subDirs)
			{
				GetAllFiles(files, subDir);
			}
		}
		
		public static void CleanDirectory(string dir)
		{
			foreach (string subdir in Directory.GetDirectories(dir))
			{
				Directory.Delete(subdir, true);		
			}

			foreach (string subFile in Directory.GetFiles(dir))
			{
				File.Delete(subFile);
			}
		}

		public static void CopyDirectory(string srcDir, string tgtDir)
		{
			DirectoryInfo source = new DirectoryInfo(srcDir);
			DirectoryInfo target = new DirectoryInfo(tgtDir);
	
			if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
			{
				throw new Exception("父目录不能拷贝到子目录！");
			}
	
			if (!source.Exists)
			{
				return;
			}
	
			if (!target.Exists)
			{
				target.Create();
			}
	
			FileInfo[] files = source.GetFiles();
	
			for (int i = 0; i < files.Length; i++)
			{
				File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
			}
	
			DirectoryInfo[] dirs = source.GetDirectories();
	
			for (int j = 0; j < dirs.Length; j++)
			{
				CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
			}
		}

        public static string LoadFile(string filePath)
        {
            string url = Application.streamingAssetsPath + "/" + filePath;
#if UNITY_EDITOR
            return File.ReadAllText(url);
#elif UNITY_ANDROID
            WWW www = new WWW(url);
            while (!www.isDone) { }
            return www.text;
#endif
			return string.Empty;
        }
    }
}
