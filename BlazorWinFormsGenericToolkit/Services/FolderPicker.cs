using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Services
{
	public class FolderPicker : IFolderPicker
	{
		public string PickFolder()
		{
			try
			{
				string selectedFolderPath = string.Empty;
				// 創建 FolderBrowserDialog 實例
				using (var folderBrowserDialog = new FolderBrowserDialog())
				{
					// 設置對話框的標題
					folderBrowserDialog.Description = "請選擇一個資料夾";

					// 顯示對話框，並等待用戶選擇
					DialogResult result = folderBrowserDialog.ShowDialog();

					// 確認用戶按下了 "確定" 按鈕
					if (result == DialogResult.OK)
					{
						// 獲取用戶選擇的資料夾路徑
						selectedFolderPath = folderBrowserDialog.SelectedPath;

					}
				}
				return selectedFolderPath;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
