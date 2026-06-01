using System.Net;
using System.Text;
//using FileAttachmentModel;

namespace TimesheetsBAL
{
    public static class Utill
    {
        public static string ConsumeJsonArrayStringForList(string url, string token = "")
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", token);
            //Add Content type
            myHttpWebRequest.ContentType = "application/json";

            //Set request method
            myHttpWebRequest.Method = "GET";

            //Perform the request
            var myHttpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            StreamReader myReader = new StreamReader(myHttpResponse.GetResponseStream(), Encoding.UTF8);

            string jsonArrayString = myReader.ReadToEnd();

            return jsonArrayString;
        }

        public static string PostJsonArrayStringForList(string url, string jsonstring, string token = "")
        {
            try
            {
                //UploadFileLogger.Writelogmessage("Save record in table starts");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.PreAuthenticate = true;
                httpWebRequest.Headers.Add("Authorization", token);

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = "{\"user\":\"test\"," +
                    // "\"password\":\"bla\"}";

                    streamWriter.Write(jsonstring);
                }
                string result;
                //UploadFileLogger.Writelogmessage("Hitting File attachement save method");
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //UploadFileLogger.Writelogmessage(result);
                return result;

            }
            catch (Exception ex)
            {
                //UploadFileLogger.Writelogmessage(ex.Message);
                throw new Exception(ex.Message);

            }
        }

        //public static async Task<string> PostFiletourlAsync(string url, IFormFile file, FileInformationModel defaultmodel, string url1,string storagecode,string token="")
        //{
        //    byte[] data; string result = "";  var responseContent = "";
        //    ByteArrayContent bytes;
        //    MultipartFormDataContent multiForm = new MultipartFormDataContent();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {

        //            FileInformationModel? fileinfo = new FileInformationModel();
        //            //fileinfo = JsonConvert.DeserializeObject<FileInformationModel>(responseContent);

        //            fileinfo.ReferenceId = defaultmodel.ReferenceId;
        //            fileinfo.CreatedByUserId = defaultmodel.CreatedByUserId;
        //            fileinfo.AppName = defaultmodel.AppName;
        //            fileinfo.FieldName = defaultmodel.FieldName;
        //            fileinfo.Suite = defaultmodel.Suite;
        //            fileinfo.ApplicationId = defaultmodel.ApplicationId;
        //            fileinfo.SubApplicationName = defaultmodel.SubApplicationName;
        //            fileinfo.SubApplicationId = defaultmodel.SubApplicationId;
        //            fileinfo.FileNameSaved = file.FileName;
        //            fileinfo.StorageCode = storagecode;
        //            fileinfo.CreationDate = DateTime.UtcNow;
        //            fileinfo.FileSize = file.Length;
        //            fileinfo.FileFieldType = defaultmodel.FileFieldType;
        //            fileinfo.FormId = defaultmodel.FormId;
        //            fileinfo.FormName = defaultmodel.FormName;
        //            var extension = Path.GetExtension(file.FileName);
        //            fileinfo.FileExtension = extension;
        //            result = JsonConvert.SerializeObject(fileinfo);
        //            var records = PostJsonArrayStringForList(url1, result, token);
        //            var responseObj = JsonConvert.DeserializeObject<FileInformationModel>(records);

        //            using (var br = new BinaryReader(file.OpenReadStream()))
        //            {
        //                data = br.ReadBytes((int)file.OpenReadStream().Length);
        //            }
        //            client.DefaultRequestHeaders.Add("Authorization", token);
        //            bytes = new ByteArrayContent(data);
        //            // multiForm.Add(bytes, "files", defaultmodel.Suite + "_" + "000" + defaultmodel.ApplicationId + "_" + "00" + defaultmodel.SubApplicationId + "_" + defaultmodel.ReferenceId + "_" + responseObj.Id + "" + extension);
        //            multiForm.Add(bytes,"files",$"{defaultmodel.Suite}_{defaultmodel.ApplicationId:D4}_{defaultmodel.SubApplicationId:D4}_{defaultmodel.ReferenceId}_{responseObj.Id:D6}{extension}");

        //            var res = await client.PostAsync(url, multiForm);
        //            responseContent = await res.Content.ReadAsStringAsync();
        //            return result;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        UploadFileLogger.Writelogmessage(ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //    //var returnfile = "";
        //    //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    //using (var streamReader = new StreamReader(returnfile))
        //    //{
        //    //    result = streamReader.ReadToEnd();
        //    //}
        //}

        public static object ConvertList(List<object> value, Type type)
        {
            var containedType = type.GenericTypeArguments.First();
            return value.Select(item => Convert.ChangeType(item, containedType)).ToList();
        }
    }
}
