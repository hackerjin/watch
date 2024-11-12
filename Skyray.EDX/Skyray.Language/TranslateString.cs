using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Language
{
    public class TranslateString
    {

        private TranslatedText responsedata;

        public TranslatedText responseData
        {

            get { return responsedata; }

            set { responsedata = value; }

        }

        private string responsedetails;

        public string responseDetails
        {

            get { return responsedetails; }

            set { responsedetails = value; }

        }

        private int responsestatus;

        public int responseStatus
        {

            get { return responsestatus; }

            set { responsestatus = value; }

        }

        /// 

        /// 译文 

        /// 

        public class TranslatedText
        {

            private string translatedtext;

            public string translatedText
            {

                get { return translatedtext; }

                set { translatedtext = value; }

            }

        }




    }
}
