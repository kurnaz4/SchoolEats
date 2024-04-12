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

		public string GenerateImageUrl(string fileName)
		{
			var myTransformation = cloudinary.Api.UrlImgUp.Add("SchoolEats");

			var generatedUrl = myTransformation.BuildUrl(fileName);

			return generatedUrl;
		}

		public async Task<JToken> GetImageDetails(string fileName)
		{
			var getResourceParams = new GetResourceParams(fileName)
			{
				QualityAnalysis = true
			};
			var getResourceResult = await cloudinary.GetResourceAsync(getResourceParams);
			var resultJson = getResourceResult.JsonObj;

			// Log quality analysis score to the console
			return resultJson["quality_analysis"];
		}
	}
}
