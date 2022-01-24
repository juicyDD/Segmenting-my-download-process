using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDM.Downloader
{
    public class GoogleDriveDirectUrl
    {
		public static string GetGoogleDriveDownloadAddress(string address)
		{
			int index = address.IndexOf("id=");
			int closingIndex;
			if (index > 0)
			{
				index += 3;
				closingIndex = address.IndexOf('&', index);
				if (closingIndex < 0)
					closingIndex = address.Length;
			}
			else
			{
				index = address.IndexOf("file/d/");
				if (index < 0) // address is not in any of the supported forms
					return string.Empty;

				index += 7;

				closingIndex = address.IndexOf('/', index);
				if (closingIndex < 0)
				{
					closingIndex = address.IndexOf('?', index);
					if (closingIndex < 0)
						closingIndex = address.Length;
				}
			}

			return string.Concat("https://drive.google.com/uc?id=", address.Substring(index, closingIndex - index), "&export=download");
		}

	}
}
