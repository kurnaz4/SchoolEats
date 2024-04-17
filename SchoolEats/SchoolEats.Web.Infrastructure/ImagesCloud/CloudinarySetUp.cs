namespace SchoolEats.Web.Infrastructure.ImagesCloud
{
	using CloudinaryDotNet;
	using CloudinaryDotNet.Actions;
	using Newtonsoft.Json.Linq;
	using static Common.GeneralApplicationConstants;
	public class CloudinarySetUp
	{
		private readonly Cloudinary cloudinary;
		//изпращане на снимката от user-a 
		public CloudinarySetUp()
		{
			cloudinary = new Cloudinary(CloudinaryApi);
			cloudinary.Api.Secure = true;
		}
		//изпраща снимка на сървъра и я запазва там
		public async Task  UploadAsync(string filePath)
		{
			var uploadParams = new ImageUploadParams()
			{
				File = new FileDescription(filePath),
				Folder = "SchoolEats",
				UseFilename = true,
				UniqueFilename = false,
				Overwrite = true
			};
			var uploadResult = await this.cloudinary.UploadAsync(uploadParams);
		}
		//взема изпратената снимка от сървъра и създава нейн линк
		public string GenerateImageUrl(string fileName)
		{
			var myTransformation = cloudinary.Api.UrlImgUp.Add("SchoolEats");

			var generatedUrl = myTransformation.BuildUrl(fileName);

			return generatedUrl;
		}

		
	}
}
