using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Skyray.Language
{
    public class GoogleTranslate
    {
        /// 使用WebRequest获取Google翻译后的内容 
        /// </summary> 
        /// <param name="strTranslateString">需要翻译的内容</param> 
        /// <param name="strRequestLanguage">原文语种</param> 
        /// <param name="strResultLanguage">译文语种</param> 
        /// <returns></returns> 
        private string GetGoogleTranslateJSONString(string strTranslateString, string strRequestLanguage, string strResultLanguage)
        {
            string url = "http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=";
            WebRequest request = HttpWebRequest.Create(url + strTranslateString + "&langpair=" + strRequestLanguage + "%7C" + strResultLanguage);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        public string MultiLanguageTranslate(string strTranslateString, string strRequestLanguage, string strResultLanguage)
        {

            try
            {

                if (!string.IsNullOrEmpty(strTranslateString))
                {
                    TranslateString transtring = (TranslateString)Newtonsoft.Json.JsonConvert.DeserializeObject(

                    GetGoogleTranslateJSONString(strTranslateString, strRequestLanguage, strResultLanguage),

                    typeof(TranslateString));

                    if (transtring.responseStatus == 200)

                        return transtring.responseData.translatedText;

                    else

                        return "There was an error.";

                }

                else
                {

                    return strTranslateString;

                }

            }

            catch (Exception e)
            {

                return e.Message;

            }
        }

        public string TranslateEnglishToChinese(string strTranslateString)
        {
            return MultiLanguageTranslate(strTranslateString, "en", "zh-CN");
        }

        public string TranslateChineseToEnglish(string strTranslateString)
        {
            return MultiLanguageTranslate(strTranslateString, "zh-CN", "en");
        }

    }
}
