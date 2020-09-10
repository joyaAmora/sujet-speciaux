using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileScanner.Models
{
	public class ItemType:INotifyPropertyChanged
    {
		private string itemLink;
		private string picturePath;

		public string ItemLink
		{
			get => itemLink; 
			set { 
				itemLink = value;
				OnPropertyChanged();
			}
		}

		public string PicturePath
		{
			get => picturePath; 
			set { 
				picturePath = value;	
				OnPropertyChanged();
			}
		}

		public ItemType(string link, string picPath)
		{
			itemLink = link;
			picturePath = picPath;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}
